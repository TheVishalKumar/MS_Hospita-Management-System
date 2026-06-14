using HospitalManagementSystem.Data.Data;
using HospitalManagementSystem.Shared.Common;
using HospitalManagementSystem.Shared.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Common
{
    /// <summary>
    /// Abstract base repository providing common CRUD implementation
    /// All entity repositories should inherit from this to avoid code duplication
    /// </summary>
    /// <remarks>
    /// This base class implements the IBaseRepository interface with standard operations.
    /// Derived repositories can override specific methods if custom logic is needed.
    /// </remarks>
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : CommonEntity
    {
        protected readonly AppDbContext _context;
        protected readonly ILogger<BaseRepository<TEntity>> _logger;

        public BaseRepository(AppDbContext context, ILogger<BaseRepository<TEntity>> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all non-deleted entities with AsNoTracking for better performance
        /// </summary>
        public virtual async Task<ApiResponse<List<TEntity>>> GetAllAsync()
        {
            try
            {
                var entities = await _context.Set<TEntity>()
                    .Where(e => !e.IsDeleted)
                    .AsNoTracking()
                    .ToListAsync();

                _logger.LogInformation($"Retrieved {entities.Count} {typeof(TEntity).Name} entities");
                return ApiResponse<List<TEntity>>.Success(entities, $"{typeof(TEntity).Name} records retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving {typeof(TEntity).Name} entities");
                return ApiResponse<List<TEntity>>.Failure(
                    $"Error retrieving {typeof(TEntity).Name} records",
                    new List<string> { ex.Message });
            }
        }

        /// <summary>
        /// Retrieves all entities including soft-deleted ones (admin view)
        /// </summary>
        public virtual async Task<ApiResponse<List<TEntity>>> GetAllIncludingDeletedAsync()
        {
            try
            {
                var entities = await _context.Set<TEntity>()
                    .AsNoTracking()
                    .ToListAsync();

                _logger.LogInformation($"Retrieved {entities.Count} {typeof(TEntity).Name} entities (including deleted)");
                return ApiResponse<List<TEntity>>.Success(entities, $"All {typeof(TEntity).Name} records retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving all {typeof(TEntity).Name} entities");
                return ApiResponse<List<TEntity>>.Failure(
                    $"Error retrieving {typeof(TEntity).Name} records",
                    new List<string> { ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a single entity by its ID
        /// </summary>
        public virtual async Task<ApiResponse<TEntity>> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _context.Set<TEntity>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.CreatedBy == id && !e.IsDeleted);

                if (entity == null)
                {
                    _logger.LogWarning($"{typeof(TEntity).Name} with ID {id} not found");
                    return ApiResponse<TEntity>.Failure(
                        $"{typeof(TEntity).Name} not found",
                        $"No record found with ID: {id}");
                }

                return ApiResponse<TEntity>.Success(entity, $"{typeof(TEntity).Name} retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving {typeof(TEntity).Name} with ID {id}");
                return ApiResponse<TEntity>.Failure(
                    $"Error retrieving {typeof(TEntity).Name}",
                    new List<string> { ex.Message });
            }
        }

        /// <summary>
        /// Creates a new entity with audit information
        /// </summary>
        public virtual async Task<ApiResponse<TEntity>> CreateAsync(TEntity entity, Guid userId)
        {
            try
            {
                entity.CreatedBy = userId;
                entity.CreatedDate = DateTime.UtcNow;
                entity.UpdateBy = userId;
                entity.UpdateDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                entity.Version = 1;

                _context.Set<TEntity>().Add(entity);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Created new {typeof(TEntity).Name} with ID {entity.CreatedBy}");
                return ApiResponse<TEntity>.Success(entity, $"{typeof(TEntity).Name} created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating {typeof(TEntity).Name}");
                return ApiResponse<TEntity>.Failure(
                    $"Error creating {typeof(TEntity).Name}",
                    new List<string> { ex.Message });
            }
        }

        /// <summary>
        /// Updates an existing entity with new audit information
        /// </summary>
        public virtual async Task<ApiResponse<TEntity>> UpdateAsync(TEntity entity, Guid userId)
        {
            try
            {
                var existing = await _context.Set<TEntity>().FindAsync(entity.CreatedBy);
                if (existing == null)
                {
                    return ApiResponse<TEntity>.Failure(
                        $"{typeof(TEntity).Name} not found",
                        $"Cannot update a non-existent record");
                }

                // Update tracking fields
                existing.UpdateBy = userId;
                existing.UpdateDate = DateTime.UtcNow;
                existing.Version += 1;

                // Copy property values (except audit fields)
                _context.Entry(existing).CurrentValues.SetValues(entity);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Updated {typeof(TEntity).Name} with ID {entity.CreatedBy}");
                return ApiResponse<TEntity>.Success(existing, $"{typeof(TEntity).Name} updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating {typeof(TEntity).Name}");
                return ApiResponse<TEntity>.Failure(
                    $"Error updating {typeof(TEntity).Name}",
                    new List<string> { ex.Message });
            }
        }

        /// <summary>
        /// Performs hard delete - permanently removes the record
        /// WARNING: This cannot be undone. Use SoftDeleteAsync for reversible deletion.
        /// </summary>
        public virtual async Task<ApiResponse<bool>> DeleteAsync(Guid id, Guid userId)
        {
            try
            {
                var entity = await _context.Set<TEntity>().FindAsync(id);
                if (entity == null)
                {
                    return ApiResponse<bool>.Failure(
                        $"{typeof(TEntity).Name} not found",
                        $"Cannot delete a non-existent record");
                }

                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();

                _logger.LogWarning($"Hard deleted {typeof(TEntity).Name} with ID {id} by user {userId}");
                return ApiResponse<bool>.Success(true, $"{typeof(TEntity).Name} deleted permanently");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting {typeof(TEntity).Name} with ID {id}");
                return ApiResponse<bool>.Failure(
                    $"Error deleting {typeof(TEntity).Name}",
                    new List<string> { ex.Message });
            }
        }

        /// <summary>
        /// Performs soft delete - marks record as deleted but keeps it in database
        /// Allows for restoration if needed
        /// </summary>
        public virtual async Task<ApiResponse<bool>> SoftDeleteAsync(Guid id, Guid userId)
        {
            try
            {
                var entity = await _context.Set<TEntity>().FindAsync(id);
                if (entity == null)
                {
                    return ApiResponse<bool>.Failure(
                        $"{typeof(TEntity).Name} not found",
                        $"Cannot delete a non-existent record");
                }

                entity.IsDeleted = true;
                entity.DeletedBy = userId;
                entity.DeletedDate = DateTime.UtcNow;
                entity.UpdateBy = userId;
                entity.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Soft deleted {typeof(TEntity).Name} with ID {id}");
                return ApiResponse<bool>.Success(true, $"{typeof(TEntity).Name} deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error soft deleting {typeof(TEntity).Name} with ID {id}");
                return ApiResponse<bool>.Failure(
                    $"Error deleting {typeof(TEntity).Name}",
                    new List<string> { ex.Message });
            }
        }

        /// <summary>
        /// Restores a soft-deleted entity
        /// </summary>
        public virtual async Task<ApiResponse<bool>> RestoreAsync(Guid id, Guid userId)
        {
            try
            {
                var entity = await _context.Set<TEntity>().AsTracking().FirstOrDefaultAsync(e => e.CreatedBy == id && e.IsDeleted);
                if (entity == null)
                {
                    return ApiResponse<bool>.Failure(
                        $"{typeof(TEntity).Name} not found",
                        $"Cannot restore a non-deleted record");
                }

                entity.IsDeleted = false;
                entity.DeletedBy = null;
                entity.DeletedDate = null;
                entity.UpdateBy = userId;
                entity.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Restored {typeof(TEntity).Name} with ID {id}");
                return ApiResponse<bool>.Success(true, $"{typeof(TEntity).Name} restored successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error restoring {typeof(TEntity).Name} with ID {id}");
                return ApiResponse<bool>.Failure(
                    $"Error restoring {typeof(TEntity).Name}",
                    new List<string> { ex.Message });
            }
        }

        /// <summary>
        /// Checks if an entity exists
        /// </summary>
        public virtual async Task<ApiResponse<bool>> ExistsAsync(Guid id)
        {
            try
            {
                var exists = await _context.Set<TEntity>()
                    .AnyAsync(e => e.CreatedBy == id && !e.IsDeleted);

                return ApiResponse<bool>.Success(exists, "");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking existence of {typeof(TEntity).Name}");
                return ApiResponse<bool>.Failure(
                    "Error checking entity existence",
                    new List<string> { ex.Message });
            }
        }

        /// <summary>
        /// Gets count of non-deleted entities
        /// </summary>
        public virtual async Task<ApiResponse<int>> GetCountAsync()
        {
            try
            {
                var count = await _context.Set<TEntity>()
                    .Where(e => !e.IsDeleted)
                    .CountAsync();

                return ApiResponse<int>.Success(count, "");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error counting {typeof(TEntity).Name}");
                return ApiResponse<int>.Failure(
                    "Error counting records",
                    new List<string> { ex.Message });
            }
        }
    }
}

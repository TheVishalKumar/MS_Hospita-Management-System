using HospitalManagementSystem.Shared.Common;
using HospitalManagementSystem.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Common
{
    /// <summary>
    /// Generic interface for repository pattern
    /// Defines standard CRUD operations for all entities
    /// Ensures consistency across all repository implementations
    /// </summary>
    /// <typeparam name="TEntity">The domain entity type</typeparam>
    public interface IBaseRepository<TEntity> where TEntity : CommonEntity
    {
        /// <summary>
        /// Retrieves all non-deleted entities
        /// </summary>
        Task<ApiResponse<List<TEntity>>> GetAllAsync();

        /// <summary>
        /// Retrieves all entities including deleted ones (admin view)
        /// </summary>
        Task<ApiResponse<List<TEntity>>> GetAllIncludingDeletedAsync();

        /// <summary>
        /// Retrieves a single entity by ID
        /// </summary>
        Task<ApiResponse<TEntity>> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates a new entity and returns the created entity with ID
        /// </summary>
        Task<ApiResponse<TEntity>> CreateAsync(TEntity entity, Guid userId);

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        Task<ApiResponse<TEntity>> UpdateAsync(TEntity entity, Guid userId);

        /// <summary>
        /// Performs hard delete (permanent removal)
        /// Use with caution - this cannot be undone
        /// </summary>
        Task<ApiResponse<bool>> DeleteAsync(Guid id, Guid userId);

        /// <summary>
        /// Performs soft delete (logical deletion)
        /// Record remains in database but marked as deleted
        /// Can be restored if needed
        /// </summary>
        Task<ApiResponse<bool>> SoftDeleteAsync(Guid id, Guid userId);

        /// <summary>
        /// Restores a soft-deleted entity
        /// </summary>
        Task<ApiResponse<bool>> RestoreAsync(Guid id, Guid userId);

        /// <summary>
        /// Checks if an entity with given ID exists
        /// </summary>
        Task<ApiResponse<bool>> ExistsAsync(Guid id);

        /// <summary>
        /// Gets count of non-deleted entities
        /// </summary>
        Task<ApiResponse<int>> GetCountAsync();
    }
}

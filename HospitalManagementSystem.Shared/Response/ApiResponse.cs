using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Shared.Response
{
    /// <summary>
    /// Standardized API Response wrapper for all endpoints
    /// Ensures consistent response format across the application
    /// </summary>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indicates whether the API request was successful
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Friendly message describing the result or error
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// The actual data returned from the API (null on error)
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// List of validation or error messages (for failed requests)
        /// </summary>
        public List<string>? Errors { get; set; }

        /// <summary>
        /// Timestamp when the response was generated
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Creates a successful API response
        /// </summary>
        public static ApiResponse<T> Success(T? data, string message = "Operation successful")
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                Data = data,
                Message = message,
                Errors = null
            };
        }

        /// <summary>
        /// Creates a failed API response
        /// </summary>
        public static ApiResponse<T> Failure(string message, List<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
                Data = default,
                Errors = errors ?? new List<string>()
            };
        }

        /// <summary>
        /// Creates a failed API response with single error
        /// </summary>
        public static ApiResponse<T> Failure(string message, string error)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
                Data = default,
                Errors = new List<string> { error }
            };
        }
    }

    /// <summary>
    /// Non-generic version for endpoints that don't return data
    /// </summary>
    public class ApiResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public static ApiResponse Success(string message = "Operation successful")
        {
            return new ApiResponse
            {
                IsSuccess = true,
                Message = message,
                Errors = null
            };
        }

        public static ApiResponse Failure(string message, List<string>? errors = null)
        {
            return new ApiResponse
            {
                IsSuccess = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }
    }
}

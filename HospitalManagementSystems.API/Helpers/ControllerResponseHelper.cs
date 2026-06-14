using HospitalManagementSystem.Models.Constants;
using HospitalManagementSystem.Shared.Response;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystems.API.Helpers
{
    /// <summary>
    /// Helper class for standardizing API responses and authorization
    /// Use this to wrap controller responses with ApiResponse<T>
    /// </summary>
    public static class ControllerResponseHelper
    {
        /// <summary>
        /// Wrap successful response with ApiResponse<T>
        /// </summary>
        public static OkObjectResult OkResponse<T>(this ControllerBase controller, T data, string message = "Operation successful")
        {
            return controller.Ok(ApiResponse<T>.Success(data, message));
        }

        /// <summary>
        /// Wrap creation response with ApiResponse<T> (simplified version)
        /// </summary>
        public static CreatedResult CreatedResponse<T>(this ControllerBase controller, T data, string message = "Resource created successfully")
        {
            return controller.Created(string.Empty, ApiResponse<T>.Success(data, message));
        }

        /// <summary>
        /// Wrap bad request response with ApiResponse<T>
        /// </summary>
        public static BadRequestObjectResult BadRequestResponse<T>(this ControllerBase controller, T? data, string message)
        {
            return controller.BadRequest(ApiResponse<T>.Failure(message, message));
        }

        /// <summary>
        /// Wrap not found response with ApiResponse<T>
        /// </summary>
        public static NotFoundObjectResult NotFoundResponse<T>(this ControllerBase controller, T? data, string message)
        {
            return controller.NotFound(ApiResponse<T>.Failure(message, message));
        }

        /// <summary>
        /// Wrap unauthorized response with ApiResponse<T>
        /// </summary>
        public static UnauthorizedObjectResult UnauthorizedResponse<T>(this ControllerBase controller, T? data, string message)
        {
            return controller.Unauthorized(ApiResponse<T>.Failure(message, message));
        }

        /// <summary>
        /// Wrap forbidden response with ApiResponse<T>
        /// </summary>
        public static ObjectResult ForbiddenResponse<T>(this ControllerBase controller, T? data, string message)
        {
            var response = ApiResponse<T>.Failure(message, message);
            return controller.StatusCode(StatusCodes.Status403Forbidden, response);
        }

        /// <summary>
        /// Wrap server error response with ApiResponse<T>
        /// </summary>
        public static ObjectResult ServerErrorResponse<T>(this ControllerBase controller, T? data, string message)
        {
            var response = ApiResponse<T>.Failure(message, message);
            return controller.StatusCode(StatusCodes.Status500InternalServerError, response);
        }
    }

    /// <summary>
    /// Authorization policy constants for common scenarios
    /// Use with [Authorize(Policy = "...")] or [Authorize(Roles = "...")]
    /// </summary>
    public static class AuthorizationPolicies
    {
        /// <summary>Admin only - can perform all operations</summary>
        public const string AdminOnly = UserRoles.Admin;

        /// <summary>Admin or Doctor - medical staff</summary>
        public static string AdminOrDoctor => $"{UserRoles.Admin},{UserRoles.Doctor}";

        /// <summary>Admin or Receptionist - can manage patients</summary>
        public static string AdminOrReceptionist => $"{UserRoles.Admin},{UserRoles.Receptionist}";

        /// <summary>Any authenticated user</summary>
        public const string AnyAuthenticated = "Authenticated";

        /// <summary>Medical staff roles</summary>
        public static string MedicalStaff => string.Join(",", UserRoles.MedicalStaff);

        /// <summary>Administrative staff roles</summary>
        public static string AdministrativeStaff => string.Join(",", UserRoles.AdministrativeStaff);
    }

    /// <summary>
    /// HTTP status code utilities
    /// </summary>
    public static class HttpStatusCodes
    {
        public const int Success = StatusCodes.Status200OK;
        public const int Created = StatusCodes.Status201Created;
        public const int BadRequest = StatusCodes.Status400BadRequest;
        public const int Unauthorized = StatusCodes.Status401Unauthorized;
        public const int Forbidden = StatusCodes.Status403Forbidden;
        public const int NotFound = StatusCodes.Status404NotFound;
        public const int Conflict = StatusCodes.Status409Conflict;
        public const int ServerError = StatusCodes.Status500InternalServerError;
    }
}


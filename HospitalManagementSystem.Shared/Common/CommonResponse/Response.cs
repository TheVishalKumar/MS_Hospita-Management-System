using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Shared.Common.CommonResponse
{
    public class Response
    {
        public int StatusCode { get; set; }
        public string? StatusMessage { get; set; }
        public object? Data { get; set; }
        public object? Error { get; set; }

        /// <summary>
        /// Constructor for success responses
        /// </summary>
        public Response(int code, string message, object data)
        {
            StatusCode = code;
            StatusMessage = message;
            Data = data;
            Error = null;
        }

        /// <summary>
        /// Constructor for error responses with error details
        /// </summary>
        public Response(int code, string message, object data, object error)
        {
            StatusCode = code;
            StatusMessage = message;
            Data = data;
            Error = error;
        }

        /// <summary>
        /// Static method for success response
        /// </summary>
        public static Response Success(object data, string message = "Success", int statusCode = 200)
        {
            return new Response(statusCode, message, data);
        }

        /// <summary>
        /// Static method for error response with error details
        /// </summary>
        public static Response ErrorResponse(string message, object error, int statusCode = 400)
        {
            return new Response(statusCode, message, null, error);
        }

        /// <summary>
        /// Static method for error response without error details
        /// </summary>
        public static Response ErrorResponse(string message, int statusCode = 400)
        {
            return new Response(statusCode, message, null, null);
        }
    }
}

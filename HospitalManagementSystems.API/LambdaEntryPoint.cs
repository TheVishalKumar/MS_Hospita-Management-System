using Amazon.Lambda.AspNetCoreServer;

namespace HospitalManagementSystems.API
{
    /// <summary>
    /// AWS Lambda entry point for the Hospital Management System API.
    /// This class bootstraps the ASP.NET Core application inside AWS Lambda
    /// using the API Gateway HTTP API (v2 payload format).
    ///
    /// For API Gateway REST API (v1 payload), inherit from APIGatewayProxyFunction instead.
    /// </summary>
    public class LambdaEntryPoint : APIGatewayHttpApiV2ProxyFunction
    {
        /// <summary>
        /// Initialise the ASP.NET Core Host.
        /// The IHostBuilder returned here is the same one used by Program.cs,
        /// so all services, middleware, and configuration are shared.
        /// </summary>
        protected override void Init(IWebHostBuilder builder)
        {
            builder.UseStartup<LambdaStartup>();
        }

        protected override void Init(IHostBuilder builder)
        {
            // No-op: startup is handled via IWebHostBuilder above.
        }
    }
}

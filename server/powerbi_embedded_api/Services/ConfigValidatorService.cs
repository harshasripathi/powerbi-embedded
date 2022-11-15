// ----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
// ----------------------------------------------------------------------------

namespace powerbi_embedded_api.Services
{
    using System;
    using System.Configuration;
    using powerbi_embedded_api.Services.Interfaces;

    public class ConfigValidatorService : IConfigValidatorService
    {
        private readonly IConfiguration _config;
        public ConfigValidatorService(IConfiguration config)
        {
            _config = config;
            ApplicationId = _config.GetSection("AppSettings:applicationId").Value;
            WorkspaceId = GetParamGuid(_config.GetSection("AppSettings:workspaceId").Value);
            ReportId = GetParamGuid(_config.GetSection("AppSettings:reportId").Value);
            AuthenticationType = _config.GetSection("AppSettings:authenticationType").Value;
            ApplicationSecret = _config.GetSection("AppSettings:applicationSecret").Value;
            Tenant = _config.GetSection("AppSettings:tenant").Value;
            Username = _config.GetSection("AppSettings:pbiUsername").Value;
            Password = _config.GetSection("AppSettings:pbiPassword").Value;
        }

        public static string? ApplicationId { get; set; }
        public static Guid WorkspaceId { get; set; }
        public static Guid ReportId { get; set; }
        public string? AuthenticationType { get; set; }
        public static string? ApplicationSecret { get; set; }
        public static string? Tenant { get; set; }
        public static string? Username { get; set; }
        public static string? Password { get; set; }

        /// <summary>
        /// Check if web.config embed parameters have valid values.
        /// </summary>
        /// <returns>Null if web.config parameters are valid, otherwise returns specific error string.</returns>
        public string GetWebConfigErrors()
        {
            string message = null;
            Guid result;

            // Application Id must have a value.
            if (string.IsNullOrWhiteSpace(ApplicationId))
            {
                message = "ApplicationId is empty. please register your application as Native app in https://dev.powerbi.com/apps and fill client Id in web.config.";
            }
            // Application Id must be a Guid object.
            else if (!Guid.TryParse(ApplicationId, out result))
            {
                message = "ApplicationId must be a Guid object. please register your application as Native app in https://dev.powerbi.com/apps and fill application Id in web.config.";
            }
            // Workspace Id must have a value.
            else if (WorkspaceId == Guid.Empty)
            {
                message = "WorkspaceId is empty or not a valid Guid. Please fill its Id correctly in web.config";
            }
            // Report Id must have a value.
            else if (ReportId == Guid.Empty)
            {
                message = "ReportId is empty or not a valid Guid. Please fill its Id correctly in web.config";
            }
            else if (AuthenticationType.Equals("masteruser", StringComparison.InvariantCultureIgnoreCase))
            {
                // Username must have a value.
                if (string.IsNullOrWhiteSpace(Username))
                {
                    message = "Username is empty. Please fill Power BI username in web.config";
                }

                // Password must have a value.
                if (string.IsNullOrWhiteSpace(Password))
                {
                    message = "Password is empty. Please fill password of Power BI username in web.config";
                }
            }
            else if (AuthenticationType.Equals("serviceprincipal", StringComparison.InvariantCultureIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(ApplicationSecret))
                {
                    message = "ApplicationSecret is empty. please register your application as Web app and fill appSecret in web.config.";
                }
                // Must fill tenant Id
                else if (string.IsNullOrWhiteSpace(Tenant))
                {
                    message = "Invalid Tenant. Please fill Tenant ID in Tenant under web.config";
                }
            }
            else
            {
                message = "Invalid authentication type";
            }

            return message;
        }

        private static Guid GetParamGuid(string param)
        {
            Guid paramGuid = Guid.Empty;
            Guid.TryParse(param, out paramGuid);
            return paramGuid;
        }
    }
}

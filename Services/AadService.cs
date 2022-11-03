// ----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
// ----------------------------------------------------------------------------

namespace powerbi_embedded_ui.Services
{
    using Microsoft.Identity.Client;
    using powerbi_embedded_ui.Services.Interfaces;
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Security;
    using System.Threading.Tasks;

    public class AadService : IAadService
    {
        private readonly IConfiguration _config;

        private readonly IConfigValidatorService _configValidatorService;

        public AadService(IConfiguration config, IConfigValidatorService configValidatorService)
        {
            _config = config;
            m_authorityUrl = _config.GetSection("AppSettings:authorityUrl").Value;
            m_scope = _config.GetSection("AppSettings:scope").Value.Split(';');
            _configValidatorService = configValidatorService;
        }

        private static string? m_authorityUrl { get; set; }
        private static string[]? m_scope { get; set; }

        /// <summary>
        /// Get Access token
        /// </summary>
        /// <returns>Access token</returns>
        public async Task<string> GetAccessToken()
        {
            AuthenticationResult authenticationResult = null;
            if (_configValidatorService.AuthenticationType.Equals("masteruser", StringComparison.InvariantCultureIgnoreCase))
            {
                IPublicClientApplication clientApp = PublicClientApplicationBuilder
                                                                    .Create(ConfigValidatorService.ApplicationId)
                                                                    .WithAuthority(m_authorityUrl)
                                                                    .Build();
                var userAccounts = await clientApp.GetAccountsAsync();

                try
                {
                    authenticationResult = await clientApp.AcquireTokenSilent(m_scope, userAccounts.FirstOrDefault()).ExecuteAsync();
                }
                catch (MsalUiRequiredException)
                {
                    SecureString secureStringPassword = new SecureString();
                    foreach (var key in ConfigValidatorService.Password)
                    {
                        secureStringPassword.AppendChar(key);
                    }
                    authenticationResult = await clientApp.AcquireTokenByUsernamePassword(m_scope, ConfigValidatorService.Username, secureStringPassword).ExecuteAsync();
                }
            }

            // Service Principal auth is recommended by Microsoft to achieve App Owns Data Power BI embedding
            else if (_configValidatorService.AuthenticationType.Equals("serviceprincipal", StringComparison.InvariantCultureIgnoreCase))
            {
                // For app only authentication, we need the specific tenant id in the authority url
                var tenantSpecificURL = m_authorityUrl.Replace("organizations", ConfigValidatorService.Tenant);

                IConfidentialClientApplication clientApp = ConfidentialClientApplicationBuilder
                                                                                .Create(ConfigValidatorService.ApplicationId)
                                                                                .WithClientSecret(ConfigValidatorService.ApplicationSecret)
                                                                                .WithAuthority(tenantSpecificURL)
                                                                                .Build();

                authenticationResult = await clientApp.AcquireTokenForClient(m_scope).ExecuteAsync();
            }

            return authenticationResult.AccessToken;
        }
    }
}

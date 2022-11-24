// ----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
// ----------------------------------------------------------------------------

namespace powerbi_embedded_api.Services
{
    using powerbi_embedded_api.Models;
    using Microsoft.PowerBI.Api;
    using Microsoft.PowerBI.Api.Models;
    using Microsoft.Rest;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using powerbi_embedded_api.Services.Interfaces;

    public class EmbedService : IEmbedService
    {

        public EmbedService(IConfiguration config, IAadService aadService)
        {
            _config = config;
            _urlPowerBiServiceApiRoot = _config.GetSection("AppSettings:urlPowerBiServiceApiRoot").Value;
            _aadService = aadService;
        }

        private readonly IAadService _aadService;
        private readonly IConfiguration _config;
        private static string? _urlPowerBiServiceApiRoot { get; set; }

        public async Task<PowerBIClient> GetPowerBiClient()
        {
            var tokenCredentials = new TokenCredentials(await _aadService.GetAccessToken(), "Bearer");
            return new PowerBIClient(new Uri(_urlPowerBiServiceApiRoot!), tokenCredentials);
        }

        /// <summary>
        /// Get all reports from a group
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <returns></returns>
        public async Task<IList<Report>> GetReportsAsync(Guid workspaceId)
        {
            using (var pbiClient = await GetPowerBiClient())
            {
                var pbiReports = await pbiClient.Reports.GetReportsAsync(workspaceId);

                return pbiReports.Value;
            }
        }

        /// <summary>
        /// Get all workspaces from an app
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Group>> GetWorkspaces()
        {
            using (var pbiClient = await GetPowerBiClient())
            {
                var pbiGroups = await pbiClient.Groups.GetGroupsAsync();

                return pbiGroups.Value;
            }
        }
        /// <summary>
        /// Get all dashboards from a group
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <returns></returns>
        public async Task<IList<Dashboard>> GetDashboardsAsync(Guid workspaceId)
        {
            using (var pbiClient = await GetPowerBiClient())
            {
                var pbiDashboards = await pbiClient.Dashboards.GetDashboardsAsync(workspaceId);

                return pbiDashboards.Value;
            }
        }


        /// <summary>
        /// Get embed params for a report
        /// </summary>
        /// <returns>Wrapper object containing Embed token, Embed URL, Report Id, and Report name for single report</returns>
        public async Task<ReportEmbedConfig> GetEmbedParams(Guid workspaceId, Guid reportId, [Optional] Guid additionalDatasetId)
        {
            using (var pbiClient = await GetPowerBiClient())
            {
                // Get report info
                var pbiReport = pbiClient.Reports.GetReportInGroup(workspaceId, reportId);

                /*
                Check if dataset is present for the corresponding report
                If no dataset is present then it is a RDL report 
                */
                bool isRDLReport = String.IsNullOrEmpty(pbiReport.DatasetId);

                EmbedToken embedToken;

                if (isRDLReport)
                {
                    // Get Embed token for RDL Report
                    embedToken = await GetEmbedTokenForRDLReport(workspaceId, reportId);
                }
                else
                {
                    // Create list of dataset
                    var datasetIds = new List<Guid>();

                    // Add dataset associated to the report
                    datasetIds.Add(Guid.Parse(pbiReport.DatasetId));

                    // Append additional dataset to the list to achieve dynamic binding later
                    if (additionalDatasetId != Guid.Empty)
                    {
                        datasetIds.Add(additionalDatasetId);
                    }

                    // Get Embed token multiple resources
                    embedToken = await GetEmbedToken(reportId, datasetIds, workspaceId);
                }

                // Add report data for embedding
                var embedReports = new List<EmbedReport>() {
                    new EmbedReport
                    {
                        ReportId = pbiReport.Id, ReportName = pbiReport.Name, EmbedUrl = pbiReport.EmbedUrl
                    }
                };

                // Capture embed params
                var embedParams = new ReportEmbedConfig
                {
                    EmbedReports = embedReports,
                    EmbedToken = embedToken
                };

                return embedParams;
            }
        }

        /// <summary>
        /// Get Embed token for single report, multiple datasets, and an optional target workspace
        /// </summary>
        /// <returns>Embed token</returns>
        /// <remarks>This function is not supported for RDL Report</remakrs>
        public async Task<EmbedToken> GetEmbedToken(Guid reportId, IList<Guid> datasetIds, [Optional] Guid targetWorkspaceId)
        {
            using (var pbiClient = await GetPowerBiClient())
            {
                // Create a request for getting Embed token 
                // This method works only with new Power BI V2 workspace experience
                var tokenRequest = new GenerateTokenRequestV2(

                reports: new List<GenerateTokenRequestV2Report>() { new GenerateTokenRequestV2Report(reportId) },

                datasets: datasetIds.Select(datasetId => new GenerateTokenRequestV2Dataset(datasetId.ToString())).ToList(),

                targetWorkspaces: targetWorkspaceId != Guid.Empty ? new List<GenerateTokenRequestV2TargetWorkspace>() { new GenerateTokenRequestV2TargetWorkspace(targetWorkspaceId) } : null
                );

                // Generate Embed token
                var embedToken = pbiClient.EmbedToken.GenerateToken(tokenRequest);

                return embedToken;
            }
        }

        /// <summary>
        /// Get Embed token for multiple reports, datasets, and an optional target workspace
        /// </summary>
        /// <returns>Embed token</returns>
        /// <remarks>This function is not supported for RDL Report</remakrs>
        public async Task<EmbedToken> GetEmbedToken(IList<Guid> reportIds, IList<Guid> datasetIds, [Optional] Guid targetWorkspaceId)
        {
            // Note: This method is an example and is not consumed in this sample app

            using (var pbiClient = await GetPowerBiClient())
            {
                // Convert reports to required types
                var reports = reportIds.Select(reportId => new GenerateTokenRequestV2Report(reportId)).ToList();

                // Convert datasets to required types
                var datasets = datasetIds.Select(datasetId => new GenerateTokenRequestV2Dataset(datasetId.ToString())).ToList();

                // Create a request for getting Embed token 
                // This method works only with new Power BI V2 workspace experience
                var tokenRequest = new GenerateTokenRequestV2(

                    datasets: datasets,

                    reports: reports,

                    targetWorkspaces: targetWorkspaceId != Guid.Empty ? new List<GenerateTokenRequestV2TargetWorkspace>() { new GenerateTokenRequestV2TargetWorkspace(targetWorkspaceId) } : null
                );

                // Generate Embed token
                var embedToken = pbiClient.EmbedToken.GenerateToken(tokenRequest);

                return embedToken;
            }
        }

        /// <summary>
        /// Get Embed token for RDL Report
        /// </summary>
        /// <returns>Embed token</returns>
        public async Task<EmbedToken> GetEmbedTokenForRDLReport(Guid targetWorkspaceId, Guid reportId, string accessLevel = "view")
        {
            using (var pbiClient = await GetPowerBiClient())
            {

                // Generate token request for RDL Report
                var generateTokenRequestParameters = new GenerateTokenRequest(
                    accessLevel: accessLevel
                );

                // Generate Embed token
                var embedToken = pbiClient.Reports.GenerateTokenInGroup(targetWorkspaceId, reportId, generateTokenRequestParameters);

                return embedToken;
            }
        }
    }
}

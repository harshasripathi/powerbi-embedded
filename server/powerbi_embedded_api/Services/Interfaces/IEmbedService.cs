using System;
using System.Runtime.InteropServices;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using powerbi_embedded_api.Models;

namespace powerbi_embedded_api.Services.Interfaces
{
    public interface IEmbedService
    {
        Task<PowerBIClient> GetPowerBiClient();
        Task<IList<Report>> GetReportsAsync(Guid workspaceId);
        Task<IList<Dashboard>> GetDashboardsAsync(Guid workspaceId);
        Task<ReportEmbedConfig> GetEmbedParams(Guid workspaceId, Guid reportId, [Optional] Guid additionalDatasetId);
        Task<DashboardEmbedConfig> EmbedDashboard(Guid workspaceId, Guid dashboardId);
        Task<TileEmbedConfig> EmbedTile(Guid workspaceId);
        Task<IList<Group>> GetWorkspaces();
    }
}


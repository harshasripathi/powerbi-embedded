using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.PowerBI.Api.Models;
using powerbi_embedded_ui.Models;
using powerbi_embedded_ui.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace powerbi_embedded_ui.Controllers
{
    [Route("api/[controller]")]
    public class DashboardsController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IEmbedService _embedService;
        private readonly Guid _workspaceId;

        public DashboardsController(IConfiguration config, IEmbedService embedService)
        {
            _config = config;
            _embedService = embedService;
            _workspaceId = new Guid(_config.GetSection("AppSettings:WorkspaceId").Value);
        }

        // GET: api/values
        [HttpGet("GetDashboards")]
        public async Task<IList<Dashboard>> GetDashboards()
        {
            var workspaceId = new Guid(_config.GetSection("AppSettings:WorkspaceId").Value);
            return await _embedService.GetDashboardsAsync(workspaceId);
        }

        [HttpGet("GetDashboardConfig/{dashboardId}")]
        public async Task<DashboardEmbedConfig> GetDashboardConfig(string dashboardId)
        {
            return await _embedService.EmbedDashboard(_workspaceId, new Guid(dashboardId));
        }
    }
}


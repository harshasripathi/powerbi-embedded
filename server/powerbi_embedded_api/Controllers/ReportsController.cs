using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.PowerBI.Api.Models;
using powerbi_embedded_api.Models;
using powerbi_embedded_api.Services;
using powerbi_embedded_api.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace powerbi_embedded_api.Controllers
{
    [Authorize(Roles = "report_admin")]
    [Route("api/[controller]")]
    public class ReportsController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IEmbedService _embedService;
        private readonly Guid _workspaceId;

        public ReportsController(IConfiguration config, IEmbedService embedService)
        {
            _config = config;
            _embedService = embedService;
            _workspaceId = new Guid(_config.GetSection("AppSettings:WorkspaceId").Value!);
        }

        [HttpGet("GetWorkspaces")]
        public async Task<IList<Group>> GetWorkspaces()
        {
            return await _embedService.GetWorkspaces();
        }

        // GET: api/values
        [HttpGet("GetReports/{workspaceId}")]
        public async Task<IList<Report>> GetReports(string workspaceId)
        {
            return await _embedService.GetReportsAsync(new Guid(workspaceId));
        }

        [HttpGet("GetReportConfig/{workspaceId}/{reportId}")]
        public async Task<ReportEmbedConfig> GetReportConfig(string workspaceId, string reportId)
        {
            return await _embedService.GetEmbedParams(new Guid(workspaceId), new Guid(reportId));
        }
    }
}


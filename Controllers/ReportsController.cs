using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.PowerBI.Api.Models;
using powerbi_embedded_ui.Models;
using powerbi_embedded_ui.Services;
using powerbi_embedded_ui.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace powerbi_embedded_ui.Controllers
{
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
            _workspaceId = new Guid(_config.GetSection("AppSettings:WorkspaceId").Value);
        }

        // GET: api/values
        [HttpGet("GetReports")]
        public async Task<IList<Report>> GetReports()
        {
            return await _embedService.GetReportsAsync(_workspaceId);
        }

        [HttpGet("GetReportConfig/{reportId}")]
        public async Task<ReportEmbedConfig> GetReportConfig(string reportId)
        {
            return await _embedService.GetEmbedParams(_workspaceId, new Guid(reportId));
        }
    }
}


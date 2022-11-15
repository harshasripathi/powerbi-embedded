using System;
using Microsoft.PowerBI.Api.Models;

namespace powerbi_embedded_api.Models
{
    public class ReportsDashboardConfig
    {
        public ReportsDashboardConfig()
        {
        }

        public IList<Report>? Reports { get; set; }
        public IList<Dashboard>? Dashboards { get; set; }
    }
}


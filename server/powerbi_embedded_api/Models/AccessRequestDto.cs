using System.Runtime.Serialization;

namespace powerbi_embedded_api.Models
{
    [DataContract(Name = "AccessRequestDto")]
    public class AccessRequestDto
    {
        [DataMember(Name = "ManagerId")]
        public string? ManagerId { get; set; }

        [DataMember(Name = "RequestedUserId")]
        public string? RequestedUserId { get; set; }

        [DataMember(Name = "RequestedUserName")]
        public string? RequestedUserName { get; set; }

        [DataMember(Name = "AdTenantId")]
        public string? AdTenantId { get; set; }

        [DataMember(Name = "WorkspaceId")]
        public string? WorkspaceId { get; set; }

        [DataMember(Name = "ReportId")]
        public string? ReportId { get; set; }

        [DataMember(Name = "ReportName")]
        public string? ReportName { get; set; }

        [DataMember(Name = "ExpiryDate")]
        public DateTime? ExpiryDate { get; set; }
    }
}
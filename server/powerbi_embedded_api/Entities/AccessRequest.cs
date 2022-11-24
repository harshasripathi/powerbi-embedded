using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using powerbi_embedded_api.Models;
using powerbi_embedded_api.Models.Enums;

namespace powerbi_embedded_api.Entities
{
    public class AccessRequest : AccessRequestDto
    {
        public AccessRequest()
        {

        }

        public AccessRequest(AccessRequestDto request)
        {
            ManagerId = request.ManagerId;
            RequestedUserId = request.RequestedUserId;
            RequestedUserName = request.RequestedUserName;
            AdTenantId = request.AdTenantId;
            WorkspaceId = request.WorkspaceId;
            ReportId = request.ReportId;
            ReportName = request.ReportName;
            ExpiryDate = request.ExpiryDate;
            Status = RequestStatus.Pending.ToString();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember(Name = "AccessRequestId")]
        public long Id { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; } = RequestStatus.Pending.ToString();

        [DataMember(Name = "IsActive")]
        public bool IsActive { get; set; } = true;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedOn { get; set; } = DateTime.UtcNow;
    }
}
using powerbi_embedded_api.Models.Enums;

namespace powerbi_embedded_api.Models
{
    public class AccessRequestApprovalDto
    {
        public AccessRequestApprovalDto(long requestId, RequestStatus status, DateTime expiryDate)
        {
            AccessRequestId = requestId;
            Status = status;
            ExpiryDate = expiryDate;
        }
        public long AccessRequestId { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
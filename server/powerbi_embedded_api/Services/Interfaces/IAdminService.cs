using powerbi_embedded_api.Entities;
using powerbi_embedded_api.Models;

namespace powerbi_embedded_api.Services.Interfaces;

public interface IAdminService
{
    Task<IEnumerable<AccessRequest>> GetAllRequestsByManagerId(string id);
    IEnumerable<AccessRequestDto>? GetAllRequestsByUserId(string id);
    Task CreateAccessRequest(AccessRequestDto request);
    Task UpdateAccessRequestApproval(AccessRequestApprovalDto approvalRequestDto);
}
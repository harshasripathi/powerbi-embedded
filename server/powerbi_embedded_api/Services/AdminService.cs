using System.Net;
using powerbi_embedded_api.DA.Interfaces;
using powerbi_embedded_api.Entities;
using powerbi_embedded_api.Exceptions;
using powerbi_embedded_api.Models;

namespace powerbi_embedded_api.Services.Interfaces;

public class AdminService : IAdminService
{
    private readonly IRepository<AccessRequest> _repository;
    public AdminService(IRepository<AccessRequest> repository)
    {
        _repository = repository;
    }
    public async Task<IEnumerable<AccessRequest>> GetAllRequestsByManagerId(string id)
    {
        var requests = await _repository.GetAsync();
        return requests.Where(i => i.IsActive);
    }

    public IEnumerable<AccessRequestDto>? GetAllRequestsByUserId(string id)
    {
        IEnumerable<AccessRequestDto>? accessRequestDto = _repository.GetList(nameof(AccessRequestDto.RequestedUserId), id)?.Where(i => i.IsActive);
        return accessRequestDto;
    }

    public async Task CreateAccessRequest(AccessRequestDto request)
    {
        var requests = await _repository.GetAsync();
        if (requests.Any(i => i.ManagerId == request.ManagerId && i.ReportId == request.ReportId))
        {
            throw new ApiException("The request already exists!", HttpStatusCode.Conflict);
        }
        AccessRequest requestEntity = new(request);
        await _repository.InsertAsync(requestEntity);
    }

    public async Task UpdateAccessRequestApproval(AccessRequestApprovalDto approvalRequestDto)
    {
        var request = await _repository.GetByIdAsync(approvalRequestDto.AccessRequestId);

        if (request == null)
            throw new ApiException("Invalid request id");

        request.ModifiedOn = DateTime.UtcNow;
        request.Status = approvalRequestDto.Status.ToString();
        request.ExpiryDate = approvalRequestDto.ExpiryDate;
        request.IsActive = false;

        await _repository.UpdateAsync(request);
    }
}
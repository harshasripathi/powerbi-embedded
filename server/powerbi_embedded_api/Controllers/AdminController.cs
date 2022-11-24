using Microsoft.AspNetCore.Mvc;
using powerbi_embedded_api.Models;
using powerbi_embedded_api.Services.Interfaces;

namespace powerbi_embedded_api.Controllers
{
    [Route("api/{controller}")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("getAllRequests/{managerId}")]
        public async Task<ActionResult> GetAllRequests(string managerId)
        {
            var requests = await _adminService.GetAllRequestsByManagerId(managerId);
            return Ok(requests);
        }

        [HttpGet("getAllRequestsByUser/{userId}")]
        public ActionResult GetAllRequestsByUser(string userId)
        {
            return Ok(_adminService.GetAllRequestsByUserId(userId));
        }

        [HttpPost("requestaccess")]
        public async Task<ActionResult> RequestAccess([FromBody] AccessRequestDto request)
        {
            await _adminService.CreateAccessRequest(request);
            return Created("Created request successfully", request);
        }

        [HttpPatch("approverequest")]
        public async Task<ActionResult> ApproveRequest([FromBody] AccessRequestApprovalDto approvalRequestDto)
        {
            await _adminService.UpdateAccessRequestApproval(approvalRequestDto);
            return Accepted("Approval request is completed.");
        }
    }
}
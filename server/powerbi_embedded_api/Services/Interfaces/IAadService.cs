using System;
namespace powerbi_embedded_api.Services.Interfaces
{
    public interface IAadService
    {
        Task<string> GetAccessToken();
    }
}


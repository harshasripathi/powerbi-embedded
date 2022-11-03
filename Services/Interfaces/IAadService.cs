using System;
namespace powerbi_embedded_ui.Services.Interfaces
{
    public interface IAadService
    {
        Task<string> GetAccessToken();
    }
}


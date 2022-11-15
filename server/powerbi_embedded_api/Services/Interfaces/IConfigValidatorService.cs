using System;
namespace powerbi_embedded_api.Services.Interfaces
{
    public interface IConfigValidatorService
    {
        string AuthenticationType { get; set; }
        string GetWebConfigErrors();
    }
}


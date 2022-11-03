using System;
namespace powerbi_embedded_ui.Services.Interfaces
{
    public interface IConfigValidatorService
    {
        string AuthenticationType { get; set; }
        string GetWebConfigErrors();
    }
}


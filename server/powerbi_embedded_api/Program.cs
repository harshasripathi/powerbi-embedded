using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using powerbi_embedded_api.Services;
using powerbi_embedded_api.Services.Interfaces;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IEmbedService, EmbedService>();
builder.Services.AddScoped<IConfigValidatorService, ConfigValidatorService>();
builder.Services.AddScoped<IAadService, AadService>();

builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration, "AzureAd", "Bearer", true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

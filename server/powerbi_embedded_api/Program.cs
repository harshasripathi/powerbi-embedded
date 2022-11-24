using Microsoft.Identity.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Serialization;

using powerbi_embedded_api.Exceptions;
using powerbi_embedded_api.Models;
using powerbi_embedded_api.DA.Interfaces;
using powerbi_embedded_api.DA;
using powerbi_embedded_api.Services.Interfaces;
using powerbi_embedded_api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
builder.Services.AddScoped<IEmbedService, EmbedService>();
builder.Services.AddScoped<IConfigValidatorService, ConfigValidatorService>();
builder.Services.AddScoped<IAadService, AadService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Add sql server data context configuration
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AZURE_POSTGRESQL_CONNECTIONSTRING"));
});

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

// Configure global exception handler
app.UseExceptionHandler((options) =>
{
    options.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>();
        ExceptionDetail exDetail;
        if (exception?.Error is ApiException apiException)
        {
            context.Response.StatusCode = (int)apiException.StatusCode;
            exDetail = new(apiException.Message);
        }

        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            exDetail = new("An error occurred while processing the request.");
        }

        await context.Response.WriteAsync(JsonConvert.SerializeObject(exDetail));
    });
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

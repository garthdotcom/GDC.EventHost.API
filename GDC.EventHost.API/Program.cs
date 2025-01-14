using GDC.EventHost.API;
using GDC.EventHost.API.DbContexts;
using GDC.EventHost.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Azure.Identity;
using static GDC.EventHost.API.DbContexts.EventHostContext;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/eventhost.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
builder.Host.UseSerilog();

// debugging
builder.Services.AddProblemDetails();
//builder.Services.AddProblemDetails(options =>
//{
//    options.CustomizeProblemDetails = ctx =>
//    {
//        ctx.ProblemDetails.Extensions
//            .Add("additionalInformation", "Additional information example.");
//        ctx.ProblemDetails.Extensions
//            .Add("server", Environment.MachineName);
//    };
//});

builder.Services.AddControllers(options =>
{
    // if the value in the caller's accept-header is not one we support,
    // return a Not Acceptable status code
    options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson()
.AddXmlDataContractSerializerFormatters();    // we support xml if requested

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            ClientCredentials = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri($"{builder.Configuration["IdpUri"]}/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    { "eventhostapi", "Access to the Event Host API" }
                }
            }
        }
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "oauth2",
                    Type = ReferenceType.SecurityScheme
                }
            }, new List<string>()
        }
    });
});

#if DEBUG
builder.Services.AddTransient<IMailService, LocalMailService>();
#else
builder.Services.AddTransient<IMailService, CloudMailService>();
#endif

builder.Services.AddDbContext<EventHostContext>(dbContextOptions
    => {dbContextOptions.UseSqlServer(
            builder.Configuration["ConnectionStrings:EventHostDBConnectionString"]);
        dbContextOptions.ConfigureWarnings(warnings => 
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
});

builder.Services.AddScoped<IEventHostRepository, EventHostRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

if (builder.Environment.IsProduction())
{
    builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/"),
        new DefaultAzureCredential());
}

// lock down all endpoints by default
builder.Services.AddControllers(c => c.Filters.Add(new AuthorizeFilter()));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdpUri"];
        options.TokenValidationParameters.ValidateAudience = false;
        options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };

        //options.TokenValidationParameters = new()
        //{
        //    ValidateAudience = false,       // turn off to adhere to OAuth2 standards
        //    ValidTypes = new[] { "at+jwt" } // access token in json web token format
        //    //ValidateIssuer = true,
        //    //ValidateAudience = true,
        //    //ValidateIssuerSigningKey = true,
        //    //ValidIssuer = builder.Configuration["Authentication:Issuer"],
        //    //ValidAudience = builder.Configuration["Authentication:Audience"],
        //    //IssuerSigningKey = new SymmetricSecurityKey(
        //    //    Convert.FromBase64String(builder.Configuration["Authentication:SecretForKey"]))
        //};
    });

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("MustBeAdministrator", policy =>
//    {
//        policy.RequireAuthenticatedUser();
//        policy.RequireClaim("admin", "True");
//    });
//});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await SeedData.Initialize(services);
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

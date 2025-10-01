using System.Text.Json.Serialization;
using Esport.Backend.Authorization;
using Esport.Backend.Entities;
using Esport.Backend.Extensions;
using Esport.Backend.Helpers;
using Esport.Backend.Services;
using Esport.Backend.Services.Message;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NLog;
using NLog.Web;

// NLog: setup the logger first to catch all errors
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddDbContext<DataContext>(options =>
        options.UseSqlServer(
        builder.Configuration.GetConnectionString("DbConnection"))
        .ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning))
        );

    builder.Services.AddControllers().AddJsonOptions(x =>
    {
        // serialize enums as strings in api responses (e.g. Role)
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

    // configure strongly typed settings object
    builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
    builder.Services.AddMemoryCache();
    // configure DI for application services
    builder.Services.AddScoped<IJwtUtils, JwtUtils>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IEventService, EventService>();
    builder.Services.AddScoped<IGamesService, GamesService>();
    builder.Services.AddScoped<INewsService, NewsService>();
    builder.Services.AddScoped<IFileService, FileService>();
    builder.Services.AddScoped<IContactService, ContactService>();
    builder.Services.AddScoped<ICaptchaService, CaptchaService>();

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<IWebHelper, WebHelper>();

    //Add RazorViewToStringRenderer so we can render razor based templated for report output and email
    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages();
    builder.Services.AddScoped<RazorViewToStringRenderer>();

    // configure DI for email and sms
    builder.Services.AddTransient<IEmailSender, MessageServices>();
    builder.Services.AddTransient<ISmsSender, MessageServices>();
    builder.Services.AddScoped<INotificationService, NotificationService>();

    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    builder.Services.AddSwaggerDocument(config =>
        {
            config.PostProcess = document =>
            {
                document.Info.Version = "v1";
                document.Info.Title = "esport.p1s.dk API";
                document.Info.Description = "API for interacting with esport.p1s.dk";
                document.Info.TermsOfService = "None";
                document.Info.Contact = new NSwag.OpenApiContact
                {
                    Name = "esport.p1s.dk",
                    Email = "rvs@easymodules.net",
                    Url = "https://p1s.dk"
                };
            };
        });
    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });

    // Register the Swagger generator and the Swagger UI middlewares
    app.UseOpenApi();
    app.UseSwaggerUi();

    //app.UseHttpsRedirection();

    // global cors policy
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
    // global error handler
    app.UseMiddleware<ErrorHandlerMiddleware>();

    // custom jwt auth middleware
    app.UseMiddleware<JwtMiddleware>();

    app.MapControllers();


    app.Run();
}
catch (Exception ex)
{
    //NLog: catch setup errors
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}

using Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddControllersConfiguration();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddCorsConfiguration(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

// Execute startup services
await app.ExecuteStartupServicesAsync();

// Configure environment-specific settings
app.ConfigureDevelopmentEnvironment();
app.ConfigureProductionEnvironment();

// Configure middleware pipeline
app.ConfigureMiddlewarePipeline();

app.Run();
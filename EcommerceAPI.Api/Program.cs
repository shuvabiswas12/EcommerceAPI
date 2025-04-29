using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using EcommerceAPI.Data;
using EcommerceAPI.Data.UnitOfWork;
using EcommerceAPI.Domain;
using EcommerceAPI.DTOs.AutoMapping;
using EcommerceAPI.Services.DataSeeder;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Services.Services;
using EcommerceAPI.Utilities.Filters;
using EcommerceAPI.Utilities.Middlewares;
using Stripe;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using EcommerceAPI.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // To disable the automatic 400 behavior, set the SuppressModelStateInvalidFilter property to true.
        options.SuppressModelStateInvalidFilter = true;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Api Versioning
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader = ApiVersionReader.Combine(
        new HeaderApiVersionReader("x-api-version"),
        new MediaTypeApiVersionReader("x-api-version"),
        new UrlSegmentApiVersionReader());
}).AddMvc()
.AddApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
    setup.AddApiVersionParametersWhenVersionNeutral = true;
});

// Configuring swagger api
builder.Services.AddSwaggerGen(options =>
{

    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Ecommerce api | V1 - Public", Version = "v1", });
    options.SwaggerDoc("v2", new OpenApiInfo { Title = "Ecommerce api | V2 - Admin", Version = "v2", });

    /***
     * Define authorization field security scheme (Bearer AccessToken).
     */
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    // Define the security scheme (API Key)
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "API Key authorization header. Example: \"ApiKey: YOUR_API_KEY\"",
        Name = "ApiKey",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });

    // Include a custom operation filter to apply security requirements based on attributes
    options.OperationFilter<SecurityRequirementsOperationFilter>();

    /***
     * These three lines for including comments of api resources url and their activites.
     */
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Database's connection string was not found."));
});

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(/**options => options.Password.RequiredLength = 8*/)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// configuration authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
    // Configuring jwt bearer
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:ValidAudience"] ?? throw new ArgumentNullException("JWT valid Audience was not found."),
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"] ?? throw new ArgumentNullException("JWT valid Issuer was not found."),
            RequireExpirationTime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"] ?? throw new ArgumentNullException("JWT secret was not found.")))
        };
    });

// Configure google authentication cred with a model class
builder.Services.Configure<GoogleAuthConfigDTO>(builder.Configuration.GetSection("GoogleAuthConfig"));
// builder.Services.AddOptions<GoogleAuthConfigDTO>().Bind(builder.Configuration.GetSection("GoogleAuthConfig"));
// builder.Services.AddOptions<GoogleAuthConfigDTO>().BindConfiguration("GoogleAuthConfig");

// Configuring CORS Policy
builder.Services.AddCors(option =>
{
    option.AddPolicy("DefaultCORS", policy =>
    {
        policy.WithOrigins(origins: builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? throw new("CORS origins did not find."))
        .AllowAnyHeader()
        .WithMethods(HttpMethod.Delete.Method, HttpMethod.Post.Method, HttpMethod.Put.Method, HttpMethod.Get.Method)
        .WithExposedHeaders("*");
    });
});

const string RateLimitingPolicyName = "Fixed";

// Rate limiting
builder.Services.AddRateLimiter(rateLimitingOptions =>
{
    rateLimitingOptions.AddFixedWindowLimiter(RateLimitingPolicyName, options =>
    {
        options.PermitLimit = 3;
        options.Window = TimeSpan.FromSeconds(9);
        options.QueueLimit = 1;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
    rateLimitingOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});


// Services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICategoryServices, CategoryServices>();
builder.Services.AddScoped<IProductServices, ProductServices>();
builder.Services.AddScoped<IAuthenticationServices, AuthenticationServices>();
builder.Services.AddScoped<IDiscountServices, DiscountServices>();
builder.Services.AddScoped<IShoppingCartServices, ShoppingCartServices>();
builder.Services.AddScoped<IOrderServices, OrderServices>();
builder.Services.AddScoped<IPaymentServices, PaymentServices>();
builder.Services.AddScoped<IQuantityServices, QuantityServices>();
builder.Services.AddScoped<IWishlistServices, WishlistServices>();
builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var rolesAndAdminSeeder = new RolesAndAdminSeeder(scope.ServiceProvider);
    await rolesAndAdminSeeder.SeedRolesAndAdminAsync();

    var modelSeeder = new ModelsSeeder(scope.ServiceProvider);
    await modelSeeder.SeedModelsAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

// Cors policy here.
app.UseCors("DefaultCORS");

// Rate limiting
app.UseRateLimiter();

// I do comment this line for api uses using non https api url.
//app.UseHttpsRedirection();

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

// Added by developer
app.UseAuthentication();

app.UseAuthorization();

// Custom middleware for exception handling globaly.
app.UseMiddleware<GlobalExceptionMiddleware>();

/***
    I use "RequireRateLimiting()" after the MapController so that all controllers can achive this rate limiting feature. Meaning, I use a global rate limiting.
    Without this rate limiting could not enable for all controllers.
***/
app.MapControllers().RequireRateLimiting(RateLimitingPolicyName);

app.Run();

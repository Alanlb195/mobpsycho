using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using mobpsycho.Models;
using mobpsycho.Models.Common;
using mobpsycho.Services;
using mobpsycho.Tools;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


//-------- Add services to the container ----------//

//--------- Mapper configuration ------------------//
var mapperConfiguration = new MapperConfiguration(m =>
{
    m.AddProfile(new MappingProfile());
});

IMapper mapper = mapperConfiguration.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddMvc(); //--------- End Mapper Config

//------------------ Controllers Config ----------//
builder.Services.AddControllers();


//---------------- DBContext Config --------------------//
// ConnectionStrings
var myLocalConnection = builder.Configuration.GetConnectionString("MobpsychoLocalDB");
var myHostedConnection = builder.Configuration.GetConnectionString("MobpsychoHostedDB");

builder.Services.AddDbContext<MobpsychoDbContext>(options =>
{
    options.UseSqlServer(myHostedConnection
       , providerOptions => providerOptions.EnableRetryOnFailure(1));
});

//----------------------- JWT Config  --------------------//
builder.Services.AddScoped<IUserService, UserService>(); // AuthConfig

var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);

var appSettings = appSettingsSection.Get<AppSettings>();
var llave = Encoding.ASCII.GetBytes(appSettings.Secret);
builder.Services.AddAuthentication(d =>
{
    d.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    d.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(d =>
    {
        d.RequireHttpsMetadata = false;
        d.SaveToken = true;
        d.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(llave),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    }); // End JWT Config

//----------------- Swagger API description ---------//
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Mobpsycho API",
        Description = "A Lambda ASP.NET Core Web API to manage mobsycho characters",
        //TermsOfService = new Uri(""),
        Contact = new OpenApiContact
        {
            Name = "Alan L. B. Github",
            Url = new Uri("https://github.com/Alanlb195")
        },
        //License = new OpenApiLicense
        //{
        //    Name = "Example License",
        //    Url = new Uri("https://example.com/license")
        //}
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

}); // END Swagger API description

//-------------- CORS -----------------// https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-6.0
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          // Allow any
                          policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
});// End of CORS


// Add AWS Lambda support
// When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);


//-------- End of the Services container ----------//


var app = builder.Build();

// Read more about Swashbuckle - https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle
if (app.Environment.IsDevelopment()) // Development Environment
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction()) // Production Environment (Lambda Deployed) check security :)
{
    app.UseSwagger();
    app.UseSwaggerUI(setup =>
    {
        var myString = "readoc"; // Swagger Doc Production Environment
        string swaggerJsonBasePath = string.IsNullOrWhiteSpace(setup.RoutePrefix) ? "." : "..";
        setup.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "version 1.0");
        setup.OAuthAppName("Lambda API");
        setup.OAuthScopeSeparator(" ");
        setup.OAuthUsePkce();
        setup.RoutePrefix = myString;
    });
}

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication(); // to use JWT
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Welcome to running ASP.NET Core Minimal API on AWS Lambda");

app.Run();

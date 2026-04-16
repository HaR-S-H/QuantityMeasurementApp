using System.Text.Json.Serialization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuantityMeasurementApp.Api.Options;
using QuantityMeasurementApp.Business;
using QuantityMeasurementApp.Repository;

var builder = WebApplication.CreateBuilder(args);

// Configure web/API services and enum JSON handling.
builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter(allowIntegerValues: false)
        );
    });
builder.Services.AddCors(options =>
{
    var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
        ??
        [
            "http://localhost:4200",
            "https://quantitymeasurementapp-frontend-bh2q.onrender.com"
        ];

    options.AddPolicy(
        "FrontendClients",
        policy =>
            policy
                .WithOrigins(allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod()
    );
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        JwtBearerDefaults.AuthenticationScheme,
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter: Bearer {your JWT token}",
        }
    );

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme,
                    },
                },
                Array.Empty<string>()
            },
        }
    );
});

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
builder.Services.Configure<GoogleAuthOptions>(
    builder.Configuration.GetSection(GoogleAuthOptions.SectionName)
);
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();
// Repository layer handles SQL persistence and optional Redis caching.
builder.Services.AddQuantityMeasurementRepository(builder.Configuration);

var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? new JwtOptions();
if (string.IsNullOrWhiteSpace(jwtOptions.Secret))
{
    throw new InvalidOperationException("JWT configuration is missing. Configure Jwt settings in appsettings.");
}

builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
            ClockSkew = TimeSpan.Zero,
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();
                var tokenId = context.Principal?.FindFirstValue(JwtRegisteredClaimNames.Jti);

                if (!string.IsNullOrWhiteSpace(tokenId) && authService.IsTokenRevoked(tokenId))
                {
                    context.Fail("Token has been revoked.");
                }

                return Task.CompletedTask;
            },
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Apply pending EF migrations at startup so schema stays aligned with code.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<QuantityMeasurementDbContext>();
    dbContext.Database.Migrate();
}

// Configure middleware pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    // app.UseSwaggerUI(options =>
    // {
    //     options.DefaultModelsExpandDepth(-1);
    // });
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseCors("FrontendClients");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

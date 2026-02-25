using AmbustockBackend.Data;
using AmbustockBackend.Models;
using AmbustockBackend.Repositories;
using AmbustockBackend.Service;
using AmbustockBackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.Configure<EmailSettings>
(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Ambustock API", Version = "v1" });
    
    // Configurar JWT en Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando el esquema Bearer. Ejemplo: \"Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configurar JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ?? "tu-clave-secreta-super-segura-de-al-menos-32-caracteres-ambustock");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // IMPORTANTE: false para desarrollo local con HTTP
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Registrar Repositories
builder.Services.AddScoped<IAmbulanciaRepository, AmbulanciaRepository>();
builder.Services.AddScoped<IZonaRepository, ZonaRepository>();
builder.Services.AddScoped<ICajonRepository, CajonRepository>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ICorreoRepository, CorreoRepository>();
builder.Services.AddScoped<IReposicionRepository, ReposicionRepository>();
builder.Services.AddScoped<IResponsableRepository, ResponsableRepository>();
builder.Services.AddScoped<IServicioRepository, ServicioRepository>();
builder.Services.AddScoped<IServicioAmbulanciaRepository, ServicioAmbulanciaRepository>();
builder.Services.AddScoped<IDetalleCorreoRepository, DetalleCorreoRepository>();
builder.Services.AddScoped<IRevisionRepository, RevisionRepository>();

// Registrar Services
builder.Services.AddScoped<AmbulanciaService>();
builder.Services.AddScoped<ZonaService>();
builder.Services.AddScoped<CajonService>();
builder.Services.AddScoped<MaterialService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<CorreoService>();
builder.Services.AddScoped<ReposicionService>();
builder.Services.AddScoped<ResponsableService>();
builder.Services.AddScoped<ServicioService>();
builder.Services.AddScoped<ServicioAmbulanciaService>();
builder.Services.AddScoped<DetalleCorreoService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<RevisionService>();
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<ReposicionService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
        else
        {
            policy.WithOrigins(
                    "http://127.0.0.1:5501",
                    "http://localhost:5501",
                    "http://127.0.0.1:5173",
                    "http://localhost:5173",
                    "http://localhost:8080",
                    "http://127.0.0.1:8080"
                )
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        }
    });
});

builder.Services.AddDbContext<AmbustockContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaConexionAmbuStock")));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseCors("AllowFrontend"); 
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

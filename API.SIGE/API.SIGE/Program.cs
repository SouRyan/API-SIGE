using System.Text;
using API.SIGE.Data;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Interfaces.Services;
using API.SIGE.Repositories;
using API.SIGE.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("WebApiDatabase");

builder.Services.AddScoped<ITipoUsuarioRepository, TipoUsuarioRepository>();
builder.Services.AddScoped<ICaixilhoRepository, CaixilhoRepository>();
builder.Services.AddScoped<IFamiliaCaixilhoRepository, FamiliaCaixilhoRepository>();
builder.Services.AddScoped<IObraRepository, ObraRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ICargoRepository, CargoRepository>();
builder.Services.AddScoped<IMedicaoRepository, MedicaoRepository>();
builder.Services.AddScoped<IProducaoFamiliaRepository, ProducaoFamiliaRepository>();
builder.Services.AddScoped<IAnexoRepository, AnexoRepository>();
builder.Services.AddScoped<INotificacaoRepository, NotificacaoRepository>();

builder.Services.AddDbContext<AppDbData>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<INotificacaoService, NotificacaoService>();
builder.Services.AddScoped<ICargoService, CargoService>();
builder.Services.AddScoped<IMedicaoService, MedicaoService>();
builder.Services.AddScoped<IProducaoFamiliaService, ProducaoFamiliaService>();
builder.Services.AddScoped<IAnexoService, AnexoService>();
builder.Services.AddScoped<IObraService, ObraService>();
builder.Services.AddScoped<IFamiliaCaixilhoService, FamiliaCaixilhoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<ICaixilhoService, CaixilhoService>();
builder.Services.AddScoped<ITipoUsuarioService, TipoUsuarioService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Insira o token JWT. Exemplo: eyJhbGciOiJI..."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("bearer", document)] = []
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

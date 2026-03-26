using API.SIGE.Data;
using API.SIGE.Interfaces;
using GerenciamentoProducao.Repositories;


//using GerenciamentoProducao.Services;
using Microsoft.EntityFrameworkCore;
using SIGE.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("WebApiDatabase");

// Program.cs
builder.Services.AddScoped<ITipoUsuarioRepository, TipoUsuarioRepository>();
builder.Services.AddScoped<ICaixilhoRepository, CaixilhoRepository>();
builder.Services.AddScoped<IFamiliaCaixilhoRepository, FamiliaCaixilhoRepository>();
builder.Services.AddScoped<IObraRepository, ObraRepository>();
//builder.Services.AddScoped<IProducaoRepository, ProducaoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();



builder.Services.AddDbContext<AppDbData>(options =>
    options.UseNpgsql(connectionString));

//builder.Services.AddScoped<GoogleCalendarService>();



//builder.Services.AddAuthentication("GerenciadorProd")
//    .AddCookie("GerenciadorProd", options =>
//    {
//        options.LoginPath = "/Usuario/Login";
//        options.AccessDeniedPath = "/Usuario/AcessoNegado";
//        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
//        options.SlidingExpiration = true;
//    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

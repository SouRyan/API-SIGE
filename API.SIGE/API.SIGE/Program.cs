using API.SIGE.Data;
//using GerenciamentoProducao.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("WebApiDatabase");

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



app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using MailKit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using PedagioPayFadamiBack.Data.Repository;
using PedagioPayFadamiBack.Data.Repository.Interface;
using PedagioPayFadamiBack.Service;
using PedagioPayFadamiBack.Service.Interface;
using PedagioPayPortalUsuario.Data;
using PedagioPayPortalUsuario.Data.Interfaces;
using PedagioPayPortalUsuario.Data.Repositories;
using PedagioPayPortalUsuario.Models;
using PedagioPayPortalUsuario.Service;
using PedagioPayPortalUsuario.Service.Interfaces;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

Log.Information("Starting up");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PEDAGIOPAYContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevelopmentDB"));
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddControllers();

builder.Services.AddScoped<IRepositoryUsuario, UsuarioRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserPlacaRepository, UserPlacaRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IRepositoryPlaca, PlacaRepository>();
builder.Services.AddScoped<IPlacaService, PlacaService>();
builder.Services.AddScoped<IRepositoryDebito, DebitoRepository>();
builder.Services.AddScoped<IDebitoService, DebitoService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAutenticacaoExterna, AutenticacaoExternaService>();




builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
});
builder.Services.AddCors(opt => opt.AddPolicy("CorsPolicy", c =>
{
    c.AllowAnyOrigin()
       .AllowAnyHeader()
       .AllowAnyMethod();
}));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.UseSerilog((ctx, lc) => lc
.WriteTo.Console()
.ReadFrom.Configuration(ctx.Configuration));

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    builder.WebHost.UseUrls("http://localhost:5010");
#if !DEBUG
    builder.WebHost.UseEnvironment("Production");
    builder.WebHost.UseIISIntegration();
#endif
}


app.UsePathBase("/pedagiopay-portalinterno-api");
//app.UsePathBase("/pedagioPay-portalUsuario");

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.Run();

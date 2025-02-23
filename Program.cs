using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using NetCamjor.Data;
using NetCamjor.Data.Inmuebles;
using NetCamjor.Data.Usuarios;
using NetCamjor.Middleware;
using NetCamjor.Models;
using NetCamjor.Profiles;
using NetCamjor.Token;

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls($"http://*:{port}");

string secretKey = $"{Guid.NewGuid()}{Guid.NewGuid()}";
//coneccion a SQLserver
builder.Services.AddDbContext<AppDbContext>(opt => {
    opt.LogTo(Console.WriteLine, new [] {
        DbLoggerCategory.Database.Command.Name}, 
        LogLevel.Information).EnableSensitiveDataLogging();

    opt.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerConnection")!);
});

//coneccion a MySql
/*var connectionMySqlString = builder.Configuration.GetConnectionString("MySqlAws"); 
builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseMySql(connectionMySqlString, ServerVersion.AutoDetect (connectionMySqlString));
});*/



builder.Services.AddScoped<IInmuebleRepository,  InmuebleRepository>();


// Add services to the container.

builder.Services.AddControllers( opt => {
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mapperConfig = new MapperConfiguration(mc => {
    mc.AddProfile(new InmuebleProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


var builderSecurity = builder.Services.AddIdentityCore<Usuario>();
var identityBuilder = new IdentityBuilder(builderSecurity.UserType, builder.Services);
identityBuilder.AddEntityFrameworkStores<AppDbContext>();
identityBuilder.AddSignInManager<SignInManager<Usuario>>();
builder.Services.AddSingleton<ISystemClock, SystemClock>();
builder.Services.AddScoped<IJwtGenerador, JwtGenerador>();
builder.Services.AddScoped<IUsuarioSesion, UsuarioSesion>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
//builder.Services.AddScoped<IInmuebleRepository, InmuebleRepository>();

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("bc2f6c58-5186-4e2c-93ca-0143b11b0c0394b6923d-53c7-4849-9d9f-a95590d588e1"));
//Console.WriteLine("secretKey"+secretKey);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer( opt => {
                    opt.TokenValidationParameters = new TokenValidationParameters 
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateAudience = false,
                        ValidateIssuer = false
                    };
                     opt.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            //Console.WriteLine("secretKey"+secretKey);
                            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                            return Task.CompletedTask;
                            
                        },
                        OnTokenValidated = context =>
                        {
                            Console.WriteLine("Token validated successfully");
                            return Task.CompletedTask;
                        }
                    };

                });


builder.Services.AddCors( o => o.AddPolicy("corsapp", builder => {
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ManagerMiddleware>();

app.UseAuthentication();
app.UseCors("corsapp");
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


using (var ambiente = app.Services.CreateScope())
{
    var services = ambiente.ServiceProvider;

    try {
        var userManager = services.GetRequiredService<UserManager<Usuario>>();
        var context = services.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();
        await LoadDatabase.InsertarData(context, userManager);

    }catch(Exception e) {
        var logging = services.GetRequiredService<ILogger<Program>>();
        logging.LogError(e, "Ocurrio un error en la migracion");
    }
}







app.Run();

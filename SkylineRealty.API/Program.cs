using SkylineRealty.API;
using SkylineRealty.API.DBContext;
using SkylineRealty.API.Services;
using SkylineRealty.API.Services.Implementations;
using SkylineRealty.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SkylineRealty.API.Entities;

var builder = WebApplication.CreateBuilder(args);

//var port = Environment.GetEnvironmentVariable("port") ?? "8080";
//builder.WebHost.UseUrls($"http://*:{port}");

// Add services to the container.
//builder.Services.AddHealthChecks();

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.AddSecurityDefinition("SkylineRealtyApiBearerAuth", new OpenApiSecurityScheme() //Esto va a permitir usar swagger con el token.
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Acá pegar el token generado al loguearse."
    });

    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "SkylineRealtyApiBearerAuth" } //Tiene que coincidir con el id seteado arriba en la definición
                }, new List<string>() }
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*builder.Services.AddSingleton<Data>();*/ // durante toda la ejecucion de la app va a existir una instancia de esta clase, esta instancia va a ser siempre la misma

builder.Services.AddDbContext<SkylineRealtyContext>(dbContextOptions => dbContextOptions.UseSqlite(builder.Configuration["ConnectionStrings:SkylineRealtyDBConnectionString"])); // agregamos el context

builder.Services.AddScoped<HttpClient>();

builder.Services.AddScoped<ICommentsRepository, CommentsRepository>();
builder.Services.AddScoped<IPropertiesRepository, PropertiesRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IMessagesRepository, MessagesRepository>();

builder.Services.AddAuthentication("Bearer") //"Bearer" es el tipo de auntenticación que tenemos que elegir después en PostMan para pasarle el token
   .AddJwtBearer(options => //Acá definimos la configuración de la autenticación. le decimos qué cosas queremos comprobar. La fecha de expiración se valida por defecto.
   {
       options.TokenValidationParameters = new()
       {
           ValidateIssuer = true,
           ValidateAudience = true,
           ValidateIssuerSigningKey = true,
           ValidIssuer = builder.Configuration["AuthenticationConfiguration:Issuer"],
           ValidAudience = builder.Configuration["AuthenticationConfiguration:Audience"],
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["AuthenticationConfiguration:Salt"]))
       };
   }
);

builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder =>
{
    builder.WithOrigins("http://localhost:5173") // Especifica el origen permitido
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials(); // Si necesitas enviar cookies o credenciales
});

app.UseHttpsRedirection();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

app.UseAuthentication();

app.UseAuthorization();

//app.UseHealthChecks("/health");

app.MapControllers();

app.Run();

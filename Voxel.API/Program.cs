using Microsoft.EntityFrameworkCore;
using Voxel.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

// Configure database connection BEFORE building the app.
// If no real connection string is provided (or the placeholder is left),
// use an in-memory database so the API can run and be tested without MySQL.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString) || connectionString.Contains("SUA_SENHA_AQUI"))
{
    // In-memory provider for local testing without DB
    builder.Services.AddDbContext<VoxelContext>(options =>
        options.UseInMemoryDatabase("VoxelInMemory"));
}
else
{
    builder.Services.AddDbContext<VoxelContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
}

// Add services to the container.
builder.Services.AddCors(options => {
    options.AddPolicy("LiberarFront", policy => {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// Configure JWT authentication
var jwtKey = builder.Configuration.GetValue<string>("Jwt:Key") ?? "CHAVE_SECRETA_DE_DESEMPENHO_LOCAL";
var jwtIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer") ?? "voxel.local";
var keyBytes = System.Text.Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtIssuer,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(keyBytes)
        };
    });

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Ensure the database is created (works for InMemory and relational providers)
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<VoxelContext>();
    ctx.Database.EnsureCreated();
}

app.UseCors("LiberarFront");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

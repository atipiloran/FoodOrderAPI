using System;
using System.IO;
using System.Text;
using FoodOrderAPI.Data;
using FoodOrderAPI.Models;
using FoodOrderAPI.Repository;
using FoodOrderAPI.Repository.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace FoodOrderAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Load configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Access configuration values
            var connectionString = configuration.GetConnectionString("DefaultSQLConnection");
            var secret = configuration.GetSection("AppSettings:ApiSettings:Secret").Value;

            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Cors",
                    builder => builder.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // Retrieve the secret key from appsettings.json
            var secretKey = Encoding.ASCII.GetBytes(secret);

            // Configure authentication with JWT Bearer
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

            builder.Services.AddControllers();

            // Configure Swagger/OpenAPI
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FoodOrderAPI", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                         new OpenApiSecurityScheme{
                            Reference=new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id="Bearer"
                            }
                         },
                    new string[]{}
                }
                });
            });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FoodOrderAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseCors("Cors");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

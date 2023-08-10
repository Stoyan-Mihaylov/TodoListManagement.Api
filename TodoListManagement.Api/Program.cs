using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListManagement.Api.Common;
using TodoListManagement.Api.Common.Configuration;
using TodoListManagement.Api.Middlewares;
using TodoListManagement.Business.Service.TodoItem;
using TodoListManagement.Data.Data;
using TodoListManagement.Data.Repositories.TodoItems;

namespace TodoListManagement.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var authOptions = builder.Configuration.GetSection(nameof(AuthOptions)).Get<AuthOptions>();
            builder.Services.AddSingleton(authOptions);

            builder.Services
                .AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<TodoItemDbContext>()
                .AddDefaultTokenProviders();

            builder.Services
                .AddScoped<ITodoItemsRepository, TodoItemsRepository>()
                .AddScoped<ITodoItemService, TodoItemService>()
                .AddDbContext<TodoItemDbContext>(options => options
                .UseSqlServer(builder.Configuration.GetConnectionString("TodoItemConnectionString")))
                .AddTransient<ApplicationInitializer>();

            var init = builder.Services.BuildServiceProvider().GetService<ApplicationInitializer>();
            await init.InitializeAsync();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TodoListManagement.Api", Version = "v1" });
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Please enter Bearer with JWT into field"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new List<string>()
                    }
                });
            });

            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(configuration =>
                {
                    configuration.RequireHttpsMetadata = false;
                    configuration.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(authOptions.SecurityKeyAsBytes),
                        ValidateAudience = true,
                        ValidAudience = authOptions.Audience,
                        ValidateIssuer = true,
                        ValidIssuer = authOptions.Issuer,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer(); ;

            var app = builder.Build();

            //await app.InitializeApplicationAsync();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoListManagement.Api v1"));
            }

            app.UseGlobalValidationMiddleware()
               .UseHttpsRedirection()
               .UseRouting()
               .UseAuthentication()
               .UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
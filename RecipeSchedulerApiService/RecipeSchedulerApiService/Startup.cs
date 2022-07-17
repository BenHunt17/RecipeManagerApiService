using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RecipeSchedulerApiService.Interfaces;
using RecipeSchedulerApiService.Services;
using RecipeSchedulerApiService.Models;
using RecipeSchedulerApiService.Repositories;
using Azure.Storage.Blobs;
using RecipeSchedulerApiService.Validators;
using FluentValidation;
using RecipeSchedulerApiService.Utilities;
using Microsoft.IdentityModel.Tokens;
using System.Text;

//TODO - investigate logging
//TODO - Investigate unit testing

namespace RecipeSchedulerApiService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Adds both the sql connection object and database transaction object. Since they are scoped, the same instance will be used within the same http request scope. This allows for a transaction to feature multiple database operations before commiting.
            services.AddScoped(s => new SqlConnection(Configuration.GetConnectionString("RecipesDatabase"))); 
            services.AddScoped<IDbTransaction>(s =>
            {
                SqlConnection conn = s.GetRequiredService<SqlConnection>();
                conn.Open();
                return conn.BeginTransaction();
            });

            //Adds azure blob storage dependencies. Blob storage controller is cusotm made
            services.AddScoped(s => new BlobServiceClient(Configuration.GetValue<string>("AzureBlobStorage:ConnectionString")));
            services.AddScoped<IBlobStorageController, AzureBlobStorageController>();
            services.AddScoped<IJwtBearerAuthenticationManager, JwtBearerAuthenticationManager>(instance =>
                new JwtBearerAuthenticationManager(Configuration.GetValue<string>("JwtBearer:key")));

            //Adds validators 
            services.AddSingleton<IValidator<IngredientModel>, IngredientValidator>();
            services.AddSingleton<IValidator<RecipeModel>, RecipeValidator>();

            //Adds scoped services for the repositories, services and unit of work objects
            services.AddScoped<IRepository<RecipeModel>, RecipesRepository>();
            services.AddScoped<IRepository<IngredientModel>, IngredientsRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRecipesService, RecipesService>();
            services.AddScoped<IIngredientsService, IngredientsService>();
            services.AddScoped<IUsersService, UsersService>();

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter())); //Adds an Enum string convertor to json serialisation to ensure that enums are converted to strings and not integers

            //Authentication uses microsoft identity service
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetValue<string>("JwtBearer:key"))),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddCors(o => o.AddPolicy("Policy", builder =>
            {
                builder.WithOrigins("https://localhost:3000", "http://localhost:3000", "https://localhost:5001", "http://localhost:5001")
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowCredentials()
                              .SetIsOriginAllowed(host => true);
            }));


            //Adds swagger configuration for adding a bearer token to requests
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RecipeSchedulerApiService", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "Bearer Authentication",
                    Type = SecuritySchemeType.Http
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new List<string>()
                        }
                    });
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RecipeSchedulerApiService v1"));
            }

            app.UseHttpsRedirection();


            app.UseRouting();

            app.UseCors("Policy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

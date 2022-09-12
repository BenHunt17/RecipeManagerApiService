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
using RecipeManagerWebApi.Interfaces;
using RecipeManagerWebApi.Services;
using RecipeManagerWebApi.Repositories;
using Azure.Storage.Blobs;
using RecipeManagerWebApi.Validators;
using FluentValidation;
using RecipeManagerWebApi.Utilities;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RecipeManagerWebApi.Types.Models;
using RecipeManagerWebApi.Utilities.PropertyFilterInterpreter;
using RecipeManagerWebApi.Types.ModelFilter;

namespace RecipeManagerWebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

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
            services.AddScoped<IJwtBearerAuthenticationManager, JwtBearerAuthenticationManager>();
            services.AddScoped<IHashManager, HashManager>();

            //Adds query filter interpretor
            services.AddSingleton<IPropertyFilterInterpreter, PropertyFilterInterpreter>();

            //Adds validators 
            services.AddSingleton<IValidator<IngredientModel>, IngredientModelValidator>();
            services.AddSingleton<IValidator<RecipeModel>, RecipeValidator>();
            services.AddSingleton<IValidator<UserModel>, UserValidator>();

            //Adds scoped services for the repositories, services and unit of work objects
            services.AddScoped<IRepository<RecipeModel, RecipeModelFilter>, RecipesRepository>();
            services.AddScoped<IRepository<IngredientModel, IngredientModelFilter>, IngredientsRepository>();
            services.AddScoped<IRepository<UserModel, UserModelFilter>, UsersRepository>();
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
                x.RequireHttpsMetadata = false; //TODO - Set to true in prod?
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetValue<string>("JwtBearer:key"))), //The symmetric key used when the token was issued
                    ValidateIssuer = false, //Ensures that the issuer of the token is the same issuer who generated it. Set to false since the issuer was 
                    ValidateAudience = false //Ensures that the intended audience of the token is correct i.e. api is correct. These are both false since they aren't configured in the generation code
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RecipeManagerWebApi", Version = "v1" });

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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RecipeManagerWebApi v1"));
            }

            app.useWebApiExceptionHandler();

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

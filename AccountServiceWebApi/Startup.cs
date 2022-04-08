using ApplicationServices;
using ApplicationServices.Shared.BaseResponse;
using ApplicationServices.Shared.ErrorModel;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Persistence.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace AccountServiceWebApi
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }
        public IWebHostEnvironment WebHostEnvironment { get; set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationServices(Configuration).AddPersistence(Configuration);
            services.AddControllers();
            ConfigureIdentity(services, Configuration);
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AccountServiceApi", Version = "v1" });
            //});
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "AccountService Api",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
                c.EnableAnnotations();
                //c.SchemaFilter<EnumSchemaFilter>();
                //c.DocumentFilter<LowercaseDocumentFilter>();
            });

            //adding fluent validation 
            services.AddControllers(options =>
            {
            })
                   .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                   .AddJsonOptions(options =>
                   {
                       options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                   })
            .ConfigureApiBehaviorOptions(options =>
             {
                 options.InvalidModelStateResponseFactory = context =>
                 {
                     var errorsInModelState = context.ModelState
                                                     .Where(x => x.Value.Errors.Count > 0)
                                                     .ToDictionary(pair => pair.Key,
                                                         pair => pair.Value.Errors.Select(x => x.ErrorMessage))
                                                     .ToArray();

                     var errorResponse = new ErrorResponse();

                     //cycle through the errors and add to response
                     foreach (var (key, value) in errorsInModelState)
                     {
                         foreach (var subError in value)
                         {
                             errorResponse.Errors.Add(new Error
                             {
                                 Code = key,
                                 Message = subError
                             });
                         }
                     }
                     var error = Result.Fail(
                         errorResponse.Errors,
                         "Error occured",
                         StatusCodes.Status400BadRequest.ToString());

                     return new BadRequestObjectResult(error);
                 };
             }).AddFluentValidation(x => x.RegisterValidatorsFromAssembly(Assembly.Load("ApplicationServices")));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AccountServiceWebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // app.UseAuthorization();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

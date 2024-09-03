using Application.Models.DTOs.Customer;
using Application.Services;
using Application.Services.Services;
using Domain.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NorthWinds.Persistence.DBContexts;
using NorthWinds.Persistence.Repositories;
using System;
using System.Diagnostics;

namespace EntityFrameworkService
{
    public class Startup
    {

        const string AppName = "EntityFrameworkService";
        const string API_Version = "v1";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options => Configuration.Bind("JwtBearerOptions", options));


            //services.AddRequiredScopeAuthorization();
            //services.AddAuthorization(options =>
            //{
            //    var scopes = Configuration.GetSection("AuthorizationOptions:DefaultPolicy:RequiredScopes").Get<string[]>();
            //    options.DefaultPolicy = new AuthorizationPolicyBuilder()
            //        .RequireAuthenticatedUser()
            //        .RequireScope(scopes)
            //        .Build();
            //});


            services.AddControllers();


            services.AddHealthChecks()
                    .AddCheck<Infrastructure.HealthChecks.HealthCheck>("TemplateCheck", Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy)
                    .AddDbContextCheck<NorthWindsContext>("Northwinds databse Health check")
                    .AddDbContextCheck<NorthWindsReadOnlyContext>("Northwinds databse Health check - Read only account");

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = AppName, Version = API_Version });
                c.EnableAnnotations();                               
            });

            services.AddHttpClient()
                .AddHeaderPropagation();         

            services.AddDbContext<NorthWindsContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("NorthWindsContext"),
                sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));
            services.AddDbContext<NorthWindsReadOnlyContext>(options => options.UseSqlServer(Configuration.GetConnectionString("NorthWindsReadOnlyContext")));
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IReadOnlyOrdersRepository, ReadOnlyOrdersRepository>();


            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddProblemDetails();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            //app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", AppName + " " + API_Version));          

            app.UseRouting();

            //app.UseAuthentication();
            //app.UseAuthorization();

            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

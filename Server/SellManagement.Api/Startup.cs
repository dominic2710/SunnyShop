using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SellManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;
using SellManagement.Api.Helpers;
using SellManagement.Api.Services;
using SellManagement.Api.Functions;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Npgsql.EntityFrameworkCore.PostgreSQL.Design;

namespace SellManagement.Api
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

            services.AddControllers();
            services.AddDbContext<SellManagementContext>(options => {
                options.UseSqlServer(Configuration["ConnectionString"]);
                //options.UseNpgsql(Configuration["ConnectionString"]);
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SellManagement.Api", Version = "v1" });
                c.AddSecurityDefinition("api_key", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\""
                });
                //c.OperationFilter<AssignJwtSecurityRequirements>()
            });

            // configure strongly typed settings object
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductFunction, ProductFunction>();
            services.AddScoped<IClassifyNameFunction, ClassifyNameFunction>();
            services.AddScoped<ICustomerFunction, CustomerFunction>();
            services.AddScoped<ISupplierFunction, SupplierFunction>();
            services.AddScoped<IShippingCompanyFunction, ShippingCompanyFunction>();
            services.AddScoped<IPurchaseOrderFunction, PurchaseOrderFunction>();
            services.AddScoped<ISellOrderFunction, SellOrderFunction>();
            services.AddScoped<IVoucherNoManagementFunction, VoucherNoManagementFunction>();
            services.AddScoped<IOverviewFunction, OverviewFunction>();
            services.AddHttpContextAccessor();
            services.AddScoped<UserOperator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SellManagement.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

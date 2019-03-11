using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Model.Data;
using Swashbuckle.AspNetCore.Swagger;
using Serilog;
using PubService;
using PubService.Server;
using sk.core.Filters;
using Serilog.Filters;

namespace sk.core
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
            CoreContext.ConnnectString = Configuration.GetConnectionString("CoreConnectString");
            services.AddDbContext<CoreContext>(options => options.UseMySql(CoreContext.ConnnectString));

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            #region api文档配置
            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info { Title = "sk.core API", Version = "v1" });
                    c.CustomSchemaIds((type) => type.FullName);
                    //Set the comments path for the swagger json and ui.
                    var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                    var xmlPath = Path.Combine(basePath, "sk.core.xml");
                    c.IncludeXmlComments(xmlPath);
                    c.OperationFilter<AuthTokenHeaderParameter>(); //添加自定义头
                });
            #endregion

            #region 跨域配置
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin(); // For anyone access.
            corsBuilder.AllowCredentials();
            //corsBuilder.WithOrigins("http://localhost:56573"); // for a specific url. Don't add a forward slash on the end!

            services.AddCors(options =>
            {
                options.AddPolicy("SiteCorsPolicy", corsBuilder.Build());
            });
            #endregion

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            //扩展服务
            services.AddServiceExtensions();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(Configuration)
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(Matching.WithProperty("ModuleName")).WriteTo.MySQL())
                 .WriteTo.Logger(l => l.Filter.ByIncludingOnly(Matching.WithProperty("Command")).WriteTo.RollingFile(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "log/{Date}.log"),
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Command} {Level}] {Message}{NewLine}param=>{Parameter}{NewLine}{Exception}{NewLine}"))
               .CreateLogger();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("SiteCorsPolicy");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseStaticFiles();
            //api文档
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.ShowExtensions();
                c.RoutePrefix = "docs";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "sk.core v1.0");
                //c.EnableValidator();
                c.DocExpansion(DocExpansion.Full);
            });


            //设置公用appsetting
            ConfigurationUtil.SetConfiguration(Configuration);

        }
    }
}

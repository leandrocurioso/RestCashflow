using System;
using System.Globalization;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestCashflowLibrary;
using RestCashflowLibrary.Application.Middleware;
using RestCashflowLibrary.Infrastructure.Connection.MySql;
using RestCashflowLibrary.Infrastructure.Consumer;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using Swashbuckle.AspNetCore.Swagger;

namespace RestCashflowWebApi
{
    public class Startup
    {
        Container _container = new Container();
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        void IntegrateSimpleInjector(IServiceCollection services)
        {
            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IControllerActivator>(
                new SimpleInjectorControllerActivator(_container));
            services.AddSingleton<IViewComponentActivator>(
                new SimpleInjectorViewComponentActivator(_container));

            services.EnableSimpleInjectorCrossWiring(_container);
            services.UseSimpleInjectorAspNetRequestScoping(_container);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add service and create Policy with options
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            IntegrateSimpleInjector(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {
                    Version = "v1",
                    Title = "RestCashflow API",
                    Description = "Web API para fluxo de caixa usando RabbitMQ e MariaDB.",
                    Contact = new Contact
                    {
                        Name = "Leandro Curioso",
                        Email = "leandro.curioso@gmail.com",
                        Url = "https://github.com/leandrocurioso"
                    }
                });

                var webApiFilepath = Path.Combine(AppContext.BaseDirectory, "RestCashflowWebApi.xml");
                c.IncludeXmlComments(webApiFilepath);
            });

            services.AddDataProtection().SetApplicationName("RestCashflowWebApi");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var cultureInfo = new CultureInfo("pt-BR");
            cultureInfo.NumberFormat.CurrencySymbol = "R$";
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            InitializeContainer(app);
            CreateDatabaseStructure();
            RegisterQueueConsumer();

            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("CorsPolicy");
            // app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionMiddleware>(_container);
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestCashflow API");
            });
            app.UseMvc();
        }

        void RegisterQueueConsumer()
        {
            var paymentConsumer = _container.GetInstance<IPaymentConsumer>();
            paymentConsumer.Consume();

            var receiptConsumer = _container.GetInstance<IReceiptConsumer>();
            receiptConsumer.Consume();

            var conciliateConsumer = _container.GetInstance<IConciliateConsumer>();
            conciliateConsumer.Consume();
        }

        void CreateDatabaseStructure()
        {
            using (AsyncScopedLifestyle.BeginScope(_container))
            {
                var mySqlBuildStructure = _container.GetInstance<IMySqlBuildStructure>();
                mySqlBuildStructure.CreateTables().GetAwaiter().GetResult();
            }
        }

        void InitializeContainer(IApplicationBuilder app)
        {
            // Add application presentation components:
            _container.RegisterMvcControllers(app);
            // _container.RegisterMvcViewComponents(app);

            _container.RegisterInstance<IConfiguration>(Configuration);

            CompositionRoot.Register(_container);

            // Allow Simple Injector to resolve services from ASP.NET Core.
            _container.AutoCrossWireAspNetComponents(app);
        }
    }
}

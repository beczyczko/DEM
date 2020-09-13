using System.Reflection;
using Autofac;
using DEM.Common.Dispatchers;
using DEM.Common.MediatR;
using DEM.Common.Mongo;
using DEM.Common.Mvc;
using DEM.Engine;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DEM
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomMvc();

            services.AddOpenApiDocument(configure =>
            {
                configure.DocumentName = "v1";
                configure.Title = "Sticker Board API";
            });

            services.AddControllers();
            services.AddOptions();
        }

        [UsedImplicitly]
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly()).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(Dispatcher).Assembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(World).Assembly).AsImplementedInterfaces();

            builder.AddDispatchers();

            AddMediatR(builder);
            builder.AddMongo();
        }

        private void AddMediatR(ContainerBuilder builder)
        {
            builder.AddMediatR();

            // how to register mediatr handlers
            // https://github.com/jbogard/MediatR/tree/master/samples/MediatR.Examples.PublishStrategies
            // finally register our custom code (individually, or via assembly scanning)
            // - requests & handlers as transient, i.e. InstancePerDependency()
            // - pre/post-processors as scoped/per-request, i.e. InstancePerLifetimeScope()
            // - behaviors as transient, i.e. InstancePerDependency()
            // builder.RegisterAssemblyTypes(typeof(Startup).GetTypeInfo().Assembly).AsImplementedInterfaces(); // via assembly scan
            // builder.RegisterType<MyHandler>().AsImplementedInterfaces().InstancePerDependency();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(builder =>
                builder.SetIsOriginAllowed(host => true).AllowAnyHeader().AllowAnyMethod().AllowCredentials());

            app.UseHttpsRedirection();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}

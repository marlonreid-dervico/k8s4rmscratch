using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Result.Data;
using Result.Hubs;
using Result.Timers;

namespace Result
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
                
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(opts => opts.EnableEndpointRouting = false)
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
                });
            services.AddSignalR();
            services.AddTransient<IResultData, MySqlResultData>()
                    .AddSingleton<PublishResultsTimer>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(routes =>
            {
                routes.MapHub<ResultsHub>("/resultsHub");
            });

            app.UseMvc();

            var timer = app.ApplicationServices.GetRequiredService<PublishResultsTimer>();
            timer.Start();
        }
    }
}

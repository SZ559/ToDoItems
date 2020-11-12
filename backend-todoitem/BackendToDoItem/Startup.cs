using BackendForToDoItem.Config;
using BackendForToDoItem.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BackendForToDoItem
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
            services.AddControllers();
            //services.AddSingleton<ITodoItemService, ToDoItemService>();
            services.AddSingleton<ITodoItemService>(provider =>
            {
                var config = Configuration.GetSection("MongoDBConfig").Get<MongoDbConfig>();
                var repository = new ToDoItemService(config);
                return repository;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", p =>
                {
                    p.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });




            //services.AddSwaggerGen();
            services.AddSwaggerGen(c =>
            {
                var folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var xmlFiles = Directory.GetFiles(folder, "*.xml").ToList();
                xmlFiles.ForEach(x => c.IncludeXmlComments(x));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseRouting();
           
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "My API V1");
            });
        }
    }
}

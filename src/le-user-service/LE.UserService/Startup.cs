using AutoMapper;
using LE.ApiGateway.Extensions;
using LE.Library.LE.Consul;
using LE.UserService.AutoMappers;
using LE.UserService.Infrastructure.Infrastructure;
using LE.UserService.Neo4jData;
using LE.UserService.Services;
using LE.UserService.Services.Implements;
using LE.UserService.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Neo4j.Driver;
using Neo4jClient;
using System;

namespace LE.UserService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public readonly string DatabaseName = Environment.GetEnvironmentVariable("NEO4J_DATABASE") ?? "neo4j";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LE.UserService", Version = "v1" });
            });

            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            //add mapper
            AddAutoMappers(services);
            //add db context
            AddDbContext(services);
            //add auth
            services.AddCustomAuthorization(Configuration);
            //register neo4j
            RegisterNeo4jDatabase(services);
            //register DAL
            AddDAL(services);

            // configure DI for application services
            services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddTransient<IMailService, MailService>();
            services.AddScoped<ILangService, LangService>();
            services.AddScoped<IUserService, Services.Implements.UserService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddConsul();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LE.UserService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseConsul();

            //add middleware
            app.UseCustomAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AddAutoMappers(IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc => {
                mc.AddProfile(new UserProfile());
                mc.AddProfile(new LanguageProfile());
                mc.AddProfile(new PostProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        private void AddDbContext(IServiceCollection services)
        {
            services.AddDbContext<LanggeneralDbContext>(options =>options.UseNpgsql(Env.DB_CONNECTION_STRING));
        }

        private IServiceCollection RegisterNeo4jDatabase(IServiceCollection services)
        {
            services.AddSingleton(typeof(IDriver), resolver =>
            {
                var uri = Environment.GetEnvironmentVariable("NEO4J_HOST");
                var user = Environment.GetEnvironmentVariable("NEO4J_USER");
                var password = Environment.GetEnvironmentVariable("NEO4J_PASSWORD");

                var authToken = AuthTokens.Basic(user, password);
                var driver = GraphDatabase.Driver(uri, authToken);
                return driver;
            });

            services.AddSingleton(typeof(IBoltGraphClient), resolver =>
            {
                var client = new BoltGraphClient(services.BuildServiceProvider().GetService<IDriver>());

                if (!client.IsConnected)
                {
                    client.ConnectAsync().GetAwaiter().GetResult();
                }

                return client;
            });

            services.AddScoped<Neo4jContext>(sp =>
            {
                var driver = sp.GetRequiredService<IDriver>();
                var boltGraphClient = sp.GetRequiredService<IBoltGraphClient>();
                return new Neo4jContext(driver, boltGraphClient, DatabaseName);
            });

            return services;
        }

        private void AddDAL(IServiceCollection services)
        {
            //services.AddScoped<ICountryDAL, CountryDAL>();
        }


    }
}

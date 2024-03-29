using System;
using System.Reflection;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using EventBus.Messages.Events;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Basket.API
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
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<DiscountGrpcService>();

            services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(opt => opt.Address = new Uri(Configuration["GrpcSettings:DiscountUrl"]));

            services.AddStackExchangeRedisCache(opt =>
            {
                opt.Configuration = Configuration["CacheSettings:ConnectionString"];
            });

            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(Configuration["EventBusSettings:HostAddress"]);
                    cfg.Message<BasketCheckoutEvent>(x =>
                    {
                        x.SetEntityName(Configuration["EventBusSettings:Publish:ExchangeName"]);
                    });
                });
            });
            services.AddMassTransitHostedService();

            var currentAssembly = Assembly.GetExecutingAssembly();
            services.AddAutoMapper(currentAssembly);
            services.AddControllers()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(currentAssembly));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = "";
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1");
                });
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

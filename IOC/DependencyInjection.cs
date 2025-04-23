using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Infrastructure;
using AutoMapper;
using Domain.Repositories;

using Application.Services.Interfaces;
using Application.Services;
using Application.Mappings;
using FluentValidation;

namespace IOC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            // Configurar o DbContext
            services.AddDbContext<MyContext>(options =>
                options.UseNpgsql(connectionString));

            // Executar migrações automaticamente
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var dbContext = serviceProvider.GetRequiredService<MyContext>();
                dbContext.Database.Migrate();
            }

            services.AddAutoMapper(typeof(AutoMapperProfile));

           

            return services;
        }
    }
}

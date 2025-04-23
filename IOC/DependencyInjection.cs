using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Infrastructure;
using AutoMapper;
using Domain.Repositories;

using Application.Services.Interfaces;
using Application.Services;
using Application.Mappings;
using FluentValidation;
using Domain.Entities;
using Infrastructure.Repositories;
using Application.Validations;

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

            services.AddScoped<IBaseRepository<Livro>, LivroRepository>();
            services.AddScoped<IBaseRepository<Autor>, AutorRepository>();

            services.AddScoped<ILivroService, LivroService>();
            services.AddScoped<IAutorService, AutorService>();

            // Configurar FluentValidation
            services.AddTransient<FluentValidation.IValidator<Livro>, LivroValidator>();
            services.AddTransient<FluentValidation.IValidator<Autor>, AutorValidator>();
            


            return services;
        }
    }
}

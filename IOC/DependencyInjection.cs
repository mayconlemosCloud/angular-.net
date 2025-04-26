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
          
            services.AddDbContext<MyContext>(options =>
                options.UseNpgsql(connectionString));

     
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var dbContext = serviceProvider.GetRequiredService<MyContext>();
                dbContext.Database.Migrate();
            }

            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddScoped<IBaseRepository<Livro>, LivroRepository>();
            services.AddScoped<IBaseRepository<Autor>, AutorRepository>();
            services.AddScoped<IBaseRepository<Assunto>, AssuntoRepository>();
            services.AddScoped<IBaseRepository<BookTransaction>, BookTransactionRepository>();

            services.AddScoped<ILivroService, LivroService>();
            services.AddScoped<IAutorService, AutorService>();
            services.AddScoped<IAssuntoService, AssuntoService>();
            

         
            services.AddTransient<FluentValidation.IValidator<Livro>, LivroValidator>();
            services.AddTransient<FluentValidation.IValidator<Autor>, AutorValidator>();
            services.AddTransient<FluentValidation.IValidator<Assunto>, AssuntoValidator>();
            services.AddTransient<FluentValidation.IValidator<BookTransaction>, BookTransactionValidator>();


            return services;
        }
    }
}

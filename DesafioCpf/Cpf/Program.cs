using Application.UseCases.AddCpf;
using Cpf.Models.AddCpf;
using Domain.Contracts.Repositories.AddCpf;
using Domain.Contracts.UseCases.AddCpf;
using FluentValidation;
using Infra.Repository.DbContext;
using Infra.Repository.Repositories.AddCpf;
using System.Globalization;

namespace Cpf
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton<IDbContext, DbContext>();
            builder.Services.AddScoped<ICpfRepository, CpfRepository>();
            builder.Services.AddScoped<ICpfUseCase, AddCpfUseCase>();
            builder.Services.AddTransient<IValidator<AddCpfInput>, AddCpfInputValidator>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            var cultureInfo = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;


            app.Run();
        }
    }
}
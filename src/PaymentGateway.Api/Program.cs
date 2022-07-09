namespace PaymentGateway.Api
{
    using MediatR;

    using Microsoft.EntityFrameworkCore;

    using PaymentGateway.Application.Common.Interfaces;
    using PaymentGateway.Domain.Services;
    using PaymentGateway.Infrastructure.Data;
    using PaymentGateway.Infrastructure.Gateways;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("PaymentsGateway")));
            builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
            builder.Services.AddScoped<IAcquiringBankGateway, MockedAcquiringBankGateway>();

            builder.Services.AddSingleton<ICardNumberMaskingService, CardNumberMaskingService>();

            builder.Services.AddMediatR(typeof(PaymentGateway.Application.Payments.Commands.Create.CreatePaymentCommand).Assembly);


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

            app.Run();
        }
    }
}
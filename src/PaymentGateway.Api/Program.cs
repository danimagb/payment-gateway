namespace PaymentGateway.Api
{
    using Hellang.Middleware.ProblemDetails;

    using MediatR;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.OpenApi.Models;

    using PaymentGateway.Api.Handlers.ApiKeyAuthentication;
    using PaymentGateway.Api.Settings;
    using PaymentGateway.Application.Common.Exceptions;
    using PaymentGateway.Application.Common.Interfaces;
    using PaymentGateway.Domain.Services;
    using PaymentGateway.Infrastructure.Data;
    using PaymentGateway.Infrastructure.Gateways;

    using Serilog;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateBootstrapLogger();

            builder.Host.UseSerilog((ctx, lc) => lc
               .WriteTo.Console()
               .ReadFrom.Configuration(ctx.Configuration)
            );

            Log.Information("Payment Gateway Api starting up...");


            // Add services to the container.

            builder.Services.AddMemoryCache();
            builder.Services.AddOptions();
            builder.Services.Configure<ApiKeysSettings>(options => builder.Configuration.GetSection("ApiKeys").Bind(options));

            builder.Services
                .AddScoped<IApiKeyCacheService, ApiKeyCacheService>()
                .AddScoped<IClientService, InMemoryClientsService>()
                .AddScoped<ApiKeyAuthenticationHandler>();

            builder.Services.AddAuthentication(ApiKeyAuthenticationOptions.DefaultScheme)
                .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationOptions.DefaultScheme, cfg => { });
            
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(setup =>
            {
                setup.AddSecurityDefinition(ApiKeyAuthenticationOptions.DefaultScheme, new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = ApiKeyAuthenticationOptions.HeaderName,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = ApiKeyAuthenticationOptions.DefaultScheme
                });

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = ApiKeyAuthenticationOptions.DefaultScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });       

            builder.Services.AddProblemDetails(setup =>
            {
                setup.IncludeExceptionDetails = (ctx, env) => builder.Environment.IsDevelopment() || builder.Environment.IsStaging();
                setup.Map<ValidationException>(exception => new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = exception.Message,
                    Status = StatusCodes.Status400BadRequest,
                    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                });

                setup.Map<DuplicatedPaymentException>(exception => new ProblemDetails
                {
                    Title = "Conflict",
                    Detail = exception.Message,
                    Status = StatusCodes.Status409Conflict,
                    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
                });
            });


            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("PaymentsGateway")));
            builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            builder.Services.AddScoped<IAcquiringBankGateway, MockedAcquiringBankGateway>();

            builder.Services.AddSingleton<ICardNumberMaskingService, CardNumberMaskingService>();

            builder.Services.AddMediatR(typeof(PaymentGateway.Application.Payments.Commands.Create.CreatePaymentCommand).Assembly);

            

            var app = builder.Build();

            app.UseSerilogRequestLogging();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseProblemDetails();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();



            app.MapControllers();

            app.Run();
        }
    }
}
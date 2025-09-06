using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Qwik.Business;
using Qwik.WebAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Autofac for DI
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterType<AppDbContext>().AsSelf().InstancePerLifetimeScope();
    containerBuilder.RegisterType<AppointmentRepository>().As<IAppointmentRepository>().InstancePerDependency();
    containerBuilder.RegisterType<AppointmentSettingsRepository>().As<IAppointmentSettingsRepository>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<AppointmentService>().As<IAppointmentService>().InstancePerDependency();
    containerBuilder.RegisterType<AppointmentSettingsService>().As<IAppointmentSettingsService>().InstancePerDependency();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandler>();
app.UseAuthorization();
app.MapControllers();
app.Run();
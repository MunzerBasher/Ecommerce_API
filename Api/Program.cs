using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependecies(builder.Configuration);

builder.Host.UseSerilog((context, services, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var stripeSettings = builder.Configuration.GetSection("Stripe");
Stripe.StripeConfiguration.ApiKey = stripeSettings["SecretKey"];

var app = builder.Build();


app.UseHangfireDashboard("/Jobs", new DashboardOptions
{
    Authorization =
    [
        new HangfireCustomBasicAuthenticationFilter
        {
            Pass = app.Configuration.GetValue<string>("HangfireSettings:Username"),
            User = app.Configuration.GetValue<string>("HangfireSettings:Password")
        }

    ],
    DarkModeEnabled = true  
    
});
app.UseSerilogRequestLogging();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
var serviceScope = scopeFactory.CreateScope();
var NotificationsServices = serviceScope.ServiceProvider.GetRequiredService<INotificationsServices>();
RecurringJob.AddOrUpdate("Products Notifications", () => NotificationsServices.ProductNotifications(), "0 0 * * *");

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapHealthChecks("/heath", new HealthCheckOptions
{
    ResponseWriter  = UIResponseWriter.WriteHealthCheckUIResponse,
});

app.UseExceptionHandler();

app.MapControllers();

app.Run();
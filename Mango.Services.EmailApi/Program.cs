using Mango.Services.EmailApi.Extensions;
using Mango.Services.EmailApi.Services;
using Mango.Services.EmailApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlite(builder.Configuration.GetConnectionString("MangoContext"));
});


var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionBuilder.UseSqlite(builder.Configuration.GetConnectionString("MangoContext"));

builder.Services.AddSingleton(new EmailService(optionBuilder.Options));

builder.Services.AddHostedService<RabbitMQAuthConsumerService>();
//builder.Services.AddSingleton<IAzureBusConsumerService, AzureBusConsumerService>();
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
ApplyMigration();
app.UseAzureServiceBusConsumer();
app.Run();


void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}
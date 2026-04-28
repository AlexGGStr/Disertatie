using EmailService.Consumers;
using EmailService.Models;
using EmailService.Services.SendEmailService;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// services
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<ISendEmailService, SendEmailService>();

// Load KafkaSettings from config
builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));

var kafkaSettings = builder.Configuration.GetSection("Kafka").Get<KafkaSettings>();
builder.Services.AddSingleton(kafkaSettings);

//Consumers 
builder.Services.AddHostedService<UserManagementConsumer>();
builder.Services.AddHostedService<PropertyConsumer>();
builder.Services.AddHostedService<BookingConsumer>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
using Mango.Services.Email.DbContexts;
using Mango.Services.Email.Extension;
using Mango.Services.Email.Messaging;
using Mango.Services.Email.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Postgres config
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("default"));
});

// App dependencies
builder.Services.AddScoped<IEmailRepository, EmailRepository>();

var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
optionBuilder.UseNpgsql(builder.Configuration.GetConnectionString("default"));
builder.Services.AddSingleton(new EmailRepository(optionBuilder.Options));

builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();

// For dotnet6 breaking changes with postgres read: https://github.com/npgsql/doc/blob/main/conceptual/Npgsql/types/datetime.md/
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

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

app.UseAzureServiceBusConsumer(app.Services);

app.Run();


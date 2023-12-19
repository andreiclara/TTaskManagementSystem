using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using TaskManagementSystem.Data;
using TaskManagementSystem.Services;
using TaskManagementSystem.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "web server api", Version = "v1" });
    c.SchemaFilter<EnumSchema>();
});
builder.Services.AddHttpClient();
builder.Services.AddDbContext<TaskDbContext>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<StatusCheckService>();
builder.Services.AddSingleton<DailyStatusCheckService>();
builder.Services.AddHostedService(
    provider => provider.GetRequiredService<DailyStatusCheckService>());
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseDeveloperExceptionPage();

app.Run();

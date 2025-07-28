using Application.Requests;
using Application.Services;
using Application.Validators;
using Domain.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddFluentValidation(); ;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IValidator<CriarSeguroRequest>, CriarSeguroRequestValidator>();
builder.Services.AddTransient<IValidator<VeiculoRequest>, VeiculoRequestValidator>(); 
builder.Services.AddTransient<IValidator<SeguradoRequest>, SeguradoRequestValidator>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddDbContext<SeguroDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ISeguroRepository, SeguroRepository>();
builder.Services.AddScoped<IVeiculoRepository, VeiculoRepository>(); 
builder.Services.AddScoped<ISeguradoRepository, SeguradoRepository>(); 


builder.Services.AddScoped<SeguroApplicationService>();
builder.Services.AddScoped<VeiculoApplicationService>(); 
builder.Services.AddScoped<SeguradoApplicationService>(); 

builder.Services.AddHttpClient<ISeguradoService, SeguradoApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["SeguradoApi:BaseUrl"] ?? "http://localhost:3000"); 
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SeguroDbContext>();
    db.Database.EnsureCreated(); 
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<SeguroDbContext>();
        dbContext.Database.Migrate(); 
    }
}

app.UseCors();

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();

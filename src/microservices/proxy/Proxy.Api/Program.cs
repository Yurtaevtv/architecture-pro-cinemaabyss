using Proxy_Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("Yarp"))
    .AddConfigFilter<YarpEnvironmentVariablesConfigFilter>();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapReverseProxy();

app.Run();

using Proxy_Api.Configuration;
using Yarp.ReverseProxy.LoadBalancing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("Yarp"))
    .AddConfigFilter<YarpEnvironmentVariablesConfigFilter>();

builder.Services.AddSingleton<ILoadBalancingPolicy, CancaryLoadBalancingPolicy>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapReverseProxy();

app.Run();

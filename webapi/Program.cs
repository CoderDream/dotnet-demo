var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

// Add services to the container.

// **添加这行：用于 API 探测器，帮助 Swagger 发现 Minimal API 端点**
builder.Services.AddEndpointsApiExplorer();

// **添加这行：注册 Swagger 生成器服务**
// 你可以保留或删除 AddOpenApi()，AddSwaggerGen() 是 Swashbuckle 的核心配置方法
// builder.Services.AddOpenApi(); // 这行可以删除或保留，AddSwaggerGen 包含其功能
// builder.Services.AddSwaggerGen(); // 如果需要定制，可以在这里配置，例如：
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "我的天气预报 API",
        Description = "一个用于获取天气预报的 ASP.NET Core Minimal API 示例"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // **添加这行：启用 Swagger 中间件，用于生成 Swagger JSON 端点**
    app.UseSwagger();

    // **添加这行：启用 Swagger UI 中间件**
    app.UseSwaggerUI(); // 默认访问路径是 /swagger

    // 你原有的 MapOpenApi() 可以删除，因为 UseSwagger() 和 UseSwaggerUI() 提供了更完整的功能
    // app.MapOpenApi();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

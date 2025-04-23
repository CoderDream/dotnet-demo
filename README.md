## dotnet-demo

VS Code Run C# Solution - 用 VS Code 创建解决方案，新增项目并打包、编译项目（含Swagger）



### 查看DOTNET版本

> D:\XXX\dotnetdemo>dotnet --version
>
> 9.0.203

### 创建解决方案

> dotnet new sln --name MySolution

![img](README/1745381184113-4d2f5d8d-2ee8-4408-a050-0c9b48192d49.png)

### 新建模块

> dotnet new webapi -o webapi --no-https 

 ![img](README/1745399596053-0a120dc9-90cd-4f3a-aebf-9708873ac421.png)

### 绑定项目的解决方案中

> D:\XXX\dotnetdemo>dotnet sln add ./webapi/webapi.csproj 
>
> 已将项目“webapi\webapi.csproj”添加到解决方案中。

![img](README/1745399357198-e76e57f7-548a-467c-be1b-b15ece552508.png)

项目信息绑定成功：

![img](README/1745399424879-2b7cfca7-2a64-40e7-b00e-e9d8ee4d45d4.png)

### 三个命令

> 创建解决方案
> dotnet new sln --name MySolution
>
> 创建项目
> dotnet new webapi -o webapi --no-https 
>
> 绑定项目到解决方案
> dotnet sln add ./webapi/webapi.csproj 

 ![img](README/1745399546991-df5a5d7d-76db-4629-a667-bd2c0c603dd6.png)

### 编译解决方案

> dotnet build

 ![img](README/1745399710150-5ff4657d-c0a6-4e23-8e39-0b807bc54f86.png)

### 启动项目（带project参数）

> D:\XXX\dotnetdemo>dotnet run
>
> 找不到要运行的项目。请确保 D:\03_Dev\dotnetdemo\api 中存在项目，或使用 --project 传递项目路径。      
>
> 
>
> D:\XXX\dotnetdemo>dotnet run --project ./webapi/webapi.csproj
>
> 从 .\webapi\Properties\launchSettings.json 使用启动设置...
>
> 正在生成...
>
> info: Microsoft.Hosting.Lifetime[14]
>
> ​      Now listening on: http://localhost:5276
>
> info: Microsoft.Hosting.Lifetime[0]
>
> ​      Application started. Press Ctrl+C to shut down.
>
> info: Microsoft.Hosting.Lifetime[0]
>
> ​      Hosting environment: Development
>
> info: Microsoft.Hosting.Lifetime[0]
>
> ​      Content root path: D:\03_Dev\dotnetdemo\api\webapi

![img](README/1745399805418-dcdb3aed-5619-4337-beb4-e2f67ccf59e4.png)

### 增加Swagger

#### 增加包

> dotnet add package Swashbuckle.AspNetCore

> D:\XXX\webapi>dotnet add package Swashbuckle.AspNetCore

#### 修改文件

修改：Program.cs

```csharp
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
```

### 编译运行

> D:\XXX\dotnetdemo>dotnet build
>
> 还原完成(0.8)
>
>   webapi 已成功 (4.1 秒) → webapi\bin\Debug\net9.0\webapi.dll
>
> 
>
> 在 5.3 秒内生成 已成功
>
> 
>
> D:\XXX\dotnetdemo>dotnet run --project ./webapi/webapi.csproj
>
> 从 .\webapi\Properties\launchSettings.json 使用启动设置...
>
> 正在生成...
>
> info: Microsoft.Hosting.Lifetime[14]
>
> ​      Now listening on: http://localhost:5276
>
> info: Microsoft.Hosting.Lifetime[0]
>
> ​      Application started. Press Ctrl+C to shut down.
>
> info: Microsoft.Hosting.Lifetime[0]
>
> ​      Hosting environment: Development
>
> info: Microsoft.Hosting.Lifetime[0]
>
> ​      Content root path: D:\03_Dev\dotnetdemo\api\webapi

### 验证效果

http://localhost:5276/swagger/index.html

![img](README/1745400639743-a872618e-e4b6-46e5-bfe2-90f4213d92ca.png)

### 参考视频

[宇宙级VS Code零基础教程 全网最全最细的VS Code，开发前端、C#(.NET Core/.NET6/Vue)S0044_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV1DUoKYXEUr)

using System.Reflection;
using KaspelTestTask.API.Middlewares;
using KaspelTestTask.Application.Common.Mappings;
using KaspelTestTask.Application.Interfaces;
using KaspelTestTask.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    config.AddProfile(new AssemblyMappingProfile(typeof(IKaspelTestTaskDbContext).Assembly));
});
//Добавление swagger с созданием xml файла (необходимо отредактировать настройки проекта, возволяет прописывать summarys)
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});



builder.Services.AddControllersWithViews();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        //Call initialize DB
        var context = serviceProvider.GetRequiredService<KaspelTestTaskDbContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine("An error occurred while app initialization, Exception: " + ex.Message);
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(config => {
        config.RoutePrefix = string.Empty;
        config.SwaggerEndpoint("swagger/v1/swagger.json", "Kaspel Test Task API");
    });   
}

app.MapGet("api/v1/HeartBeatTest", () => "Service Is Working!");

app.UseRouting();
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseEndpoints(endpoints => endpoints.MapControllers());
app.Run();


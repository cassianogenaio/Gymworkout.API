using GymWorkout.API.Configuration;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiConfig();
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors("DevCors");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    var spaDistPath = Path.Combine(app.Environment.ContentRootPath, "FrontEnd", "dist");
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(spaDistPath),
        RequestPath = ""
    });

    app.MapFallback(async context =>
    {
        if (context.Request.Method == "GET" && !Path.HasExtension(context.Request.Path.Value ?? string.Empty))
        {
            await context.Response.SendFileAsync(Path.Combine(spaDistPath, "index.html"));
        }
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
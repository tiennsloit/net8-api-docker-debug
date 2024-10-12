using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add CORS services and configure policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://35.208.237.59","http://35.208.95.118") // Change to your specific origin
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

builder.Services.AddControllers();

var app = builder.Build();
app.UseMiddleware<IpRestrictionMiddleware>();

// Use CORS policy
app.UseCors("AllowSpecificOrigin");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();

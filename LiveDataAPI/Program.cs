using LiveDataAPI;
using LiveDataAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSignalR();
var app = builder.Build();

//use pipline
app.UseRouting();
app.UseCors(builder =>
{
    builder
          .WithOrigins("http://localhost:4200", "https://localhost:4200")
          .SetIsOriginAllowedToAllowWildcardSubdomains()
          .AllowAnyHeader()
          .AllowCredentials()
          .WithMethods("GET", "PUT", "POST", "DELETE", "OPTIONS")
          .SetPreflightMaxAge(TimeSpan.FromSeconds(3600));

}
);
app.UseAuthorization();


app.MapControllers();


app.MapHub<BroadcastHub>("/broadcastHub");


app.Run();

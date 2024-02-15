using Microsoft.EntityFrameworkCore;

using VideoSharingPlatform.Application;
using VideoSharingPlatform.Persistent.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(opt => {
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection"));
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Lib).Assembly));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();

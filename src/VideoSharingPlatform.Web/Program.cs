using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VideoSharingPlatform.Application;
using VideoSharingPlatform.Core.Entities.AppUserAggregate;
using VideoSharingPlatform.Persistent.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(opt => {
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(AssemblyMarker).Assembly));

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

using Booking.Areas.BackOffice.Data.Interface;
using Booking.Areas.BackOffice.Data.Services;
using Booking.Areas.FrontOffice.Data.Interface;
using Booking.Areas.FrontOffice.Data.Services;
using Booking.DBEngine;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register Dependency Injection
builder.Services.AddTransient<IDBHandler, DBHandler>();
builder.Services.AddTransient<IRoomsRepository, RoomsRepository>();
builder.Services.AddTransient<IBookMyRoomRepository, BookMyRoomRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
            pattern: "{area:exists}/{controller=BookMyRoom}/{action=Homepage}/{id?}");

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();

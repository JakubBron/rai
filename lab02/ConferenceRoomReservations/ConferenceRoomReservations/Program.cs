using ConferenceRoomReservations.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = $"/Account/Login";      // Redirect if unauthenticated
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.Redirect("/Home/Index?authError=" + AppKeywords.AuthError_LoginRequired);
            return Task.CompletedTask;
        };
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/Denied";   // Redirect if unauthorized
    });

builder.Services.AddSingleton<IConferenceRepository, ConferenceRepository>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();



var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();



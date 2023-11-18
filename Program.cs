using JustShortIt.Components;
using JustShortIt.Model;
using JustShortIt.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables("JSI_");
builder.Services.AddRazorPages();

builder.Services.AddAntiforgery();
builder.Services.AddRazorComponents();

// Get Configurations
var redisConnection = builder.Configuration.GetSection("Redis").Get<RedisConnection>();
var user = builder.Configuration.GetSection("Account").Get<User>();
string? baseUrl = builder.Configuration.GetValue<string>("BaseUrl");

#if DEBUG
baseUrl = "http://localhost/";
user = new User("test", "test");
Console.Error.WriteLine("YOU ARE RUNNING A DEBUG BUILD WITH TEST CREDENTIALS, " +
                        "DO NOT UNDER ANY CIRCUMSTANCES RUN THIS IN PRODUCTION, YOU HAVE BEEN WARNED.");
#endif

// Check if everything is configured (right)
if (string.IsNullOrEmpty(baseUrl) || Uri.IsWellFormedUriString(baseUrl, UriKind.Absolute) is false)
	throw new ApplicationException(
		"Base-URL is not set to a correct URL, please provide JSI_BaseUrl with a valid url.");
if (user is null || string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password)) 
    throw new ApplicationException(
        "Credentials not set, please provide JSI_Account__Username and JSI_Account__Password.");

// Set up Distributed Cache
if (string.IsNullOrEmpty(redisConnection?.ConnectionString) is false) {
    builder.Services.AddStackExchangeRedisCache(options => {
        options.Configuration = redisConnection.ConnectionString;
        options.InstanceName = redisConnection.InstanceName;
    });
    Console.WriteLine("Running with Redis distributed Cache.");
} else {
    builder.Services.AddDistributedMemoryCache();
    Console.WriteLine("Running with in-memory distributed Cache.");
}

// Add Authentication
builder.Services.AddScoped(_ => new AuthenticationService(user));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => {
    options.ExpireTimeSpan = TimeSpan.FromHours(24);
    options.SlidingExpiration = true;
    options.AccessDeniedPath = "/Login";
    options.LoginPath = "/Login";
    options.LogoutPath = "/Logout";
});
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure Cookies (used in Authentication)
app.UseCookiePolicy(new CookiePolicyOptions {
    MinimumSameSitePolicy = SameSiteMode.Strict,
    Secure = CookieSecurePolicy.Always
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/error", createScopeForErrors: true);
}


app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseAntiforgery();
app.MapRazorPages();
app.MapRazorComponents<App>();
app.Run();
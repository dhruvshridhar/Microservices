using Mango.Web;
using Mango.Web.Services;
using Mango.Web.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<ICartService, CartService>();

StaticDetails.ProductAPIBase = builder.Configuration["ServiceUrls:ProductAPI"];
StaticDetails.ShoppingCartAPIBase = builder.Configuration["ServiceUrls:ShoppingCartAPI"];
StaticDetails.CouponAPIBase = builder.Configuration["ServiceUrls:CouponApi"];

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService,CartService>();
builder.Services.AddScoped<ICouponService, CouponService>();

//Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
}).AddCookie("Cookies",c=>c.ExpireTimeSpan=TimeSpan.FromMinutes(10))
.AddOpenIdConnect("oidc", optionss =>
{
    optionss.Authority = builder.Configuration["ServiceUrls:IdentityAPI"];
    optionss.GetClaimsFromUserInfoEndpoint = true;
    optionss.ClientId = "mango";
    optionss.ClientSecret = "secret";
    optionss.ResponseType = "code";
    optionss.TokenValidationParameters.NameClaimType = "name";
    optionss.TokenValidationParameters.RoleClaimType = "role";
    optionss.Scope.Add("mango");
    optionss.SaveTokens = true;
});

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


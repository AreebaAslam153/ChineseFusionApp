using Google.Apis.Auth.OAuth2;
using FirebaseAdmin;
using FirebaseAdmin.Auth;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession();

// ✅ Add Firebase Admin initialization (only ONCE)
var serviceAccountPath = Path.Combine(Directory.GetCurrentDirectory(), "serviceAccount.json");

// Create the Firebase App only if it doesn’t already exist
if (FirebaseApp.DefaultInstance == null)
{
    FirebaseApp.Create(new AppOptions()
    {
        Credential = GoogleCredential.FromFile(serviceAccountPath)
    });
}

// ✅ Register FirebaseAuth as a service
builder.Services.AddSingleton(FirebaseAuth.DefaultInstance);

// Add MVC support
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware setup
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Default MVC route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

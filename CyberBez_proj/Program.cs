using CyberBez_proj.Data;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

public class Program {
    public static async Task Main(string[] args)
    {


        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddControllersWithViews();
        IWebHostEnvironment _env = builder.Environment;

        builder.Services.AddDNTCaptcha(options =>
        {
            options.UseCookieStorageProvider(SameSiteMode.Strict)
            .AbsoluteExpiration(minutes: 7)
            .ShowThousandsSeparators(false)
            .WithEncryptionKey("This is my secure key!")
            .InputNames(
                new DNTCaptchaComponent
                {
                    CaptchaHiddenInputName = "DNT_CaptchaText",
                    CaptchaHiddenTokenName = "DNT_CaptchaToken",
                    CaptchaInputName = "DNT_CaptchaInputText"
                })
            .Identifier("dnt_Captcha")
            ;
        });



        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 12;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireDigit = true;

            //Blokowanie u¿ytkowników na 15 min po wpisaniu Ÿle has³a 5 razy
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        });

        //builder.Services
        //.AddDNTCaptcha(option => option.UseCookieStorageProvider()
        //.ShowThousandsSeparators(false)
        //.WithEncryptionKey("123456"));

      


        // Other services and configurations

        builder.Services.Configure<PasswordHasherOptions>(option =>
        {
            option.IterationCount = 12000;
        });

        builder.Services.ConfigureApplicationCookie(options => {
            options.Cookie.Name = "AspNetCore.Identity.Application";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
            options.SlidingExpiration = true;
        });

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
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
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new[] { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new string[] { "Administrator", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string mailAdmin = "admin@admin.admin";
            string password = "a!A123456789";
            string mailUser = "user@user.com";

            if (await userManager.FindByEmailAsync(mailAdmin) == null)
            {
                var user = new IdentityUser
                {
                    UserName = mailAdmin,
                    Email = mailAdmin,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, "Admin");
            }

            if (await userManager.FindByEmailAsync(mailUser) == null)
            {
                var user = new IdentityUser
                {
                    UserName = mailUser,
                    Email = mailUser,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, "User");
            }

            for (int i = 0; i < 5; i++)
            {
                var user = new IdentityUser
                {
                    UserName = $"user{i}@user.com",
                    Email = $"user{i}@user.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, "User");
            }

        }

        app.Run();
    }
}
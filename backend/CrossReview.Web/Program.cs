using CrossReview;
using Crossreview.Infrastructure.Identity;
using CrossReview.Infrastructure.Postgres;
using CrossReview.Infrastructure.Postgres.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls(Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://0.0.0.0:5171");

var configuration = builder.Configuration;

builder.Services.AddProgramDependencies(configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000", "https://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ОДИН scope для всех операций
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
    // 1. Миграции
    var context = services.GetRequiredService<CrossReviewDbContext>();
    await context.Database.MigrateAsync();
    
    // 2. Создаем пользователей
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    await IdentitySeeder.SeedAsync(userManager, roleManager);
    
    // ✅ ПРИНУДИТЕЛЬНО сохраняем все изменения в БД
    await context.SaveChangesAsync();
    
    // 3. Теперь создаем проекты (пользователи физически есть в БД)
    var seeder = services.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAsync(services);
}

await app.RunAsync();
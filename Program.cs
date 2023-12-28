using AutoMapper;
using Cqrs.Hosts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProvaPortal.Controllers;
using ProvaPortal.Data;
using ProvaPortal.Data.Map;
using ProvaPortal.Filters;
using ProvaPortal.Repository;
using ProvaPortal.Repository.Interface;
using ProvaPortal.SessaoUsuario;

namespace ProvaPortal
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ProvaPortalContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
            });
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<IProvaRepository, ProvaRepository>();
            builder.Services.AddScoped<ISessao, Sessao>();
            builder.Services.AddScoped<IProfessorRepository, ProfessorRepository>();
            builder.Services.AddScoped<ProfessorRepository>();
            builder.Services.AddScoped<PaginaSomenteAdmin>();
            builder.Services.AddScoped<IEmail, Email>();
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            builder.Services.AddHttpClient();

            builder.Services.AddSession(o =>
            {
                o.Cookie.HttpOnly = true;
                o.Cookie.IsEssential = true;
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

            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
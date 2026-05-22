using MenuiserieApp.Core.Interfaces;
using MenuiserieApp.Infrastructure.Database;
using MenuiserieApp.Infrastructure.Repositories;
using MenuiserieApp.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace MenuiserieApp.UI
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            services.AddDbContext<AppDbContext>();

            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<ICommandeRepository, CommandeRepository>();

            services.AddTransient<ClientViewModel>();
            services.AddTransient<CommandeViewModel>();
            services.AddTransient<HistoriqueCommandeViewModel>();
            services.AddTransient<TableauBordViewModel>();
            services.AddSingleton<MainViewModel>();

            services.AddSingleton<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var scope = ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                dbContext.Database.EnsureCreated();

                MenuiserieApp.Infrastructure.Data.DataSeeder.InitialiserDonnees(dbContext);
            }

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = ServiceProvider.GetRequiredService<MainViewModel>();
            mainWindow.Show();
        }
    }
}
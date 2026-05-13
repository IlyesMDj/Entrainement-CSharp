using Exercice3.Models;
using Exercice3.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.IO;
using System.Windows;

namespace Exercice3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App ()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("logs/erp_log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            
            Log.Information("=== Démarrage de l'application ===");

            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        public static IServiceProvider ServiceProvider { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            var services = new ServiceCollection();

            services.AddLogging();

            services.AddAutoMapper(config =>
            {
                config.AddProfile<Exercice3.Mapping.ClientProfile>();
            });
            services.AddDbContext<ErptestContext>(options => options.UseSqlite("Data Source=mon_erp.db"));
            services.AddScoped<IProduitService, ProduitService>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<ClientsViewModel>();
            services.AddTransient<ProduitsViewModel>();

            ServiceProvider = services.BuildServiceProvider();

            using (var scope = ServiceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ErptestContext>();
                db.Database.EnsureCreated();

                db.Database.ExecuteSqlRaw(@" 
                    CREATE VIRTUAL TABLE IF NOT EXISTS ProduitsIndex USING fts5(Nom, content='Produits', content_rowid='Id');
        
                    -- Déclencheur : Ajout
                    CREATE TRIGGER IF NOT EXISTS Produits_Insert AFTER INSERT ON Produits BEGIN
                        INSERT INTO ProduitsIndex(rowid, Nom) VALUES (new.Id, new.Nom);
                    END;
        
                    -- Déclencheur : Suppression
                    CREATE TRIGGER IF NOT EXISTS Produits_Delete AFTER DELETE ON Produits BEGIN
                        INSERT INTO ProduitsIndex(ProduitsIndex, rowid, Nom) VALUES('delete', old.Id, old.Nom);
                    END;
        
                    -- Déclencheur : Modification
                    CREATE TRIGGER IF NOT EXISTS Produits_Update AFTER UPDATE ON Produits BEGIN
                        INSERT INTO ProduitsIndex(ProduitsIndex, rowid, Nom) VALUES('delete', old.Id, old.Nom);
                        INSERT INTO ProduitsIndex(rowid, Nom) VALUES (new.Id, new.Nom);
                    END;
                ");
            }

            var mainWindow = new MainWindow();

            mainWindow.DataContext = ServiceProvider.GetRequiredService<MainViewModel>();

            mainWindow.Show();

            base.OnStartup(e);

        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {

            Log.Fatal(e.Exception, "Un crash inattendu s'est produit !");
           
            MessageBox.Show(
                "Oups ! Une action inattendue s'est produite, mais l'application peut continuer à fonctionner.",
                "Avertissement de sécurité",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            e.Handled = true;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.Information("=== Arrêt normal de l'application ===");
            Log.CloseAndFlush();
            base.OnExit(e);
        }
    }

}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace MenuiserieApp.UI.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;

        [ObservableProperty]
        private ObservableObject? currentViewModel;

        public MainViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            AfficherTableauBord();
        }

        [RelayCommand]
        private void AfficherEcranClients()
        {
            CurrentViewModel = _serviceProvider.GetRequiredService<ClientViewModel>();
        }

        [RelayCommand]
        private void AfficherEcranCommande()
        {
            CurrentViewModel = _serviceProvider.GetRequiredService<CommandeViewModel>();
        }

        [RelayCommand]
        private void AfficherEcranHistorique()
        {
            CurrentViewModel = _serviceProvider.GetRequiredService<HistoriqueCommandeViewModel>();
        }

        [RelayCommand]
        private void AfficherTableauBord()
        {
            CurrentViewModel = _serviceProvider.GetRequiredService<TableauBordViewModel>();
        }
    }
}

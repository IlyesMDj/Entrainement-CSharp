using System.Windows.Controls;

namespace MenuiserieApp.UI.Views
{
    /// <summary>
    /// Logique d'interaction pour CommandeView.xaml
    /// </summary>
    public partial class CommandeView : UserControl
    {
        public CommandeView()
        {
            InitializeComponent();
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var dataGrid = (DataGrid)sender;

                System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    dataGrid.CommitEdit(DataGridEditingUnit.Row, true);

                    if (DataContext is ViewModels.CommandeViewModel vm)
                    {
                        vm.CalculerTotal();

                        dataGrid.Items.Refresh();
                    }
                }, System.Windows.Threading.DispatcherPriority.Background);
            }
        }
    }
}

using System;
using System.Windows.Input;

namespace Exercice2
{
    public class RelayCommand : ICommand
    {
        private readonly Action _actionAExecuter;

        public RelayCommand(Action actionAExecuter)
        {
            _actionAExecuter = actionAExecuter;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _actionAExecuter();
        }
    }
}

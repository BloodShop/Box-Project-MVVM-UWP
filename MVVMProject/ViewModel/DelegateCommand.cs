using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMProject.ViewModel
{
    public class DelegateCommand : ICommand
    {
        public delegate void SimpleEventHandler();
        SimpleEventHandler handler;
        public event EventHandler CanExecuteChanged;

        bool isEnabled = true;

        public DelegateCommand(SimpleEventHandler handler) => this.handler = handler;
        public bool IsEnabled  => isEnabled; 
        bool ICommand.CanExecute(object parameter) => this.IsEnabled;
        void ICommand.Execute(object parameter) => handler();
        private void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
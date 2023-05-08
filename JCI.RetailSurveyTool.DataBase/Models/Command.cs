using System;
using System.ComponentModel;
using System.Windows.Input;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    internal class Command : ICommand
    {
        private Action execute;
        private Func<bool> canExecute;

        public Command(Action Execute, Func<bool> CanExecute, PropertyChangedEventHandler CanExecuteChangeHint = null)
        {
            this.execute = Execute;
            this.canExecute = CanExecute;
            CanExecuteChangeHint += PossibleChangeHinted;
        }

        public void PossibleChangeHinted(object sender, EventArgs e)
        {
            CanExecuteChanged.Invoke(sender, e);
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return canExecute.Invoke();
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                execute.Invoke();
            }
        }
    }
}
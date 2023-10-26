using System;
using Prism.Commands;

namespace LIT.Common
{
    
    public interface IOperations
    {
        DelegateCommand AddCommand { get; }
        DelegateCommand DeleteCommand { get; }
        DelegateCommand UpdateCommand { get; }
        DelegateCommand SaveCommand { get; }
        DelegateCommand RetrieveCommand { get; }

        bool CanExecuteCommand(string commandName);
    }
    
}

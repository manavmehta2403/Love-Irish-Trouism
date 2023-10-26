using Prism.Commands;
using System;
using System.Globalization;
using System.Linq;

namespace LIT.Core.Mvvm
{
    public abstract class CrudOperations<T> : DelegateCommandBase
        where T : class
    {
        private readonly DelegateCommand _addCommand;
        private readonly DelegateCommand _deleteCommand;
        private readonly DelegateCommand _updateCommand;
        private readonly DelegateCommand _saveCommand;
        private readonly DelegateCommand _retrieveCommand;
        // Add fields for other commands if needed

        public CrudOperations(params string[] commandNames)
        {
            _addCommand = new DelegateCommand(ExecuteAdd, CanExecuteAdd);
            _deleteCommand = new DelegateCommand(ExecuteDelete, CanExecuteDelete);
            _updateCommand = new DelegateCommand(ExecuteUpdate, CanExecuteUpdate);
            _saveCommand = new DelegateCommand(ExecuteSave, CanExecuteSave);
            _retrieveCommand = new DelegateCommand(ExecuteRetrieve, CanExecuteRetrieve);

            // Convert command names to PascalCase and disable unused commands
            if (commandNames != null && commandNames.Length > 0)
            {
                DisableUnusedCommands(commandNames.Select(ToPascalCase).ToArray());
            }
        }

        private void DisableUnusedCommands(string[] enabledCommands)
        {
            if (!enabledCommands.Contains("Add"))
            {
                _addCommand.IsActive = false;
            }
            if (!enabledCommands.Contains("Delete"))
            {
                _deleteCommand.IsActive = false;
            }
            if (!enabledCommands.Contains("Update"))
            {
                _updateCommand.IsActive = false;
            }
            if (!enabledCommands.Contains("Save"))
            {
                _saveCommand.IsActive = false;
            }
            if (!enabledCommands.Contains("Retrieve"))
            {
                _retrieveCommand.IsActive = false;
            }
        }

        protected virtual bool CanExecuteAdd() => true;
        protected virtual void ExecuteAdd() => throw new InvalidOperationException("This command is not implemented.");

        protected virtual bool CanExecuteDelete() => true;
        protected virtual void ExecuteDelete() => throw new InvalidOperationException("This command is not implemented.");

        protected virtual bool CanExecuteUpdate() => true;
        protected virtual void ExecuteUpdate() => throw new InvalidOperationException("This command is not implemented.");

        protected virtual bool CanExecuteSave() => true;
        protected virtual void ExecuteSave() => throw new InvalidOperationException("This command is not implemented.");

        protected virtual bool CanExecuteRetrieve() => true;
        protected virtual void ExecuteRetrieve() => throw new InvalidOperationException("This command is not implemented.");

        protected override bool CanExecute(object parameter)
        {
            if (parameter is string commandName)
            {
                return commandName switch
                {
                    "Add" => CanExecuteAdd(),
                    "Delete" => CanExecuteDelete(),
                    "Update" => CanExecuteUpdate(),
                    "Save" => CanExecuteSave(),
                    "Retrieve" => CanExecuteRetrieve(),
                    _ => false,
                };
            }
            return true;
        }

        protected override void Execute(object parameter)
        {
            if (parameter is string commandName)
            {
                switch (commandName)
                {
                    case "Add":
                        ExecuteAdd();
                        break;
                    case "Delete":
                        ExecuteDelete();
                        break;
                    case "Update":
                        ExecuteUpdate();
                        break;
                    case "Save":
                        ExecuteSave();
                        break;
                    case "Retrieve":
                        ExecuteRetrieve();
                        break;
                    default:
                        return;
                }
            }
        }

        private string ToPascalCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(input.ToLower()).Replace(" ", "");
        }
    }
}

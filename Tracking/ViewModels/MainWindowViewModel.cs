/*
 CompositeCommands - learn how to use compositeCommands to invoke multiple commands as a signle command
 */
using Prism.Commands;
using Prism.Mvvm;
using System;
using TabControl;

namespace Tracking.ViewModels
{
  // using the View Model Locator
  public class MainWindowViewModel : BindableBase
  {
    private string _title = "Main Windown";
    public string Title
    {
      get { return _title; }
      set { SetProperty(ref _title, value); }
    }

    public MainWindowViewModel(IApplicationCommands applicationCommands)
    {
      ApplicationCommands = applicationCommands;

      ExecuteDelegateCommand = new DelegateCommand(Execute, CanExecute);

      DelegateCommandObservesProperty = new DelegateCommand(Execute, CanExecute).ObservesProperty(() => IsEnabled);

      DelegateCommandObservesCanExecute = new DelegateCommand(Execute).ObservesCanExecute(() => IsEnabled);

      ExecuteGenericDelegateCommand = new DelegateCommand<string>(ExecuteGeneric).ObservesCanExecute(() => IsEnabled);
    }

    private bool _isEnabled;
    public bool IsEnabled
    {
      get { return _isEnabled; }
      set
      {
        SetProperty(ref _isEnabled, value);
        ExecuteDelegateCommand.RaiseCanExecuteChanged();
      }
    }

    private string _updateText;
    public string UpdateText
    {
      get { return _updateText; }
      set { SetProperty(ref _updateText, value); }
    }

    // use delegate command and DelegateCommand;
    public DelegateCommand ExecuteDelegateCommand { get; private set; }

    public DelegateCommand<string> ExecuteGenericDelegateCommand { get; private set; }


    public DelegateCommand DelegateCommandObservesProperty { get; private set; }

    public DelegateCommand DelegateCommandObservesCanExecute { get; private set; }

    private void Execute()
    {
      UpdateText = $"Updated: {DateTime.Now}";
    }

    private void ExecuteGeneric(string parameter)
    {
      UpdateText = parameter;
    }

    private bool CanExecute()
    {
      return IsEnabled;
    }

    private IApplicationCommands _applicationCommands;
    public IApplicationCommands ApplicationCommands
    {
      get { return _applicationCommands; }
      set { SetProperty(ref _applicationCommands, value); }
    }

  }
}

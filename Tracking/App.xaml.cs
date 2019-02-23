using Prism.Unity;
using Prism.Ioc;
using Tracking.Views;
using System.Windows;
using Prism.Regions;
using System.Windows.Controls;
using Prism.Modularity;
using Prism.Mvvm;
using Tracking.ViewModels;
using TabControl;
using NavigControl;
using MapControl;

namespace Tracking
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : PrismApplication
  {
    protected override Window CreateShell()
    {
      return Container.Resolve<MainWindow>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
      containerRegistry.RegisterSingleton<IApplicationCommands, ApplicationCommands>();
    }

    // create a custom region adapter for the stackPannel
    protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
    {
      base.ConfigureRegionAdapterMappings(regionAdapterMappings);
      regionAdapterMappings.RegisterMapping(typeof(StackPanel), Container.Resolve<StackPanelRegionAdapter>());
    }

    // Load module using code;
    //protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    //{
    //  moduleCatalog.AddModule<MapControl.MapControlModule>();
    //}

      //Load modules manually using IModuleManager
    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
      moduleCatalog.AddModule<TabControlModule>();
      moduleCatalog.AddModule<NavigControlModule>();
      moduleCatalog.AddModule<MapControlModule>();
    }

    protected override void ConfigureViewModelLocator()
    {
      base.ConfigureViewModelLocator();

      // Change convention -  change the ViewMoelLocator naming conventions;

      //ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
      //{
      //  var viewName = viewType.FullName;
      //  var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
      //  var viewModelName = $"{viewName}ViewModel, {viewAssemblyName}";
      //  return Type.GetType(viewModelName);
      //});

      // View Model locator - Custom Registration manually register ViewModels for specific views
      //ViewModelLocationProvider.Register<MainWindow, CustomViewModel>();
      ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();

    }
  }
}

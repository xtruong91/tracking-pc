using Prism.Unity;
using Prism.Ioc;
using Tracking.Views;
using System.Windows;
using Prism.Regions;
using System.Windows.Controls;

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

    }

    // create a custom region adapter for the stackPannel
    protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
    {
      base.ConfigureRegionAdapterMappings(regionAdapterMappings);
      regionAdapterMappings.RegisterMapping(typeof(StackPanel), Container.Resolve<StackPanelRegionAdapter>());
    }
  }
}

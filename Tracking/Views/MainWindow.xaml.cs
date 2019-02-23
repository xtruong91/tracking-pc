using Prism.Ioc;
using Prism.Regions;
using System.Windows;

namespace Tracking.Views
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private IContainerExtension _container;
    private IRegionManager _regionManager;

    public MainWindow(IContainerExtension container, IRegionManager regionManager)
    {
      InitializeComponent();
      _container = container;
      _regionManager = regionManager;
      // View discovery: automatically inject views with View discovery
      //regionManager.RegisterViewWithRegion("ContentMap", typeof(MapView));
      //regionManager.RegisterViewWithRegion("ContentChart", typeof(ChartView));
      //regionManager.RegisterViewWithRegion("ContentCamera", typeof(CameraView));
    }

    private void Map_ItemClick(object sender, RoutedEventArgs e)
    {
      var view = _container.Resolve<MapView>();
      IRegion region = _regionManager.Regions["ContentMap"];
      region.Add(view);
    }

    private void Camera_ItemClick(object sender, RoutedEventArgs e)
    {
      var view = _container.Resolve<CameraView>();
      IRegion region = _regionManager.Regions["ContentChart"];
      region.Add(view);
    }

    private void Char_ItemClick(object sender, RoutedEventArgs e)
    {
      var view = _container.Resolve<ChartView>();
      IRegion region = _regionManager.Regions["ContentChart"];
      region.Add(view);
    }
  }
}

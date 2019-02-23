using Prism.Ioc;
using Prism.Regions;
using System;
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
    private IRegion _region;

    private ChartView _chartView;
    private MapView _mapView;
    private CameraView _cameraView;

    public MainWindow(IContainerExtension container, IRegionManager regionManager)
    {
      InitializeComponent();
      _container = container;
      _regionManager = regionManager;

      this.Loaded += MainWindow_Loaded;
      // View discovery: automatically inject views with View discovery
      //regionManager.RegisterViewWithRegion("ContentMap", typeof(MapView));
      //regionManager.RegisterViewWithRegion("ContentChart", typeof(ChartView));
      //regionManager.RegisterViewWithRegion("ContentCamera", typeof(CameraView));
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
      _cameraView = _container.Resolve<CameraView>();
      _chartView = _container.Resolve<ChartView>();
      _mapView = _container.Resolve<MapView>();

      _region = _regionManager.Regions["ContentRegion"];
      _region.Add(_cameraView);
      _region.Add(_mapView);
      _region.Add(_chartView);

    }

    private void Map_ItemClick(object sender, RoutedEventArgs e)
    {
      //var view = _container.Resolve<MapView>();
      //IRegion region = _regionManager.Regions["ContentMap"];
      //region.Add(view);
      _region.Activate(_mapView);
      _region.Deactivate(_chartView);
      _region.Deactivate(_cameraView);
    }

    private void Camera_ItemClick(object sender, RoutedEventArgs e)
    {
      //var view = _container.Resolve<CameraView>();
      //IRegion region = _regionManager.Regions["ContentChart"];
      //region.Add(view);
      _region.Deactivate(_mapView);
      _region.Deactivate(_chartView);
      _region.Activate(_cameraView);
    }

    private void Char_ItemClick(object sender, RoutedEventArgs e)
    {
      //var view = _container.Resolve<ChartView>();
      //IRegion region = _regionManager.Regions["ContentChart"];
      //region.Add(view);
      _region.Deactivate(_mapView);
      _region.Activate(_chartView);
      _region.Deactivate(_cameraView);
    }
  }
}

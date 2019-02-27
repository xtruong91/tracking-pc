using GMap.NET;
using GMap.NET.MapProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tracking.Views
{
  /// <summary>
  /// Interaction logic for MapView.xaml
  /// </summary>
  public partial class MapView : UserControl
  {
    public MapView()
    {
      InitializeComponent();
      Init();
    }

    private void Init()
    {
      MainMap.MapProvider = GMapProviders.GoogleHybridMap;
      MainMap.Position = new PointLatLng(54.6961334816182, 25.2985095977783);
      MainMap.CanDragMap = true;
      MainMap.DragButton = MouseButton.Left;
      MainMap.MouseWheelZoomEnabled = true;
      MainMap.MaxZoom = 24;
      MainMap.MinZoom = 1;
    }
  }
}

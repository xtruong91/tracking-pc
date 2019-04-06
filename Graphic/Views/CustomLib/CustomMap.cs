using MapControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Graphic.Views.CustomLib
{
  public class CustomMap : Map
  {
    private Point? mousePosition;

    public new void MouseWheel(MouseWheelEventArgs e)
    {
      var zoomDelta = MouseWheelZoomDelta * e.Delta / 120d;
      ZoomMap(e.GetPosition(this), TargetZoomLevel + zoomDelta);
    }

    public new void MouseLeftButtonDown(MouseButtonEventArgs e)
    {
      if (CaptureMouse())
      {
        mousePosition = e.GetPosition(this);
      }
    }

    public new void MouseLeftButtonUp(MouseButtonEventArgs e)
    {
      if (mousePosition.HasValue)
      {
        mousePosition = null;
        ReleaseMouseCapture();
      }
    }

    public new void MouseMove(MouseEventArgs e)
    {
      if (mousePosition.HasValue)
      {
        var position = e.GetPosition(this);
        TranslateMap(position - mousePosition.Value);
        mousePosition = position;
      }
    }
  }
}

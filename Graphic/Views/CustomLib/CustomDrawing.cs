using DynamicGeometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Graphic.Views.CustomLib
{
  public class CustomDrawing : DrawingControl
  {
    public CustomDrawing()
    {
      this.Background = new SolidColorBrush(Colors.Transparent);
    }
  }
}

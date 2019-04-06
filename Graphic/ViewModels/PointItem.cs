using MapControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphic.ViewModels
{
  public class PointItem : BaseViewModel
  {
    private string name;
    public string Name
    {
      get { return name; }
      set
      {
        name = value;
        RaisePropertyChanged(nameof(Name));
      }
    }

    private Location location;
    public Location Location
    {
      get { return location; }
      set
      {
        location = value;
        RaisePropertyChanged(nameof(Location));
      }
    }
  }

  public class Polyline
  {
    public LocationCollection Locations { get; set; }
  }
}

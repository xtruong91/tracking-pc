using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTest.ViewModels
{
  public class MainWindowViewModel : PropertyChangedBase
  {
    private string _name = "Philip";
    public string Name
    {
      get { return _name; }
      set
      {
        _name = value;
        NotifyOfPropertyChange(() => Name);
      }
    }
  }
}

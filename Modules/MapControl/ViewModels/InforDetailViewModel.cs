using MapControl.Models;
using Prism.Mvvm;

namespace MapControl.ViewModels
{
  public class InforDetailViewModel : BindableBase
  {
    private InfoModel _infoModel;

    public InfoModel InfoModel
    {
      get => _infoModel;
      set
      {
        SetProperty(ref _infoModel, value);
      }
    }
  }
}

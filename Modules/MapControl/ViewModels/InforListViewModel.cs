using MapControl.Models;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;

namespace MapControl.ViewModels
{
  public class InforListViewModel : BindableBase
  {
    private ObservableCollection<InfoModel> _infors;

    public ObservableCollection<InfoModel> Infors
    {
      get => _infors;
      set
      {
        SetProperty(ref _infors, value);
      }
    }
    public InforListViewModel()
    {
      CreateInfors();
    }

    private void CreateInfors()
    {
      var infors = new ObservableCollection<InfoModel>();
      for (int i = 0; i < 10; i++)
      {
        infors.Add(new InfoModel()
        {
          Longitude = i + 180,
          Latitude = i + 90,
          LastUpdated = DateTime.Now
        });
      }
      Infors = infors;
    }
  }
}

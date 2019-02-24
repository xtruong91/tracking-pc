using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MapControl.Models
{
  public class InfoModel : INotifyPropertyChanged
  {
    private int _id;
    private double _longitude;
    private double _latitude;
    private string _description;
    private DateTime? _lastUpdated;

    public int Id
    {
      get => _id;
      set
      {
        _id = value;
        OnPropertyChanged();
      }
    }
    public double Longitude
    {
      get => _longitude;
      set
      {
        _longitude = value;
        OnPropertyChanged();
      }
    }
    public double Latitude
    {
      get => _latitude;
      set
      {
        _latitude = value;
        OnPropertyChanged();
      }
    }
    public string Description
    {
      get => _description;
      set
      {
        _description = value;
        OnPropertyChanged();
      }
    }
    public DateTime? LastUpdated
    {
      get => _lastUpdated;
      set
      {
        _lastUpdated = value;
        OnPropertyChanged();
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyname = null)
    {
      PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyname));
    }
    public override string ToString()
    {
      return String.Format("{0}, {1}", Longitude, Latitude);
    }
  }
}

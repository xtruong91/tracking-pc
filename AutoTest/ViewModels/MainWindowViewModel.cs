using Caliburn.Micro;
using Infralution.Localization.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoTest.ViewModels
{
  public class MainWindowViewModel : PropertyChangedBase
  {

    public void HandleEnglish(object sender, RoutedEventArgs args)
    {
      if (CultureManager.UICulture.Name == "en-GB")
      {
        return;
      }
      var culture = new CultureInfo("en-GB");
      CultureManager.UICulture = culture;
    }

    public void HandleFrench(object sender, RoutedEventArgs args)
    {
      if (CultureManager.UICulture.Name == "fr-FR")
      {
        return;
      }

      var culture = new CultureInfo("fr-FR");
      CultureManager.UICulture = culture;
    }

    public void HandleVietNamese(object sender, RoutedEventArgs args)
    {
      if (CultureManager.UICulture.Name == "de-DE")
      {
        return;
      }
      var culture = new CultureInfo("de-DE");
      CultureManager.UICulture = culture;
    }
  }
}

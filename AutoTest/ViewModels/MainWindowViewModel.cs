using Caliburn.Micro;
using Infralution.Localization.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AutoTest.ViewModels
{
  public class MainWindowViewModel : PropertyChangedBase
  {

    public TextBlock texBlock;

    public MainWindowViewModel()
    {
      texBlock = new TextBlock();
    }

    public void HandleEnglish(object sender, RoutedEventArgs args)
    {
      if (CultureManager.UICulture.Name == "en-GB")
      {
        return;
      }
      var culture = new CultureInfo("en-GB");
      CultureManager.UICulture = culture;
      texBlock.Text = "Handle English";
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

    #region Handle keyboard gesture
    private string enterMessage = "enterMessage";

    public string EnterMessage
    {
      get { return enterMessage; }
      set
      {
        enterMessage = value;
        NotifyOfPropertyChange(() => EnterMessage);
      }
    }

    public void EnterPressed()
    {
      EnterMessage = "Enter has been pressed";
    }

    public void CtrlEnterPressed()
    {
      EnterMessage = "Ctrl+Enter has been pressed";
    }

    public void AltEnterPressed()
    {
      EnterMessage = "Alt+Enter has been pressed";
    }
    #endregion
  }
}

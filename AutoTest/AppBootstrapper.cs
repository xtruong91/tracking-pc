using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Infralution.Localization.Wpf;

namespace AutoTest
{
  public class AppBootstrapper : BootstrapperBase
  {
    public AppBootstrapper()
    {
      Initialize();
      CultureManager.UICulture = Thread.CurrentThread.CurrentCulture;
      Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
    }

    protected override void OnStartup(object sender, StartupEventArgs e)
    {
      DisplayRootViewFor<ViewModels.MainWindowViewModel>();
    }

    protected override void OnExit(object sender, EventArgs e)
    {
      //CultureManager.UICultureChanged -= HandleUICultureChanged;
    }

    private void InitilazeCulture(string culture)
    {

      //CultureManager.UICultureChanged += HandleUICultureChanged;
    }


  }
}

using MapControl.Models;
using MapControl.ViewModels;
using Prism.Common;
using Prism.Regions;
using System.Windows.Controls;

namespace MapControl.Views
{
  /// <summary>
  /// Interaction logic for InforDetail.xaml
  /// </summary>
  public partial class InforDetail : UserControl
  {
    public InforDetail()
    {
      InitializeComponent();
      RegionContext.GetObservableContext(this).PropertyChanged += InforDetail_PropertyChanged;
    }
    private void InforDetail_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      var context = (ObservableObject<object>)sender;
      var selectedPerson = (InfoModel)context.Value;
      (DataContext as InforDetailViewModel).InfoModel = selectedPerson;
    }

  }

}

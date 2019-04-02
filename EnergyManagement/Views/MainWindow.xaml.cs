using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EnergyManagementSystem
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      Point p = Mouse.GetPosition(this);
      if (p.Y < 100)
      {
        this.DragMove();
      }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      this.WindowState = WindowState.Normal;
    }

    private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      Point p = Mouse.GetPosition(this);

      if (p.Y < 100)
      {
        if (this.WindowState != WindowState.Maximized)
        {
          this.WindowState = WindowState.Maximized;
          //BudgetChartControl.Height = 350;
          //ChartControl.Height = 450;
        }
        else
        {
          this.WindowState = WindowState.Normal;
          //BudgetChartControl.Height = 230;
          //ChartControl.Height = 350;
        }

      }
    }

    private void DeviceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void DateSelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void CurveComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void DeviceScheList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void btnUpdateSche_Click(object sender, RoutedEventArgs e)
    {

    }

    private void AddedDeviceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void btnAddDev_Click(object sender, RoutedEventArgs e)
    {

    }

    private void btnRemoveDev_Click(object sender, RoutedEventArgs e)
    {

    }

    private void btnUpdateDev_Click(object sender, RoutedEventArgs e)
    {

    }
  }
}

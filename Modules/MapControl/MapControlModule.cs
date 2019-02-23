using MapControl.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;


namespace MapControl
{
  public class MapControlModule : IModule
  {
    public void OnInitialized(IContainerProvider containerProvider)
    {
      var regionManager = containerProvider.Resolve<IRegionManager>();
      //regionManager.RegisterViewWithRegion("ContentRegion", typeof(MapControlView));
      regionManager.RegisterViewWithRegion("LeftRegion", typeof(MessageView));
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {

    }
  }
}

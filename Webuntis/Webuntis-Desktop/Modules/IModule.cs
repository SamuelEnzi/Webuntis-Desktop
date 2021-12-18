using Webuntis_API;

namespace Webuntis_Desktop.Modules
{
    public interface IModule
    {
        public delegate void OnFinishedLoadingEventHandler(object sender);
        public event OnFinishedLoadingEventHandler OnFinishedLoading;
        public object Display(WebuntisClient client);
        public void Render();
    }
}

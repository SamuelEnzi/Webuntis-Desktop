using Webuntis_API;

namespace Webuntis_Desktop.Modules
{
    public interface IModule
    {
        public object Display(WebuntisClient client);
        public void Render();
    }
}

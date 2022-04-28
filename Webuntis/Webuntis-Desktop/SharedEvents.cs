using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webuntis_Desktop.Modules;

namespace Webuntis_Desktop
{
    public static class SharedEvents
    {
        public delegate void LoadModuleEventHandler(IModule module);
        public static event LoadModuleEventHandler? LoadModule;

        public static void RegisterLoadModule(IModule module) => LoadModule?.Invoke(module);
    }
}

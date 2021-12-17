using System;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Webuntis_Desktop.Modules;

namespace Webuntis_Desktop.Models
{
    public class Module
    {
        public ImageSource? Image { get; set; }
        public string Name { get; set; }
        public IModule? module { get; set; }

        public Module(string Name, ImageSource? source = null)
        {
            this.Name = Name;
            this.Image = source;
        }

        public Module(string Name, IModule module)
        {
            this.module = module;
            this.Name = Name;
        }

        public Module(string Name, string source)
        {
            var uri = string.Format(
            "pack://application:,,,/{0};component/{1}"
           , Assembly.GetExecutingAssembly().GetName().Name
           , source
            );

            this.Name = Name;

            try
            {
                this.Image = new BitmapImage(new Uri(uri));
            }
            catch { }
        }

        public Module(string Name, string source, IModule module)
        {
            this.module = module;

            var uri = string.Format(
            "pack://application:,,,/{0};component/{1}"
           , Assembly.GetExecutingAssembly().GetName().Name
           , source
            );

            this.Name = Name;

            try
            {
                this.Image = new BitmapImage(new Uri(uri));
            }
            catch { }
        }
    }
}

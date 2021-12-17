using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Webuntis_Desktop.Helpers
{
    public static class WebWorker
    {
        public static BitmapImage ImageFromUrl(string url)
        {
            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(url);
                bitmap.EndInit();
                return bitmap;
            }
            catch
            {
                return null;
            }

        }
    }
}

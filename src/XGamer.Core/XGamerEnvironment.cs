using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Media;

namespace XGamer.Core
{
    public static class XGamerEnvironment
    {
        public static string EmulatorsPath
        {
            get
            {
                string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (ConfigurationManager.AppSettings.AllKeys.Contains(Resource.EmulatorsPath))
                {
                    return Path.Combine(assemblyPath, ConfigurationManager.AppSettings[Resource.EmulatorsPath]);
                }
                else
                {
                    return Path.Combine(assemblyPath, Resource.DefaultEmulatorsPath);
                }
            }
        }

        public static string RomsPath
        {
            get
            {
                string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (ConfigurationManager.AppSettings.AllKeys.Contains(Resource.RomsPath))
                {
                    return Path.Combine(assemblyPath, ConfigurationManager.AppSettings[Resource.RomsPath]);
                }
                else
                {
                    return Path.Combine(assemblyPath, Resource.DefaultRomsPath);
                }
            }
        }

        public static string PicturesPath
        {
            get
            {
                string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (ConfigurationManager.AppSettings.AllKeys.Contains(Resource.PicturesPath))
                {
                    return Path.Combine(assemblyPath, ConfigurationManager.AppSettings[Resource.PicturesPath]);
                }
                else
                {
                    return Path.Combine(assemblyPath, Resource.DefaultPicturesPath);
                }
            }
        }

        public static Color BackgroundColor
        {
            get
            {
                byte r = Convert.ToByte(128);
                byte g = 0;
                byte b = Convert.ToByte(128);
                if (ConfigurationManager.AppSettings.AllKeys.Contains(Resource.BackgroundR) && ConfigurationManager.AppSettings.AllKeys.Contains(Resource.BackgroundG) && ConfigurationManager.AppSettings.AllKeys.Contains(Resource.BackgroundB))
                {
                    r = Convert.ToByte(ConfigurationManager.AppSettings[Resource.BackgroundR]);
                    g = Convert.ToByte(ConfigurationManager.AppSettings[Resource.BackgroundG]);
                    b = Convert.ToByte(ConfigurationManager.AppSettings[Resource.BackgroundB]);
                }

                return new Color() { R = r, G = g, B = b, A = 255 };
            }
        }
    }
}

using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;

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
    }
}

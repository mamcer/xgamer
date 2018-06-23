using System;

using XGamer.Data.XML;

namespace XGamer.Data.Core
{
    public enum XGamerDataProviderType
    {
        EntityFramework = 0,
        Xml = 1
    }

    public static class XGamerDataProviderFactory
    {
        public static IXGamerDataProvider GetDataProvider(XGamerDataProviderType type)
        {
            if(type == XGamerDataProviderType.Xml)
            {
                return XMLDataProvider.Instance;
            }

            throw new NotImplementedException("XGamerDataProviderType not supported.");
        }
    }
}

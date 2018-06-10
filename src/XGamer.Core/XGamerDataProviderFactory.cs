using System;
using XGamer.Data.Core;
using XGamer.Data.EF;

namespace XGamer.Core
{
    public enum XGamerDataProviderType
    {
        EntityFramework = 0
    }

    public static class XGamerDataProviderFactory
    {
        public static IXGamerDataProvider GetDataProvider(XGamerDataProviderType type)
        {
            if (type == XGamerDataProviderType.EntityFramework)
            {
                return EFDataProvider.Instance;
            }

            throw new NotImplementedException("XGamerDataProviderType not supported.");
        }
    }
}
using LegionWebApp.Data;
using Microsoft.Extensions.Localization;

namespace LegionWebApp.Localization
{
    public class PostgreSqlStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public PostgreSqlStringLocalizerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            return new PostgreSqlStringLocalizer(resourceSource.FullName, _serviceProvider);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new PostgreSqlStringLocalizer(baseName, _serviceProvider);
        }
    }
}

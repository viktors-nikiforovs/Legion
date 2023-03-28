using LegionWebApp.Data;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace LegionWebApp.Localization
{
    public class PostgreSqlStringLocalizer : IStringLocalizer
    {
        private readonly string _resourceName;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PostgreSqlStringLocalizer> _logger;

        public PostgreSqlStringLocalizer(string resourceName, IServiceProvider serviceProvider)
        {
            _resourceName = resourceName;
            _serviceProvider = serviceProvider;
        }

        public LocalizedString this[string name]
        {
            get
            {
                var culture = CultureInfo.CurrentCulture.Name;
                var value = GetString(name, culture);
                return new LocalizedString(name, value ?? name, value == null);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = this[name].Value;
                return new LocalizedString(name, string.Format(format, arguments), format == null);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string GetString(string key, string culture)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var record = dbContext.Localization
                .Select(r => new
                {
                    r.Key,
                    Value_FR = r.Value_FR ?? "",
                    Value_DE = r.Value_DE ?? "",
                    Value_UK = r.Value_UK ?? "",
                })
                .FirstOrDefault(r => r.Key == key);

            if (record == null)
            {
                return null;
            }

            return culture switch
            {
                "fr" => string.IsNullOrEmpty(record.Value_FR) ? null : record.Value_FR,
                "de" => string.IsNullOrEmpty(record.Value_DE) ? null : record.Value_DE,
                "uk" => string.IsNullOrEmpty(record.Value_UK) ? null : record.Value_UK,
                _ => null
            };
        }


    }
}

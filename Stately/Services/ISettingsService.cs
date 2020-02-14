using Stately.Models;
using System.Collections.Generic;

namespace Stately.Services
{
    public interface ISettingsService
    {
        List<StatelySettings> Get();
        List<StatelySettings> GetActiveSettings();
        bool Set(List<StatelySettings> settings);

        IEnumerable<string> GetAliases();
    }
}

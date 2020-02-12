using Stately.Models;
using System.Collections.Generic;

namespace Stately.Services
{
    public interface ISettingsService
    {
        List<StatelySettings> Get();
        bool Set(List<StatelySettings> settings);
    }
}

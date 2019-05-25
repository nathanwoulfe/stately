using Stately.Models;
using System.Collections.Generic;

namespace Stately.Services
{
    public interface ISettingsService
    {
        List<Settings> Get();
        bool Set(List<Settings> settings);
    }
}

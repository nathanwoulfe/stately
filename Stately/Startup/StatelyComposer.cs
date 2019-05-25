using Stately.Services;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Stately.Startup
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class StatelyComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register<ISettingsService, SettingsService>();
            composition.Components().Append<StatelyComponent>();
        }
    }
}

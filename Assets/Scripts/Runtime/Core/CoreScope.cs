using VContainer;
using VContainer.Unity;

namespace TandC.GeometryAstro.Core
{
    public sealed class CoreScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<CoreFlow>();
        }
    }
}
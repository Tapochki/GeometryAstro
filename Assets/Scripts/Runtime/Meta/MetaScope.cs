using VContainer;
using VContainer.Unity;

namespace TandC.GeometryAstro.Meta
{
    public sealed class MetaScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<MetaFlow>();
        }
    }
}
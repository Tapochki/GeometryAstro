using VContainer;
using VContainer.Unity;

namespace TandC.GeometryAstro.Loading
{
    public sealed class LoadingScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<LoadingFlow>();
        }
    }
}
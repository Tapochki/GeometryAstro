using TandC.GeometryAstro.Utilities;
using VContainer;
using VContainer.Unity;


namespace TandC.GeometryAstro.Menu
{
    public class MenuScope : LifetimeScope
    {
        protected override void Awake()
        {
            DontDestroyOnLoad(this);
            base.Awake();
        }

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterUIService(builder);

            RegisterEntryPoint(builder);
        }

        private void RegisterUIService(IContainerBuilder builder)
        {
            builder.Register<UIService>(Lifetime.Scoped);
        }

        private void RegisterEntryPoint(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<MenuFlow>();
        }
    }

}
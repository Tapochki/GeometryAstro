namespace TandC.GeometryAstro.EventBus
{
    public readonly struct DashEvent : IEvent
    {
        public readonly bool IsActive;

        public DashEvent(bool isActive)
        {
            IsActive = isActive;
        }
    }
}


namespace TandC.GeometryAstro.EventBus
{
    public readonly struct LaserEvent : IEvent
    {
        public readonly bool IsActive;

        public LaserEvent(bool isActive)
        {
            IsActive = isActive;
        }
    }
}


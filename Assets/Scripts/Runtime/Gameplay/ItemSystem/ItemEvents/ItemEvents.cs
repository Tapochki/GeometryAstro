
namespace TandC.GeometryAstro.EventBus
{
    public readonly struct BombItemReleaseEvent : IEvent
    {
        public readonly int BombDamage;

        public BombItemReleaseEvent(int bombDamage)
        {
            BombDamage = bombDamage;
        }
    }

    public readonly struct ChestItemReleaseEvent : IEvent { }

    public readonly struct CointItemReleaseEvent : IEvent
    {
        public readonly int CoinAmount;

        public CointItemReleaseEvent(int coinAmount)
        {
            CoinAmount = coinAmount;
        }
    }

    public readonly struct ExpirienceItemReleaseEvent : IEvent
    {
        public readonly int ExpAmount;

        public ExpirienceItemReleaseEvent(int expAmount)
        {
            ExpAmount = expAmount;
        }
    }

    public readonly struct FrozeBombItemReleaseEvent : IEvent
    {
        public readonly float FreezeTime;

        public FrozeBombItemReleaseEvent(float freezeTime)
        {
            FreezeTime = freezeTime;
        }
    }

    public readonly struct MagnetItemReleaseEvent : IEvent
    {
    }

    public readonly struct MedecineItemReleaseEvent : IEvent
    {
        public readonly int HealAmount;

        public MedecineItemReleaseEvent(int healAmount)
        {
            HealAmount = healAmount;
        }
    }

    public readonly struct RocketAmmoItemReleaseEvent : IEvent
    {
        public readonly int AmmoCount;

        public RocketAmmoItemReleaseEvent(int ammoCount)
        {
            AmmoCount = ammoCount;
        }
    }
}

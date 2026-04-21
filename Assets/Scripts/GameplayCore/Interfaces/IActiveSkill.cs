namespace GameplayCore
{
    public interface IActiveSkill
    {
        bool IsWeapon { get; }

        void Initialization();
        void Tick();
        void Upgrade(float value = 0);
        void Evolve();
    }
}

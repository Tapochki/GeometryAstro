namespace TandC.GeometryAstro.Gameplay
{
    public interface IWeapon
    {
        void UpdateWeapon(float deltaTime);
        void Upgrade();
    }
}

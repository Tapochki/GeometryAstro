using TandC.GeometryAstro.Gameplay;
using TandC.GeometryAstro.Settings;
using UnityEngine;

public class WeaponController
{
    private IWeapon _currentWeapon;
    private readonly IWeaponFactory _weaponFactory;

    public WeaponController(IWeaponFactory weaponFactory)
    {
        _weaponFactory = weaponFactory;
    }

    public void EquipWeapon(WeaponType type)
    {
        _currentWeapon = _weaponFactory.CreateWeapon(type);
    }

    public void Update(float deltaTime)
    {
        _currentWeapon?.UpdateWeapon(deltaTime);
    }
}

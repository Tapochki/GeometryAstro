using System.Collections.Generic;
using System.Linq;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Gameplay;
using TandC.GeometryAstro.Services;
using TandC.GeometryAstro.Settings;
using UnityEngine;
using VContainer;

public class WeaponController
{
    private List<IWeapon> _weapons = new List<IWeapon>();
    private IWeaponFactory _weaponFactory;
    private WeaponConfig _config;
    private TickService _tickService;

    [Inject]
    private void Construct(GameConfig config, TickService tickService) 
    {
        _config = config.WeaponConfig;
        _tickService = tickService;
    }

    public WeaponController() 
    {
        _weapons = new List<IWeapon>();
        _weaponFactory = new WeaponFactory();
    }

    public void Init()
    {
        _tickService.RegisterUpdate(Tick);
    }

    private IWeapon CreateWeapon(WeaponType type) 
    {
        return _weaponFactory.GetBuilder(type)
            .SetConfig(_config)
            .SetProjectileFactory()
            .SetReloader()
            .SetEnemyDetector()
            .Build();
    }

    public IWeapon RegisterWeapon(WeaponType type)
    {
        IWeapon weapon = CreateWeapon(type);
        weapon.Initialization();
        _weapons.Add(weapon);
        return weapon;
    }

    private void Tick() 
    {
        for (int i = 0; i < _weapons.Count; i++)
        {
            _weapons[i].Tick();
        }
    }

    private void UpgradeWeapon(WeaponType type)
        => _weapons.FirstOrDefault(w => w.WeaponType == type)?.Upgrade();

    public IReadOnlyList<IWeapon> GetAllWeapons() => _weapons;
}

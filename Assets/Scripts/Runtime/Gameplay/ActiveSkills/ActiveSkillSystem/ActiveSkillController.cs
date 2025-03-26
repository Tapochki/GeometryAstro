using System.Collections.Generic;
using System.Linq;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Gameplay;
using TandC.GeometryAstro.Services;
using TandC.GeometryAstro.Settings;
using UnityEngine;
using VContainer;

public class ActiveSkillController: IEventReceiver<ActiveSkillUpgradeEvent>, IEventReceiver<ActiveSkillEvolveEvent>
{
    private List<IActiveSkill> _activeSkills = new List<IActiveSkill>();
    private IActiveSkillFactory _activeSkillFactory;
    private ActiveSkillConfig _config;
    private TickService _tickService;
    private ModificatorContainer _modificatorContainer;

    public UniqueId Id { get; } = new UniqueId();

    [Inject]
    private void Construct(GameConfig config, TickService tickService, ModificatorContainer modificatorContainer) 
    {
        _config = config.ActiveSkillConfig;
        _tickService = tickService;
        _modificatorContainer = modificatorContainer;
    }

    private void RegisterEvent()
    {
        EventBusHolder.EventBus.Register(this as IEventReceiver<ActiveSkillUpgradeEvent>);
        EventBusHolder.EventBus.Register(this as IEventReceiver<ActiveSkillEvolveEvent>);
    }

    private void UnregisterEvent()
    {
        EventBusHolder.EventBus.Unregister(this as IEventReceiver<ActiveSkillUpgradeEvent>);
        EventBusHolder.EventBus.Unregister(this as IEventReceiver<ActiveSkillEvolveEvent>);
    }

    public ActiveSkillController() 
    {
        _activeSkills = new List<IActiveSkill>();
        _activeSkillFactory = new ActiveSkillFactory();
    }

    public void Init()
    {
        RegisterEvent();
        _tickService.RegisterUpdate(Tick);
    }

    public void Dispose() 
    {
        UnregisterEvent();
    }

    private IActiveSkill CreateWeapon(ActiveSkillType type) 
    {
        return _activeSkillFactory.GetBuilder(type)
            .SetConfig(_config)
            .SetModificators(_modificatorContainer)
            .Build();
    }

    private IActiveSkill RegisterWeapon(ActiveSkillType type)
    {
        IActiveSkill weapon = CreateWeapon(type);
        weapon.Initialization();
        _activeSkills.Add(weapon);
        return weapon;
    }

    private void Tick() 
    {
        for (int i = 0; i < _activeSkills.Count; i++)
        {
            _activeSkills[i].Tick();
        }
    }

    public void OnEvent(ActiveSkillUpgradeEvent @event)
    {
        IActiveSkill skill = GetActiveSkill(@event.ActiveSkillType);

        if (skill == null)
            RegisterWeapon(@event.ActiveSkillType);
        else
            skill.Upgrade();
    }

    public void OnEvent(ActiveSkillEvolveEvent @event)
    {
        GetActiveSkill(@event.ActiveSkillType).Evolve();
    }

    private IActiveSkill GetActiveSkill(ActiveSkillType type)
        => _activeSkills.FirstOrDefault(w => w.SkillType == type);

    public IReadOnlyList<IActiveSkill> GetAllWeapons() => _activeSkills;
}

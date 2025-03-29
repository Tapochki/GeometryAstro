using System.Collections.Generic;
using System.Linq;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Gameplay;
using TandC.GeometryAstro.Services;
using TandC.GeometryAstro.Settings;
using UniRx;
using VContainer;

public class ActiveSkillController : IActiveSkillController, IEventReceiver<ActiveSkillUpgradeEvent>, IEventReceiver<ActiveSkillEvolveEvent>, IEventReceiver<CloakingEvent> 
{
    private List<IActiveSkill> _activeSkills = new List<IActiveSkill>();
    private IActiveSkillFactory _activeSkillFactory;
    private ActiveSkillConfig _config;
    private TickService _tickService;
    private ModificatorContainer _modificatorContainer;

    protected CompositeDisposable _disposables = new CompositeDisposable();

    public UniqueId Id { get; } = new UniqueId();

    [Inject]
    private void Construct(GameConfig config, TickService tickService, ModificatorContainer modificatorContainer, IActiveSkillFactory activeSkillFactory)
    {
        _config = config.ActiveSkillConfig;
        _tickService = tickService;
        _modificatorContainer = modificatorContainer;
        _activeSkillFactory = activeSkillFactory;
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
    }

    public void Init()
    {
        RegisterEvent();
        _tickService.RegisterUpdate(TickAll);
        _activeSkillFactory.SetActiveSkillContainer(this);
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

    private void TickAll()
    {
        for (int i = 0; i < _activeSkills.Count; i++)
        {
            _activeSkills[i].Tick();
        }
    }

    private void TickNotWeapon()
    {
        for (int i = 0; i < _activeSkills.Count; i++)
        {
            if (!_activeSkills[i].IsWeapon)
                _activeSkills[i].Tick();
        }
    }

    public void RegisterMask()
    {
        EventBusHolder.EventBus.Register(this as IEventReceiver<CloakingEvent>);
    }

    public void UnRegisterMask()
    {
        EventBusHolder.EventBus.Unregister(this as IEventReceiver<CloakingEvent>);
    }

    public void OnEvent(CloakingEvent @event)
    {
        if (@event.IsEvolved) 
        {
            UnRegisterMask();
            SetUpdateAll();
            return;
        }
        if (@event.IsActive)
            SetUpdateOnlyAbilities();
        else if (!@event.IsActive)
            SetUpdateAll();
    }

    private void SetUpdateOnlyAbilities()
    {
        _tickService.UnregisterUpdate(TickAll);
        _tickService.RegisterUpdate(TickNotWeapon);
    }

    private void SetUpdateAll()
    {
        _tickService.UnregisterUpdate(TickNotWeapon);
        _tickService.RegisterUpdate(TickAll);
    }

    public void OnEvent(ActiveSkillUpgradeEvent @event)
    {
        IActiveSkill skill = GetActiveSkill(@event.ActiveSkillType);

        if (skill == null)
            RegisterWeapon(@event.ActiveSkillType);
        else
            skill.Upgrade(@event.UpgradeValue);
    }

    public void OnEvent(ActiveSkillEvolveEvent @event)
    {
        GetActiveSkill(@event.ActiveSkillType).Evolve();
    }

    private IActiveSkill GetActiveSkill(ActiveSkillType type)
        => _activeSkills.FirstOrDefault(w => w.SkillType == type);

    private IReadOnlyList<IActiveSkill> GetAllWeapons() => _activeSkills;
}

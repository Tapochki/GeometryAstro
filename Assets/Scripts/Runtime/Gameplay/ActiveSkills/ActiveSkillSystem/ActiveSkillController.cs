using System.Collections.Generic;
using System.Linq;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Gameplay;
using TandC.GeometryAstro.Services;
using TandC.GeometryAstro.Settings;
using VContainer;

public class ActiveSkillController
{
    private List<IActiveSkill> _activeSkills = new List<IActiveSkill>();
    private IActiveSkillFactory _activeSkillFactory;
    private ActiveSkillConfig _config;
    private TickService _tickService;
    private ModificatorContainer _modificatorContainer;

    [Inject]
    private void Construct(GameConfig config, TickService tickService, ModificatorContainer modificatorContainer) 
    {
        _config = config.ActiveSkillConfig;
        _tickService = tickService;
        _modificatorContainer = modificatorContainer;
    }

    public ActiveSkillController() 
    {
        _activeSkills = new List<IActiveSkill>();
        _activeSkillFactory = new ActiveSkillFactory();
    }

    public void Init()
    {
        _tickService.RegisterUpdate(Tick);
    }

    private IActiveSkill CreateWeapon(ActiveSkillType type) 
    {
        return _activeSkillFactory.GetBuilder(type)
            .SetConfig(_config)
            .SetModificators(_modificatorContainer)
            .Build();
    }

    public IActiveSkill RegisterWeapon(ActiveSkillType type)
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

    private void UpgradeWeapon(ActiveSkillType type)
        => _activeSkills.FirstOrDefault(w => w.SkillType == type)?.Upgrade();

    public IReadOnlyList<IActiveSkill> GetAllWeapons() => _activeSkills;
}

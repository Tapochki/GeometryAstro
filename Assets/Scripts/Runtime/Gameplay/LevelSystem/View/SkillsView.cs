using System.Collections.Generic;
using System.Linq;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class SkillsView : MonoBehaviour
    {
        [SerializeField]
        private Transform _activeSkillParent;
        [SerializeField]
        private Transform _passiveSkillParent;

        [SerializeField]
        private SkillViewItem _skillViewItemPrefab;

        private List<SkillViewItem> _activeSkills;
        private List<SkillViewItem> _passiveSkills;

        public void Init() 
        {
            InitLists();
        }

        private void InitLists() 
        {
            _passiveSkills = new List<SkillViewItem>();
            _activeSkills = new List<SkillViewItem>();
        }

        public void AddSkillItem(SkillUseType useType, SkillType skillType, Sprite sprite) 
        {
            List<SkillViewItem> targetList = GetSkillList(useType);
            Transform parent = GetParent(useType);
            targetList.Add(SpawnSkillItemView(parent, skillType, sprite));
        }

        private SkillViewItem SpawnSkillItemView(Transform parent, SkillType skillType, Sprite sprite) 
        {
            SkillViewItem skillViewItem = MonoBehaviour.Instantiate(_skillViewItemPrefab, parent);
            skillViewItem.Init(skillType);
            skillViewItem.SetImage(sprite);

            return skillViewItem;
        }

        private List<SkillViewItem> GetSkillList(SkillUseType useType) => useType == SkillUseType.Active ? _activeSkills : _passiveSkills;

        private Transform GetParent(SkillUseType useType) => useType == SkillUseType.Active ? _activeSkillParent : _passiveSkillParent;

        public void UpdateSkillSprite(SkillUseType useType, SkillType skillType, Sprite sprite) 
        {
            var skillItem = GetSkillList(useType).FirstOrDefault(skill => skill.SkillType == skillType);
            if (skillItem != null)
            {
                skillItem.SetImage(sprite);
            }
        }
    }
}

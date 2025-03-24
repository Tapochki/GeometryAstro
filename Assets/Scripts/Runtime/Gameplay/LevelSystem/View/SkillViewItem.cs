
using TandC.GeometryAstro.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace TandC.GeometryAstro.Gameplay 
{
    public class SkillViewItem : MonoBehaviour
    {
        public SkillType SkillType { get; private set;}

        private Image _skillImage;

        public void Init(SkillType skillType) 
        {
            SkillType = skillType;

            _skillImage = gameObject.GetComponent<Image>();
        }

        public void SetImage(Sprite sprite) 
        {
            _skillImage.sprite = sprite;
        }
    }
}


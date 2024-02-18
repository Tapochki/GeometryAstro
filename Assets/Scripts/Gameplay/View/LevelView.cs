using TMPro;
using Unity.Services.Analytics.Internal;
using UnityEngine;
using UnityEngine.UI;

namespace TandC.Gameplay 
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField]
        private Image _expirienceImage;

        [SerializeField]
        private TextMeshProUGUI _levelText;

        public void UpdateLevel(int level) 
        {
            //TODO change to localization

            _levelText.text = $"Level: {level}";
        }

        public void UpdateExpririence(float currentXp, float xpToNextLevel)
        {
            _expirienceImage.fillAmount = currentXp / xpToNextLevel;
        }
    }
}


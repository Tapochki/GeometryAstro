using TandC.GeometryAstro.EventBus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TandC.GeometryAstro.Gameplay 
{
    public class LevelView : MonoBehaviour, IEventReceiver<ExperienceChangeEvent>
    {
        [SerializeField]
        private Image _experienceImage;

        [SerializeField]
        private TextMeshProUGUI _levelText;

        public UniqueId Id => new UniqueId();

        private void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this as IEventReceiver<ExperienceChangeEvent>);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this as IEventReceiver<ExperienceChangeEvent>);
        }

        private void OnEnable()
        {
            RegisterEvent();
        }

        private void OnDisable()
        {
            UnregisterEvent();
        }

        public void OnEvent(ExperienceChangeEvent @event)
        {
            UpdateLevel(@event.CurrentLevel);
            UpdateExperience(@event.CurrentExperience, @event.MaxExperienceForNextLevel);
        }

        public void UpdateLevel(int level) 
        {
            //TODO change to localization

            _levelText.text = $"Level: {level}";
        }

        public void UpdateExperience(float currentXp, float xpToNextLevel)
        {
            _experienceImage.fillAmount = currentXp / xpToNextLevel;
        }
    }
}


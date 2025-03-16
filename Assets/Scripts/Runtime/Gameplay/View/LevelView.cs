using TandC.GeometryAstro.EventBus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TandC.GeometryAstro.Gameplay 
{
    public class LevelView : MonoBehaviour, IEventReceiver<ExpirienceChangeEvent>
    {
        [SerializeField]
        private Image _expirienceImage;

        [SerializeField]
        private TextMeshProUGUI _levelText;

        public UniqueId Id => new UniqueId();

        private void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this as IEventReceiver<ExpirienceChangeEvent>);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this as IEventReceiver<ExpirienceChangeEvent>);
        }

        private void OnEnable()
        {
            RegisterEvent();
        }

        private void OnDisable()
        {
            UnregisterEvent();
        }

        public void OnEvent(ExpirienceChangeEvent @event)
        {
            UpdateLevel(@event.CurrentLevel);
            UpdateExpririence(@event.CurrentExpirience, @event.MaxExpirienceForNextLevel);
        }

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


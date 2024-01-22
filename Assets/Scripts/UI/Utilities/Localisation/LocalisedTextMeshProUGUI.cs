using Studio.ProjectSystems;
using Studio.Settings;
using TMPro;
using UnityEngine;
using Zenject;

namespace Studio.UI.Utilities.Localisation
{
    public class LocalisedTextMeshProUGUI : MonoBehaviour
    {
        [SerializeField] private string key;
        private TextMeshProUGUI _textMesh;

        [Inject] private LocalisationSystem _localisationSystem;

        protected void Awake()
        {
            _textMesh = GetComponent<TextMeshProUGUI>();

            //EventBus.OnLanguageWasChangedEvent += OnLanguageWasChangedEventHandler;
        }

        protected void OnDestroy()
        {
            //EventBus.OnLanguageWasChangedEvent -= OnLanguageWasChangedEventHandler;
        }

        protected void OnEnable()
        {
            _textMesh.text = _localisationSystem.GetString(key, gameObject);
        }

        private void OnLanguageWasChangedEventHandler(Languages lang)
        {
            _textMesh.text = _localisationSystem.GetString(key, gameObject);
        }
    }
}
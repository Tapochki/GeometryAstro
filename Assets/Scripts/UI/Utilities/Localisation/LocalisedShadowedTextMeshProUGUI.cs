using TandC.ProjectSystems;
using TandC.Settings;
using TandC.Utilities;
using UnityEngine;
using Zenject;

namespace TandC.UI.Utilities.Localisation
{
    public class LocalisedShadowedTextMeshProUGUI : MonoBehaviour
    {
        [SerializeField] private string key;
        private ShadowedTextMexhProUGUI _textMesh;

        [Inject] private LocalisationSystem _localisationSystem;

        protected void Awake()
        {
            _textMesh = GetComponent<ShadowedTextMexhProUGUI>();

            //EventBus.OnLanguageWasChangedEvent += OnLanguageWasChangedEventHandler;
        }

        protected void OnDestroy()
        {
            //EventBus.OnLanguageWasChangedEvent -= OnLanguageWasChangedEventHandler;
        }

        protected void OnEnable()
        {
            if (_textMesh == null)
            {
                return;
            }

            if (_localisationSystem == null)
            {
                return;
            }

            if (gameObject == null)
            {
                return;
            }

            _textMesh.UpdateTextAndShadowValue(_localisationSystem.GetString(key, gameObject));
        }

        private void OnLanguageWasChangedEventHandler(Languages lang)
        {
            if (_textMesh == null)
            {
                return;
            }

            if (_localisationSystem == null)
            {
                return;
            }

            if (gameObject == null)
            {
                return;
            }

            _textMesh.UpdateTextAndShadowValue(_localisationSystem.GetString(key, gameObject));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Xml;
using TandC.GeometryAstro.Settings;
using TandC.GeometryAstro.Utilities.Logging;
using UnityEngine;

namespace TandC.GeometryAstro.Services
{
    public class LocalisationService : MonoBehaviour
    {
        public event Action<Languages> OnLanguageWasChangedEvent;

        private readonly string _pathToLocalisation = "Localisation/";

        private Dictionary<string, string> _stringsDict;
        private XmlDocument _languageXMLFile;
        private TextAsset _languageAsset;

        private bool _isLanguageLoaded;

        private DataService _dataSystem;

        public SystemLanguage CurrentLanguage { get; private set; }

        public void SetLanguage(SystemLanguage lang) => CurrentLanguage = lang;

        public void Construct(DataService dataSystem)
        {
            _dataSystem = dataSystem;
        }

        public void Initialize()
        {
            _dataSystem.OnCacheLoadedEvent += OnCacheLoadedEventHandler;
        }

        private void DetectLanguage()
        {
            CurrentLanguage = GetSavedLanguage();
        }

        public void LoadLanguage()
        {
            _languageXMLFile = new XmlDocument();
            _stringsDict = new Dictionary<string, string>();

            _languageAsset = Resources.Load<TextAsset>($"{_pathToLocalisation}" + CurrentLanguage);

            if (_languageAsset == null)
            {
                Log.Default.ThrowException($"Language file [{CurrentLanguage}] not found, loaded default: English");

                _languageAsset = Resources.Load<TextAsset>($"{_pathToLocalisation}English");

                if (_languageAsset == null)
                {
                    Log.Default.ThrowException($"Language file [{CurrentLanguage}] not found");
                    return;
                }
            }

            if (_languageAsset.text == string.Empty)
            {
                return;
            }

            _languageXMLFile.LoadXml(_languageAsset.text);

            XmlElement root = _languageXMLFile.DocumentElement;
            XmlNodeList nodes = root.SelectNodes("//string"); //ignore all nodes but <string>

            foreach (XmlNode node in nodes)
            {
                string key = node.Attributes["name"].Value;
                if (!_stringsDict.ContainsKey(key))
                {
                    _stringsDict.Add(key, node.InnerText);
                }
                else
                {
                    Log.Default.ThrowException($"Alias [{key}] already exist in file [{CurrentLanguage}]");
                }
            }
            _isLanguageLoaded = true;
            OnLanguageWasChangedEvent?.Invoke((Languages)CurrentLanguage);
        }

        public string GetString(string searchString, UnityEngine.Object context = null)
        {
            if (!_isLanguageLoaded)
            {
                DetectLanguage();
                LoadLanguage();
            }

            if (_stringsDict.ContainsKey(searchString))
            {
                return _stringsDict[searchString];
            }
            else
            {
                Log.Default.ThrowException($"Unknown string: [{searchString}] in object [{context}]");
                return "^" + searchString;
            }
        }

        private SystemLanguage GetSavedLanguage()
        {
            if (AppConstants.LANGUAGE_CAN_CHANGE_IN_GAME)
            {
                CurrentLanguage = (SystemLanguage)_dataSystem.AppSettingsData.appLanguage;
            }
            else
            {
                CurrentLanguage = Application.systemLanguage;
            }

            CurrentLanguage = SystemLanguage.English;

            return CurrentLanguage;
        }

        public void UpdateLocalisation(Languages languages)
        {
            CurrentLanguage = (SystemLanguage)languages;
            LoadLanguage();
        }

        private void OnCacheLoadedEventHandler()
        {
            CurrentLanguage = GetSavedLanguage();

            Log.Default.D($"Loaded language is [{CurrentLanguage}]");

            LoadLanguage();
        }
    }
}
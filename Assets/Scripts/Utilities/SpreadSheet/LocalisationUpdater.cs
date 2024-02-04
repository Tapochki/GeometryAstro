using Studio.Utilities.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace Studio.Utilities
{
    public class LocalisationUpdater : MonoBehaviour
    {
        [SerializeField] private string _spreadsheetId = "1OiVTcTFqvMneKSivHXrfYYxvqKC5z0smVA7Txw3y_go";

        private Dictionary<SpreadsheetDataType, SpreadsheetInfo> _spreadsheetsInfo;

        [ContextMenu("Update Localisation")]
        public async void StartUpdatingLocalisation()
        {
            FillSpreadsheetsInfo();

            await StartLoadSpreadsheetsData();

            SyncLocalisationWithSpreadsheet();

            AssetDatabase.Refresh();
        }

        private async Task StartLoadSpreadsheetsData()
        {
            foreach (var item in _spreadsheetsInfo)
            {
                await item.Value.LoadSpreadsheetData();
            }
        }

        private void FillSpreadsheetsInfo()
        {
            _spreadsheetsInfo = new Dictionary<SpreadsheetDataType, SpreadsheetInfo>
            {
                { SpreadsheetDataType.Localization, new SpreadsheetInfo(_spreadsheetId) }
            };
        }

        private void SyncLocalisationWithSpreadsheet()
        {
            var spreadsheet = _spreadsheetsInfo[SpreadsheetDataType.Localization];//_dataSystem.GetSpreadsheetByType(SpreadsheetDataType.Localization);

            if (spreadsheet == null)
            {
                Debug.LogError("Failed to refresh localization. Spreadsheet is null.");
                return;
            }

            var localisationSheetData = spreadsheet.GetObject<LocalisationSheetData>();

            foreach (var lang in Enum.GetValues(typeof(LanguageTypes)))
            {
                XmlWriter writer = XmlWriter.Create($"Assets/Resources/Localisation/{lang}.xml");

                writer.WriteStartDocument();
                writer.WriteStartElement("Document");
                foreach (var element in localisationSheetData)
                {
                    switch (lang)
                    {
                        case LanguageTypes.Russian:
                            writer.WriteStartElement("string");
                            writer.WriteAttributeString("name", element.Key);
                            writer.WriteString(element.Russian);
                            writer.WriteEndElement();
                            break;

                        case LanguageTypes.Ukrainian:
                            writer.WriteStartElement("string");
                            writer.WriteAttributeString("name", element.Key);
                            writer.WriteString(element.Ukrainian);
                            writer.WriteEndElement();
                            break;

                        case LanguageTypes.English:
                            writer.WriteStartElement("string");
                            writer.WriteAttributeString("name", element.Key);
                            writer.WriteString(element.English);
                            writer.WriteEndElement();
                            break;
                    }
                }
                writer.WriteEndElement();
                writer.Close();
            }
        }

        internal enum LanguageTypes
        {
            Russian,
            Ukrainian,
            English,
        }

        internal enum SpreadsheetDataType
        {
            Localization,
        }

        internal class LocalisationSheetData
        {
            public string Key;
            public string English;
            public string Ukrainian;
            public string Russian;
        }
    }
}
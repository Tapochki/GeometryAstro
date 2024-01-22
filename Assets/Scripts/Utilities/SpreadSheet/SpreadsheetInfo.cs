using Studio.Utilities.SpreadSheet;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Studio.Utilities.Spreadsheet
{
    public class SpreadsheetInfo
    {
        public string SpreadsheetId { get; private set; } = string.Empty;
        public string Gid { get; private set; } = "0";
        public string Format { get; private set; } = "csv";
        public string Data { get; private set; } = string.Empty;

        private bool _isLoaded;

        private CSVParser _csvParser;

        public SpreadsheetInfo(string spreadSheetId)
        {
            SpreadsheetId = spreadSheetId;

            _csvParser = new CSVParser();
        }

        public async Task LoadSpreadsheetData()
        {
            Data = await GetRequest($"https://docs.google.com/spreadsheets/export?id={SpreadsheetId}&exportFormat={Format}&gid={Gid}");

            _isLoaded = Data != null;

#if UNITY_EDITOR
            if (_isLoaded)
            {
                Utilities.Logger.Log($"Spreadsheet loaded: {SpreadsheetId}:{Gid}", Settings.LogTypes.Info);
            }
            else
            {
                Utilities.Logger.Log($"Spreadsheet not loaded: {SpreadsheetId}:{Gid}", Settings.LogTypes.Error);
            }
#endif
        }

        public List<T> GetObject<T>()
        {
            if (!_isLoaded)
            {
                return null;
            }

            return _csvParser.ParseCSV<T>(Data);
        }

        public void Dispose()
        {
            Data = null;
        }

        private async Task<string> GetRequest(string url, Dictionary<string, string> headers = null)
        {
            using (UnityWebRequest uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET))
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        uwr.SetRequestHeader(header.Key, header.Value);
                    }
                }

                DownloadHandler downloadHandler = new DownloadHandlerBuffer();
                uwr.downloadHandler = downloadHandler;

                var operation = uwr.SendWebRequest();

                while (!operation.isDone)
                {
                    await Task.Delay(100);
                }

                if (uwr.result != UnityWebRequest.Result.Success)
                {
#if UNITY_EDITOR
                    Utilities.Logger.Log($"Failed to load: {url} due to error: {uwr.error}", Settings.LogTypes.Error);
#endif
                    return null;
                }
                else
                {
                    return System.Text.Encoding.UTF8.GetString(uwr.downloadHandler.data);
                }
            }
        }
    }
}
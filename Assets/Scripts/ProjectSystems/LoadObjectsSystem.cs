using ChebDoorStudio.Settings;
using UnityEngine;
using Zenject;

namespace ChebDoorStudio.ProjectSystems
{
    public class LoadObjectsSystem : MonoBehaviour
    {
        [Inject]
        public void Construct()
        {
            Utilities.Logger.Log("LoadObjectsSystem Construct", LogTypes.Info);
        }

        public void Initialize()
        {
        }

        public T GetObjectByPath<T>(string path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(ParsePath(path));
        }

        public T[] GetObjectsByPath<T>(string path) where T : UnityEngine.Object
        {
            return Resources.LoadAll<T>(ParsePath(path));
        }

        private string ParsePath(string path)
        {
            string[] parsed = path.Split('/');
            path = string.Empty;

            for (int i = 0; i < parsed.Length; i++)
            {
                path += parsed[i][0].ToString().ToUpper() + parsed[i].Substring(1, parsed[i].Length - 1);

                if (i < parsed.Length - 1)
                {
                    path += '/';
                }
            }
            return path;
        }
    }
}
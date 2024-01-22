using TMPro;
using UnityEngine;

namespace Studio.Utilities
{
    public class AppVersionChecker : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _versionText;

        private void Awake()
        {
            _versionText.text = "VERSION - " + Application.version.ToString();
        }
    }
}
using TandC.GeometryAstro.Utilities;
using TMPro;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class MoneyVaultView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _moneyCountText;

        private int _currentMoney;

        public void Init()
        {
            _currentMoney = 0;
            UpdateText(0);
        }

        public void UpdateText(int newMoney)
        {
            InternalTools.DOTextInt(_moneyCountText, _currentMoney, newMoney, 0.5f);

            _currentMoney = newMoney;
        }
    }
}


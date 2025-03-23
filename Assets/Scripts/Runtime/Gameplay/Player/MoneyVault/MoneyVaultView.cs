using DG.Tweening;
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
            DOTween.To(() => _currentMoney, x =>
            {
                _currentMoney = x;
                _moneyCountText.text = _currentMoney.ToString();
            }, newMoney, 0.5f)
            .SetEase(Ease.OutQuad);
        }
    }
}


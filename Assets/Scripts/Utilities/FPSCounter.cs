using UnityEngine;

namespace Studio.Utilities
{
    public class FPSCounter : MonoBehaviour
    {
        private int _frameLimit = 500;

        private int[] _fpsBuffer;
        private int _fpsIndex;

        [SerializeField]
        private TMPro.TextMeshProUGUI _fpsText;

        public int AvarageFPS { get; private set; }

        public void Update()
        {
            if (_fpsBuffer == null || _frameLimit != _fpsBuffer.Length)
            {
                InitBuffer();
            }

            UpdateBuffer();
            CalculateFPS();

            _fpsText.text = "FPS: " + Mathf.Clamp(AvarageFPS, 0, _frameLimit).ToString();
        }

        private void InitBuffer()
        {
            if (_frameLimit <= 0)
            {
                _frameLimit = 1;
            }

            _fpsBuffer = new int[_frameLimit];
            _fpsIndex = 0;
        }

        private void UpdateBuffer()
        {
            _fpsBuffer[_fpsIndex++] = (int)(1f / Time.unscaledDeltaTime);

            if (_fpsIndex >= _frameLimit)
            {
                _fpsIndex = 0;
            }
        }

        private void CalculateFPS()
        {
            int sum = 0;

            for (int i = 0; i < _frameLimit; i++)
            {
                int fps = _fpsBuffer[i];
                sum += fps;
            }

            AvarageFPS = sum / _frameLimit;
        }
    }
}
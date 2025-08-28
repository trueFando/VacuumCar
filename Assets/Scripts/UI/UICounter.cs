using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UICounter : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI _countText;

        protected Color _defaultColor;

        private GraphicBlinker _blinker;
        private Coroutine _blinkCoroutine;

        protected virtual void Awake()
        {
            _defaultColor = _countText.color;
            _blinker = new GraphicBlinker(_countText);
        }

        public virtual void SetCount(int count, int? capacity = null, bool isInitial = false)
        {
            _countText.text = capacity != null ? $"{count}/{capacity}" : $"{count}";
        }
 
        protected virtual void RunBlinking(Color color, float durationSeconds, float intervalSeconds)
        {
            if (_blinkCoroutine != null)
            {
                StopCoroutine(_blinkCoroutine);
            }

            _blinkCoroutine = StartCoroutine(_blinker.Blink(color, durationSeconds, intervalSeconds));
        }
    }
}
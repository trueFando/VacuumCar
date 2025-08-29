using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GraphicBlinker
    {
        private readonly Graphic _graphic;
        private readonly Color _originalColor;

        public GraphicBlinker(Graphic graphic)
        {
            _graphic = graphic;
            _originalColor = graphic.color;
        }

        public IEnumerator Blink(Color blinkColor, float duration, float interval)
        {
            var elapsed = 0f;
            var toggle = false;

            while (elapsed < duration)
            {
                _graphic.color = toggle ? blinkColor : _originalColor;
                toggle = !toggle;

                yield return new WaitForSeconds(interval);
                elapsed += interval;
            }

            _graphic.color = _originalColor;
        }
    }
}
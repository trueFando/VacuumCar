using System.Collections;
using Extensions;
using UnityEngine;

namespace UI
{
    public class ContainerCounter : UICounter
    {
        private Coroutine _blinkingRoutine;

        private Color _yellowWithAlpha;
        private Color _redWithAlpha;

        protected override void Awake()
        {
            base.Awake();

            _yellowWithAlpha = Color.yellow.WithAlpha(_defaultColor.a);
            _redWithAlpha = Color.red.WithAlpha(_defaultColor.a);
        }

        public override void SetCount(int count, int? capacity = null, bool isInitial = false)
        {
            base.SetCount(count, capacity, isInitial);

            _countText.color = ((float)count / capacity) switch
            {
                < 0.6f => _defaultColor,
                < 0.8f => _yellowWithAlpha,
                _ => _redWithAlpha
            };
        }
        
        public void NotifyFull()
        {
            if (_blinkingRoutine != null)
            {
                StopCoroutine(_blinkingRoutine);
            }
            
            _blinkingRoutine = StartCoroutine(Blink(Color.red));
        }

        private IEnumerator Blink(Color color)
        {
            for (var i = 0; i < 10; i++)
            {
                _countText.color = Color.white;

                yield return new WaitForSeconds(0.1f);
                
                _countText.color = color;

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
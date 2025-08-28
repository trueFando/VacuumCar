using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ContainerCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _countText;

        private Coroutine _blinkingRoutine;

        public void SetCount(int count, int capacity)
        {
            _countText.text = $"{count}/{capacity}";

            _countText.color = count == capacity ? Color.red : Color.white;
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
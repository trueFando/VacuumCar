using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HealthCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _countText;
        
        public void SetCount(int count)
        {
            _countText.text = count.ToString();

            StartCoroutine(Blink(Color.red));
        }

        private IEnumerator Blink(Color color)
        {
            for (var i = 0; i < 10; i++)
            {
                _countText.color = color;

                yield return new WaitForSeconds(0.1f);
                
                _countText.color = Color.white;

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
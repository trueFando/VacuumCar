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
        }
    }
}
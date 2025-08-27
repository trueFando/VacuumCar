using TMPro;
using UnityEngine;

namespace UI
{
    public class ContainerCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _countText;
        
        public void SetCount(int count, int capacity)
        {
            _countText.text = $"{count}/{capacity}";
        }
    }
}
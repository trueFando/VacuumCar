using TMPro;
using UnityEngine;

namespace UI
{
    public class CollectorCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _countText;

        public void SetCount(int count, int target)
        {
            _countText.text = $"{count}/{target}";
        }
    }
}
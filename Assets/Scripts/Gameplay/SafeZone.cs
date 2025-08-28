using UnityEngine;

namespace Gameplay
{
    public class SafeZone : MonoBehaviour
    {
        [SerializeField] private float _startScale;
        [SerializeField] private float _minimumScale;
        [SerializeField] private float _narrowingSpeed;

        private void Update()
        {
            transform.localScale -= new Vector3(1f, 1f, 0f) * (_narrowingSpeed * Time.deltaTime);
            
            transform.localScale = Vector3.Max(transform.localScale, Vector3.one * _minimumScale);
        }
    }
}
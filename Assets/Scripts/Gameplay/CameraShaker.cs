using System.Collections;
using UnityEngine;

namespace Gameplay
{
    public class CameraShaker : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.2f;
        [SerializeField] private float _magnitude = 0.1f;
        
        private Vector3 _originalPos;
        
        private Coroutine _shakeCoroutine;

        public void Shake()
        {
            Destroy(GetComponent<Animator>());
            
            if (_shakeCoroutine != null)
            {
                StopCoroutine(_shakeCoroutine);
                _shakeCoroutine = null;
            }

            _shakeCoroutine = StartCoroutine(ShakeCoroutine());
        }

        private IEnumerator ShakeCoroutine()
        {
            _originalPos = transform.localPosition;

            var elapsed = 0.0f;

            while (elapsed < _duration)
            {
                var x = Random.Range(-1f, 1f) * _magnitude;
                var y = Random.Range(-1f, 1f) * _magnitude;

                transform.localPosition = _originalPos + new Vector3(x, y, 0);

                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = _originalPos;
        }
    }
}
using System.Collections;
using UnityEngine;

namespace Gameplay
{
    public class CameraShaker : MonoBehaviour
    {
        private Vector3 _originalPos;
        
        private Coroutine _shakeCoroutine;

        public void Shake(float duration = 0.2f, float magnitude = 0.1f)
        {
            if (_shakeCoroutine != null)
            {
                StopCoroutine(_shakeCoroutine);
                _shakeCoroutine = null;
            }

            _shakeCoroutine = StartCoroutine(ShakeCoroutine(duration, magnitude));
        }

        private IEnumerator ShakeCoroutine(float duration, float magnitude)
        {
            _originalPos = transform.localPosition;

            var elapsed = 0.0f;

            while (elapsed < duration)
            {
                var x = Random.Range(-1f, 1f) * magnitude;
                var y = Random.Range(-1f, 1f) * magnitude;

                transform.localPosition = _originalPos + new Vector3(x, y, 0);

                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = _originalPos;
        }
    }
}
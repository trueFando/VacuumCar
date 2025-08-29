using System.Collections;
using System.Collections.Generic;
using UI;
using UI.Menu;
using UnityEngine;

namespace Gameplay
{
    public class Collector : MonoBehaviour
    {
        [SerializeField] private CollectorCounter _counter;
        [SerializeField] private Transform _pickableIdlePrefab;

        [SerializeField] private int _targetCount;
        private int _currentCount;

        private void Start()
        {
            _counter.SetCount(_currentCount, _targetCount, true);
        }

        public void Collect(int count)
        {
            _currentCount += count;

            _counter.SetCount(_currentCount, _targetCount);
            
            Animate(count);

            if (_currentCount >= _targetCount)
            {
                Win();
            }
        }

        private void Animate(int count)
        {
            var toVacuum = new List<Transform>();

            for (var i = 0; i < count; i++)
            {
                var position = transform.position 
                               + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);

                var idlePickable = Instantiate(_pickableIdlePrefab, position, Quaternion.identity);

                toVacuum.Add(idlePickable);
            }

            StartCoroutine(Vaccum(toVacuum));
        }

        private IEnumerator Vaccum(List<Transform> pickables)
        {
            if (pickables == null || pickables.Count == 0)
            {
                yield break;
            }

            var defaultScale = pickables[0].localScale;
            
            while (pickables.Count > 0)
            {
                var toRemove = new List<Transform>();

                foreach (var pickable in pickables)
                {
                    pickable.position = Vector3.MoveTowards(pickable.position, transform.position, Time.deltaTime);
                    pickable.localScale = Vector3.Lerp(
                        defaultScale,
                        Vector3.zero,
                        1f - Vector3.Distance(pickable.position, transform.position) / 0.5f
                    );

                    if (Vector3.Distance(pickable.position, transform.position) < 0.05f)
                    {
                        toRemove.Add(pickable);
                    }
                }

                foreach (var pickable in toRemove)
                {
                    pickables.Remove(pickable);

                    Destroy(pickable.gameObject);
                }

                yield return null;
            }
        }

        private void Win()
        {
            Debug.Log("Win");

            GameOverMenu.Instance.NextLevel();
        }
    }
}
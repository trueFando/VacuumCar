using System;
using Gameplay.Pickable;
using UI;
using UnityEngine;

namespace Gameplay
{
    public class VacuumContainer : MonoBehaviour
    {
        public event Action<int> OnBadPickableCollected;

        [SerializeField] private ContainerCounter _containerCounter;
        [SerializeField] private Animator _animator;

        [SerializeField] private int _capacity;

        private int _collectedCount;

        private int CollectedCount
        {
            get => _collectedCount;
            set
            {
                _collectedCount = value;

                _containerCounter.SetCount(value, _capacity);
            }
        }

        private void Awake()
        {
            CollectedCount = 0;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out PickableBase pickable))
            {
                TryCollect(pickable);
            }

            if (other.TryGetComponent(out Collector collector))
            {
                collector.Collect(CollectedCount);

                CollectedCount = 0;
            }
        }

        private void TryCollect(PickableBase pickable)
        {
            if (pickable is PickableTrap)
            {
                CollectBad(pickable);

                return;
            }
            
            if (CollectedCount + 1 > _capacity)
            {
                CollectBad(pickable);

                return;
            }
            
            CollectedCount += pickable.Mass;
            
            _animator.SetTrigger("Collect");

            Destroy(pickable.gameObject);
        }

        private void CollectBad(PickableBase pickable)
        {
            pickable.Rigidbody.AddForce(
                (pickable.Rigidbody.transform.position - transform.position) * 50f,
                ForceMode2D.Impulse
            );
            
            OnBadPickableCollected?.Invoke(pickable.Mass);
        }
    }
}
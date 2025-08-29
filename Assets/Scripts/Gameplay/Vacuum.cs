using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Gameplay.Pickable;
using UnityEngine;

namespace Gameplay
{
    public class Vacuum : MonoBehaviour
    {
        private const float MaxDistance = 3f;

        public VacuumMode Mode => _mode;
        
        [SerializeField] private VacuumMode _mode;

        [SerializeField] private VacuumContainer _vacuumContainer;
        [SerializeField] private ParticleSystem _vacuumParticles;
        [SerializeField] private ParticleSystem _turboParticles;
        
        private readonly List<Rigidbody2D> _objectsInArea = new();

        private void Awake()
        {
            _vacuumContainer.OnBadPickableCollected += OnBadCollected;
            
            SetMode(VacuumMode.Vacuum);
        }

        private void OnBadCollected(PickableBase pickableBase)
        {
            foreach (var rb in _objectsInArea.Where(rb => rb))
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(transform.up * 5f, ForceMode2D.Impulse);
            }

            if (!_objectsInArea.Contains(pickableBase.Rigidbody))
            {
                pickableBase.Rigidbody.velocity = Vector2.zero;
                pickableBase.Rigidbody.AddForce(transform.up * 5f, ForceMode2D.Impulse);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SwitchMode();
            }
        }

        private void FixedUpdate()
        {
            switch (_mode)
            {
                case VacuumMode.Vacuum:
                    Suck();
                    _vacuumContainer.IsActive = true;
                    break;
                case VacuumMode.Turbo:
                    Blow();
                    _vacuumContainer.IsActive = true;
                    break;
                case VacuumMode.Off:
                    _vacuumContainer.IsActive = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SwitchMode()
        {
            SetMode(_mode.Next() == VacuumMode.Off ? _mode.Next().Next() : _mode.Next());
        }

        public void SetMode(VacuumMode mode)
        {
            _mode = mode;
            
            _turboParticles.Stop();
            _vacuumParticles.Stop();

            switch (mode)
            {
                case VacuumMode.Vacuum:
                    _vacuumParticles.Play();
                    break;
                case VacuumMode.Turbo:
                    _turboParticles.Play();
                    break;
                case VacuumMode.Off:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Rigidbody2D rb))
            {
                return;
            }

            _objectsInArea.Add(rb);
            rb.drag = 0f;
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Rigidbody2D rb))
            {
                return;
            }

            ResetTarget(rb);

            _objectsInArea.Remove(rb);
        }

        private void Suck()
        {
            // TODO:
            foreach (var rb in _objectsInArea.Where(rb => rb))
            {
                rb.AddForce((transform.position - rb.transform.position) 
                    * MaxDistance / Vector3.Distance(transform.position, rb.transform.position) * 10f);
            }
        }

        private void Blow()
        {
            foreach (var rb in _objectsInArea.Where(rb => rb))
            {
                rb.AddForce((rb.transform.position - transform.position) 
                    * MaxDistance / Vector3.Distance(transform.position, rb.transform.position) * 10f);
            }
        }

        private void ResetTarget(Rigidbody2D rb)
        {
            rb.drag = 1f;
        }
    }

    public enum VacuumMode
    {
        Off,
        Vacuum,
        Turbo
    }
}
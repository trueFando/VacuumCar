using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

namespace Gameplay
{
    public class Vacuum : MonoBehaviour
    {
        public VacuumMode Mode => _mode;
        
        [SerializeField] private VacuumMode _mode;

        [SerializeField] private ParticleSystem _vacuumParticles;
        [SerializeField] private ParticleSystem _turboParticles;
        
        private readonly List<Rigidbody2D> _objectsInArea = new();

        private void Awake()
        {
            SetMode(VacuumMode.Vacuum);
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
                    break;
                case VacuumMode.Turbo:
                    Blow();
                    break;
                case VacuumMode.Off:
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
                    // foreach (var rb in _objectsInArea)
                    // {
                    //     ResetTarget(rb);
                    // }
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
                rb.AddForce(transform.position - rb.transform.position);
            }
        }

        private void Blow()
        {
            foreach (var rb in _objectsInArea.Where(rb => rb))
            {
                rb.AddForce(rb.transform.position - transform.position);
            }
        }

        private void ResetTarget(Rigidbody2D rb)
        {
            rb.velocity = Vector2.zero;
        }
    }

    public enum VacuumMode
    {
        Off,
        Vacuum,
        Turbo
    }
}
using System;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    public class Car : MonoBehaviour
    {
        public event Action OnDied;
        
        [SerializeField] private Vacuum _vacuum;
        [SerializeField] private VacuumContainer _vacuumContainer;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Camera _camera;
        [SerializeField] private CameraShaker _cameraShaker;
        [SerializeField] private HealthCounter _healthCounter;
        [SerializeField] private FloatingJoystick _joystick;
        [SerializeField] private ParticleSystem _particleWheel;
        private float _particleDefaultEmission;

        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private int _maxHealth;
        [SerializeField] private float _accidentCooldown;
        private float _timeFromAccident = -1f;

        private int _health;

        private Vector2 _input;

        private void Awake()
        {
            _health = _maxHealth;

            _vacuumContainer.OnBadPickableCollected += pickable => TakeDamage(pickable.Mass);
        }

        private void Start()
        {
            _healthCounter.SetCount(_health, isInitial: true);
        }

        private void Update()
        {
            _input = _joystick.Direction;

            if (_timeFromAccident >= 0f)
            {
                _timeFromAccident += Time.deltaTime;
            }
        }

        private void LateUpdate()
        {
            _camera.transform.localRotation = Quaternion.Euler(-transform.localEulerAngles);
        }

        private void FixedUpdate()
        {
            if (_timeFromAccident >= 0 && _timeFromAccident < _accidentCooldown)
            {
                return;
            }
            
            if (_input.magnitude > 0.1f)
            {
                var targetAngle = -Mathf.Atan2(_input.x, _input.y) * Mathf.Rad2Deg;

                var currentAngle = _rigidbody.rotation;

                var newAngle = Mathf.LerpAngle(currentAngle, targetAngle, 10f * Time.fixedDeltaTime);

                _rigidbody.MoveRotation(newAngle);
            }

            _rigidbody.angularVelocity = 0f;
            _rigidbody.velocity = transform.up * (_movementSpeed * _input.magnitude);

            if (_rigidbody.velocity.magnitude > 0.2f)
            {
                _particleWheel.Play();
            }
            else
            {
                _particleWheel.Stop();
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out Obstacle obstacle))
            {
                _timeFromAccident = 0f;

                TakeDamage(1);

                _rigidbody.AddForce(((Vector2)transform.position - other.contacts[0].point), ForceMode2D.Impulse);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent(out SafeZone safeZone))
            {
                return;
            }

            Die();
        }

        private void TakeDamage(int damage)
        {
            _health = Mathf.Max(_health - damage, 0);
            
            _cameraShaker.Shake();
            
            TurnOffVacuum(3f);
            
            _healthCounter.SetCount(_health, isInitial: true);

            if (_health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            OnDied?.Invoke();
        }

        private void TurnOffVacuum(float time)
        {
            StartCoroutine(DelayVacuum(time));
        }

        private IEnumerator DelayVacuum(float time)
        {
            var mode = _vacuum.Mode;
            
            _vacuum.SetMode(VacuumMode.Off);
            
            yield return new WaitForSeconds(time);

            // если юзер сам не включил во время кулдауна
            if (_vacuum.Mode == VacuumMode.Off)
            {
                _vacuum.SetMode(mode);   
            }
        }
    }
}

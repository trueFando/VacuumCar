using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Gameplay
{
    public class Car : MonoBehaviour
    {
        [SerializeField] private Vacuum _vacuum;
        [SerializeField] private VacuumContainer _vacuumContainer;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Camera _camera;
        [SerializeField] private CameraShaker _cameraShaker;
        [SerializeField] private HealthCounter _healthCounter;
        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _rightButton;

        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private int _maxHealth;
        [SerializeField] private float _accidentCooldown;
        private float _timeFromAccident = -1f;

        private int _health;

        private Vector2 _input;

        private bool _movingForward = true;

        private bool _leftPressed;
        private bool _rightPressed;

        private void Awake()
        {
            _health = _maxHealth;

            _healthCounter.SetCount(_health);

            _vacuumContainer.OnBadPickableCollected += TakeDamage;
        }

        public void PressLeft()
        {
            _leftPressed = true;
        }

        public void ReleaseLeft()
        {
            _leftPressed = false;
        }
        
        public void PressRight()
        {
            _rightPressed = true;
        }

        public void ReleaseRight()
        {
            _rightPressed = false;
        }

        private void OnDestroy()
        {
            _vacuumContainer.OnBadPickableCollected -= TakeDamage;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                _movingForward = !_movingForward;
            }

            // _input = _movingForward
            //     ? new Vector2(Input.GetAxis("Horizontal"), 1f)
            //     : new Vector2(Input.GetAxis("Horizontal"), -1f);

            var x = _leftPressed ? -1f : 0f;
            x += _rightPressed ? 1f : 0f;
            
            _input = _movingForward
                ? new Vector2(x, 1f)
                : new Vector2(x, -1f);

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

            _rigidbody.angularVelocity = 0f;
            _rigidbody.velocity = transform.up * (_movementSpeed * _input.y);
            _rigidbody.SetRotation(transform.eulerAngles.z - _rotationSpeed * _input.x);
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

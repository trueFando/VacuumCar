using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private int _maxHealth;

        private int _health;

        private Vector2 _input;

        private void Awake()
        {
            _health = _maxHealth;

            _healthCounter.SetCount(_health);

            _vacuumContainer.OnBadPickableCollected += TakeDamage;
        }

        private void OnDestroy()
        {
            _vacuumContainer.OnBadPickableCollected -= TakeDamage;
        }

        private void Update()
        {
            _input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }

        private void LateUpdate()
        {
            _camera.transform.localRotation = Quaternion.Euler(-transform.localEulerAngles);
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = transform.up * (_movementSpeed * _input.y);
            _rigidbody.SetRotation(transform.eulerAngles.z - _rotationSpeed * _input.x);
        }

        private void TakeDamage(int damage)
        {
            Debug.Log($"TakeDamage {damage}");

            _health = Mathf.Max(_health - damage, 0);
            
            _cameraShaker.Shake();
            
            TurnOffVacuum(3f);
            
            _healthCounter.SetCount(_health);

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
            _vacuum.SetMode(VacuumMode.Off);
            
            yield return new WaitForSeconds(time);
            
            _vacuum.SetMode(VacuumMode.Vacuum);
        }
    }
}

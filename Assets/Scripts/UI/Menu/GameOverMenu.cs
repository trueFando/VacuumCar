using System;
using System.Collections;
using Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Menu
{
    public class GameOverMenu : MonoBehaviour
    {
        public static GameOverMenu Instance;
        
        [SerializeField] private Animator _fadeAnimator;
        [SerializeField] private GameObject _gameOverPanel;

        private Car _car;
        
        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            
            _gameOverPanel.SetActive(false);

            _car = FindAnyObjectByType<Car>();

            if (!_car)
            {
                throw new ApplicationException("Car is missing");
            }

            _car.OnDied += OnGameOver;
        }

        private void OnDestroy()
        {
            _car.OnDied -= OnGameOver;
        }

        public void Retry()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void NextLevel()
        {
            var nextLevel = Convert.ToInt32(SceneManager.GetActiveScene().name[^1].ToString()) + 1;

            StartCoroutine(WaitForFade(
                () => { SceneManager.LoadScene(nextLevel < 5 ? $"Level{nextLevel}" : "Menu"); },
                1f)
            );
        }

        private void OnGameOver()
        {
            StartCoroutine(WaitForFade(() => _gameOverPanel.SetActive(true), 0.2f));

            _fadeAnimator.SetTrigger("FadeIn");
        }

        private IEnumerator WaitForFade(Action callback, float timeBeforeFade = 0f)
        {
            yield return new WaitForSeconds(timeBeforeFade);
            
            _fadeAnimator.SetTrigger("FadeIn");

            yield return new WaitForSeconds(0.3f);

            callback();
        }
    }
}
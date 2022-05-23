using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BrickBreaker
{
    public class GameUi : MonoBehaviour
    {
        [SerializeField] private TMP_Text livesText;
        [SerializeField] private TMP_Text currentScoreText;
        [SerializeField] private TMP_Text highScoreText;
        [SerializeField] private GameObject pauseScreen;

        private void Start()
        {
            GameManager.Instance.onScoreChanged.AddListener(UpdateCurrentScore);
            GameManager.Instance.onLivesChanged.AddListener(UpdateLives);
        }

        /// <summary>
        /// Update the lives text
        /// </summary>
        public void UpdateLives(int lives)
        {
            livesText.text = $"Lives: {lives}";
        }

        /// <summary>
        /// Update the current score text
        /// </summary>
        public void UpdateCurrentScore(int score)
        {
            currentScoreText.text = $"Score: {score}";
        }

        public void UpdateHighScore(HighScoreData highScore)
        {
            highScoreText.text = highScore.playerName == string.Empty ? 
                $"High Score: {highScore.highScore}" : 
                $"High Score: {highScore.playerName}: {highScore.highScore}";
        }

        /// <summary>
        /// Set pause menu visibility
        /// </summary>
        public void SetPauseScreenVisible(bool isVisible)
        {
            pauseScreen.SetActive(isVisible);
        }

        /// <summary>
        /// Go back to menu
        /// </summary>
        public void BackToMenu()
        {
            GameManager.Instance.CheckHighScore();
            GameManager.Instance.TogglePause();
            SceneManager.LoadScene(0);
        }

        private void OnDestroy()
        {
            GameManager.Instance.onScoreChanged.RemoveListener(UpdateCurrentScore);
            GameManager.Instance.onLivesChanged.RemoveListener(UpdateLives);
        }
    }
}
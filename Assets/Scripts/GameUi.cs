using TMPro;
using UnityEngine;

namespace BrickBreaker
{
    public class GameUi : MonoBehaviour
    {
        [SerializeField] private TMP_Text livesText;
        [SerializeField] private TMP_Text currentScoreText;
        [SerializeField] private TMP_Text highScoreText;

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
        private void UpdateCurrentScore(int score)
        {
            currentScoreText.text = $"Score: {score}";
        }

        private void OnDestroy()
        {
            GameManager.Instance.onScoreChanged.RemoveListener(UpdateCurrentScore);
            GameManager.Instance.onLivesChanged.RemoveListener(UpdateLives);
        }
    }
}
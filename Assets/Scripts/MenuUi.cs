using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BrickBreaker
{
    public class MenuUi : MonoBehaviour
    {
        [SerializeField] private TMP_Text highScoreText;
        [SerializeField] private TMP_InputField playerNameField;

        /// <summary>
        /// Set the high score text
        /// </summary>
        public void SetHighScore(HighScoreData highScore)
        {
            highScoreText.text = highScore.playerName == string.Empty ? 
                $"High Score: {highScore.highScore}" : 
                $"High Score: {highScore.playerName}: {highScore.highScore}";
        }

        /// <summary>
        /// Go to the game scene
        /// </summary>
        public void PlayGame()
        {
            GameManager.Instance.SetPlayerName(playerNameField.text);
            SceneManager.LoadScene(1);
        }

        /// <summary>
        /// Quit the game
        /// </summary>
        public void QuitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
            #else
            Application.Quit();
            #endif
        }
    }
}
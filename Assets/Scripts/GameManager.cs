using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace BrickBreaker
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton
        public static GameManager Instance;
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
        #endregion

        private string _currentPlayerName;
        private PaddleController _paddle;
        [SerializeField] private Ball ballPrefab;
        [SerializeField] private int currentScore;
        [SerializeField] private int lives = 3;

        public Ball CurrentBall { get; private set; }
        [SerializeField] private HighScoreData highScore;
        private bool _gameIsPaused;
        public bool GameIsPaused => _gameIsPaused;

        [Header("Events")]
        public UnityEvent<int> onScoreChanged;
        public UnityEvent<int> onLivesChanged;

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            LoadHighScore();
            SetMenuHighScore();
            FindPaddle();
            InitializeGameUi();
        }
        
        /// <summary>
        /// Called when a new scene is loaded
        /// </summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SetMenuHighScore();
            
            FindPaddle();
            InitializeGameUi();
        }

        /// <summary>
        /// Find the paddle and spawn the ball
        /// </summary>
        private void FindPaddle()
        {
            _paddle = FindObjectOfType<PaddleController>();
            if (_paddle != null)
            {
                lives = 3;
                currentScore = 0;
                SpawnBall();
            }
        }

        /// <summary>
        /// Set the menu high score text
        /// </summary>
        private void SetMenuHighScore()
        {
            var menuUi = FindObjectOfType<MenuUi>();
            if (menuUi != null)
            {
                menuUi.SetHighScore(highScore);
            }
        }

        /// <summary>
        /// Initialize the game UI
        /// </summary>
        private void InitializeGameUi()
        {
            var gameUi = FindObjectOfType<GameUi>();
            if (gameUi != null)
            {
                gameUi.UpdateLives(lives);
                gameUi.UpdateCurrentScore(currentScore);
                gameUi.UpdateHighScore(highScore);
            }
        }

        /// <summary>
        /// Spawn the ball
        /// </summary>
        private void SpawnBall()
        {
            var spawnPos = _paddle.transform.position + Vector3.up * _paddle.BallOffsetY;
            CurrentBall = Instantiate(ballPrefab, spawnPos, ballPrefab.transform.rotation);
            CurrentBall.SetPaddle(_paddle);
            
            CurrentBall.onBallFallBellowPaddle.AddListener(OnBallFall);
        }

        /// <summary>
        /// Called when the ball falls bellow the paddle
        /// </summary>
        private void OnBallFall()
        {
            if (CurrentBall == null) return;
            Destroy(CurrentBall.gameObject);
            
            // Update the current lives count
            lives--;
            onLivesChanged?.Invoke(lives);

            if (lives <= 0)
            {
                CheckHighScore();
                //TODO: Show game over screen
            }
            else
            {
                SpawnBall();
            }
        }

        /// <summary>
        /// Add points to current score
        /// </summary>
        public void AddScore(int value)
        {
            currentScore += value;
            onScoreChanged?.Invoke(currentScore);
        }

        /// <summary>
        /// Set the player name
        /// </summary>
        public void SetPlayerName(string playerName)
        {
            _currentPlayerName = playerName;
        }

        /// <summary>
        /// Check if the current score is higher than the current high score
        /// </summary>
        public void CheckHighScore()
        {
            if (currentScore > highScore.highScore)
            {
                highScore.playerName = _currentPlayerName;
                highScore.highScore = currentScore;
                SaveHighScore();
            }
        }

        /// <summary>
        /// Save the high score in a file
        /// </summary>
        private void SaveHighScore()
        {
            var json = JsonUtility.ToJson(highScore);
            File.WriteAllText(Application.persistentDataPath + "/highscore.json", json);
        }

        /// <summary>
        /// Load the high score save file
        /// </summary>
        private void LoadHighScore()
        {
            var path = Application.persistentDataPath + "/highscore.json";
            if (!File.Exists(path)) return;
            var json = File.ReadAllText(path);
            highScore = JsonUtility.FromJson<HighScoreData>(json);
        }

        /// <summary>
        /// Toggle pause
        /// </summary>
        public void TogglePause()
        {
            var gameUi = FindObjectOfType<GameUi>();
            if (gameUi == null) return;
            _gameIsPaused = !_gameIsPaused;

            if (_gameIsPaused)
            {
                gameUi.SetPauseScreenVisible(true);
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
                gameUi.SetPauseScreenVisible(false);
            }
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
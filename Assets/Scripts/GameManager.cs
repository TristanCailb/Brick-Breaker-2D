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

        private PaddleController _paddle;
        [SerializeField] private Ball ballPrefab;
        [SerializeField] private int currentScore;
        [SerializeField] private int lives = 3;

        public Ball CurrentBall { get; private set; }

        [Header("Events")]
        public UnityEvent<int> onScoreChanged;
        public UnityEvent<int> onLivesChanged;

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            FindPaddle();
            InitializeLivesUi();
        }
        
        /// <summary>
        /// Called when a new scene is loaded
        /// </summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            FindPaddle();
            InitializeLivesUi();
        }

        /// <summary>
        /// Find the paddle and spawn the ball
        /// </summary>
        private void FindPaddle()
        {
            _paddle = FindObjectOfType<PaddleController>();
            if (_paddle != null)
            {
                SpawnBall();
            }
        }

        /// <summary>
        /// Initialize the lives UI
        /// </summary>
        private void InitializeLivesUi()
        {
            var gameUi = FindObjectOfType<GameUi>();
            if (gameUi != null)
            {
                gameUi.UpdateLives(lives);
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

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
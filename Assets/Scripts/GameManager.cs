using UnityEngine;

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
        
        public Ball CurrentBall { get; private set; }

        private void Start()
        {
            _paddle = FindObjectOfType<PaddleController>();
            if (_paddle != null)
            {
                SpawnBall();
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
            
            CurrentBall.onBallFallBellowPaddle.AddListener(DestroyBall);
        }

        /// <summary>
        /// Called when the ball falls bellow the paddle
        /// </summary>
        private void DestroyBall()
        {
            if (CurrentBall == null) return;
            Destroy(CurrentBall.gameObject);
            SpawnBall();
        }

        /// <summary>
        /// Add points to current score
        /// </summary>
        public void AddScore(int value)
        {
            currentScore += value;
        }
    }
}
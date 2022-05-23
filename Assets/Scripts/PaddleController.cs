using UnityEngine;

namespace BrickBreaker
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PaddleController : MonoBehaviour
    {
        private Rigidbody2D _rb;

        [SerializeField] private float speed = 10f;

        private float _horizontalInput;
        public float BallOffsetY => 0.5f;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            GetInputs();
        }

        private void FixedUpdate()
        {
            MovePaddle();
        }

        /// <summary>
        /// Get the inputs from the player
        /// </summary>
        private void GetInputs()
        {
            if (!GameManager.Instance.GameIsPaused)
            {
                // Paddle movement
                _horizontalInput = Input.GetAxis("Horizontal");
                
                // Launch the ball
                if (Input.GetButtonDown("Launch"))
                {
                    var ball = GameManager.Instance.CurrentBall;
                    if (ball != null && !ball.IsLaunched)
                    {
                        ball.Launch();
                    }
                }
            }
            
            // Toggle pause
            if (Input.GetButtonDown("Pause"))
            {
                GameManager.Instance.TogglePause();
            }
        }

        /// <summary>
        /// Move the paddle
        /// </summary>
        private void MovePaddle()
        {
            var pos = _rb.position;
            var desiredPos = pos + Vector2.right * (_horizontalInput * speed * Time.fixedDeltaTime);
            _rb.MovePosition(desiredPos);
        }
    }
}

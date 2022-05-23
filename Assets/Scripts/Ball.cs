using UnityEngine;
using UnityEngine.Events;

namespace BrickBreaker
{
    [RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(TrailRenderer))]
    public class Ball : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private TrailRenderer _trail;
        
        private PaddleController _paddle;
        public bool IsLaunched { get; private set; }
        [SerializeField] private float speed = 5f;
        [SerializeField] private float maxLaunchAngle = 10f;
        private float _bottomLimit = -6f;

        public UnityEvent onBallFallBellowPaddle;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _trail = GetComponent<TrailRenderer>();
            _trail.emitting = false;
        }
        
        private void Update()
        {
            MoveWithPaddle();
            CheckBellowPaddle();
        }

        private void FixedUpdate()
        {
            // Set a fixed velocity
            if (!IsLaunched) return;
            var velocity = _rb.velocity.normalized;
            velocity *= speed * Time.fixedDeltaTime;
            _rb.velocity = velocity;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            // Add a random force to the ball when colliding with another object to avoid infinite bounces
            if (other.GetContact(0).normal.x == 0f)
            {
                _rb.AddForce(Vector2.right * Random.Range(-1f, 1f), ForceMode2D.Impulse);
            }
            else if (other.GetContact(0).normal.y == 0f)
            {
                _rb.AddForce(Vector2.up * Random.Range(0f, 1f), ForceMode2D.Impulse);
            }
            
            // Collision with a brick
            if (other.gameObject.CompareTag("Brick"))
            {
                other.gameObject.GetComponent<Brick>().DestroyBrick();
            }
        }

        /// <summary>
        /// Check if the ball falls bellow the paddle
        /// </summary>
        private void CheckBellowPaddle()
        {
            if (transform.position.y < _bottomLimit)
            {
                onBallFallBellowPaddle?.Invoke();
            }
        }

        /// <summary>
        /// Set the paddle to update the ball position while not launched
        /// </summary>
        public void SetPaddle(PaddleController paddle)
        {
            _paddle = paddle;
        }

        /// <summary>
        /// Move the ball with the paddle if not launched
        /// </summary>
        private void MoveWithPaddle()
        {
            if (IsLaunched || _paddle == null) return;
            var pos = _paddle.transform.position;
            pos.y += _paddle.BallOffsetY;
            transform.position = pos;
        }

        /// <summary>
        /// Launch the ball
        /// </summary>
        public void Launch()
        {
            IsLaunched = true;
            var launchForce = Vector2.up * speed + Vector2.right * (Random.Range(-1f, 1f) * maxLaunchAngle);
            _rb.AddForce(launchForce, ForceMode2D.Impulse);
            _trail.emitting = true;
        }

        private void OnDestroy()
        {
            onBallFallBellowPaddle?.RemoveAllListeners();
        }
    }
}
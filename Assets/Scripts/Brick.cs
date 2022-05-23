using UnityEngine;

namespace BrickBreaker
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Brick : MonoBehaviour
    {
        private SpriteRenderer _rend;
        
        [SerializeField] private ParticleSystem sparksParticle;
        [SerializeField] private int score = 10;
        private bool _hasBeenHitted;

        private void Start()
        {
            _rend = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Destroy the brick when hitted by the ball
        /// </summary>
        public void DestroyBrick()
        {
            if (_hasBeenHitted) return;
            _hasBeenHitted = true;
            GameManager.Instance.AddScore(score);
            SpawnSparks();
            Destroy(gameObject);
        }

        /// <summary>
        /// Spawn the sparks particles
        /// </summary>
        private void SpawnSparks()
        {
            var sparks = Instantiate(sparksParticle, transform.position, Quaternion.identity);
            var main = sparks.main;
            var color = _rend.color;
            main.startColor = color;
        }
    }
}
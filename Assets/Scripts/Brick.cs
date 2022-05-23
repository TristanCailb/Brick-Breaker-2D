using UnityEngine;

namespace BrickBreaker
{
    public class Brick : MonoBehaviour
    {
        [SerializeField] private int score = 10;
        private bool _hasBeenHitted;
        
        /// <summary>
        /// Destroy the brick when hitted by the ball
        /// </summary>
        public void DestroyBrick()
        {
            if (_hasBeenHitted) return;
            _hasBeenHitted = true;
            GameManager.Instance.AddScore(score);
            Destroy(gameObject);
        }
    }
}
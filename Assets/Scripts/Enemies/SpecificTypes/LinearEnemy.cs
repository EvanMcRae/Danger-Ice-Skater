using UnityEngine;

namespace Enemies
{
    public class LinearEnemy : Follower
    {
        public new void Start()
        {
            base.Start();
            moveTowardsPlayer = true;
        }
        public new void Update()
        {
        }
    }
}

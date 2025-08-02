using UnityEngine;

namespace Enemies {
    public class HoleFallable : MonoBehaviour {
        
        public Vector3 fallPos;
        
        public void Fall()
        {
            fallPos = transform.position;
        }
    }
}
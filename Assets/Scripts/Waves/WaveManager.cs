using System.Collections.Generic;
using UnityEngine;

namespace Waves {
    
    [CreateAssetMenu(fileName = "Wave Manager")]
    public class WaveManager : ScriptableObject {
        public List<Wave> waves;
    }
}
using System;

namespace Enemies {
    [Serializable]
    public enum EnemyType {
        BASIC,
        LINEAR,
        CIRCULAR,
        PROJECTILE,
        TRAP,
        SPECIAL,
        BOSS
    }
}
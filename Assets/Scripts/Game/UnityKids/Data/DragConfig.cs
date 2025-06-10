using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DragConfig", menuName = "Configs/DragConfig")]
    public class DragConfig : ScriptableObject
    {
        [field: SerializeField] public float LongPressTime { get; private set; } = 0.25f;
        [field: SerializeField] public float SwipeSpeedThreshold { get; private set; } = 1000f;
        [field: SerializeField] public float SwipeDirectionFactor { get; private set; } = 0.6f;
    }
}
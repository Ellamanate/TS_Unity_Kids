using DG.Tweening;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "AnimationsConfig", menuName = "Configs/AnimationsConfig")]
    public class AnimationsConfig : ScriptableObject
    {
        [field: SerializeField] public float BounceDuration { get; private set; } = 0.5f;
        [field: SerializeField] public float BounceHeight { get; private set; } = 0.2f;
        [field: SerializeField] public float DestroyDuration { get; private set; } = 0.25f;
        [field: SerializeField] public float TowerFallDuration { get; private set; } = 0.5f;
        [field: SerializeField] public float PitFallDuration { get; private set; } = 1f;
        [field: SerializeField] public float PitOffsetUp { get; private set; } = 1f;
        [field: SerializeField] public float PitOffsetDown { get; private set; } = 1.5f;
        [field: SerializeField] public Ease BounceEase { get; private set; }
        [field: SerializeField] public Ease DestroyEase { get; private set; }
        [field: SerializeField] public Ease TowerFallEase { get; private set; }
        [field: SerializeField] public Ease PitFallEase { get; private set; }
    }
}
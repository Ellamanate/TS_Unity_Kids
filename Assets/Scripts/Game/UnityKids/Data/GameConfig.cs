using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public CubeView CubeViewPrefab { get; private set; }
        [field: SerializeField] public Color[] Colors { get; private set; }
    }
}
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "MessagesConfig", menuName = "Configs/MessagesConfig")]
    public class MessagesConfig : ScriptableObject
    {
        [field: SerializeField] public float MessageDuration { get; private set; } = 2f;
        [field: SerializeField] public string AddTowerCubeKey { get; private set; } = "AddTowerCube";
        [field: SerializeField] public string RemoveTowerCubeKey { get; private set; } = "RemoveTowerCube";
        [field: SerializeField] public string TowerFullKey { get; private set; } = "TowerFull";
        [field: SerializeField] public string DropPitKey { get; private set; } = "DropPit";
        [field: SerializeField] public string DestroyCubeKey { get; private set; } = "DestroyCube";
    }
}
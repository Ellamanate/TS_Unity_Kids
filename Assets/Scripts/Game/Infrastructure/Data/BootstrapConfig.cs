using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "BootstrapConfig", menuName = "Configs/BootstrapConfig")]
    public class BootstrapConfig : ScriptableObject
    {
        /// <summary>
        /// ToDo:
        ///     Сделать поле которое принимает ссылку на сцену 
        /// </summary>
        [field: SerializeField] public int GameSceneIndex { get; private set; }
    }
}
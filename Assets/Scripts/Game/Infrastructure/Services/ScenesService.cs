using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class ScenesService
    {
        public int CurrentScene { get; private set; }
        
        public ScenesService()
        {
            CurrentScene = SceneManager.GetActiveScene().buildIndex;
        }
        
        public async UniTask LoadScene(int sceneIndex, CancellationToken token)
        {
            Debug.Log($"Loading scene with index {sceneIndex}");
            
            await SceneManager
                .LoadSceneAsync(sceneIndex, LoadSceneMode.Single)
                .ToUniTask(cancellationToken: token);
            
            CurrentScene = sceneIndex;
        }
    }
}
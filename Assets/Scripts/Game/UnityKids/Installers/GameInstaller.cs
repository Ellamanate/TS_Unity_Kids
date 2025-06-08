using Zenject;

namespace Game
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindGameInstance();
        }

        private void BindGameInstance()
        {
            Container
                .BindInterfacesAndSelfTo<GameInstance>()
                .AsSingle();
        }
    }
}
using JGM.GameStore.Coroutines;
using JGM.GameStore.Events;
using JGM.GameStore.Loaders;
using JGM.GameStore.Localization;
using JGM.GameStore.Transaction;
using UnityEngine;
using Zenject;

namespace JGM.GameStore.Installers
{
    public class GameStoreSceneInstaller : MonoInstaller
    {
        [SerializeField] private CoroutineService _coroutineServiceInstance;
        [SerializeField] private GameEventTriggerService _gameEventTriggerServiceInstance;

        public override void InstallBindings()
        {
            Container.Bind<ICoroutineService>().FromInstance(_coroutineServiceInstance);
            Container.Bind<ILocalizationService>().To<LocalizationService>().AsSingle();
            Container.Bind<IUserWallet>().To<UserWallet>().AsSingle();
            Container.Bind<IStoreAssetsLibrary>().To<StoreAssetsLibrary>().AsSingle();
            Container.Bind<IEventTriggerService>().FromInstance(_gameEventTriggerServiceInstance);
        }
    }
}
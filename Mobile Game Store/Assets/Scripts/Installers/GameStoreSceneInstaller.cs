using JGM.GameStore.Coroutines;
using JGM.GameStore.Events.Services;
using JGM.GameStore.Loaders;
using JGM.GameStore.Localization;
using JGM.GameStore.Packs;
using JGM.GameStore.Transaction.User;
using UnityEngine;
using Zenject;

namespace JGM.GameStore.Installers
{
    public class GameStoreSceneInstaller : MonoInstaller
    {
        [SerializeField] private CoroutineService _coroutineServiceInstance;
        [SerializeField] private GameEventTriggerService _gameEventTriggerServiceInstance;
        [SerializeField] private UserProfileService _userWalletInstance;

        public override void InstallBindings()
        {
            Container.Bind<IAssetsLibrary>().To<AssetsLibrary>().AsSingle();
            Container.Bind<ICoroutineService>().FromInstance(_coroutineServiceInstance);
            Container.Bind<IEventTriggerService>().FromInstance(_gameEventTriggerServiceInstance);
            Container.Bind<ILocalizationService>().To<LocalizationService>().AsSingle();
            Container.Bind<IUserProfileService>().FromInstance(_userWalletInstance);
            Container.BindFactory<Pack, Pack.Factory>();
            Container.BindFactory<Transaction.Transaction, Transaction.Transaction.Factory>();
#if UNITY_EDITOR
//editor instance
#endif

#if UNITY_ANDROID
//android
#endif
        }
    }
}
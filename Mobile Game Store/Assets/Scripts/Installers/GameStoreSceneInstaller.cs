using JGM.GameStore.Coroutines;
using JGM.GameStore.Localization;
using JGM.GameStore.Transaction;
using UnityEngine;
using Zenject;

namespace JGM.GameStore.Installers
{
    public class GameStoreSceneInstaller : MonoInstaller
    {
        [SerializeField] private CoroutineService _coroutineServiceInstance;

        public override void InstallBindings()
        {
            Container.Bind<ICoroutineService>().FromInstance(_coroutineServiceInstance);
            Container.Bind<ILocalizationService>().To<LocalizationService>().AsSingle();
            Container.Bind<IUserWallet>().To<UserWallet>().AsSingle();
        }
    }
}
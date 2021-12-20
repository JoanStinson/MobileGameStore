using JGM.GameStore.Coroutines;
using JGM.GameStore.Events.Services;
using JGM.GameStore.Loaders;
using JGM.GameStore.Localization;
using JGM.GameStore.Packs;
using JGM.GameStore.Packs.Displayers;
using JGM.GameStore.Transaction.User;
using UnityEngine;
using Zenject;

namespace JGM.GameStore.Installers
{
    public class GameStoreSceneInstaller : MonoInstaller
    {
        [Header("Services Instances")]
        [SerializeField] private CoroutineService _coroutineServiceInstance;
        [SerializeField] private GameEventTriggerService _gameEventTriggerServiceInstance;
        [SerializeField] private UserProfileService _userWalletInstance;

        [Header("Prefabs")]
        [SerializeField] private OfferPackDisplayer _featuredOfferPackDisplayerPrefab;
        [SerializeField] private OfferPackDisplayer _offerPackDisplayerPrefab;
        [SerializeField] private GemsPackDisplayer _gemsPackDisplayerPrefab;
        [SerializeField] private CoinsPackDisplayer _coinsPackDisplayerPrefab;
        [SerializeField] private PackItemDisplayer _packItemDisplayerPrefab;

        public override void InstallBindings()
        {
            Container.Bind<IAssetsLibrary>().To<AssetsLibrary>().AsSingle();
            Container.Bind<ICoroutineService>().FromInstance(_coroutineServiceInstance);
            Container.Bind<IEventTriggerService>().FromInstance(_gameEventTriggerServiceInstance);
            Container.Bind<ILocalizationService>().To<LocalizationService>().AsSingle();
            Container.Bind<IUserProfileService>().FromInstance(_userWalletInstance);
            Container.BindFactory<Pack, Pack.Factory>();
            Container.BindFactory<OfferPackDisplayer, OfferPackDisplayer.FeaturedFactory>().FromComponentInNewPrefab(_featuredOfferPackDisplayerPrefab);
            Container.BindFactory<OfferPackDisplayer, OfferPackDisplayer.Factory>().FromComponentInNewPrefab(_offerPackDisplayerPrefab);
            Container.BindFactory<GemsPackDisplayer, GemsPackDisplayer.Factory>().FromComponentInNewPrefab(_gemsPackDisplayerPrefab);
            Container.BindFactory<CoinsPackDisplayer, CoinsPackDisplayer.Factory>().FromComponentInNewPrefab(_coinsPackDisplayerPrefab);
            Container.BindFactory<PackItemDisplayer, PackItemDisplayer.Factory>().FromComponentInNewPrefab(_packItemDisplayerPrefab);
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
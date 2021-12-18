using JGM.GameStore.Loaders;
using JGM.GameStore.Packs.Data;

namespace JGM.GameStore.Packs.Displayers
{
    public interface IStorePackDisplayer
    {
        void SetPackData(StorePack storePack, IStoreAssetsLibrary assetsLibrary);
    }
}
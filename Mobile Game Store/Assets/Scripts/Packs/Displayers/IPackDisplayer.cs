using JGM.GameStore.Loaders;

namespace JGM.GameStore.Packs.Displayers
{
    public interface IPackDisplayer
    {
        void SetPackData(Pack pack, IAssetsLibrary assetsLibrary);
        void PurchasePack();
    }
}
using JGM.GameStore.Loaders;
using JGM.GameStore.Packs.Data;

namespace JGM.GameStore.Packs.Displayers
{
    public interface IPackDisplayer
    {
        void SetPackData(Pack pack, IAssetsLibrary assetsLibrary);
    }
}
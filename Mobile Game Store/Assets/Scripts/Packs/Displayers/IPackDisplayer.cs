using JGM.GameStore.Libraries;

namespace JGM.GameStore.Packs.Displayers
{
    public interface IPackDisplayer
    {
        void SetPackData(in Pack pack);
        void PurchasePack();
    }
}
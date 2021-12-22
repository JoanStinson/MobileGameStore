using JGM.GameStore.Packs.Displayers;

namespace JGM.GameStore.Packs
{
    public interface IPacksFactory
    {
        PackDisplayer CreatePackDisplayer(in Pack pack);
        OfferPackDisplayer CreateFeaturedOfferPack(in Pack pack);
        void SetPackDisplayerParent(PackDisplayer packDisplayer, int index);
        void ResetSiblingIndexes();
    }
}
using JGM.GameStore.Packs;

namespace JGM.GameStore.Events.Data
{
    public class PurchasePackEventData : IGameEventData
    {
        public readonly Pack StorePack;

        public PurchasePackEventData(in Pack pack)
        {
            StorePack = pack;
        }
    }
}
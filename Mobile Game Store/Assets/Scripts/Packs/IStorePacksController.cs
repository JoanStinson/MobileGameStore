using JGM.GameStore.Packs.Data;
using System.Collections.Generic;

namespace JGM.GameStore.Packs
{
    public interface IStorePacksController
    {
        List<StorePack> ActivePacks { get; }

        void Initialize();
        void Refresh();
    }
}
using System.Collections.Generic;

namespace JGM.GameStore.Packs
{
    public interface IPacksController
    {
        List<Pack> ActivePacks { get; }

        void Initialize();
        void Refresh();
    }
}
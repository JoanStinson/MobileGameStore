using System.Collections.Generic;
using UnityEngine.Events;

namespace JGM.GameStore.Packs
{
    public interface IPacksController
    {
        public class ShopPackEvent : UnityEvent<Pack> { }

        ShopPackEvent OnPackActivated { get; set; }
        ShopPackEvent OnPackRemoved { get; set; }
        List<Pack> ActivePacks { get; }

        void Initialize();
        void Refresh();
    }
}
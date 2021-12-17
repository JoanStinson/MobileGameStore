//using System.Collections.Generic;
//using Ubisoft.UIProgrammerTest.Data;
//using Ubisoft.UIProgrammerTest.Logic;
//using Ubisoft.UIProgrammerTest.Singletons;
//using UnityEngine;
//using UnityEngine.UI;

//namespace JGM.GameStore.Sample
//{
//    public class SampleSceneController : MonoBehaviour
//    {
//        [SerializeField] private Text m_activePacksText = null;
//        [Space]
//        [SerializeField] private Text m_coinsText = null;
//        [SerializeField] private Text m_gemsText = null;
//        [Space]
//        [SerializeField] private Text m_lastTransactionResult = null;
//        [SerializeField] private Text m_busyText = null;
//        [SerializeField] private GameObject m_busyPanel = null;

//        private Transaction m_transaction = null;

//        private void Start()
//        {
//            Refresh();
//            m_lastTransactionResult.text = "";
//        }

//        private void Update()
//        {
//            Refresh();
//        }

//        private void Refresh()
//        {
//            m_busyPanel.SetActive(m_transaction != null);
//            if (m_transaction != null)
//            {
//                StorePack pack = (StorePack)m_transaction.Data;
//                m_busyText.text = "Purchasing pack " + pack.PackData.Id;
//            }

//            string str = "ACTIVE PACKS:\n\n";
//            foreach (StorePack pack in StorePacksController.Instance.ActivePacks)
//            {
//                str += pack.ToString() + "\n\n";
//            }
//            m_activePacksText.text = str;

//            m_coinsText.text = LocalizationManager.Instance.Localize("TID_COINS_NAME") + ": " + UserProfile.Instance.GetCurrency(UserProfile.Currency.Coins).ToString();
//            m_gemsText.text = LocalizationManager.Instance.Localize("TID_GEMS_NAME") + ": " + UserProfile.Instance.GetCurrency(UserProfile.Currency.Gems).ToString();
//        }

//        private void BuyPack(StorePack pack)
//        {
//            if (m_transaction != null)
//            {
//                return;
//            }

//            m_transaction = UserProfile.Instance.CreateTransaction(pack.PackData.PackCurrency, -pack.PackData.Price);
//            m_transaction.Data = pack;
//            m_transaction.OnFinished.AddListener(OnTransactionFinished);
//            m_transaction.StartTransaction();
//        }

//        public void OnTransactionFinished(Transaction transaction, bool success)
//        {
//            if (transaction == m_transaction)
//            {
//                StorePack pack = (StorePack)transaction.Data;
//                if (success)
//                {
//                    pack.ApplyTransaction();
//                }

//                if (success)
//                {
//                    m_lastTransactionResult.text = "<color=#00ff00>SUCCESS!</color>\n" + pack.PackData.Id;
//                }
//                else
//                {
//                    m_lastTransactionResult.text = "<color=#ff0000>FAILED! " + transaction.TransactionError.ToString() + "</color>\n" + pack.PackData.Id;
//                }

//                m_transaction = null;
//            }
//        }

//        public void OnBuyRandomPack()
//        {
//            var pool = StorePacksController.Instance.ActivePacks;
//            if (pool.Count > 0)
//            {
//                StorePack targetPack = pool[Random.Range(0, pool.Count)];
//                BuyPack(targetPack);
//            }
//        }

//        public void OnBuyRandomOfferPack()
//        {
//            var allPacks = StorePacksController.Instance.ActivePacks;
//            var pool = new List<StorePack>();
//            for (int i = 0; i < allPacks.Count; ++i)
//            {
//                if (allPacks[i].PackData.PackType == StorePackData.Type.Offer)
//                {
//                    pool.Add(allPacks[i]);
//                }
//            }

//            if (pool.Count > 0)
//            {
//                StorePack targetPack = pool[Random.Range(0, pool.Count)];
//                BuyPack(targetPack);
//            }
//        }

//        public void OnBuyRandomCoinsPack()
//        {
//            var allPacks = StorePacksController.Instance.ActivePacks;
//            var pool = new List<StorePack>();
//            for (int i = 0; i < allPacks.Count; ++i)
//            {
//                if (allPacks[i].PackData.PackType == StorePackData.Type.Coins)
//                {
//                    pool.Add(allPacks[i]);
//                }
//            }

//            if (pool.Count > 0)
//            {
//                StorePack targetPack = pool[Random.Range(0, pool.Count)];
//                BuyPack(targetPack);
//            }
//        }

//        public void OnBuyRandomGemsPack()
//        {
//            var allPacks = StorePacksController.Instance.ActivePacks;
//            var pool = new List<StorePack>();
//            for (int i = 0; i < allPacks.Count; ++i)
//            {
//                if (allPacks[i].PackData.PackType == StorePackData.Type.Gems)
//                {
//                    pool.Add(allPacks[i]);
//                }
//            }

//            if (pool.Count > 0)
//            {
//                StorePack targetPack = pool[Random.Range(0, pool.Count)];
//                BuyPack(targetPack);
//            }
//        }

//        public void OnChangeLanguageRandom()
//        {
//            LocalizationManager.Language newLanguage = (LocalizationManager.Language)Random.Range(0, (int)LocalizationManager.Language.Count);
//            LocalizationManager.Instance.SetLanguage(newLanguage);
//            Refresh();
//        }
//    }
//}
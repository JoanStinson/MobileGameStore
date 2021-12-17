using UnityEngine;
using UnityEngine.UI;

namespace JGM.GameStore.Tabs
{
    public class TabsController : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private RectTransform _content;
        [SerializeField] [Range(0, 600f)] private float yOffset = 300f;

        //TODO refactor all of these methods xD
        //getcomponent and duplicated code ewww
        public void CenterScrollViewToOffers()
        {
            var firstOfferPack = FindObjectOfType<OfferPackDisplayer>().gameObject;
            var target = firstOfferPack.GetComponent<RectTransform>();
            _scrollRect.content.localPosition = CenterScrollViewToTarget(target);
        }

        public void CenterScrollViewToGems()
        {
            var firstGemsPack = FindObjectOfType<GemsPackDisplayer>().gameObject;
            var target = firstGemsPack.GetComponent<RectTransform>();
            _scrollRect.content.localPosition = CenterScrollViewToTarget(target);
        }

        public void CenterScrollViewToCoins()
        {
            var firstCoinsPack = FindObjectOfType<CoinsPackDisplayer>().gameObject;
            var target = firstCoinsPack.GetComponent<RectTransform>();
            _scrollRect.content.localPosition = CenterScrollViewToTarget(target);
        }

        public Vector2 CenterScrollViewToTarget(RectTransform target)
        {
            Canvas.ForceUpdateCanvases();
            Vector2 viewportLocalPosition = _scrollRect.viewport.localPosition;
            Vector2 childLocalPosition = target.localPosition;
            Vector2 result = new Vector2(0 - (viewportLocalPosition.x + childLocalPosition.x), 0 - (viewportLocalPosition.y + childLocalPosition.y) - yOffset);
            return result;
        }
    }
}
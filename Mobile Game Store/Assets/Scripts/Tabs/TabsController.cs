using UnityEngine;
using UnityEngine.UI;

namespace JGM.GameStore.Tabs
{
    public class TabsController : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] [Range(-100f, 100f)] private float yOffset = 0f;
        [Space]
        [SerializeField] private RectTransform _offersPackParent;
        [SerializeField] private RectTransform _gemsPackParent;
        [SerializeField] private RectTransform _coinsPackParent;

        public void CenterScrollViewToOffers()
        {
            _scrollRect.content.localPosition = CenterScrollViewToTarget(_offersPackParent);
        }

        public void CenterScrollViewToGems()
        {
            _scrollRect.content.localPosition = CenterScrollViewToTarget(_gemsPackParent);
        }

        public void CenterScrollViewToCoins()
        {
            _scrollRect.content.localPosition = CenterScrollViewToTarget(_coinsPackParent);
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
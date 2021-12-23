using UnityEngine;
using UnityEngine.UI;

namespace JGM.GameStore.Tabs
{
    public class TabsController : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] [Range(-200f, 0f)] private float _heightOffset = -100f;
        [Space]
        [SerializeField] private RectTransform _offersPackParent;
        [SerializeField] private RectTransform _gemsPackParent;
        [SerializeField] private RectTransform _coinsPackParent;

        public void CenterScrollViewToOffers()
        {
            Vector3 centeredPosition = new Vector3(_scrollRect.content.localPosition.x, GetScrollRectPositionToCenterTarget(_offersPackParent).y, _scrollRect.content.localPosition.z);
            _scrollRect.content.localPosition = centeredPosition;
        }

        public void CenterScrollViewToGems()
        {
            Vector3 centeredPosition = new Vector3(_scrollRect.content.localPosition.x, GetScrollRectPositionToCenterTarget(_gemsPackParent).y, _scrollRect.content.localPosition.z);
            _scrollRect.content.localPosition = centeredPosition;
        }

        public void CenterScrollViewToCoins()
        {
            Vector3 centeredPosition = new Vector3(_scrollRect.content.localPosition.x, GetScrollRectPositionToCenterTarget(_coinsPackParent).y, _scrollRect.content.localPosition.z);
            _scrollRect.content.localPosition = centeredPosition;
        }

        public Vector2 GetScrollRectPositionToCenterTarget(RectTransform target)
        {
            Canvas.ForceUpdateCanvases();
            float xPositionResult = 0 - (_scrollRect.viewport.localPosition.x + target.localPosition.x);
            float yPositionResult = 0 - (_scrollRect.viewport.localPosition.y + target.localPosition.y) - _heightOffset;
            if (yPositionResult < 0)
            {
                yPositionResult = 0;
            }
            return new Vector2(xPositionResult, yPositionResult);
        }
    }
}
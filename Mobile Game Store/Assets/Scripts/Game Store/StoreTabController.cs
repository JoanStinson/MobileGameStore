using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Game_Store
{
    public class StoreTabController : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private RectTransform _contentPanel;
        [SerializeField] private float yOffset = 0.1f;

        public void ScrollSliderToOffers()
        {
            var firstOffer = FindObjectOfType<OfferPack>().gameObject;
            //SnapTo(gameObject.GetComponent<RectTransform>());
            _scrollRect.content.localPosition = GetSnapToPositionToBringChildIntoView(_scrollRect, firstOffer.GetComponent<RectTransform>());
            //StartCoroutine(LerpToChild(firstOffer.gameObject.GetComponent<RectTransform>()));
        }

        public void ScrollSliderToGems()
        {
            var firstOffer = FindObjectOfType<GemsPack>().gameObject;
            //SnapTo(gameObject.GetComponent<RectTransform>());
            _scrollRect.content.localPosition = GetSnapToPositionToBringChildIntoView(_scrollRect, firstOffer.GetComponent<RectTransform>());
            //StartCoroutine(LerpToChild(firstOffer.gameObject.GetComponent<RectTransform>()));
        }

        public void ScrollSliderToCoins()
        {
            var firstOffer = FindObjectOfType<CoinsPack>().gameObject;
            //SnapTo(gameObject.GetComponent<RectTransform>());
            _scrollRect.content.localPosition = GetSnapToPositionToBringChildIntoView(_scrollRect, firstOffer.GetComponent<RectTransform>());
            //StartCoroutine(LerpToChild(firstOffer.gameObject.GetComponent<RectTransform>()));
        }

        private void SnapTo(RectTransform target)
        {
            Canvas.ForceUpdateCanvases();
            _contentPanel.anchoredPosition = (Vector2)_scrollRect.transform.InverseTransformPoint(_contentPanel.position) - (Vector2)_scrollRect.transform.InverseTransformPoint(target.position);
        }

        public Vector2 GetSnapToPositionToBringChildIntoView(ScrollRect instance, RectTransform child)
        {
            Canvas.ForceUpdateCanvases();
            Vector2 viewportLocalPosition = instance.viewport.localPosition;
            Vector2 childLocalPosition = child.localPosition;
            Vector2 result = new Vector2(
                0 - (viewportLocalPosition.x + childLocalPosition.x),
                0 - (viewportLocalPosition.y + childLocalPosition.y) - yOffset
            );
            return result;
        }

        private IEnumerator LerpToChild(RectTransform target)
        {
            Vector2 _lerpTo = (Vector2)_scrollRect.transform.InverseTransformPoint(_contentPanel.position) - (Vector2)_scrollRect.transform.InverseTransformPoint(target.position);
            bool _lerp = true;
            Canvas.ForceUpdateCanvases();

            while (_lerp)
            {
                float decelerate = Mathf.Min(10f * Time.deltaTime, 1f);
                _contentPanel.anchoredPosition = Vector2.Lerp(_scrollRect.transform.InverseTransformPoint(_contentPanel.position), _lerpTo, decelerate);
                if (Vector2.SqrMagnitude((Vector2)_scrollRect.transform.InverseTransformPoint(_contentPanel.position) - _lerpTo) < 0.25f)
                {
                    _contentPanel.anchoredPosition = _lerpTo;
                    _lerp = false;
                }
                yield return null;
            }
        }
    }
}
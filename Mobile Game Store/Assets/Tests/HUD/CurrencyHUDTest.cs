using JGM.GameStore.Events.Data;
using JGM.GameStore.HUD;
using Moq;
using NUnit.Framework;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;

namespace JGM.GameStoreTests.HUD
{
    public class CurrencyHUDTest
    {
        private CurrencyHUD _currencyHUD;
        private Mock<ICurrencyEventData> _gameEventDataMock;
        private const float _testCurrencyAmount = 200f;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            var dummyGO = new GameObject("Dummy");
            _currencyHUD = dummyGO.AddComponent<CurrencyHUD>();
            _gameEventDataMock = new Mock<ICurrencyEventData>();
            _gameEventDataMock.SetupGet(x => x.Amount).Returns(_testCurrencyAmount);
            yield return new WaitForEndOfFrame();
        }

        [UnityTest]
        public IEnumerator OnComponentAwake_NoTMPIsAttached_LogsError()
        {
            GameObject.Destroy(_currencyHUD.GetComponent<TextMeshProUGUI>());
            yield return new WaitForEndOfFrame();
            LogAssert.Expect(LogType.Error, "Can't remove TextMeshProUGUI (Script) because CurrencyHUD (Script) depends on it");
        }

        [Test]
        public void OnComponentAwake_TMPIsAttached_AmountIsZero()
        {
            float currencyAmount = float.Parse(_currencyHUD.GetComponent<TextMeshProUGUI>().text);
            Assert.AreEqual(0f, currencyAmount);
        }

        [Test]
        public void OnRefreshCurrencyAmount_DataPassedIsNull_AmountIsZero()
        {
            _currencyHUD.RefreshCurrencyAmount(new Mock<ICurrencyEventData>().Object);
            float currencyAmount = float.Parse(_currencyHUD.GetComponent<TextMeshProUGUI>().text);
            Assert.AreEqual(0f, currencyAmount);
        }

        [Test]
        public void OnRefreshCurrencyAmount_DataPassedIsValid_ReturnsExpectedAmount()
        {
            _currencyHUD.RefreshCurrencyAmount(_gameEventDataMock.Object);
            float currencyAmount = float.Parse(_currencyHUD.GetComponent<TextMeshProUGUI>().text);
            Assert.AreEqual(_testCurrencyAmount, currencyAmount);
        }
    }
}
using Ubisoft.UIProgrammerTest.Data;
using Ubisoft.UIProgrammerTest.Logic;

public interface IPurchasePack
{
    void PopulatePackData(StorePack packData);
    void OnSelect();
    void OnPurchase();
}

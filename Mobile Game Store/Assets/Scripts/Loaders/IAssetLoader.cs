namespace JGM.GameStore.Loaders
{
    public interface IAssetLoader<T>
    {
        public void LoadAllInPath(in string resourcesPath);
        public T GetAsset(in string assetName);
    }
}
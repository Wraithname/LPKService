namespace LPKServiceSDK
{
    public interface IServiceWorker
    {
        void Load();
        void Unload();
        void DoWork();
    }
}

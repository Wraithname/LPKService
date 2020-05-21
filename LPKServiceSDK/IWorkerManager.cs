using System.ComponentModel.Composition.Hosting;
namespace LPKServiceSDK
{
    public delegate void DoWorkEventHandle();
    public delegate void LoadEventHandle();
    public delegate void UnloadEventHandle();
    public interface IWorkerManager
    {
        CompositionContainer InitialiseWork(DirectoryCatalog catalog);
        DoWorkEventHandle DoWorkEvent { get; set; }
        LoadEventHandle LoadEvent { get; set; }
        UnloadEventHandle UnloadEvent { get; set; }
    }
}

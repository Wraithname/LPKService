namespace LPKService
{
    public delegate void DoWorkEventHandler();
    public delegate void LoadEventHandler();
    public delegate void UnloadEventHandler();
    interface IServiceWork
    {
        DoWorkEventHandler DoWorkEvent { get; set; }
        LoadEventHandler LoadEvent { get; set; }
        UnloadEventHandler UnloadEvent { get; set; }
    }
}

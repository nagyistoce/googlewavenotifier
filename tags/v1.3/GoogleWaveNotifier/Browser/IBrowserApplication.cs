namespace GoogleWaveNotifier.Browser
{
    public interface IBrowserApplication
    {
        string Name { get; }
        void Launch(string uri);
    }
}
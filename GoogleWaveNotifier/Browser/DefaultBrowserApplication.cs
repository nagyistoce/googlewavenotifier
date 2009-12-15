namespace GoogleWaveNotifier.Browser
{
    public class DefaultBrowserApplication : IBrowserApplication
    {
        public string Name { get { return "Default"; } }
        public void Launch(string uri)
        {
            Utilities.Execute(uri);
        }
    }
}
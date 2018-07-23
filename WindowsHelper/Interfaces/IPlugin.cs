namespace WindowsHelper.Interfaces
{
    public interface IPlugin
    {
        string Name { get; set; }
        
        bool Init();
    }
}
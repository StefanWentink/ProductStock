namespace ProductStock.DAL.Interfaces
{
    public interface ISettingsModel
    {
        string ConnectionString { get; set; }

        string Database { get; set; }
    }
}

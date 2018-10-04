namespace ProductStock.DAL.Models
{
    using ProductStock.DAL.Interfaces;
    using System;

    public class SettingsModel : ISettingsModel
    {
        public string ConnectionString { get; set; }

        public string Database { get; set; }

        [Obsolete("Only for serialization.", true)]
        public SettingsModel()
        { }

        public SettingsModel(string connectionString, string database)
        {
            ConnectionString = connectionString;
            Database = database;
        }
    }
}
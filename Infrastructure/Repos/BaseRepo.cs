using System.Reflection;
using System.Text.Json;
using Infrastructure.DataStorage;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repos
{
    public class BaseRepo : IBaseRepo
    {
        private readonly string DataFilePath;

        public BaseRepo()
        {
            string currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            while (currentDirectory != null && !Directory.Exists(Path.Combine(currentDirectory, "Infrastructure")))
            {
                currentDirectory = Directory.GetParent(currentDirectory)?.FullName!;

            }
            string appSettingsPath = Path.Combine(currentDirectory!, "Infrastructure", "AppSettings.json");

            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile(appSettingsPath, optional: true, reloadOnChange: true)
                .Build();

            DataFilePath = configuration["EnvironmentVariables:DataFilePath"]!;
        }



        public void SaveData(Data data)
        {
            string json = JsonSerializer.Serialize(data);
            File.WriteAllText(DataFilePath, json);
        }

        public Data LoadData()
        {
            if (File.Exists(DataFilePath))
            {
                string json = File.ReadAllText(DataFilePath);
                return JsonSerializer.Deserialize<Data>(json) ?? new Data();
            }
            return new Data();
        }
    }
}

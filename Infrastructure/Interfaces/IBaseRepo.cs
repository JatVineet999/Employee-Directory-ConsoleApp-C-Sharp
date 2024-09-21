using Infrastructure.DataStorage;

namespace Infrastructure.Interfaces
{
    public interface IBaseRepo
    {
        void SaveData(Data data);
        Data LoadData();
    }
}

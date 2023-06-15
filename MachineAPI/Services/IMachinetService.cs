using MachineAPI.Models;
using MongoDB.Bson;

namespace MachineAPI.Services
{
    public abstract class IMachinetService
    {
        public abstract bool AddAssetOrMachine(MachineModel machineDetails);
        public abstract Task<BsonDocument?> RemoveAssetFromMachine(string machineName, string assetName);
        public abstract List<MapBsonToMachineModel>? SearchDocument(string strToSearch);
        public abstract List<MapBsonToMachineModel>? SearchAllDocument();
        public abstract List<MapBsonToMachineModel>? GetLatestMachine();
    }
}

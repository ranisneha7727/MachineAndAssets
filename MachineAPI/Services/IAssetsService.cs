using MachineAPI.Models;
using MongoDB.Bson;

namespace MachineAPI.Services
{
    public abstract class IAssetsService
    {
        public abstract bool AddAssetOrMachine(MachineModel machineDetails);
        public abstract Task<BsonDocument?> RemoveAssetFromMachine(string machineName, string assetName);
        public abstract IEnumerable<MachineModel?> Search(string strToSearch);
        public abstract IEnumerable<MachineModel> SearchAll();
        public abstract IEnumerable<MachineModel?> GetLatestMachine();
    }
}

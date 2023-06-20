using MachineAPI.Entities;
using MachineAPI.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Data;
using System.Text.RegularExpressions;

namespace MachineAPI.Services
{
    public class AssetsServiceMongo : IAssetsService
    {
        public override bool AddAssetOrMachine(MachineModel machineDetails)
        {
            try
            {
                if (machineDetails != null && machineDetails.Assets.Count > 0)
                {
                    var client = new MongoClient(Constants.ConnectionString);
                    var db = client.GetDatabase(Constants.DbName);
                    var collectionData = db.GetCollection<BsonDocument>(Constants.MachineCollectionName);
                    BsonDocument documnt = new() 
                {
                    { Constants.ColumnNameMachine, machineDetails.MachineName},
                    { Constants.ColumnNameAsset, machineDetails.Assets[0].Name},
                    { Constants.ColumnNameSeriesNo , machineDetails.Assets[0].SeriesNo}
                };
                    collectionData.InsertOneAsync(documnt);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to Add Asset......." + ex.Message);
                return false;
            }
        }

        public override async Task<BsonDocument?> RemoveAssetFromMachine(string machineName, string assetName)
        {
            try
            {
                var client = new MongoClient(Constants.ConnectionString);
                var db = client.GetDatabase(Constants.DbName);
                var collectionData = db.GetCollection<BsonDocument>(Constants.MachineCollectionName);
                var asset = await collectionData.FindOneAndDeleteAsync(Builders<BsonDocument>.Filter.Eq(Constants.ColumnNameMachine, machineName) & Builders<BsonDocument>.Filter.Eq(Constants.ColumnNameAsset, assetName));
                if (asset != null)
                {
                    return asset;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to Create Client......." + ex.Message);
                return null;
            }
        }

        public override IEnumerable<MachineModel?> Search(string searchString)
        {
            try
            {
                var _machine = new MachineModel();
                var client = new MongoClient(Constants.ConnectionString);
                var db = client.GetDatabase(Constants.DbName);
                var machinecollectionData = db.GetCollection<BsonDocument>(Constants.MachineCollectionName);
                var queryExp = new BsonRegularExpression(new Regex(searchString, RegexOptions.IgnoreCase));
                var builder = Builders<BsonDocument>.Filter;
                var filter = builder.Regex(Constants.ColumnNameMachine, queryExp);
                var documents = machinecollectionData.Find(filter).ToList();
                if (documents.Count == 0)
                {
                    filter = builder.Regex(Constants.ColumnNameAsset, queryExp);
                    documents = machinecollectionData.Find(filter).ToList();
                }
                if (documents.Count > 0)
                {
                    List<MapBsonToMachineModel?> listOfMachines = new();
                    foreach (var item in documents)
                    {
                        listOfMachines.Add(BsonSerializer.Deserialize<MapBsonToMachineModel>(item));
                    }
                    IEnumerable<MachineModel> listWithGrouping = from p in listOfMachines
                                                                 group p by p.MachineName into sl
                                                                 select new MachineModel()
                                                                 {
                                                                     MachineName = sl.Key,
                                                                     Assets = sl.Select(x => new AssetModel
                                                                     {
                                                                         Name = x.AssetName,
                                                                         SeriesNo = x.SeriesNo
                                                                     }).ToList()
                                                                 };
                    if (listWithGrouping != null)
                    {
                        return listWithGrouping;
                    }
                    else
                    {
                        return Enumerable.Empty<MachineModel>();
                    }
                }
                else
                { return Enumerable.Empty<MachineModel>(); }

            }
            catch
            {
                return Enumerable.Empty<MachineModel>();
            }
        }

        public override IEnumerable<MachineModel> SearchAll()
        {
            try
            {
                var client = new MongoClient(Constants.ConnectionString);
                var db = client.GetDatabase(Constants.DbName);
                var machinecollectionData = db.GetCollection<BsonDocument>(Constants.MachineCollectionName);
                var documents = machinecollectionData.Find(new BsonDocument()).ToList();
                if (documents.Count > 0)
                {
                    List<MapBsonToMachineModel?> listOfMachines = new();
                    foreach (var item in documents)
                    {
                        listOfMachines.Add(BsonSerializer.Deserialize<MapBsonToMachineModel>(item));
                    }
                    IEnumerable<MachineModel> listWithGrouping = from p in listOfMachines
                                                  group p by p.MachineName into sl
                                             select new MachineModel()
                                             {
                                                 MachineName = sl.Key,
                                                 Assets = sl.Select(x => new AssetModel
                                                 {
                                                     Name = x.AssetName,
                                                     SeriesNo = x.SeriesNo
                                                 }).ToList()
                                             };
                    if (listWithGrouping != null)
                    {
                        return listWithGrouping;
                    }
                    else { return Enumerable.Empty<MachineModel>(); }
                }
                else
                    return Enumerable.Empty<MachineModel>();
            }
            catch
            {
                return Enumerable.Empty<MachineModel>();
            }
        }

        public override IEnumerable<MachineModel> GetLatestMachine()
        {
            int latestCountCheck = 0;
            string[] machineNames, assetNames;
            var client = new MongoClient(Constants.ConnectionString);
            var db = client.GetDatabase(Constants.DbName);
            var machinecollectionData = db.GetCollection<BsonDocument>(Constants.MachineCollectionName);
            var documents = machinecollectionData.Find(new BsonDocument()).ToList();
            List<MapBsonToMachineModel?> listOfAllMachines = new();
            List<MapBsonToMachineModel?> listOfMachines = new();
            if (documents.Count > 0)
            {
                foreach (var item in documents)
                {
                    listOfAllMachines.Add(BsonSerializer.Deserialize<MapBsonToMachineModel>(item));
                }
                if (listOfMachines != null)
                {
                    machineNames = listOfAllMachines.Select(x => x.MachineName).Distinct().ToArray();
                    assetNames = listOfAllMachines.Select(x => x.AssetName).Distinct().ToArray();
                    foreach (var item in machineNames)
                    {
                        var filteredAssetDetails = listOfAllMachines.Where(x => x.MachineName.Equals(item)).ToList();
                        if (filteredAssetDetails.Count > 0)
                        {
                            foreach (var itemAsset in filteredAssetDetails)
                            {
                                var allSeriesOfAsset = listOfAllMachines.Where(x => x.AssetName.Equals(itemAsset.AssetName)).Select(y => y.SeriesNo).ToList();
                                for (int i = 0; i < allSeriesOfAsset.Count; i++)
                                {
                                    allSeriesOfAsset[i] = allSeriesOfAsset[i].Remove(0, 1);
                                }
                                string strLatestSeriesOfAsset = allSeriesOfAsset.Max();
                                if (itemAsset.SeriesNo.Contains(strLatestSeriesOfAsset))
                                    latestCountCheck++;
                            }
                            if (latestCountCheck == filteredAssetDetails.Count())
                            {
                                listOfMachines.AddRange(filteredAssetDetails);
                            }
                            latestCountCheck = 0;
                        }
                    }
                }
                IEnumerable<MachineModel> listWithGrouping = from p in listOfMachines
                                                             group p by p.MachineName into sl
                                                             select new MachineModel()
                                                             {
                                                                 MachineName = sl.Key,
                                                                 Assets = sl.Select(x => new AssetModel
                                                                 {
                                                                     Name = x.AssetName,
                                                                     SeriesNo = x.SeriesNo
                                                                 }).ToList()
                                                             };
                return listWithGrouping;
            }
            else
                return Enumerable.Empty<MachineModel>();
        }

        private void UpdateUrl()
        {

        }
    }
}

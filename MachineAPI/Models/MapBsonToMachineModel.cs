using MongoDB.Bson.Serialization.Attributes;

namespace MachineAPI.Models
{
    [BsonIgnoreExtraElements]
    public class MapBsonToMachineModel
    {
        private AssetModel _asset = new();

        public MapBsonToMachineModel()
        {
            _asset.AssetName = AssetName;
            _asset.SeriesNo = SeriesNo;
        }

        [BsonElement("MachineName")]
        public string MachineName { get; set; }

        [BsonElement("AssetName")]
        public string AssetName { get; set; }

        [BsonElement("SeriesNo")]
        public string SeriesNo { get; set; }
    }
}

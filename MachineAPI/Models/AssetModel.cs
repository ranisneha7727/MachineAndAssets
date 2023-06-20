using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MachineAPI.Models
{
    public class AssetModel
    {
        [MaxLength(40)]
        [BsonElement("AssetName")]
        public string Name { get; set; }

        [BsonElement("SeriesNo")]
        [MaxLength(10)]
        public string SeriesNo { get; set; }
    }
}

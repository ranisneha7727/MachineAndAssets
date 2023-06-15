using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MachineAPI.Models
{
    public class AssetModel
    {
        [MaxLength(40)]
        public string AssetName { get; set; }

        [BsonElement("SeriesNo")]
        [MaxLength(10)]
        public string SeriesNo { get; set; }
    }
}

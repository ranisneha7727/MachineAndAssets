using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MachineAPI.Models
{
    [BsonIgnoreExtraElements]
    public class MachineModel
    {
        public MachineModel()
        {
            Assets = new List<AssetModel>();
        }

        [Required(ErrorMessage = "You should provide machine name.")]
        [MaxLength(20)]
        [BsonElement("MachineName")]
        public string MachineName { get; set; }
        
        public List<AssetModel> Assets { get; set; }
    }
}
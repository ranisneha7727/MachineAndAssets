using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MachineAPI.Models
{
    public class MachineModel
    {
        [Required(ErrorMessage = "You should provide machine name.")]
        [MaxLength(20)]
        public string MachineName { get; set; }
        
        public List<AssetModel?> Assets { get; set; }
    }
}
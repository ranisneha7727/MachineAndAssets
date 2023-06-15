using Microsoft.AspNetCore.Mvc;
using MachineAPI.Services;
using MachineAPI.Models;

namespace MachineAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MachinesController : ControllerBase
    {
        private readonly IMachinetService _machineAndAssetService;
        public MachinesController(IMachinetService machineAndAssetService)
        {
            _machineAndAssetService = machineAndAssetService;
        }

        [HttpGet]
        public IActionResult? Search(string? search)
        {
            try
            {
                List<MapBsonToMachineModel>? documents = null;
                if(search != null)  
                {
                    documents = _machineAndAssetService.SearchDocument(search);
                }
                else
                    documents = _machineAndAssetService.SearchAllDocument();

                if (documents != null)
                    return Ok(documents);
                else
                    return NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult AddAsset(MachineModel machine)
        {
            try
            {
                if (machine.MachineName != null && machine.Assets.Count > 0)
                {
                    bool ifCreated = _machineAndAssetService.AddAssetOrMachine(machine);
                    if (ifCreated)
                        return Ok();
                    else
                        return BadRequest();
                }
                else
                    return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("{machine}/{asset}")]
        public async Task<IActionResult> DeleteAsset(string machine, string asset)
        {
            try
            {
                if (!string.IsNullOrEmpty(machine) && !string.IsNullOrEmpty(asset))
                {
                    var deletedDoc = await _machineAndAssetService.RemoveAssetFromMachine(machine, asset);
                    if (deletedDoc != null)
                        return Ok(deletedDoc);
                    else
                        return NotFound();
                }
                else
                    return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }

        [Route("latest")]
        [HttpGet]
        public IActionResult? LatestMachine()
        {
            try
            {
                var docs = _machineAndAssetService.GetLatestMachine();
                if (docs !=null)
                    return Ok(docs);
                return
                    NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
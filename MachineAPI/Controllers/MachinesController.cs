using Microsoft.AspNetCore.Mvc;
using MachineAPI.Services;
using MachineAPI.Models;

namespace MachineAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MachinesController : ControllerBase
    {
        private readonly IAssetsService _assetService;
        public MachinesController(IAssetsService assetService)
        {
            _assetService = assetService;
        }

        [HttpGet]
        public IActionResult? Search(string? search)
        {
            try
            {
                if (search != null)
                {
                    var documents = _assetService.Search(search);
                    return Ok(documents);
                }
                else
                {
                    var documents = _assetService.SearchAll();
                    return Ok(documents);
                }
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
                    bool ifCreated = _assetService.AddAssetOrMachine(machine);
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
                    var deletedDoc = await _assetService.RemoveAssetFromMachine(machine, asset);
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
                var docs = _assetService.GetLatestMachine();
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
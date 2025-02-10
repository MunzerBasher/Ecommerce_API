using SurveyManagementSystemApi.Abstractions.Consts;
using SurveyManagementSystemApi.Securty.Filters;


namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController(IAddressServices addressServices, ILogger<AddressController> logger) : ControllerBase
    {
        private readonly IAddressServices _addressServices = addressServices;
        private readonly ILogger<AddressController> _logger = logger;

        [HasPermission(Permissions.AddAddress)]
        [HttpPost("")]

        public async Task<IActionResult> AddAddress([FromBody] AddressRequest address)
        {
            var result = await _addressServices.AddAsync(address);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

        }

        [HasPermission(Permissions.UpdateAddress)]
        [HttpPut("{Id}")]

        public async Task<IActionResult> UpdateAddress([FromBody] AddressRequest address, int  Id)
        {
            var result = await _addressServices.UpdateAsync(address,Id);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HasPermission(Permissions.GetAddress)]
        [HttpGet("{Id}")]

        public async Task<IActionResult> GetAddressByID(int Id)
        {
            var result = await _addressServices.GetByIdAsync(Id);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HasPermission(Permissions.GetAddress)]
        [HttpGet("User-Addresses/{Id}")]

        public async Task<ActionResult> GetAllAddressesByUserID(string Id)
        {
            _logger.LogInformation("user id {userID} : and Time  {Date} : ", Id, DateTime.UtcNow);
            var result = await _addressServices.GetAllUserAddressesAsync(Id);
            return result .IsSuccess? Ok(result.Value) : result.ToProblem();
        }


        [HasPermission(Permissions.GetAddress)]
        [HttpGet("")]

        public async Task<IActionResult> GetAllAddresses()
        {
            var result = await _addressServices.GetAllAsync();
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
       


        [HasPermission(Permissions.DeleteAddress)]
        [HttpDelete("{Id}")]

        public async Task<IActionResult> DeleteAddress(int Id)
        {
            var result = await _addressServices.DeleteAsync(Id);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

       


    }


}
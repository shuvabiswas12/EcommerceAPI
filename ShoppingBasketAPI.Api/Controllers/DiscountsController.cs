using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingBasketAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        [HttpPost("{id}")]
        public async Task SetProductDiscount([FromRoute] string id)
        {
            await Task.CompletedTask;
        }

        [HttpDelete("{id}")]
        public async Task RemoveProductDiscount([FromRoute] string id)
        {
            await Task.CompletedTask;
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace CentreBookingApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CentreBookingApplicationController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private static readonly string _apiurl = "https://localhost:7225/api/";

        public CentreBookingApplicationController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        [Route("get-centres")]
        public async Task<IActionResult> GetCentres()
        {
            string route = _apiurl + "get-centres"
            
            var response = await _httpClient.GetAsync(route);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();

                // Return the response content
                return Ok(responseContent);
            }
            else
            {
                // Get the error message from the response content
                string errorContent = await response.Content.ReadAsStringAsync();
                string errorMessage = $"Error: {errorContent}";

                // Return a custom error message
                return new BadRequestObjectResult(errorMessage);
            }
        }
    }
}

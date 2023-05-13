using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using APIClasses;

namespace CentreBookingApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CentreBookingApplicationController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private static readonly string _apiurl = "https://localhost:7225/api/CentreBooking/";

        public CentreBookingApplicationController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        [Route("get-centres")]
        public async Task<IActionResult> GetCentres()
        {
            string route = _apiurl + "get-centres";
            var response = await _httpClient.GetAsync(route);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return Ok(responseContent);
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                string errorMessage = $"Error: {errorContent}";
                return new NotFoundObjectResult(errorMessage);
            }
        }

        [HttpPost]
        [Route("post-centre")]
        public async Task<IActionResult> PostCentre([FromBody] Centre centre)
        {
            var jsonContent = JsonContent.Create(centre);
            string route = _apiurl + "post-centre";
            var response = await _httpClient.PostAsync(route, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var result = await JsonSerializer.DeserializeAsync<Centre>(responseStream, options);
                return Ok(result);
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                string errorMessage = $"Error: {errorContent}";
                return new BadRequestObjectResult(errorMessage);
            }
        }

        [HttpGet]
        [Route("get-booking/{centreName}")]
        public  async Task<IActionResult> GetBooking(string centreName)
        {
            string route = _apiurl + "get-booking/" + centreName;
            var response = await _httpClient.GetAsync(route);
            
            if (response.IsSuccessStatusCode)
            {
                var bookings = await response.Content.ReadAsStringAsync();
                return Ok(bookings);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound) 
            {
                return NotFound();
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }

        [HttpPost]
        [Route("post-booking")]
        public async Task<IActionResult> PostBooking([FromBody] Booking booking)
        {
            var jsonContent = JsonContent.Create(booking);
            string route = _apiurl + "post-booking";
            var response = await _httpClient.PostAsync(route, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var result = await JsonSerializer.DeserializeAsync<Centre>(responseStream, options);
                return Ok(result);
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                string errorMessage = $"Error: {errorContent}";
                return new BadRequestObjectResult(errorMessage);
            }
        }
    }
}

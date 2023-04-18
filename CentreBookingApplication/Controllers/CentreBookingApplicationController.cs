﻿using CentreBookingApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;

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
                return new BadRequestObjectResult(errorMessage);
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
    }
}

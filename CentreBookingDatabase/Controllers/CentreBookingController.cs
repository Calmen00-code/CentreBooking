using CentreBookingDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;

namespace CentreBookingDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CentreBookingController : ControllerBase
    {
        public readonly IConfiguration _configuration;

        public CentreBookingController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("get-centres")]
        public IActionResult GetCentres()
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("CentreBooking").ToString());
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Centre", conn);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            List<Centre> centres = new List<Centre>();
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Centre centre = new Centre();
                    centre.CentreName = dt.Rows[i]["CentreName"].ToString();
                    centres.Add(centre);
                }
            }

            if (centres.Count > 0)
            {
                return Ok(centres);
            }
            else
            {
                /*
                response.StatusCode = 100;
                response.Message = "No data found";
                return JsonConvert.SerializeObject(response);
                */
                return new NotFoundObjectResult("No data found");
            }

        }

        [HttpPost]
        [Route("post-centre")]
        public IActionResult PostCentre([FromBody] Centre centre)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("CentreBooking").ToString());
            // TODO: Use better queries instead of string like this
            string sql_script = "INSERT INTO Centre (CentreName) VALUES (@CentreName);";
            SqlCommand sqlCommand = new SqlCommand(sql_script, conn);
            sqlCommand.Parameters.AddWithValue("@CentreName", centre.CentreName);
            Response response = new Response();

            try
            {
                conn.Open();
                int rowsAffected = sqlCommand.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Centre is successfully registered!";
                    return Ok(response);
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Internal server error, centre is not registered";
                    return new BadRequestObjectResult(response);
                }
            }
            catch (SqlException) 
            {
                response.StatusCode = 500;
                response.Message = "Internal server error";
                return new BadRequestObjectResult(response);
            }
            finally 
            { 
                conn.Close(); 
            }
        }

        [HttpPost]
        [Route("post-booking")]
        public IActionResult PostBooking([FromBody] Booking booking)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("CentreBooking").ToString());
            string sql_script = "INSERT INTO Booking (GuestName, StartDate, EndDate, CentreName) VALUES (@GuestName, @StartDate, @EndDate, @CentreName);";
            SqlCommand sqlCommand = new SqlCommand(sql_script, conn);
            sqlCommand.Parameters.AddWithValue("@GuestName", booking.GuestName);
            sqlCommand.Parameters.AddWithValue("@StartDate", booking.StartDate.ToString());
            sqlCommand.Parameters.AddWithValue("@EndDate", booking.EndDate.ToString());
            sqlCommand.Parameters.AddWithValue("@CentreName", booking.CentreName);
            Response response = new Response();

            try
            {
                conn.Open();
                int rowsAffected = sqlCommand.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Booking is successfully registered!";
                    return Ok(response);
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Internal server error, centre is not registered";
                    return new BadRequestObjectResult(response);
                }
            }
            catch (SqlException) 
            {
                response.StatusCode = 500;
                response.Message = "Internal server error";
                return new BadRequestObjectResult(response);
            }
            finally 
            { 
                conn.Close(); 
            }
        }
    }
}

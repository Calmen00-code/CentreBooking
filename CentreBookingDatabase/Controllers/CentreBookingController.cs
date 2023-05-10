using CentreBookingDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

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

            string[] possibleFormats = { "d/M/yyyy", "dd/MM/yyyy" };

            string startDate = booking.StartDate.ToString();
            DateTime parsedStartDate;

            if (DateTime.TryParseExact(startDate, possibleFormats, null, DateTimeStyles.None, out parsedStartDate))
            {
                string formattedStartDate = parsedStartDate.ToString("yyyy-MM-dd");
                sqlCommand.Parameters.AddWithValue("@StartDate", formattedStartDate);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Invalid start date format");
            }

            string endDate = booking.EndDate.ToString();
            DateTime parsedEndDate;

            if (DateTime.TryParseExact(endDate, possibleFormats, null, DateTimeStyles.None, out parsedEndDate))
            {
                string formattedEndDate = parsedEndDate.ToString("yyyy-MM-dd");
                sqlCommand.Parameters.AddWithValue("@EndDate", formattedEndDate);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Invalid end date format");
            }

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
            catch (SqlException e) 
            {
                response.StatusCode = 500;
                response.Message = "Internal server error";
                System.Diagnostics.Debug.WriteLine("Error: " + e.Message);
                return new BadRequestObjectResult(response);
            }
            finally 
            { 
                conn.Close(); 
            }
        }

        [HttpGet]
        [Route("get-booking/{centreName}")]
        public IActionResult GetBooking(string centreName)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("CentreBooking").ToString());
            string sql_command = "SELECT * FROM Booking WHERE Booking.CentreName = \'" + centreName + "\';";
            SqlDataAdapter adapter = new SqlDataAdapter(sql_command, conn);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            List<Booking> bookings = new List<Booking>();
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Booking booking = new Booking();
                    booking.CentreName = dt.Rows[i]["CentreName"].ToString();

                    DateTime startDateTime = (DateTime)dt.Rows[i]["StartDate"];
                    DateOnly startDate = new DateOnly(startDateTime.Year, startDateTime.Month, startDateTime.Day);
                    booking.StartDate = startDate;

                    DateTime endDateTime = (DateTime)dt.Rows[i]["EndDate"];
                    DateOnly endDate = new DateOnly(endDateTime.Year, endDateTime.Month, endDateTime.Day);
                    booking.EndDate = endDate;

                    booking.GuestName = dt.Rows[i]["GuestName"].ToString();

                    bookings.Add(booking);
                }
            }

            if (bookings.Count > 0)
            {
                return Ok(bookings);
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
    }
}

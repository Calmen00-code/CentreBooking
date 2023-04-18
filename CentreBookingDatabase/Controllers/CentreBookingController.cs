using CentreBookingDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
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
        public string GetCentres()
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
                return JsonConvert.SerializeObject(centres);
            }
            else
            {
                response.StatusCode = 100;
                response.Message = "No data found";
                return JsonConvert.SerializeObject(response);
            }

        }

        [HttpPost]
        [Route("post-centre")]
        public string PostCentre([FromBody] Centre centre)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("CentreBooking").ToString());
            string sql_script = "INSERT INTO Centre (CentreName) VALUES (\'" + centre.CentreName + "\');";
            SqlCommand sqlCommand = new SqlCommand(sql_script, conn);
            Response response = new Response();

            try
            {
                conn.Open();
                int rowsAffected = sqlCommand.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Centre is successfully registered!";
                    System.Diagnostics.Debug.WriteLine("SUCCESSFUL");
                }
                else
                {
                    response.StatusCode = 500;
                    response.Message = "Internal server error, centre is not registered";
                    System.Diagnostics.Debug.WriteLine("Internal server error, centre is not registered");
                }
            }
            catch (SqlException) 
            {
                System.Diagnostics.Debug.WriteLine("SQL Exception");
                response.StatusCode = 500;
                response.Message = "Internal server error";
            }
            conn.Close();
            return JsonConvert.SerializeObject(response);
        }
    }
}

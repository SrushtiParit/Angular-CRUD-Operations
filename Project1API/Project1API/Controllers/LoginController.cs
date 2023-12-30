using Project1API.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using Microsoft.IdentityModel.Tokens;

namespace Project1API.Controllers
{
    public class LoginController : ApiController
    {
        static string cs = ConfigurationManager.ConnectionStrings["mycon"].ConnectionString;
        private const string SecretKey = "yourSecretKey1234567890123456"; // Replace with your actual secret key

        public HttpResponseMessage Post(LoginModel _user)
        {
            string query = "select * from tblLogin where Username = @user";

            using (SqlConnection con = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@user", _user.Username);

                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();

                        string storedPassword = reader["Password"].ToString();
                        if (storedPassword == _user.Password)
                        {
                            // Passwords match, user authenticated
                            // Generate and return the token

                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
                            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                            var claims = new[]
                            {
                                new Claim(ClaimTypes.Name, _user.Username),
                                // Add more claims as needed
                            };

                            var token = new JwtSecurityToken(
                                issuer: "yourIssuer",
                                audience: "yourAudience",
                                claims: claims,
                                expires: DateTime.Now.AddMinutes(30), // Token expiration time
                                signingCredentials: creds
                            );

                            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                            // Send the token as part of the login response
                            return Request.CreateResponse(HttpStatusCode.OK, tokenString);
                        }
                        else
                        {
                            // Passwords don't match
                            return Request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid password");
                        }
                    }
                    else
                    {
                        // No user found
                        return Request.CreateResponse(HttpStatusCode.NotFound, "No user found");
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error fetching data: " + ex.Message);
                }
            }
        }
    }
}

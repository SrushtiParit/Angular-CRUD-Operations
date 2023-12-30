using Project1API.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Project1API.Controllers
{
    public class AllUsersController : ApiController
    {
        static string cs = ConfigurationManager.ConnectionStrings["mycon"].ConnectionString;
        public SqlConnection con = new SqlConnection(cs);
        public HttpResponseMessage Get(string Search)
        {
            List<User> lst = new List<User>();
            SqlCommand cmd = new SqlCommand("spUserList", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Search", Search);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                lst.Add(new User
                {
                    UserID = Convert.ToInt16(dr["UserID"]),
                    Username = Convert.ToString(dr["Username"]),
                    Phone = Convert.ToString(dr["Phone"]),
                    Address = Convert.ToString(dr["Address"]),
                    Email = Convert.ToString(dr["Email"])

                });
            }
            return Request.CreateResponse(HttpStatusCode.OK, lst);
        }

        public string Post(User _userModel)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("spInsertUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", _userModel.Username);
                    cmd.Parameters.AddWithValue("@Phone", _userModel.Phone);
                    cmd.Parameters.AddWithValue("@Address", _userModel.Address);
                    cmd.Parameters.AddWithValue("@Email", _userModel.Email);
                    con.Open();
                    int num = cmd.ExecuteNonQuery();
                    con.Close();
                }
                return "Added Successfully!!!!!";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return "Failed to add data. Exception: " + ex.Message;
            }
        }

        public string Put(User _userModel)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("spUpdateUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", _userModel.UserID);
                    cmd.Parameters.AddWithValue("@Username", _userModel.Username);
                    cmd.Parameters.AddWithValue("@Phone", _userModel.Phone);
                    cmd.Parameters.AddWithValue("@Address", _userModel.Address);
                    cmd.Parameters.AddWithValue("@Email", _userModel.Email);
                    con.Open();
                    int num = cmd.ExecuteNonQuery();
                    con.Close();
                }
                return "Updated Successfully!!!!!";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return "Failed to update data. Exception: " + ex.Message;
            }
        }

        public string Delete(User _userModel)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("spDeleteUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", _userModel.UserID);
                    con.Open();
                    int num = cmd.ExecuteNonQuery();
                    con.Close();
                }
                return "Deleted Successfully!!!!!";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return "Failed to delete data. Exception: " + ex.Message;
            }
        }
    }
}

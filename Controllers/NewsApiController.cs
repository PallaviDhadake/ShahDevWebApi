using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using ShahDevWebApi.Models;
using System.Collections.Generic;
using NewProjCrud;

namespace ShahDevWebApi.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class NewsApiController : Controller
    {
        private readonly IConfiguration _configuration;
        iClass c;

        public NewsApiController(IConfiguration configuration)
        {
            _configuration = configuration;
            c = new iClass(_configuration);
        }

        //Get Method
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<NewsDataClass> GetsDataClass()
        {
            List<NewsDataClass> nwslist= new List<NewsDataClass> (); 
            using(DataTable dtnews=c.GetDataTable("Select * from NewsData"))
            {
                foreach (DataRow row in dtnews.Rows)
                {
                    NewsDataClass news = new NewsDataClass
                    {
                        NewsId = (int)row["newsId"],
                        NameTitle = row["newsTitle"].ToString(),
                        NewsDescription = row["newsInfo"].ToString()

                    };

                    nwslist.Add(news);                }
            }

            return nwslist;
        }

        //Post Method
        [HttpPost]
        [Route("AddNew")]
        public bool CreateNews(string newsTitle, string newsInfo)
        {

            using(SqlConnection conn = new SqlConnection(c.GetConnectionString()))
            {
                conn.Open();
                SqlCommand cmdInsertNews = new SqlCommand("spInsertNews", conn);
                cmdInsertNews.CommandType=CommandType.StoredProcedure;
                //Add Parameters
                cmdInsertNews.Parameters.AddWithValue("@newsTitle", newsTitle);
                cmdInsertNews.Parameters.AddWithValue("@newsInfo", newsInfo);
                cmdInsertNews.ExecuteNonQuery();

            }
            return true;
        }

        //Put Method
        [HttpPut]
        [Route("Update")]
        public bool UpdateNews(int newsId, string newsTitle, string newsInfo)
        {

            using(SqlConnection conn = new SqlConnection(c.GetConnectionString()))
            {
                conn.Open();
                SqlCommand cmdUpdatenews = new SqlCommand("spUpdateNews", conn);
                cmdUpdatenews.CommandType=CommandType.StoredProcedure;

                //Add Parameters
                cmdUpdatenews.Parameters.AddWithValue("@newsId", newsId);
                cmdUpdatenews.Parameters.AddWithValue("@newsTitle", newsTitle);
                cmdUpdatenews.Parameters.AddWithValue("@newsInfo", newsInfo);
                cmdUpdatenews.ExecuteNonQuery();
            }
            return true;
        }


        //Delete Method
        [HttpDelete]
        [Route("Delete")]

        public bool DeleteNews(int newsId)
        {
            using(SqlConnection conn=new SqlConnection(c.GetConnectionString()))
            {
                conn.Open();

                // Delete from Students table using query
                string deleteStudentQuery = "DELETE FROM NewsData WHERE newsId=@newsId";
                SqlCommand cmdDeleteNews = new SqlCommand(deleteStudentQuery, conn);
                cmdDeleteNews.Parameters.AddWithValue("@newsId", newsId);
                cmdDeleteNews.ExecuteNonQuery();
            } 

            return true;
        }

    }
}

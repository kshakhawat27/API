using DAY_ONE.DAL;
using DAY_ONE.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAY_ONE.Controllers
{
    public class ClassController : Controller
    {
        // GET: Class
        public ActionResult Names()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SaveData(ClassDBModel _dbModel)
        {
            int Result = 0;
            Result = SaveName( _dbModel);
            if (Result > 0)
                return Json(new { Success = true });
            else
                return Json(new { Success = true });

        }


        [HttpGet]
        public JsonResult LoadAllData()
        {
            List<ClassDBModel> _dbModelList = new List<ClassDBModel>();
            _dbModelList = LoadDataFromDataSet();
            return this.Json(_dbModelList, JsonRequestBehavior.AllowGet);
        }
        private List<ClassDBModel> LoadDataFromDataSet()
        {
            List<ClassDBModel> _modelList = new List<ClassDBModel>();
            SqlConnection conn = new SqlConnection(DBConnection.GetConnection());
            conn.Open();
            SqlCommand dAd = new SqlCommand("SELECT * FROM Class", conn);
            SqlDataAdapter sda = new SqlDataAdapter(dAd);
            dAd.CommandType = CommandType.Text;
            DataTable dt = new DataTable();
            try
            {
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    _modelList = (from DataRow row in dt.Rows
                                  select new ClassDBModel
                                  {
                                      ClassID = Convert.ToInt32(row["ClassID"].ToString()),
                                      ClassName = row["ClassName"].ToString(),
                                      AddedBy = row["AddedBy"].ToString()
                                  }).ToList();
                }
                return _modelList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt.Dispose();
                dAd.Dispose();
                conn.Close();
                conn.Dispose();
            }
        }

        private int SaveName(ClassDBModel _dbModel)
        {
            SqlConnection conn = new SqlConnection(DBConnection.GetConnection());

            conn.Open();
            SqlCommand dCmd = new SqlCommand(@"INSERT INTO Class (ClassName, AddedBy)
                                              VALUES(@ClassName, @AddedBy)", conn);
            dCmd.CommandType = System.Data.CommandType.Text;

            try
            {
                dCmd.Parameters.AddWithValue("@ClassID", 0);
                dCmd.Parameters.AddWithValue("@ClassName", _dbModel.ClassName);
                dCmd.Parameters.AddWithValue("@AddedBy", "Iqbal");
                return dCmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dCmd.Dispose();
                conn.Close();
                conn.Dispose();

            }
        }
    }
}
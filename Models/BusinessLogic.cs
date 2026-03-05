using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Xml;
using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace API_Project_dotnetcore.Models
{
    public class BussinessLogic
    {
        ConnectionString conn = new ConnectionString();
        public string getdatatablejsondata(string proc, string json, string connection)
        {
            DataSet ds = new DataSet("data1");
            XmlDocument xml = new XmlDocument();
            XmlDocument doc = new XmlDocument();
            try
            {
                using (SqlConnection con = conn.sqlCon(connection))
                {
                    SqlCommand sqlComm = new SqlCommand(proc, con);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    if (json != "" && json != null)
                    {
                        var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        foreach (var kv in dict)
                        {
                            if (kv.Key == "xml")
                            {
                                xml = (XmlDocument)JsonConvert.DeserializeXmlNode(kv.Value);
                                sqlComm.Parameters.AddWithValue("@xml", xml.InnerXml.ToString());
                            }
                            else
                            {
                                sqlComm.Parameters.AddWithValue("@" + kv.Key, kv.Value);
                            }
                        }

                    }
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {

            }
            string JSONresult = JsonConvert.SerializeObject(ds.Tables[0]);
            return JSONresult;
        }
        public string saveJson(string proc, string json, string otherparameter, string connection)
        {
            XmlDocument doc = new XmlDocument();
            using (SqlConnection con = conn.sqlCon(connection))
            {
                string query = proc;
                SqlCommand cmd = new SqlCommand(query);
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                if (json != "")
                {
                    var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    foreach (var kv in dict)
                    {
                        if (kv.Key == "xml")
                        {
                            doc = (XmlDocument)JsonConvert.DeserializeXmlNode(kv.Value);
                            cmd.Parameters.AddWithValue("@xml", doc.InnerXml.ToString());
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@" + kv.Key, kv.Value);
                        }
                    }
                }
                if (otherparameter != "" && otherparameter != null)
                {
                    doc.LoadXml(otherparameter);
                    XmlNodeList nodes = doc.GetElementsByTagName("Definition");
                    foreach (XmlNode node in nodes)
                    {
                        string param = node.Attributes["paramname"].Value;
                        string value = node.Attributes["paramvalue"].Value;
                        cmd.Parameters.AddWithValue("@" + param, value);

                    }
                }
                int i = 0;
                i = cmd.ExecuteNonQuery();

                try
                {
                    if (i > 0)
                    {
                        return "status1";

                    }
                    else
                    {
                        return "status0";
                    }
                }
                catch
                {
                    return "status0";
                }
            }
        }

        public string GetDataXmltoJson(string proc, string json, string connection)
        {
            DataSet ds = new DataSet("data1");
            XmlDocument xml = new XmlDocument();
            XmlDocument doc = new XmlDocument();
            if (connection == "" || connection == null) connection = "connectionstring";
            //using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[connection].ConnectionString))
            //using (SqlConnection con = conn.sqlConnection("connectionstring"))
            using (SqlConnection con = conn.sqlCon(connection))
            {
                SqlCommand sqlComm = new SqlCommand(proc, con);
                sqlComm.CommandType = CommandType.StoredProcedure;
                if (json != "" && json != null)
                {
                    var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    foreach (var kv in dict)
                    {
                        //string value = kv.Value;
                        //sqlComm.Parameters.AddWithValue("@" + kv.Key, value);
                        if (kv.Key == "xml")
                        {
                            xml = (XmlDocument)JsonConvert.DeserializeXmlNode(kv.Value);
                            sqlComm.Parameters.AddWithValue("@xml", xml.InnerXml.ToString());
                        }
                        else
                        {
                            sqlComm.Parameters.AddWithValue("@" + kv.Key, kv.Value);
                        }
                    }
                }
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                da.Fill(ds);
                string str = "";
                if (ds.Tables[0].Rows.Count >= 1)
                {
                    for (var i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                    {
                        str = str + ds.Tables[0].Rows[i][0].ToString();

                    }
                    if (str == "")
                    {
                        str = "<root><subroot></subroot></root>";
                    }

                    xml.LoadXml(str);
                }
                else
                {
                    str = "<root><subroot><error>error</error></subroot></root>";
                    xml.LoadXml(str);
                }
            }
            return JsonConvert.SerializeXmlNode(xml);
        }

        public string GetDataTableJsonDataSql(string proc, string json, string connection)
        {
            DataSet ds = new DataSet("data1");

            try
            {
                using (SqlConnection con = conn.sqlCon(connection))
                using (SqlCommand sqlComm = new SqlCommand(proc, con))
                {
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        try
                        {
                            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                            if (dict != null)
                            {
                                foreach (var kv in dict)
                                {
                                    if (kv.Key == "xml")
                                    {
                                        if (!string.IsNullOrWhiteSpace(kv.Value))
                                        {
                                            XmlNode? node = JsonConvert.DeserializeXmlNode(kv.Value);

                                            if (node != null)
                                            {
                                                XmlDocument xml = new XmlDocument();
                                                xml.AppendChild(xml.ImportNode(node, true));
                                                sqlComm.Parameters.AddWithValue("@xml", xml.InnerXml);
                                            }
                                            else
                                            {
                                                Console.WriteLine("Warning: XML deserialization returned null.");
                                                sqlComm.Parameters.AddWithValue("@xml", DBNull.Value);
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Warning: XML input is null or empty.");
                                            sqlComm.Parameters.AddWithValue("@xml", DBNull.Value);
                                        }
                                    }
                                    else
                                    {
                                        sqlComm.Parameters.AddWithValue("@" + kv.Key, string.IsNullOrWhiteSpace(kv.Value) ? (object)DBNull.Value : kv.Value);
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Warning: JSON deserialization returned null.");
                            }
                        }
                        catch (JsonException jsonEx)
                        {
                            Console.WriteLine("JSON Deserialization Exception: " + jsonEx.Message);
                            throw;
                        }
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(sqlComm))
                    {
                        da.Fill(ds);
                    }
                }

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0]?.ToString() ?? string.Empty;
                }
                else
                {
                    Console.WriteLine("Warning: No data returned from stored procedure.");
                    return string.Empty;
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("SQL Exception: " + sqlEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                throw;
            }
        }


        //public string savexml(string proc, string json, string otherparameter)
        //{
        //    XmlDocument doc = new XmlDocument();
        //    using (SqlConnection con = conn.sqlCon("connectionstringCommonRoleRights"))
        //    using (SqlCommand sqlComm = new SqlCommand(proc, con))
        //    {
        //        string query = proc;
        //        SqlCommand cmd = new SqlCommand(query);
        //        cmd.Connection = con;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        con.Open();
        //        if (json != "")
        //        {
        //            doc = (XmlDocument)JsonConvert.DeserializeXmlNode(json);
        //            cmd.Parameters.AddWithValue("@xml", doc.InnerXml.ToString());
        //        }
        //        if (otherparameter != "" && otherparameter != null)
        //        {
        //            doc.LoadXml(otherparameter);
        //            XmlNodeList nodes = doc.GetElementsByTagName("Definition");
        //            foreach (XmlNode node in nodes)
        //            {
        //                string param = node.Attributes["paramname"].Value;
        //                string value = node.Attributes["paramvalue"].Value;
        //                cmd.Parameters.AddWithValue("@" + param, value);

        //            }
        //        }
        //        int i = 0;
        //        i = cmd.ExecuteNonQuery();

        //        try
        //        {
        //            if (i > 0)
        //            {
        //                return "success";

        //            }
        //            else
        //            {
        //                return "failed";
        //            }
        //        }
        //        catch
        //        {

        //            return "error";
        //        }
        //    }

        //}

        //public XmlDocument getXmlData(string proc, string variable, string connectionstring)
        //{

        //    DataSet ds = new DataSet("data1");
        //    XmlDocument xml = new XmlDocument();
        //    //   using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[connectionstring].ConnectionString))
        //    if (connectionstring == "" || connectionstring == null) connectionstring = "connectionstring";
        //    using (SqlConnection con = conn.sqlCon(connectionstring))
        //    {
        //        SqlCommand sqlComm = new SqlCommand(proc, con);
        //        sqlComm.CommandType = CommandType.StoredProcedure;

        //        if (variable != "" && variable != null)
        //        {
        //            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(variable);
        //            foreach (var kv in dict)
        //            {

        //                if (kv.Key == "xml")
        //                {
        //                    xml = (XmlDocument)JsonConvert.DeserializeXmlNode(kv.Value);
        //                    sqlComm.Parameters.AddWithValue("@xml", xml.InnerXml.ToString());
        //                }
        //                else
        //                {
        //                    sqlComm.Parameters.AddWithValue("@" + kv.Key, kv.Value);
        //                }
        //            }

        //        }
        //        SqlDataAdapter da = new SqlDataAdapter();
        //        da.SelectCommand = sqlComm;
        //        da.Fill(ds);
        //    }

        //    try
        //    {
        //        string str = "";
        //        if (ds.Tables[0].Rows.Count >= 1)
        //        {
        //            for (var i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
        //            {
        //                str = str + ds.Tables[0].Rows[i][0].ToString();

        //            }
        //            if (str == "")
        //            {
        //                str = "<root><subroot></subroot></root>";
        //            }

        //            xml.LoadXml(str);


        //        }
        //        else
        //        {
        //            str = "<root><subroot><error>error</error></subroot></root>";
        //            xml.LoadXml(str);
        //        }

        //        return xml;
        //    }
        //    catch (Exception ex)
        //    {
        //        xml.Load("<root><subroot><messege>There is some error while fetching data.Plz contact service provider</messege></subroot></root>");
        //        return xml;

        //    }
        //}
    }

    public class JsonTextActionResult : IActionResult
    {
        public string JsonText { get; }

        public JsonTextActionResult(string jsonText)
        {
            JsonText = jsonText;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = "application/json";
            await response.WriteAsync(JsonText, Encoding.UTF8);
        }
    }

    public class GetAll
    {
        public string FilterParameter { get; set; }
    }
}

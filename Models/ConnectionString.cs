using Microsoft.Data.SqlClient;


namespace API_Project_dotnetcore.Models
{
    public class ConnectionString
    {
        string? strUserId;
        string? strPassword;
        string? strServerName;
        string? strDBName;

        public SqlConnection sqlCon(string connstr)
        {
            string strMasterCon = "";

            // for shubham pc only START
            //if (connstr == "connectionstring")
            //{
            //    strServerName = @"DESKTOP-DAJQCR7\SQLEXPRESS";
            //    strDBName = "Ecommmerce";
            //}

            //strMasterCon += "Data Source=" + strServerName;
            //strMasterCon += ";Initial Catalog=" + strDBName;
            //strMasterCon += ";Integrated Security=True";  // ✅ Enables Windows Authentication
            //strMasterCon += ";TrustServerCertificate=True";
            //strMasterCon += ";Encrypt=False";
            //strMasterCon += ";Connect Timeout=10000";
            // for shubham pc only END


            // for other pc or users start


            if (connstr == "connectionstring")
            {
                strUserId = "sa";
                strPassword = "pass@123";
                strServerName = @"KAUSTUBH";
                strDBName = "Ecommmerce";
            }

            strMasterCon = "User ID=" + strUserId;
            strMasterCon += ";password=" + strPassword;
            strMasterCon += ";data source=" + strServerName;
            strMasterCon += ";persist security info=False";
            strMasterCon += ";initial catalog=" + strDBName;
            strMasterCon += ";TrustServerCertificate=True";  // ✅ Important Fix
            strMasterCon += ";Encrypt=False";  // ✅ Important Fix
            strMasterCon += ";Connect Timeout=10000";

            return new SqlConnection(strMasterCon);
        }

    }
}

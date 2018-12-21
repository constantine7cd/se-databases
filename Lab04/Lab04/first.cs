using Microsoft.SqlServer.Server;  
using System.Data.SqlClient;  

public class first
{
    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static int ReturnFineCount()
    {
        using (SqlConnection conn
            = new SqlConnection("context connection=true"))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(*) AS 'Fine Count' FROM Fines", conn);
            return (int)cmd.ExecuteScalar();
        }
    }
}

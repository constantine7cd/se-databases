using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class fourth
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void updateFines (SqlString cause, double factor)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();
            SqlCommand command = new SqlCommand("update Fines set Cost = Cost * @fact where Cause = @cause", connection);

            command.Parameters.AddWithValue("@fact", factor);
            command.Parameters.AddWithValue("@cause", cause);

            SqlDataReader r = command.ExecuteReader();
            SqlContext.Pipe.Send(r);
        }
    }
}

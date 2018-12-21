using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

public partial class Triggers
{
    // Enter existing table or view for the target and uncomment the attribute line
    //[Microsoft.SqlServer.Server.SqlTrigger(Name = "", Target = "Fines", Event = "FOR DELETE")]
    public static void fifth()
    {
        // Replace with your own code
        SqlCommand command;
        SqlTriggerContext triggContext = SqlContext.TriggerContext;
        SqlPipe pipe = SqlContext.Pipe;
        SqlDataReader reader;

        using (SqlConnection connection
            = new SqlConnection("context connection=true"))
        {
            connection.Open();
            command = new SqlCommand("SELECT * FROM DELETED;",
                connection);
            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                pipe.Send("You deleted the following rows:");
                while (reader.Read())
                {
                    pipe.Send("'" + reader.GetInt32(0)
                    + "', '" + reader.GetInt32(1) + "'");
                }

                reader.Close();

                //alternately, to just send a tabular resultset back:  
                //pipe.ExecuteAndSend(command);  
            }
            else
            {
                pipe.Send("No rows affected.");
            }
        }

    }

}

using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

using Microsoft.SqlServer.Server;

public partial class third
{
    private class AccidentResult
    {
        public SqlInt32 AccidentId;
        public double Fraction;

        public AccidentResult(SqlInt32 accidentId, double fraction)
        {
            AccidentId = accidentId;
            Fraction = fraction;
        }
    }

    public static double ComputeFraction(SqlInt32 died, SqlInt32 particip)
    {
        if (particip != 0)
        {
            return (double) died / (double) particip;
        }

        return 0;
    }

    [SqlFunction(
        DataAccess = DataAccessKind.Read,
        FillRowMethodName = "Accidents_FillRow",
        TableDefinition = "AccidentId int, Fraction float")]
    public static IEnumerable Accidents(SqlDateTime sinceDate)
    {
        ArrayList resultCollection = new ArrayList();

        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            using (SqlCommand selectAccidents = new SqlCommand(
                "SELECT " +
                "AccidentId, AmountOfDied, AmountParticipants " +
                "FROM Accidents " +
                "WHERE DateOfAccident >= @data",
                connection))
            {
                SqlParameter modifiedSinceParam = selectAccidents.Parameters.Add(
                    "@data",
                    SqlDbType.DateTime);
                modifiedSinceParam.Value = sinceDate;

                using (SqlDataReader accReader = selectAccidents.ExecuteReader())
                {
                    while (accReader.Read())
                    {
                        resultCollection.Add(new AccidentResult(accReader.GetSqlInt32(0),
                            ComputeFraction(accReader.GetSqlInt32(1), accReader.GetSqlInt32(2))));
                    }
                }
            }
        }

        return resultCollection;
    }

    public static void Accidents_FillRow(
    object AccidentResultObj,
    out SqlInt32 AccId,
    out double Fraction)
    {
        AccidentResult accRes = (AccidentResult)AccidentResultObj;

        AccId = accRes.AccidentId;
        Fraction = accRes.Fraction;
    }

};

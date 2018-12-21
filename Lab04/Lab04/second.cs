using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(Format.Native)]
public struct AvgWithCondition
{
    public void Init()
    {
        sum = 0;
        count = 0;
    }

    public void Accumulate(SqlInt32 Value, SqlBoolean isPaid)
    {
        if (!Value.IsNull)
        {
            if (!isPaid)
            {
                Value *= 2;
            }

            sum += (long)Value;
            count++;
        }
    }

    public void Merge (AvgWithCondition Group)
    {
        sum += Group.sum;
        count += Group.count;
    }

    public SqlInt32 Terminate ()
    {
        if (count > 0)
        {
            int value = (int)(sum / count);
            return new SqlInt32(value);
        }
        else
        {
            return SqlInt32.Null;
        }
    }

    // This is a place-holder member field
    private long sum;
    private int count;
}

/*
[Serializable]
[SqlUserDefinedAggregate(
    Format.Native,
    IsInvariantToDuplicates = false,
    IsInvariantToNulls = true,
    IsInvariantToOrder = true,
    IsNullIfEmpty = true,
    Name = "WeightedAvg")]
public struct WeightedAvg
{
    /// <summary>  
    /// The variable that holds the intermediate sum of all values multiplied by their weight  
    /// </summary>  
    private long sum;

    /// <summary>  
    /// The variable that holds the intermediate sum of all weights  
    /// </summary>  
    private int count;

    /// <summary>  
    /// Initialize the internal data structures  
    /// </summary>  
    public void Init()
    {
        sum = 0;
        count = 0;
    }

    /// <summary>  
    /// Accumulate the next value, not if the value is null  
    /// </summary>  
    /// <param name="Value">Next value to be aggregated</param>  
    /// <param name="Weight">The weight of the value passed to Value parameter</param>  
    public void Accumulate(SqlInt32 Value, SqlInt32 Weight)
    {
        if (!Value.IsNull && !Weight.IsNull)
        {
            sum += (long)Value * (long)Weight;
            count += (int)Weight;
        }
    }

    /// <summary>  
    /// Merge the partially computed aggregate with this aggregate  
    /// </summary>  
    /// <param name="Group">The other partial results to be merged</param>  
    public void Merge(WeightedAvg Group)
    {
        sum += Group.sum;
        count += Group.count;
    }

    /// <summary>  
    /// Called at the end of aggregation, to return the results of the aggregation.  
    /// </summary>  
    /// <returns>The weighted average of all inputed values</returns>  
    public SqlInt32 Terminate()
    {
        if (count > 0)
        {
            int value = (int)(sum / count);
            return new SqlInt32(value);
        }
        else
        {
            return SqlInt32.Null;
        }
    }
}
*/
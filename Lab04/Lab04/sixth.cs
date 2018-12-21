using System;
using System.Data;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Text;



[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.Native,
     IsByteOrdered = true)]
public struct Vector3f : INullable
{
    private bool is_Null;
    private Int32 _x;
    private Int32 _y;
    private Int32 _z;

    public bool IsNull
    {
        get
        {
            return (is_Null);
        }
    }

    public static Vector3f Null
    {
        get
        {
            Vector3f v = new Vector3f();
            v.is_Null = true;
            return v;
        }
    }
 
    public override string ToString()
    {

        if (this.IsNull)
            return "NULL";
        else
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(_x);
            builder.Append(",");
            builder.Append(_y);
            builder.Append(",");
            builder.Append(_z);
            return builder.ToString();
        }
    }

    [SqlMethod(OnNullCall = false)]
    public static Vector3f Parse(SqlString s)
    {
        if (s.IsNull)
            return Null;
 
        Vector3f v = new Vector3f();
        string[] xyz = s.Value.Split(",".ToCharArray());
        v.X = Int32.Parse(xyz[0]);
        v.Y = Int32.Parse(xyz[1]);
        v.Z = Int32.Parse(xyz[2]);

        return v;
    }


    public Int32 X
    {
        get
        {
            return this._x;
        }
 
        set
        {
            _x = value;
        }
    }

    public Int32 Y
    {
        get
        {
            return this._y;
        }
        set
        {
            _y = value;
        }
    }

    public Int32 Z
    {
        get
        {
            return this._z;
        }
        set
        {
            _z = value;
        }
    }

    [SqlMethod(OnNullCall = false)]
    public Double Length()
    {
        return Math.Sqrt(_x * _x + _y * _y + _z * _z);
    }

}
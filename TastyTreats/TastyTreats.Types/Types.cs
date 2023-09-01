using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TastyTreats.Types
{
    public struct ParmStruct
    {
        public ParmStruct(string name, SqlDbType dataType,
            object value, int size = 0, ParameterDirection direction = ParameterDirection.Input)
        {
            Name = name;
            Value = value;
            Size = size;
            DataType = dataType;
            Direction = direction;
        }

        public string Name;
        public object Value;
        public int Size;
        public SqlDbType DataType;
        public ParameterDirection Direction;
    }

    public enum ErrorType
    {
        Model,
        Business
    }
}

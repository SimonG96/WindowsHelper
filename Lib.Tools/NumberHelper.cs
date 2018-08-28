using System;

namespace Lib.Tools
{
    public static class NumberHelper
    {
        public static bool IsNumericType(this object obj)
        {
            return obj is Byte ||
                   obj is SByte ||
                   obj is UInt16 ||
                   obj is UInt32 ||
                   obj is UInt64 ||
                   obj is Int16 ||
                   obj is Int32 ||
                   obj is Int64 ||
                   obj is Decimal ||
                   obj is Double ||
                   obj is Single;
        }

        public static object Increment(this object obj)
        {
            if (!obj.IsNumericType())
                return obj;

            switch (obj)
            {
                case Byte byteVal:
                    return byteVal + 1;
                case SByte sbyteVal:
                    return sbyteVal + 1;
                case UInt16 uint16Val:
                    return uint16Val + 1;
                case UInt32 uint32Val:
                    return uint32Val + 1;
                case UInt64 uint64Val:
                    return uint64Val + 1;
                case Int16 int16Val:
                    return int16Val + 1;
                case Int32 int32Val:
                    return int32Val + 1;
                case Int64 int64Val:
                    return int64Val + 1;
                case Decimal decVal:
                    return decVal + 1;
                case Double doubleVal:
                    return doubleVal + 1;
                case Single singleVal:
                    return singleVal + 1;
                default:
                    return obj;
            }
        }

        public static object Decrement(this object obj)
        {
            if (!obj.IsNumericType())
                return obj;

            switch (obj)
            {
                case Byte byteVal:
                    return byteVal - 1;
                case SByte sbyteVal:
                    return sbyteVal - 1;
                case UInt16 uint16Val:
                    return uint16Val - 1;
                case UInt32 uint32Val:
                    return uint32Val - 1;
                case UInt64 uint64Val:
                    return uint64Val - 1;
                case Int16 int16Val:
                    return int16Val - 1;
                case Int32 int32Val:
                    return int32Val - 1;
                case Int64 int64Val:
                    return int64Val - 1;
                case Decimal decVal:
                    return decVal - 1;
                case Double doubleVal:
                    return doubleVal - 1;
                case Single singleVal:
                    return singleVal - 1;
                default:
                    return obj;
            }
        }
    }
}
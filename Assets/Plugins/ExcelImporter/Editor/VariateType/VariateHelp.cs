using UnityEngine;

namespace Excel
{
    public static class VariateHelp
    {
        // sbyte parse
        public static bool TryGetValue(string valueString, out sbyte result)
        {
            var ok = sbyte.TryParse(valueString, out result);
            result = ok ? result : default(sbyte);
            return ok;
        }
        
        // short parse
        public static bool TryGetValue(string valueString, out short result)
        {
            var ok = short.TryParse(valueString, out result);
            result = ok ? result : default(short);
            return ok;
        }
        
        // int parse
        public static bool TryGetValue(string valueString, out int result)
        {
            var ok = int.TryParse(valueString, out result);
            result = ok ? result : default(int);
            return ok;
        }
        
        // long parse
        public static bool TryGetValue(string valueString, out long result)
        {
            var ok = long.TryParse(valueString, out result);
            result = ok ? result : default(long);
            return ok;
        }
        
        // byte parse
        public static bool TryGetValue(string valueString, out byte result)
        {
            var ok = byte.TryParse(valueString, out result);
            result = ok ? result : default(byte);
            return ok;
        }
        
        // ushort parse
        public static bool TryGetValue(string valueString, out ushort result)
        {
            var ok = ushort.TryParse(valueString, out result);
            result = ok ? result : default(ushort);
            return ok;
        }
        
        // uint parse
        public static bool TryGetValue(string valueString, out uint result)
        {
            var ok = uint.TryParse(valueString, out result);
            result = ok ? result : default(uint);
            return ok;
        }
        
        // ulong parse
        public static bool TryGetValue(string valueString, out ulong result)
        {
            var ok = ulong.TryParse(valueString, out result);
            result = ok ? result : default(ulong);
            return ok;
        }
        
        // float parse
        public static bool TryGetValue(string valueString, out float result)
        {
            var ok = float.TryParse(valueString, out result);
            result = ok ? result : default(float);
            return ok;
        }
        
        // double parse
        public static bool TryGetValue(string valueString, out double result)
        {
            var ok = double.TryParse(valueString, out result);
            result = ok ? result : default(double);
            return ok;
        }
        
        // string parse
        public static bool TryGetValue(string valueString, out string result)
        {
            result = valueString;
            return true;
        }
        
        // bool parse
        public static bool TryGetValue(string valueString, out bool result)
        {
            var ok = bool.TryParse(valueString, out result);
            result = ok ? result : default(bool);
            return ok;
        }

        #region Vector

        // Vector2 parse
        public static bool TryGetValue(string valueString, out Vector2 result,char splitChar = '_')
        {
            result = Vector2.zero;
            if (string.IsNullOrEmpty(valueString))
            {
                return true;
            }

            var variates = valueString.Split(splitChar);
            if (variates.Length != 2)
            {
                return false;
            }

            if (!float.TryParse(variates[0], out float x))
            {
                return false;
            }
            if (!float.TryParse(variates[1], out float y))
            {
                return false;
            }
            
            result.Set(x,y);
            return true;
        }
        
        // Vector2Int parse
        public static bool TryGetValue(string valueString, out Vector2Int result,char splitChar = '_')
        {
            result = Vector2Int.zero;
            if (string.IsNullOrEmpty(valueString))
            {
                return true;
            }

            var variates = valueString.Split(splitChar);
            if (variates.Length != 2)
            {
                return false;
            }

            if (!int.TryParse(variates[0], out int x))
            {
                return false;
            }
            if (!int.TryParse(variates[1], out int y))
            {
                return false;
            }
            
            result.Set(x,y);
            return true;
        }
        
        // Vector3 parse
        public static bool TryGetValue(string valueString, out Vector3 result,char splitChar = '_')
        {
            result = Vector3.zero;
            if (string.IsNullOrEmpty(valueString))
            {
                return true;
            }

            var variates = valueString.Split(splitChar);
            if (variates.Length != 3)
            {
                return false;
            }

            if (!float.TryParse(variates[0], out float x))
            {
                return false;
            }
            if (!float.TryParse(variates[1], out float y))
            {
                return false;
            }
            if (!float.TryParse(variates[2], out float z))
            {
                return false;
            }


            result.Set(x, y, z);
            return true;
        }
        
        // Vector3Int parse
        public static bool TryGetValue(string valueString, out Vector3Int result,char splitChar = '_')
        {
            result = Vector3Int.zero;
            if (string.IsNullOrEmpty(valueString))
            {
                return true;
            }

            var variates = valueString.Split(splitChar);
            if (variates.Length != 3)
            {
                return false;
            }

            if (!int.TryParse(variates[0], out int x))
            {
                return false;
            }
            if (!int.TryParse(variates[1], out int y))
            {
                return false;
            }
            if (!int.TryParse(variates[2], out int z))
            {
                return false;
            }


            result.Set(x, y, z);
            return true;
        }
        #endregion
        
    }
}
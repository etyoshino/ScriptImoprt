using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Excel
{
    public static class ExcelHelp
    {
        public static System.Object[] ToObjectArray<T>(this List<T> @this, Func<T, System.Object> func)
        {
            return ToArray(@this, func);
        }
        
        public static TValue[] ToArray<T, TValue>(this List<T> @this, Func<T, TValue> func)
        {
            TValue[] values = new TValue[@this.Count];
            for (int i = 0, iLength = @this.Count; i < iLength; i++)
            {
                values[i] = func(@this[i]);
            }
            return values;
        }
    }
}
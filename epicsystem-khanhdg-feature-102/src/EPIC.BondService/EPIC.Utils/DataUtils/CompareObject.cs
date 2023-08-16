using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.DataUtils
{
    public static class CompareObject
    {
        public static List<Variance> DetailedCompare<T>(this T val1, T val2)
        {
            List<Variance> variances = new List<Variance>();
            var fi = val1.GetType().GetProperties();
            foreach (var f in fi)
            {
                Variance v = new Variance();
                v.FieldName = f.Name;
                v.OldName = f.GetValue(val1);
                v.NewValue = f.GetValue(val2);
                if (!Equals(v.OldName, v.NewValue))
                    variances.Add(v);

            }
            return variances;
        }
    }

    public class Variance
    {
        public string FieldName { get; set; }
        public object OldName { get; set; }
        public object NewValue { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModControlSimu
{
    /// <summary>
    /// 数学関数の拡張メソッドクラス
    /// </summary>
    static public class ClsMathExpansion
    {
        /// <summary>
        /// 配列の順列全パターンをListで取得<br/>
        /// 引数の配列をソート済みのものであること
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        static public List<T[]> AllPermutation<T>(params T[] array) where T : IComparable
        {
            var a = new List<T>(array).ToArray();
            var res = new List<T[]>();
            res.Add(new List<T>(a).ToArray());
            var n = a.Length;
            var next = true;
            while (next)
            {
                next = false;

                int i;
                for (i = n - 2; i >= 0; i--) { if (a[i].CompareTo(a[i + 1]) < 0) break; }

                if (i < 0) break;
                var j = n;
                do { j--; } while (a[i].CompareTo(a[j]) > 0);

                if (a[i].CompareTo(a[j]) < 0)
                {
                    (a[j], a[i]) = (a[i], a[j]);
                    Array.Reverse(a, i + 1, n - i - 1);
                    res.Add(new List<T>(a).ToArray());
                    next = true;
                }
            }
            return res;
        }
    }
}

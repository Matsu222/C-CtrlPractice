using System.Diagnostics;
using System.IO.Compression;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace ModControlSimu
{
    /// <summary>
    /// 伝達関数や状態方程式の離散シミュレーションを行うクラス
    /// </summary>
    public class ClsTfSs
    {
        /// <summary>伝達関数の分母多項式係数</summary>
        private double[] TfDen;
        /// <summary>伝達関数の分子多項式係数</summary>
        private double[] TfNum;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ClsTfSs()
        {
            TfDen = new double[1];
            TfNum = new double[1];
        }

        /// <summary>
        /// システムの伝達関数の分母・分子多項式係数をセット
        /// </summary>
        /// <param name="Den">分母多項式の係数</param>
        /// <param name="Num">分子多項式の係数</param>
        public void SetTf(double[] Den, double[] Num)
        {
            TfDen = Den;
            TfNum = Num;
        }

        /// <summary>
        /// セット済みのシステムの伝達関数の分母・分子多項式係数を取得
        /// </summary>
        /// <param name="Den">分母多項式の係数</param>
        /// <param name="Num">分子多項式の係数</param>
        public void GetTf(out double[] Den, out double[] Num)
        {
            Den = TfDen;
            Num = TfNum;
        }

        /// <summary>
        /// セット済みのシステムの伝達関数の周波数応答を計算
        /// </summary>
        /// <param name="Freq">計算範囲の周波数[Hz]</param>
        /// <param name="Gain">ゲイン[dB]</param>
        /// <param name="Phase">位相[Phase]</param>
        public void FreqResp(double[] Freq, out double[] Gain, out double[] Phase)
        {
            Gain = new double[Freq.Length];
            Phase = new double[Freq.Length];
            for (int Point = 0; Point < Freq.Length; Point++)
            {
                var Omega = 2 * Math.PI * Freq[Point];
                var NumValue = new Complex(0, 0);
                for (int Order = 0; Order < TfNum.Length; Order++)
                {
                    if (Order % 2 == 0) NumValue += new Complex(TfNum[Order] * Math.Pow(-1, Order / 2) * Math.Pow(Omega, Order), 0);
                    else NumValue += new Complex(0, TfNum[Order] * Math.Pow(-1, (Order - 1) / 2) * Math.Pow(Omega, Order));
                }
                var DenValue = new Complex(0, 0);
                for (int Order = 0; Order < TfDen.Length; Order++)
                {
                    if (Order % 2 == 0) DenValue += new Complex(TfDen[Order] * Math.Pow(-1, Order / 2) * Math.Pow(Omega, Order), 0);
                    else DenValue += new Complex(0, TfDen[Order] * Math.Pow(-1, (Order - 1) / 2) * Math.Pow(Omega, Order));
                }
                Gain[Point] = 20 * Math.Log10((NumValue / DenValue).Magnitude);
                Phase[Point] = (NumValue / DenValue).Phase;
            }
        }
    }
}

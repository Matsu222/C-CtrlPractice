using System.Data;
using System.Diagnostics;
using System.IO.Compression;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace ModControlSimu
{
    /// <summary>
    /// 伝達関数や状態方程式の離散シミュレーションを行うクラス
    /// </summary>
    public class ClsTfSs
    {
        /// <summary>伝達関数の分母多項式係数</summary>
        private Polynomial[] _TfDen;
        /// <summary>伝達関数の分子多項式係数</summary>
        private Polynomial[] _TfNum;
        /// <summary>状態方程式のA行列</summary>
        private Matrix _A;
        /// <summary>状態方程式のB行列</summary>
        private Matrix _B;
        /// <summary>状態方程式のC行列</summary>
        private Matrix _C;
        /// <summary>状態方程式のD行列</summary>
        private Matrix _D;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ClsTfSs()
        {
            _TfDen = new Polynomial[1];
            _TfNum = new Polynomial[1];
            _A = new Matrix();
            _B = new Matrix();
            _C = new Matrix();
            _D = new Matrix();
        }

        /// <summary>
        /// システムを伝達関数の分母・分子多項式係数でセット<br/>
        /// 内部的には分母多項式の最高次数の係数が1となるように自動的に変換されます
        /// </summary>
        /// <param name="Den">分母多項式の係数</param>
        /// <param name="Num">分子多項式の係数</param>
        public void SetSys(Polynomial Den, Polynomial Num)
        {
            _TfDen = new Polynomial[1];
            _TfDen[0] = Den;
            _TfNum = new Polynomial[1];
            _TfNum[0] = Num;
            _A = new Matrix(Den.GetOrder() + 1, Den.GetOrder() + 1);
            _B = new Matrix(Den.GetOrder() + 1, 1);
            _C = new Matrix(1, Den.GetOrder() + 1);
            _D = new Matrix();

            Parallel.For(0, Den.GetOrder() + 1 - 1, i =>
            {
                _A[i, i + 1] = 1;
            });
            Parallel.For(0, Den.GetOrder() + 1, i =>
            {
                _A[Den.GetOrder() + 1 - 1, i] = -_TfDen[0][i];
            });
            _B[Den.GetOrder() + 1 - 1, 0] = 1;
            Parallel.For(0, Den.GetOrder() + 1 - 1, i =>
            {
                if (i < _TfNum.Length) _C[0, i] = _TfNum[0][i];
                else _C[0, i] = 0;
            });

        }

        /// <summary>
        /// システムを状態方程式の係数行列でセット
        /// </summary>
        /// <param name="A">A行列</param>
        /// <param name="B">B行列</param>
        /// <param name="C">C行列</param>
        /// <param name="D">D行列</param>
        public void SetSys(Matrix A, Matrix B, Matrix C, Matrix D)
        {
            _A = A;
            _B = B;
            _C = C;
            _D = D;
        }

        /// <summary>
        /// セット済みのシステムの伝達関数の分母・分子多項式係数を取得
        /// </summary>
        /// <param name="Den">分母多項式の係数</param>
        /// <param name="Num">分子多項式の係数</param>
        /// <param name="InputNo">入力ベクトルのインデックス</param>
        public void GetSys(out Polynomial Den, out Polynomial Num, int InputNo)
        {
            if (_TfDen.GetLength(0) < InputNo)
            {
                Den = new Polynomial();
                Num = new Polynomial();
            }
            else
            {
                Den = _TfDen[InputNo];
                Num = _TfNum[InputNo];
            }
        }

        /// <summary>
        /// セット済みのシステムの係数行列を取得
        /// </summary>
        /// <param name="A">A行列</param>
        /// <param name="B">B行列</param>
        /// <param name="C">C行列</param>
        /// <param name="D">D行列</param>
        public void GetSys(out Matrix A, out Matrix B, out Matrix C, out Matrix D)
        {
            A = _A;
            B = _B;
            C = _C;
            D = _D;
        }

        /// <summary>
        /// セット済みのシステムの伝達関数の周波数応答を計算
        /// </summary>
        /// <param name="Freq">計算範囲の周波数[Hz]</param>
        /// <param name="Gain">ゲイン[dB]</param>
        /// <param name="Phase">位相[Phase]</param>
        /// <param name="InputNo">入力ベクトルのインデックス</param>
        public void FreqResp(double[] Freq, out double[] Gain, out double[] Phase, int InputNo)
        {
            Gain = new double[Freq.Length];
            Phase = new double[Freq.Length];

            if (_TfDen.Length < InputNo) return;

            for (int Point = 0; Point < Freq.Length; Point++)
            {
                var Omega = 2 * Math.PI * Freq[Point];
                var NumValue = new Complex(0, 0);
                for (int Order = 0; Order < _TfNum[InputNo].GetOrder() + 1; Order++)
                {
                    if (Order % 2 == 0) NumValue += new Complex(_TfNum[InputNo][Order] * Math.Pow(-1, Order / 2) * Math.Pow(Omega, Order), 0);
                    else NumValue += new Complex(0, _TfNum[InputNo][Order] * Math.Pow(-1, (Order - 1) / 2) * Math.Pow(Omega, Order));
                }
                var DenValue = new Complex(0, 0);
                for (int Order = 0; Order < _TfDen[InputNo].GetOrder() + 1; Order++)
                {
                    if (Order % 2 == 0) DenValue += new Complex(_TfDen[InputNo][Order] * Math.Pow(-1, Order / 2) * Math.Pow(Omega, Order), 0);
                    else DenValue += new Complex(0, _TfDen[InputNo][Order] * Math.Pow(-1, (Order - 1) / 2) * Math.Pow(Omega, Order));
                }
                Gain[Point] = 20 * Math.Log10((NumValue / DenValue).Magnitude);
                Phase[Point] = (NumValue / DenValue).Phase;
            }
        }
    }
}

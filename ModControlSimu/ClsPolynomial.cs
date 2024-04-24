using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModControlSimu
{
    /// <summary>
    /// 多項式その基本演算の定義クラス
    /// </summary>
    public class Polynomial
    {
        /// <summary>格納する多項式の係数配列</summary>
        double[]? Data;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Polynomial()
        {
            Data = null;
        }

        /// <summary>
        /// コンストラクタ<br/>
        /// 任意の次数で初期化
        /// </summary>
        /// <param name="Order">次数</param>
        public Polynomial(int Order)
        {
            Data = new double[Order];
        }

        /// <summary>
        /// コンストラクタ<br/>
        /// 任意の係数配列で初期化
        /// </summary>
        /// <param name="InitData">初期化する係数配列</param>
        public Polynomial(double[] InitData)
        {
            Data = new double[InitData.Length];
            Parallel.For(0, InitData.Length, Order =>
            {
                Data[Order] = InitData[Order];
            });
        }

        /// <summary>
        /// 多項式用の+演算子
        /// </summary>
        /// <param name="X">多項式 X</param>
        /// <returns>計算結果</returns>
        public static Polynomial operator +(Polynomial X)
        {
            return X;
        }

        /// <summary>
        /// 多項式用の+演算子
        /// </summary>
        /// <param name="X">多項式 X</param>
        /// <param name="Y">多項式 Y</param>
        /// <returns>計算結果</returns>
        public static Polynomial operator +(Polynomial X, Polynomial Y)
        {
            int Length;
            if (X.GetOrder() > Y.GetOrder()) Length = X.GetOrder() + 1;
            else Length = Y.GetOrder() + 1;
            double[] Coef = new double[Length];
            Parallel.For(0, Length, Order =>
            {
                Coef[Order] = X[Order] + Y[Order];
            });
            return new Polynomial(Coef);
        }

        /// <summary>
        /// 多項式用の-演算子
        /// </summary>
        /// <param name="X">多項式 X</param>
        /// <returns>計算結果</returns>
        public static Polynomial operator -(Polynomial X)
        {
            var Ans = new double[X.GetOrder() + 1];
            Parallel.For(0, Ans.Length, Order =>
            {
                Ans[Order] = -X[Order];
            });
            return new Polynomial(Ans);
        }

        /// <summary>
        /// 多項式用の-演算子
        /// </summary>
        /// <param name="X">多項式 X</param>
        /// <param name="Y">多項式 Y</param>
        /// <returns>計算結果</returns>
        public static Polynomial operator -(Polynomial X, Polynomial Y)
        {
            return X + (-Y);
        }

        /// <summary>
        /// 多項式用の*演算子
        /// </summary>
        /// <param name="X">多項式 X</param>
        /// <param name="Y">多項式 Y</param>
        /// <returns>計算結果</returns>
        public static Polynomial operator *(Polynomial X, Polynomial Y)
        {
            var Coef = new double[X.GetOrder() + Y.GetOrder() + 1];
            Parallel.For(0, X.GetOrder() + 1, OrderX =>
            {
                Parallel.For(0, Y.GetOrder() + 1, OrderY =>
                {
                    Coef[OrderX + OrderY] += X[OrderX] * Y[OrderY];
                });
            });
            return new Polynomial(Coef);
        }

        /// <summary>
        /// インデクサーによる係数の取得
        /// </summary>
        /// <param name="Order">取得する次数</param>
        /// <returns>係数</returns>
        public double this[int Order]
        {
            set
            {
                if (Data == null)
                {
                    Data = new double[Order + 1];
                    Data[Order] = value;
                }
                else
                {
                    if (Data.Length <= Order)
                    {
                        double[] StoreData = new double[Order + 1];
                        Parallel.For(0, Data.Length, i =>
                        {
                            StoreData[i] = Data[i];
                        });
                        Data = new double[Order + 1];
                        Parallel.For(0, Data.Length, i =>
                        {
                            Data[i] = StoreData[i];
                        });
                    }
                    Data[Order] = value;
                }
            }
            get
            {
                if (Data == null) return 0;
                if (Data.Length > Order) return Data[Order];
                return 0;
            }
        }

        /// <summary>
        /// 多項式の次数を取得<br/>
        /// 未定義の場合は-1を返します
        /// </summary>
        /// <returns>次数</returns>
        public int GetOrder()
        {
            if (Data == null) return -1;
            return Data.Length - 1;
        }

        /// <summary>
        /// 多項式の係数を文字列として取得する
        /// </summary>
        /// <param name="IsOnlyCoef">trueで係数配列のみ。trueで多項式として表示</param>
        /// <param name="Format">係数の表示フォーマット</param>
        /// <param name="Literal">多項式の文字</param>
        /// <returns>多項式文字列</returns>
        public string ToString(bool IsOnlyCoef = false, string Format = "", char Literal = 's')
        {
            if (Data == null) return "";
            if (IsOnlyCoef)
            {
                var Sb = new StringBuilder();
                Sb.Append('(');
                for (int Order = this.GetOrder(); Order >= 0; Order--)
                {
                    Sb.Append(Data[Order].ToString(Format));
                    Sb.Append('\t');
                }
                Sb.Append(')');
                return Sb.ToString();
            }
            else
            {
                var Sb = new StringBuilder();
                for (int Order = this.GetOrder(); Order >= 0; Order--)
                {
                    if (Data[Order] == 0) continue;
                    if (Order != this.GetOrder() & Data[Order] > 0) Sb.Append('+');

                    if (Order > 1)
                    {
                        if (Data[Order] == -1) Sb.Append('-');
                        else if (Data[Order] != 1) Sb.Append(Data[Order].ToString(Format));
                        Sb.Append(Literal);
                        Sb.Append('^').Append(Order);
                    }
                    else if (Order == 1)
                    {
                        if (Data[Order] == -1) Sb.Append('-');
                        else if (Data[Order] != 1) Sb.Append(Data[Order].ToString(Format));
                        Sb.Append(Literal);
                    }
                    else
                    {
                        Sb.Append(Data[Order].ToString(Format));
                    }
                    Sb.Append(' ');
                }
                return Sb.ToString();
            }
        }
    }
}

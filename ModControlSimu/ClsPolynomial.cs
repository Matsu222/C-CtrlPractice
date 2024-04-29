using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
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
        private double[] _Data;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Polynomial()
        {
            _Data = null;
        }

        /// <summary>
        /// コンストラクタ<br/>
        /// 任意の次数で初期化
        /// </summary>
        /// <param name="Order">次数</param>
        public Polynomial(int Order)
        {
            _Data = new double[Order];
        }

        /// <summary>
        /// コンストラクタ<br/>
        /// 任意の係数配列で初期化
        /// </summary>
        /// <param name="InitData">初期化する係数配列</param>
        public Polynomial(double[] InitData)
        {
            _Data = new double[InitData.Length];
            Parallel.For(0, InitData.Length, Order =>
            {
                _Data[Order] = InitData[Order];
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
            if (X.GetOrder() != -1 & Y.GetOrder() != -1)
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
            else
            {
                return new Polynomial();
            }
        }

        /// <summary>
        /// double型からPolinomial型への変換演算子
        /// </summary>
        /// <param name="A"></param>
        public static implicit operator Polynomial(double A)
        {
            var Coef = new double[1];
            Coef[0] = A;
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
                if (_Data == null)
                {
                    _Data = new double[Order + 1];
                    _Data[Order] = value;
                }
                else
                {
                    if (_Data.Length <= Order)
                    {
                        double[] StoreData = new double[Order + 1];
                        Parallel.For(0, _Data.Length, i =>
                        {
                            StoreData[i] = _Data[i];
                        });
                        _Data = new double[Order + 1];
                        Parallel.For(0, _Data.Length, i =>
                        {
                            _Data[i] = StoreData[i];
                        });
                    }
                    _Data[Order] = value;
                }
            }
            get
            {
                if (_Data == null) return 0;
                if (_Data.Length > Order) return _Data[Order];
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
            if (_Data == null) return -1;
            return _Data.Length - 1;
        }

        /// <summary>
        /// 多項式の任意の点での値を計算
        /// </summary>
        /// <param name="X">任意の点(複素数)</param>
        /// <returns>計算結果</returns>
        public Complex GetValue(Complex X)
        {
            if (_Data == null) return new Complex();
            var Ans = new Complex(0, 0);
            Parallel.For(0, GetOrder() + 1, Order =>
            {
                var OrderValue = new Complex(1, 0);
                Parallel.For(0, Order, i =>
                {
                    OrderValue *= X;
                });
                Ans += _Data[Order] * OrderValue;
            });
            return Ans;
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
            if (_Data == null) return "";
            if (IsOnlyCoef)
            {
                var Sb = new StringBuilder();
                Sb.Append('(');
                for (int Order = this.GetOrder(); Order >= 0; Order--)
                {
                    Sb.Append(_Data[Order].ToString(Format));
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
                    if (_Data[Order] == 0) continue;
                    if (Order != this.GetOrder() & _Data[Order] > 0) Sb.Append('+');

                    if (Order > 1)
                    {
                        if (_Data[Order] == -1) Sb.Append('-');
                        else if (_Data[Order] != 1) Sb.Append(_Data[Order].ToString(Format));
                        Sb.Append(Literal);
                        Sb.Append('^').Append(Order);
                    }
                    else if (Order == 1)
                    {
                        if (_Data[Order] == -1) Sb.Append('-');
                        else if (_Data[Order] != 1) Sb.Append(_Data[Order].ToString(Format));
                        Sb.Append(Literal);
                    }
                    else
                    {
                        Sb.Append(_Data[Order].ToString(Format));
                    }
                    Sb.Append(' ');
                }
                return Sb.ToString();
            }
        }
    }
}

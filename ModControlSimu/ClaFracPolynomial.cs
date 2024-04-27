using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModControlSimu
{
    /// <summary>
    /// 分数多項式とその基本演算の定義クラス
    /// </summary>
    public class FracPolynomial
    {
        /// <summary>分子多項式</summary>
        public Polynomial? Num { get; private set; }
        /// <summary>分母多項式</summary>
        public Polynomial? Den { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FracPolynomial()
        {
            Num = null;
            Den = null;
        }

        /// <summary>
        /// 任意の分子/分母多項式で初期化
        /// </summary>
        /// <param name="num"></param>
        /// <param name="den"></param>
        public FracPolynomial(Polynomial num, Polynomial den)
        {
            Num = num;
            Den = den;
        }

        /// <summary>
        /// 分数多項式用の+演算子
        /// </summary>
        /// <param name="X">分数多項式 X</param>
        /// <returns>計算結果</returns>
        public static FracPolynomial operator +(FracPolynomial X)
        {
            return X;
        }

        /// <summary>
        /// 分数多項式用の+演算子
        /// </summary>
        /// <param name="X">分数多項式 X</param>
        /// <param name="Y">分数多項式 Y</param>
        /// <returns>計算結果</returns>
        public static FracPolynomial operator +(FracPolynomial X, FracPolynomial Y)
        {
            if (X.Num == null) return new FracPolynomial();
            if (X.Den == null) return new FracPolynomial();
            if (Y.Num == null) return new FracPolynomial();
            if (Y.Den == null) return new FracPolynomial();
            return new FracPolynomial(X.Num * Y.Den + X.Den * Y.Num, X.Den * Y.Den);
        }

        /// <summary>
        /// 分数多項式用の-演算子
        /// </summary>
        /// <param name="X">分数多項式 X</param>
        /// <returns>計算結果</returns>
        public static FracPolynomial operator -(FracPolynomial X)
        {
            if (X.Num == null) return new FracPolynomial();
            if (X.Den == null) return new FracPolynomial();
            return new FracPolynomial(-X.Num, X.Den);
        }

        /// <summary>
        /// 分数多項式用の-演算子
        /// </summary>
        /// <param name="X">分数多項式 X</param>
        /// <param name="Y">分数多項式 Y</param>
        /// <returns>計算結果</returns>
        public static FracPolynomial operator -(FracPolynomial X, FracPolynomial Y)
        {
            return X + (-Y);
        }

        /// <summary>
        /// 分数多項式用の*演算子
        /// </summary>
        /// <param name="X">分数多項式 X</param>
        /// <param name="Y">分数多項式 Y</param>
        /// <returns>計算結果</returns>
        public static FracPolynomial operator *(FracPolynomial X, FracPolynomial Y)
        {
            if (X.Num == null) return new FracPolynomial();
            if (X.Den == null) return new FracPolynomial();
            if (Y.Num == null) return new FracPolynomial();
            if (Y.Den == null) return new FracPolynomial();
            return new FracPolynomial(X.Num * Y.Num, X.Den * Y.Den);
        }

    }
}

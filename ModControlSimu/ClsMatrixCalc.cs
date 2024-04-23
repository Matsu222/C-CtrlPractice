using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Metadata;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ModControlSimu
{
    /// <summary>
    /// 行列型とその基本演算の定義クラス
    /// </summary>
    public class ClsMatrix
    {
        /// <summary>格納するデータ</summary>
        protected double[][]? Data;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ClsMatrix()
        {
            Data = null;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Row">初期化時の行数</param>
        /// <param name="Col">初期化時の列数</param>
        public ClsMatrix(int Row, int Col)
        {
            Array.Resize(ref Data, Row);
            Parallel.For(0, Row, Item =>
            {
                Array.Resize(ref Data[Item], Col);
            });
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Data">初期化時の行列</param>
        protected ClsMatrix(double[][] InitData)
        {
            Array.Resize(ref Data, InitData.Length);
            Parallel.For(0, InitData.Length, Row =>
            {
                Array.Resize(ref Data[Row], InitData[Row].Length);
                Parallel.For(0, InitData[0].Length, Col =>
                {
                    Data[Row][Col] = InitData[Row][Col];
                });
            });

        }

        /// <summary>
        /// インデクサーによる行列要素の取得
        /// </summary>
        /// <param name="Row">行</param>
        /// <param name="Col">列</param>
        /// <returns>要素の値</returns>
        public double? this[int Row, int Col]
        {
            set
            {
                if (value == null)
                {
                    Data = null;
                }
                else
                {
                    Data ??= new double[Row][];
                    if (Row >= RowCount())
                    {
                        int AddRow = Row + 1 - RowCount();
                        Array.Resize(ref Data, Row + 1);
                        Parallel.For(Row - AddRow + 1, Row + 1, InitRow =>
                        {
                            Data[InitRow] = new double[ColCount()];
                        });
                    }
                    if (Col >= ColCount())
                    {
                        Parallel.For(0, Data.Length, _Row =>
                        {
                            Array.Resize(ref Data[_Row], Col + 1);
                        });
                    }
                    Data[Row][Col] = (double)value;
                }
            }
            get
            {
                if (Data == null) return null;
                if (Row > RowCount()) return null;
                if (Col > ColCount()) return null;
                return Data[Row][Col];
            }

        }

        /// <summary>
        /// 行列用の+演算子
        /// </summary>
        /// <param name="A">Matrix A</param>
        /// <returns>計算結果</returns>
        public static ClsMatrix operator +(ClsMatrix A)
        {
            return A;
        }

        /// <summary>
        /// 行列用の+演算子
        /// </summary>
        /// <param name="A">Matrix A</param>
        /// <param name="B">Matrix B</param>
        /// <returns>計算結果 (計算不可では空行列)</returns>
        public static ClsMatrix operator +(ClsMatrix A, ClsMatrix B)
        {
            //行列のサイズをチェック
            if (A.RowCount() != B.RowCount()) return new ClsMatrix();
            if (A.ColCount() != B.ColCount()) return new ClsMatrix();

            //並列処理で高速化
            var Ans = new ClsMatrix(A.RowCount(), A.ColCount());
            Parallel.For(0, A.RowCount(), Row =>
            {
                Parallel.For(0, A.ColCount(), Col =>
                {
                    Ans[Row, Col] = A[Row, Col] + B[Row, Col];
                });
            });
            return Ans;
        }

        /// <summary>
        /// 行列用の-演算子
        /// </summary>
        /// <param name="A">Matrix A</param>
        /// <returns>計算結果</returns>
        public static ClsMatrix operator -(ClsMatrix A)
        {
            var Ans = new ClsMatrix();
            Parallel.For(0, A.RowCount(), Row =>
            {
                Parallel.For(0, A.ColCount(), Col =>
                {
                    Ans[Row, Col] = -A[Row, Col];
                });
            });
            return Ans;
        }

        /// <summary>
        /// 行列用の-演算子
        /// </summary>
        /// <param name="A">Matrix A</param>
        /// <param name="B">Matrix B</param>
        /// <returns>計算結果 (計算不可では空行列)</returns>
        public static ClsMatrix operator -(ClsMatrix A, ClsMatrix B)
        {
            return A + (-B);
        }

        /// <summary>
        /// 行列用の*演算子
        /// </summary>
        /// <param name="A">Matrix A</param>
        /// <param name="B">Matrix B</param>
        /// <returns>計算結果 (計算不可では空行列)</returns>
        public static ClsMatrix operator *(ClsMatrix A, ClsMatrix B)
        {
            //行列のサイズをチェック
            if (A.ColCount() != B.RowCount()) return new ClsMatrix();

            //並列処理で高速化
            ClsMatrix Ans = new ClsMatrix(A.RowCount(), B.ColCount());
            Parallel.For(0, A.RowCount(), Row =>
            {
                Parallel.For(0, B.ColCount(), Col =>
                {
                    Ans[Row, Col] = 0;
                    Parallel.For(0, A.ColCount(), i =>
                    {
                        Ans[Row, Col] += A[Row, i] * B[i, Col];
                    });
                });
            });
            return Ans;
        }

        /// <summary>
        /// N次の単位行列を取得
        /// </summary>
        /// <param name="N">次数</param>
        /// <returns>単位行列</returns>
        public ClsMatrix GetIMatrix(int N)
        {
            var I = new ClsMatrix(N, N);
            for (int i = 0; i < N; i++) I[i, i] = 1;
            return I;
        }

        /// <summary>
        /// 行の数を取得
        /// </summary>
        /// <returns>行の数</returns>
        public int RowCount()
        {
            if (Data == null) return 0;
            else return Data.Length;
        }

        /// <summary>
        /// 列の数を取得
        /// </summary>
        /// <returns>列の数</returns>
        public int ColCount()
        {
            if (Data == null) return 0;
            else if (Data[0] == null) return 0;
            else return Data[0].Length;
        }

        /// <summary>
        /// 行列式の値を取得
        /// </summary>
        /// <returns>行列式の値(正方行列以外では-1)</returns>
        public double Det()
        {
            if (RowCount() != ColCount()) return -1;
            if (Data == null) return -1;
            //上三角行列へ変形し対角成分のみで計算する
            double? Ans = 1;
            var UpTriMat = new ClsMatrix(this.Data);
            for (int ZeroCol = 0; ZeroCol < ColCount() - 1; ZeroCol++)
            {
                if (UpTriMat[ZeroCol, ZeroCol] == 0)
                {
                    int SwapRow;
                    for (SwapRow = ZeroCol; SwapRow < RowCount(); SwapRow++)
                    {
                        if (UpTriMat[SwapRow, ZeroCol] != 0) break;
                    }
                    if (SwapRow == RowCount()) return 0;
                    UpTriMat.RowSwap(ZeroCol, SwapRow);
                    Ans *= -1;
                }
                for (int Row = ZeroCol + 1; Row < RowCount(); Row++)
                {
                    double? Coef = UpTriMat[Row, ZeroCol] / UpTriMat[ZeroCol, ZeroCol];
                    Parallel.For(ZeroCol, ColCount(), Col =>
                    {
                        UpTriMat[Row, Col] -= UpTriMat[ZeroCol, Col] * Coef;
                    });
                }
            }
            //行列式の値を計算
            Parallel.For(0, ColCount(), Col => { Ans *= UpTriMat[Col, Col]; });

            return (double)Ans;
        }

        /// <summary>
        /// 転置行列を取得<br/>
        /// 転置行列が存在しない場合は空行列を返します
        /// </summary>
        /// <returns>転置行列</returns>
        public ClsMatrix Transpose()
        {
            if (Data == null) return new ClsMatrix();
            var TransMat = new ClsMatrix(this.ColCount(), this.RowCount());
            Parallel.For(0, this.RowCount(), Row =>
            {
                Parallel.For(0, this.ColCount(), Col =>
                {
                    TransMat[Col, Row] = this[Row, Col];
                });
            });
            return TransMat;
        }

        /// <summary>
        /// 逆行列の取得<br/>
        /// 逆行列が存在しない場合は空行列を返します
        /// </summary>
        /// <returns>逆行列</returns>
        public ClsMatrix Inv()
        {
            if (RowCount() != ColCount()) return new ClsMatrix();
            if (Det() == 0) return new ClsMatrix();

            //掃き出し法の対象とする行列を計算
            var AI = new ClsMatrix(RowCount(), ColCount() * 2);
            Parallel.For(0, RowCount(), Row =>
            {
                Parallel.For(0, ColCount() * 2, Col =>
                {
                    if (Col < ColCount()) AI[Row, Col] = this[Row, Col];
                    else if (Col - ColCount() == Row) AI[Row, Col] = 1;
                    else AI[Row, Col] = 0;
                });
            });
            //掃き出し法の対象から逆行列部分を抽出
            var IB = AI.RowReduction();
            var Ans = new ClsMatrix(RowCount(), ColCount());
            Parallel.For(0, RowCount(), Row =>
            {
                Parallel.For(0, ColCount(), Col =>
                {
                    Ans[Row, Col] = IB[Row, Col + ColCount()];
                });
            });

            return Ans;
        }

        /// <summary>
        /// ムーア・ペンローズの疑似逆行列を取得
        /// </summary>
        /// <returns>疑似逆行列</returns>
        public ClsMatrix Pinv()
        {
            return (this.Transpose() * this).Inv() * this.Transpose();
        }

        /// <summary>
        /// 掃き出し法による変形をした行列を返す<br/>
        /// 計算不可能な場合は空行列を返します
        /// </summary>
        /// <returns>掃き出し法の結果</returns>
        public ClsMatrix RowReduction()
        {
            if (Data == null) return new ClsMatrix();
            if (RowCount() >= ColCount()) return new ClsMatrix();
            if (this.GetSquarePart().Det() == 0) return new ClsMatrix();

            //計算用に別の行列に格納
            var Mat = new ClsMatrix(this.Data);

            //対角成分より下を"0"にする
            for (int ZeroCol = 0; ZeroCol < Mat.RowCount() - 1; ZeroCol++)
            {
                if (Mat[ZeroCol, ZeroCol] == 0)
                {
                    int SwapRow;
                    for (SwapRow = ZeroCol; SwapRow < Mat.RowCount(); SwapRow++)
                    {
                        if (Mat[SwapRow, ZeroCol] != 0) break;
                    }
                    if (SwapRow == Mat.RowCount()) return new ClsMatrix();
                    Mat.RowSwap(ZeroCol, SwapRow);
                }
                for (int Row = ZeroCol + 1; Row < Mat.RowCount(); Row++)
                {
                    double? Coef = Mat[Row, ZeroCol] / Mat[ZeroCol, ZeroCol];
                    Parallel.For(ZeroCol, Mat.ColCount(), Col =>
                    {
                        Mat[Row, Col] -= Mat[ZeroCol, Col] * Coef;
                    });
                }
            }
            //対角成分を"0"にする
            Parallel.For(0, Mat.RowCount(), Row =>
            {
                double? Coef = Mat[Row, Row];
                Parallel.For(0, Mat.ColCount(), Col =>
                {
                    Mat[Row, Col] /= Coef;
                });
            });
            //対角成分より上を削除する
            for (int ZeroCol = Mat.RowCount() - 1; ZeroCol >= 0; ZeroCol--)
            {
                for (int Row = 0; Row < ZeroCol; Row++)
                {
                    double? Coef = Mat[Row, ZeroCol];
                    Parallel.For(ZeroCol, ColCount(), Col =>
                    {
                        Mat[Row, Col] -= Mat[ZeroCol, Col] * Coef;
                    });
                }
            }

            return Mat;
        }

        /// <summary>
        /// 正方行列部分を取得
        /// </summary>
        /// <returns>抽出した正方行列</returns>
        public ClsMatrix GetSquarePart()
        {
            int N;
            if (RowCount() < ColCount()) N = RowCount();
            else N = ColCount();

            var SquareMat = new ClsMatrix(N, N);
            Parallel.For(0, N, Row =>
            {
                Parallel.For(0, N, Col =>
                {
                    SquareMat[Row, Col] = this[Row, Col];
                });
            });

            return SquareMat;
        }

        /// <summary>
        /// 行列で指定した二行を入れ替える
        /// </summary>
        /// <param name="RowA">行1</param>
        /// <param name="RowB">行2</param>
        protected ClsMatrix RowSwap(int RowA, int RowB)
        {
            if (Data == null) return new ClsMatrix();
            double[] TmpArray = new double[ColCount()];
            Array.Copy(Data[RowA], TmpArray, ColCount());
            Array.Copy(Data[RowB], Data[RowA], ColCount());
            Array.Copy(TmpArray, Data[RowB], ColCount());
            return this;
        }

        /// <summary>
        /// 行列をコンマ区切りと改行を使用した形の文字列を取得<br/>
        /// どのサイズでも使用可能です
        /// </summary>
        /// <param name="NewLine">行列の行を改行で区切る場合はtrue</param>
        /// <param name="Format">文字列変換のフォーマット</param>
        /// <returns>表示用文字列</returns>
        public string ToString(bool NewLine = false, string Format = "")
        {
            if (Data == null) return "null";

            var Sb = new StringBuilder();
            Sb.Length = 0;

            for (int Row = 0; Row < RowCount(); Row++)
            {
                if (Data[Row] == null) return string.Empty;
                Sb.Append('(');
                for (int Col = 0; Col < ColCount(); Col++)
                {
                    Sb.Append(Data[Row][Col].ToString(Format));
                    Sb.Append("\t,");
                }
                Sb.Remove(Sb.Length - 1, 1);

                if (NewLine) Sb.Append(")\r\n");
                else Sb.Append("),");
            }

            return Sb.Remove(Sb.Length - 1, 1).ToString();
        }
    }
}

using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text;
using ModControlSimu;
using System.Drawing.Printing;
using System.Configuration;
using System.Numerics;
using System.Xml.Linq;

namespace SimuTester
{
    public partial class FrmMain : Form
    {
        private readonly ClsTfSs ClsSimu = new();
        private readonly Matrix A = new();
        private readonly Matrix B = new();

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {

        }

        private void Matrix_Button_Click(object sender, EventArgs e)
        {
            Ans_Box.Text = string.Empty;
            var Rnd = new Random();
            for (int Row = 0; Row < 5; Row++)
            {
                for (int Col = 0; Col < 5; Col++)
                {
                    A[Row, Col] = (double)Rnd.Next(10)/10;
                    Console.WriteLine(A[Row, Col]);
                }
                B[Row, 0] = Rnd.Next(10);
            }

            var Sb = new StringBuilder();

            Sb.Append(A.ToString(true, "0.######"));
            Sb.Append("\r\n\r\n");
            Sb.Append(A.Inv().ToString(true, "0.######"));
            Sb.Append("\r\n\r\n");
            Sb.Append((A.Inv() * A).ToString(true, "0.######"));

            Ans_Box.Text = Sb.ToString();
        }

        private void FreqResp_Button_Click(object sender, EventArgs e)
        {
            Ans_Box.Text = "";
            double[] Den = new double[3];
            double[] Num = new double[1];
            Den[2] = 1;
            Den[1] = 2;
            Den[0] = 10;
            Num[0] = Den[0];

            ClsSimu.SetSys(new Polynomial(Den), new Polynomial(Num));
            ClsSimu.GetSys(out Matrix A, out Matrix B, out Matrix C, out Matrix D);
            double[] Freq = new double[100];
            for (int Point = 0; Point < Freq.Length; Point++) Freq[Point] = (double)Point / 100 + 0.01;
            ClsSimu.FreqResp(Freq, out double[] Gain, out double[] Phase, 0);
            var Sb = new StringBuilder();
            for (int Point = 0; Point < Gain.Length; Point++)
            {
                Sb.Append(Freq[Point].ToString("0.###"));
                Sb.Append("\t,");
                Sb.Append(Gain[Point].ToString("0.###"));
                Sb.Append("\t,");
                Sb.Append((Phase[Point] * 180 / Math.PI).ToString("0.###"));
                Sb.Append("\r\n");
            }

            Sb.Append(A.ToString(true));
            Sb.Append("\r\n");
            Sb.Append("\r\n");
            Sb.Append(B.ToString(true));
            Sb.Append("\r\n");
            Sb.Append("\r\n");
            Sb.Append(C.ToString(true));
            Sb.Append("\r\n");
            Sb.Append("\r\n");
            Sb.Append(D.ToString(true));
            Sb.Append("\r\n");

            Ans_Box.Text = Sb.ToString();

        }

        private void MathCheck_Button_Click(object sender, EventArgs e)
        {
            int[] Perm = new int[] { 1, 2, 3, 4, 5, 6 };
            var AllPattern = ClsMathExpansion.AllPermutation(Perm);
            var Sb = new StringBuilder();
            for (int i = 0; i < AllPattern.Count; i++)
            {
                Sb.Append(string.Join(",", AllPattern[i]));
                Sb.Append("\r\n");
            }
            Ans_Box.Text = Sb.ToString();
        }

        private void Polynomial_Button_Click(object sender, EventArgs e)
        {
            var Sb = new StringBuilder();
            var Coef = new double[4];
            var Rnd = new Random();
            for (int Order = 0; Order < Coef.Length; Order++)
            {
                Coef[Order] = Rnd.Next(-10, 10);
            }
            var A = new Polynomial(Coef);
            Sb.Append("A=\t");
            Sb.Append(A.ToString());
            Sb.Append("\r\n\r\n");
            Sb.Append("A(0)=\t");
            Sb.Append(A.GetValue(0).ToString());
            Sb.Append("\r\n\r\n");
            Sb.Append("A(1)=\t");
            Sb.Append(A.GetValue(1).ToString());
            Sb.Append("\r\n\r\n");
            Sb.Append("A(i)=\t");
            Sb.Append(A.GetValue(new Complex(0, 1)).ToString());
            Sb.Append("\r\n\r\n");
            Sb.Append("A(1+i)=\t");
            Sb.Append(A.GetValue(new Complex(1, 1)).ToString());
            Sb.Append("\r\n\r\n");

            Ans_Box.Text = Sb.ToString();
        }
    }
}

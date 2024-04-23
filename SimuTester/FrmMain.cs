using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text;
using ModControlSimu;

namespace SimuTester
{
    public partial class FrmMain : Form
    {
        private ModControlSimu.ClsTfSs ClsSimu = new ModControlSimu.ClsTfSs();
        private ModControlSimu.ClsMatrix A = new ModControlSimu.ClsMatrix();
        private ModControlSimu.ClsMatrix B = new ModControlSimu.ClsMatrix();

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
            Random Rnd = new Random();
            for (int Row = 0; Row < 10; Row++)
            {
                for (int Col = 0; Col < 2; Col++)
                {
                    A[Row, Col] = Rnd.Next(10);
                }
                A[Row, A.ColCount() - 1] = 1;
                B[Row, 0] = Rnd.Next(10);
            }

            var Sb = new StringBuilder();

            Sb.Append(A.ToString(true, "0.######"));
            Sb.Append("\r\n\r\n");
            Sb.Append(B.ToString(true, "0.######"));
            Sb.Append("\r\n\r\n");
            Sb.Append((A.Transpose() * A).ToString(true, "0.######"));
            Sb.Append("\r\n\r\n");
            Sb.Append(A.Pinv().ToString(true, "0.######"));
            Sb.Append("\r\n\r\n");
            Sb.Append((A.Pinv() * B).ToString(true, "0.######"));

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

            ClsSimu.SetSys(Den, Num);
            ClsSimu.GetSys(out ClsMatrix A, out ClsMatrix B, out ClsMatrix C, out ClsMatrix D);
            double[] Freq = new double[100];
            for (int Point = 0; Point < Freq.Length; Point++) Freq[Point] = (double)Point / 10 + 0.01;
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
    }
}

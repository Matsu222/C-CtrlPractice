using System;
using System.Threading.Tasks;
using System.Diagnostics;

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


            Ans_Box.Text = string.Empty;
            Ans_Box.Text += A.ToString(true, "0.######");
            Ans_Box.Text += "\r\n\r\n";
            Ans_Box.Text += B.ToString(true, "0.######");
            Ans_Box.Text += "\r\n\r\n";
            Ans_Box.Text += (A.Transpose() * A).ToString(true, "0.######");
            Ans_Box.Text += "\r\n\r\n";
            Ans_Box.Text += A.Pinv().ToString(true, "0.######");
            Ans_Box.Text += "\r\n\r\n";
            Ans_Box.Text += (A.Pinv() * B).ToString(true, "0.######");
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

            ClsSimu.SetTf(Den, Num);
            double[] Freq = new double[100];
            for (int Point = 0; Point < Freq.Length; Point++) Freq[Point] = (double)Point / 1000 + 0.45;
            ClsSimu.FreqResp(Freq, out double[] Gain, out double[] Phase);
            for (int Point = 0; Point < Gain.Length; Point++)
            {
                Ans_Box.Text += Freq[Point].ToString("0.###");
                Ans_Box.Text += "\t,";
                Ans_Box.Text += Gain[Point].ToString("0.###");
                Ans_Box.Text += "\t,";
                Ans_Box.Text += (Phase[Point] * 180 / Math.PI).ToString("0.###");
                Ans_Box.Text += "\r\n";
            }

        }
    }
}

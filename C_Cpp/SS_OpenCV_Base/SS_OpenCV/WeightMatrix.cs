using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SS_OpenCV
{
    public partial class WeightMatrix : Form
    {
        public float[,] mat3x3 = new float[3, 3];
        public float matrixWeight;

        public WeightMatrix()
        {
            InitializeComponent();
        }

        /// PRIMEIRA LINHA

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }


        ///SEGUNDA LINHA

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }


        ///TERCEIRA LINHA

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        ///BOTOES

        private void OK_Click(object sender, EventArgs e)
        {

            mat3x3[0, 0] = Convert.ToSingle(textBox1.Text);
            mat3x3[0, 1] = Convert.ToSingle(textBox2.Text);
            mat3x3[0, 2] = Convert.ToSingle(textBox3.Text);

            mat3x3[1, 0] = Convert.ToSingle(textBox6.Text);
            mat3x3[1, 1] = Convert.ToSingle(textBox5.Text);
            mat3x3[1, 2] = Convert.ToSingle(textBox4.Text);

            mat3x3[2, 0] = Convert.ToSingle(textBox9.Text);
            mat3x3[2, 1] = Convert.ToSingle(textBox8.Text);
            mat3x3[2, 2] = Convert.ToSingle(textBox7.Text);

            matrixWeight = Convert.ToSingle(textBox10.Text);

        }

        private void Cancel_Click(object sender, EventArgs e)
        {

        }

        ///WEIGHT

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        ///OFFSET

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        ///COMBO BOX

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            //Mean 3x3

            if (comboBox1.SelectedIndex == 0)
            {
                textBox1.Text = "1";
                textBox2.Text = "1";
                textBox3.Text = "1";
                textBox4.Text = "1";
                textBox5.Text = "1";
                textBox6.Text = "1";
                textBox7.Text = "1";
                textBox8.Text = "1";
                textBox9.Text = "1";

                textBox10.Text = "9";
                textBox11.Text = "0";
            }

            //Realce de Contornos

            if (comboBox1.SelectedIndex == 1)
            {
                textBox1.Text = "-1";
                textBox2.Text = "-1";
                textBox3.Text = "-1";
                textBox4.Text = "-1";
                textBox5.Text = "9";
                textBox6.Text = "-1";
                textBox7.Text = "-1";
                textBox8.Text = "-1";
                textBox9.Text = "-1";

                textBox10.Text = "1";
                textBox11.Text = "0";
            }


            //Gaussiano

            if (comboBox1.SelectedIndex == 2)
            {
                textBox1.Text = "1";
                textBox2.Text = "2";
                textBox3.Text = "1";
                textBox4.Text = "2";
                textBox5.Text = "4";
                textBox6.Text = "2";
                textBox7.Text = "1";
                textBox8.Text = "2";
                textBox9.Text = "1";


                textBox10.Text = "16";
                textBox11.Text = "0";

            }

            //Laplacian Hard

            if (comboBox1.SelectedIndex == 3)
            {
                textBox1.Text = "1";
                textBox2.Text = "-2";
                textBox3.Text = "1";
                textBox4.Text = "-2";
                textBox5.Text = "4";
                textBox6.Text = "-2";
                textBox7.Text = "1";
                textBox8.Text = "-2";
                textBox9.Text = "1";


                textBox10.Text = "1";
                textBox11.Text = "0";
            }

            //Linhas Verticais

            if (comboBox1.SelectedIndex == 4)
            {
                textBox1.Text = "0";
                textBox2.Text = "0";
                textBox3.Text = "0";
                textBox4.Text = "-1";
                textBox5.Text = "2";
                textBox6.Text = "-1";
                textBox7.Text = "0";
                textBox8.Text = "0";
                textBox9.Text = "0";


                textBox10.Text = "1";
                textBox11.Text = "0";
            }



        }
    }
}
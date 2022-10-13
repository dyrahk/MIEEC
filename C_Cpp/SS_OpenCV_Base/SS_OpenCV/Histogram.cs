using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SS_OpenCV
{
    public partial class Histogram : Form
    {
        public Histogram(int [] array)
        {
            InitializeComponent();

            DataPointCollection list1 = chart1.Series[0].Points;

            for(int i = 0; i < array.Length; i++)
            {
                list1.AddXY(i, array[i]);
            }

            chart1.Series[0].Color = Color.Gray;
            chart1.ChartAreas[0].AxisX.Maximum = 255;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Title = "Intensidade";
            chart1.ChartAreas[0].AxisY.Title = "Número Pixeis";
            chart1.ResumeLayout();
        }

        public Histogram(int[,] array)
        {
            InitializeComponent();

            Series green = new Series("green"); 
            chart1.Series.Add(green);
            Series red = new Series("red");
            chart1.Series.Add(red);

            DataPointCollection list1 = chart1.Series[0].Points;
            DataPointCollection list2 = chart1.Series["green"].Points;
            DataPointCollection list3 = chart1.Series["red"].Points;

            if (array.GetUpperBound(0) == 3)
            {
                Series gray = new Series("gray");
                chart1.Series.Add(gray);
                DataPointCollection list4 = chart1.Series["gray"].Points;
                for (int i = 0; i < 256; i++)
                {
                    list1.AddXY(i, array[1, i]);
                    list2.AddXY(i, array[2, i]);
                    list3.AddXY(i, array[3, i]);
                    list4.AddXY(i, array[0, i]);
                }
                chart1.Series["gray"].Color = Color.Gray;
            }
            else
            {
                for (int i = 0; i < 256; i++)
                {
                    list1.AddXY(i, array[0, i]);
                    list2.AddXY(i, array[1, i]);
                    list3.AddXY(i, array[2, i]);
                }
            }

            chart1.Series[0].Color = Color.Blue;
            chart1.Series["green"].Color = Color.Green;
            chart1.Series["red"].Color = Color.Red;
            
            chart1.ChartAreas[0].AxisX.Maximum = 255;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Title = "Intensidade";
            chart1.ChartAreas[0].AxisY.Title = "Número Pixeis";
            chart1.ResumeLayout();
        }
    }
}

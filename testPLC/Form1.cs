using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Threading;

namespace testPLC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "03excel|*.xls|07excel|*.xlsx";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                String fName = openFileDialog.FileName;
                Dictionary<string, List<PackingType>> dicPacking = ExcelHelper.ToDataTable(fName);

                StartWork(dicPacking);
            }
        }

        /// <summary>
        /// 开始工作
        /// </summary>
        /// <param name="dicPacking">包装信息</param>
        private void StartWork(Dictionary<string, List<PackingType>> dicPacking)
        {
            Plc plc = new Plc(new Linker(), txtInfo);

            foreach (var packerNo in dicPacking.Keys)
            {
                Thread t = new Thread(new ParameterizedThreadStart(plc.execute));
                t.Start(dicPacking[packerNo]);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //string bar = "1S243-024-030-36-1-068-A804002015A003143";
            string bar = "1234567890123";
            string name = "图号:1S243-024-030-36-1-068-A\r\n合格证号：804002015A003143";
            Image printImage = getPrintImage(bar, name, 800, 600);
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += (sender1, e1) => {
                using (Graphics g = e1.Graphics)
                {
                    g.DrawImage(printImage, 0, 0);
                }
            };
            pd.Print();
        }

        private Image getPrintImage(string barcode, string name, int w, int h)
        {
            Image im = new Bitmap(w, h);
            int fontSize = 15;
            Font fon = new System.Drawing.Font("宋体", fontSize);
            Code128 code = new Code128();
            code.Height = 80;
            code.Magnify = 2;
            code.ValueFont = fon;
            Image iim = code.GetCodeImage(barcode, Code128.Encode.Code128B);

            Graphics g = Graphics.FromImage(im);
            SolidBrush wbrush = new SolidBrush(Color.White);
            g.FillRectangle(wbrush, 0, 0, w, h);
            Pen pen = new Pen(Color.Black);
            SolidBrush brush = new SolidBrush(Color.Black);
            g.DrawRectangle(pen, 5, 5, w - 10, h - 10);

            int length = name.Length/2;
            int stringW = (length + 2) * fontSize;
            int xs = (w - 10 - stringW) / 2;
            int xy = (h - 10 - fontSize * 2 - iim.Height) / 2;
            g.DrawString(name, fon, brush, xs, xy);
            int xi = (w - 10 - iim.Width) / 2;
            g.DrawImage(iim, xi, xy + fontSize * 2 * 2);

            return im;
        }
    }
}

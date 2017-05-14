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
            string bar = "1S243-024-030-36-1-068-A";
            string name = "图号：1S243-024-030-36-1-068-A\r\n合格证号：804002015A003143\r\n名称：垫圈1\r\n数量：5\r\n";
            Barcode.Print(bar, name);
        }
    }
}

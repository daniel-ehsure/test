using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace testPLC
{
    public class Plc
    {
        Linker linker;
        TextBox tb;
        object o = new object();

        public Plc(Linker linker, TextBox tb)//可以改成委托
        {
            this.linker = linker;
            this.tb = tb;
        }

        public void execute(object list)
        {
            List<PackingType> listPackingType = (List<PackingType>)list;

            foreach (var pt in listPackingType)
            {
                Console.WriteLine(pt.PackerNo);
                lock (o)
                {
                    string result = linker.send(pt.ToString());

                    if (result.Equals("error"))
                    {
                        tb.Invoke(new MethodInvoker(() => tb.AppendText("error")));
                        break;
                    }
                    else
                    {
                        tb.Invoke(new MethodInvoker(() => tb.AppendText(string.Format("包装机：{0} 振盘：{1} 产品：{2}\r\n", pt.PackerNo, pt.PlateNo, pt.Name))));
                    }
                }
            }
        }
    }
}

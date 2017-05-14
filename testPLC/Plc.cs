using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace testPLC
{
    /// <summary>
    /// plc逻辑和命令处理类
    /// </summary>
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

        /// <summary>
        /// 开始执行相应的指令
        /// </summary>
        /// <param name="list">List<PackingType>指令集合</param>
        public void execute(object list)
        {
            List<PackingType> listPackingType = (List<PackingType>)list;

            foreach (var pt in listPackingType)
            {
                foreach (var number in pt.Item)
                {
                    lock (o)//这里防止其它线程一起访问Linker，造成死锁
                    {
                        try
                        {
                            //发送命令
                            string result = linker.send(pt.ToString());
                            if (result.Equals("error"))
                            {
                                tb.Invoke(new MethodInvoker(() => tb.AppendText("error")));
                                break;
                            }
                            else
                            {
                                tb.Invoke(new MethodInvoker(() => tb.AppendText(string.Format("包装机：{0} 振盘：{1} 产品：{2} 数量：{3}\r\n", pt.PackerNo, pt.PlateNo, pt.Name, number))));
                                tb.Invoke(new MethodInvoker(() => tb.AppendText("打印~~\r\n")));
                                //Barcode.Print(pt.ImageNo, string.Format("图号：{0}\r\n合格证号：{1}\r\n名称：{2}\r\n数量：{3}\r\n", pt.ImageNo, pt.CertificateNo, pt.Name, number));
                            }
                        }
                        catch (Exception ex)
                        {
                            tb.Invoke(new MethodInvoker(() => tb.AppendText(ex.Message)));
                            return;
                        }
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Printing;

namespace testPLC
{
    public class Barcode
    {
        static PrintDocument pd = new PrintDocument();

        public static void Print(string bar, string name)
        {
            Image printImage = getPrintImage(bar, name, 800, 600);
            
            pd.PrintPage += (sender1, e1) =>
            {
                using (Graphics g = e1.Graphics)
                {
                    g.DrawImage(printImage, 0, 0);
                }
            };
            pd.Print();
        }

        /// <summary>
        /// 获得要打印的图片
        /// 具体位置需要调整
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="name"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        private static Image getPrintImage(string barcode, string name, int w, int h)
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

            int length = name.Length / 2;
            int stringW = (length + 2) * fontSize;
            int xs = (w - 10 - stringW) / 2;
            int xy = (h - 10 - fontSize * 2 - iim.Height) / 2;
            g.DrawString(name, fon, brush, xs, 10);
            int xi = (w - 10 - iim.Width) / 2;
            g.DrawImage(iim, xi, xy + fontSize * 2 * 2);

            return im;
        }
    }
}

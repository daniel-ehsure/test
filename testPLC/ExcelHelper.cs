using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace testPLC
{
    public class ExcelHelper
    {
        /// <summary>  
        /// 读取Excel文件  
        /// </summary>  
        /// <param name="filePath">文件路径</param>  
        /// <returns></returns>  
        public static Dictionary<string, List<PackingType>> ToDataTable(string filePath)
        {
            string connStr = "";
            string fileType = System.IO.Path.GetExtension(filePath);
            if (string.IsNullOrEmpty(fileType)) return null;

            if (fileType == ".xls")
                connStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
            else
                connStr = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";
            string sql_F = "Select * FROM [{0}]";

            OleDbConnection conn = null;
            OleDbDataAdapter da = null;
            DataTable dtSheetName = null;
            Dictionary<string, List<PackingType>> result = new Dictionary<string, List<PackingType>>();

            try
            {
                // 初始化连接，并打开  
                conn = new OleDbConnection(connStr);
                conn.Open();

                // 获取数据源的表定义元数据                         
                string mySheetName = "物料数据表导入$";
                dtSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                // 初始化适配器  
                da = new OleDbDataAdapter();
                for (int i = 0; i < dtSheetName.Rows.Count; i++)
                {
                    String sheetName = (string)dtSheetName.Rows[i]["TABLE_NAME"];

                    if (sheetName.Contains("$") && !sheetName.Replace("'", "").EndsWith("$"))
                    {
                        continue;
                    }
                    else if (sheetName.Equals(mySheetName))
                    {
                        da.SelectCommand = new OleDbCommand(String.Format(sql_F, sheetName), conn);
                        DataSet dsItem = new DataSet();
                        da.Fill(dsItem, "物料数据表导入");

                        DataTable dt = dsItem.Tables[0].Copy();

                        for (int k = 1; k < dt.Columns.Count; k++)
                        {
                            PackingType pt = new PackingType();

                            string columnName = dt.Columns[k].ColumnName;

                            if (columnName.Length == 21)//排除多余列
                            {
                                pt.PackerNo = columnName[11].ToString();//取包装机号
                                pt.PlateNo = columnName[16].ToString();//取振盘号

                                pt.Item = new List<int>();

                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    if (j == 0)
                                    {
                                        string[] arr = dt.Rows[j][k].ToString().Split('+');
                                        pt.ImageNo = arr[0];
                                        pt.CertificateNo = arr[1];
                                        pt.Name = arr[2];
                                    }
                                    else
                                    {
                                        int itemValue;
                                        if (int.TryParse(dt.Rows[j][k].ToString(), out itemValue))//排除多余行
                                        {
                                            pt.Item.Add(itemValue);
                                        }
                                    }
                                }

                                if (result.ContainsKey(pt.PackerNo))
                                {
                                    result[pt.PackerNo].Add(pt);
                                }
                                else
                                {
                                    result.Add(pt.PackerNo, new List<PackingType> { pt });
                                }
                            }
                        }

                        #region 注释掉的持久化方法
                        //XmlSerializer fommatter = new XmlSerializer(typeof(List<PackingType>));
                        //using (Stream fs = new FileStream("myData.xml", FileMode.Create, FileAccess.Write, FileShare.None))
                        //{
                        //    fommatter.Serialize(fs, listPT);//序列化对象
                        //}

                        //XmlReader xr = XmlReader.Create("myData.xml");
                        //List<PackingType> list = (List<PackingType>)fommatter.Deserialize(xr);//反序列化对象 
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                // 关闭连接  
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    da.Dispose();
                    conn.Dispose();
                }
            }

            return result;
        }
    }
}

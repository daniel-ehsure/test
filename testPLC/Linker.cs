using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testPLC
{
    /// <summary>
    /// 负责与设备通讯的类
    /// </summary>
    public class Linker
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public Linker()
        {

        }

        /// <summary>
        /// 发送命令，并同步返回结果
        /// 这里处理发送失败重试的次数，以及每个命令等待的时长
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public string send(string command)
        {
            string result = "";

            return result;
        }
    }
}

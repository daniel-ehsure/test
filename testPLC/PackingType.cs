using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testPLC
{   
    /// <summary>
    /// 包装类型
    /// 把excel中的内容映射到该类
    /// </summary>
    [Serializable]
    public class PackingType
    {
        /// <summary>
        /// 包装机号
        /// </summary>
        public string PackerNo { get; set; }

        /// <summary>
        /// 振盘号
        /// </summary>
        public string PlateNo { get; set; }

        /// <summary>
        /// 图号
        /// </summary>
        public string ImageNo { get; set; }

        /// <summary>
        /// 合格证号
        /// </summary>
        public string CertificateNo { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public List<int> Item { get; set; }
    }
}

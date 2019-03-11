using System;
using System.Collections.Generic;
using System.Text;

namespace Model.CommandData
{
    public class SimpleFileInfo
    {
        /// <summary>
        /// 文件编号
        /// </summary>
        public int FileId { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string FileExt { get; set; }
    }
}

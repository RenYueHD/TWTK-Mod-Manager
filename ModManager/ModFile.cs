using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModManager
{
    /// <summary>
    /// Mod文件信息
    /// </summary>
    class ModFile
    {

        /// <summary>
        /// Mod名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 文件完整名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 是否冲突
        /// </summary>
        public bool ConflictTable { get; set; }

        /// <summary>
        /// 词条冲突
        /// </summary>
        public bool ConflictField { get; set; }

        /// <summary>
        /// 创意工坊信息
        /// </summary>
        public WorkshopInfo WorkshopInfo { get; set; }
    }
}

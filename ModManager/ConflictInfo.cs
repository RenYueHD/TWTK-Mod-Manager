using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModManager
{
    /// <summary>
    /// 冲突信息
    /// </summary>
    class ConflictInfo
    {
        /// <summary>
        /// 所在文件完整名
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// 所在表
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// 冲突词条字段名
        /// </summary>
        public string[] FieldName { get; set; }

        /// <summary>
        /// 冲突词条值
        /// </summary>
        public string[] FieldValue { get; set; }

        /// <summary>
        /// 冲突文件完整名
        /// </summary>
        public string ConflictFile { get; set; }

        /// <summary>
        /// 冲突类型
        /// </summary>
        public ConflictType ConflictType { get; set; }
    }
}

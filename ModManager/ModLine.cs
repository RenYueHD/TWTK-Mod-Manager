using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModManager
{
    /// <summary>
    /// Mod词条行信息
    /// </summary>
    class ModLine
    {
        /// <summary>
        /// 所在物理文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 所在表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 所在词条Key名称
        /// </summary>
        public string[] FieldKeyName { get; set; }

        /// <summary>
        /// 所在词条Key值
        /// </summary>
        public string[] FieldKeyValue { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModManager
{
    /// <summary>
    /// 冲突类型
    /// </summary>
    enum ConflictType
    {
        /// <summary>
        /// 不冲突
        /// </summary>
        NONE = 0,
        /// <summary>
        /// 表名冲突
        /// </summary>
        TABLE = 2,
        /// <summary>
        /// 主键冲突
        /// </summary>
        KEY = 3
    }
}

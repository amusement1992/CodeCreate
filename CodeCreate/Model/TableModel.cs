using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeCreate.Model
{
    [Serializable]
    public class TableModel
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 表名描述
        /// </summary>
        public string TableDesc { get; set; }

        /// <summary>
        /// 不包含的属性
        /// </summary>
        public List<string> ExcludePropertys { get; set; }

        public List<ColumnModel> List { get; set; }
    }

    public class ColumnModel
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string ColumnName { get; set; }
        public string ColumnType { get; set; }
        /// <summary>
        /// 新的字段名
        /// </summary>
        public string NewColumnName { get; set; }

        /// <summary>
        /// 默认
        /// </summary>
        public string NewColumnType { get; set; }
        public string NewColumnComment { get; set; }

        public string NewColumnType_Dto { get; set; }
        public string NewColumnName_Dto { get; set; }

        public string NewColumnType_VM { get; set; }
        public string NewColumnName_VM { get; set; }

        public bool IsMapper { get; set; }
        public string MapperModel { get; set; }
        public string MapperName { get; set; }


        public List<string> ListFilterDto { get; set; }
        public List<string> ListFilterVM { get; set; }


        //public List<string> ListVM { get; set; }
        public List<string> ListCmdDto { get; set; }

        public List<string> ListDeleteCmdDto { get; set; }

    }
}

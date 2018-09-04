using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace CodeCreate
{
    /// <summary>
    /// 创建Model文件
    /// </summary>
    public class Create_FilterViewModel
    {
        public void Create(string str_nameSpace, DataTable dt_tables, string tableName)
        {
            string tablePrefix = CommonCode.GetTablePrefix(tableName);
            tableName = CommonCode.GetTableName(tableName);
            string tableDesc = CommonCode.GetTableDesc(tableName);

            bool isPrimeKey = false;
            string primaryKey = "";

            StringBuilder sb = new StringBuilder();
            StringBuilder sb_search = new StringBuilder();

            #region Model

            sb.AppendLine("        #region Model");
            //遍历每个字段
            foreach (DataRow dr in dt_tables.Rows)
            {
                string columnName = dr["columnName"].ToString().Trim();//字段名
                string columnType = dr["columnType"].ToString().Trim();//字段类型
                string columnComment = dr["columnComment"].ToString().Trim();//字段注释
                string nullable = dr["nullable"].ToString().Trim();//是否可空（Y是不为空，N是为空）
                string data_default = dr["data_default"].ToString().Trim();//默認值
                string data_maxLength = dr["char_col_decl_length"].ToString().Trim();//最大長度
                string bool_primaryKey = dr["primaryKey"].ToString().Trim();//主键 值为Y或N

                if (bool_primaryKey.ToUpper() == "Y")//存在主键
                {
                    isPrimeKey = true;
                    primaryKey = columnName.ToUpper();
                }
                if (string.IsNullOrEmpty(columnComment))
                {
                    columnComment = columnName;
                }

                CommonCode.GetColumnType(ref columnType, ref data_default);

                nullable = CommonCode.GetNullable(columnType, nullable);

                nullable = columnType == "string" ? "" : "?";

                sb.AppendLine("");
                sb.AppendLine(@"        /// <summary>");
                sb.AppendLine(@"        /// " + columnComment);
                sb.AppendLine(@"        /// </summary>");
                sb.AppendLine("        public " + columnType + nullable + " " + columnName + " { get; set; }");

                if (columnType == "string")
                {
                    sb_search.AppendLine("");
                    sb_search.AppendLine(@"        /// <summary>");
                    sb_search.AppendLine(@"        /// " + columnComment);
                    sb_search.AppendLine(@"        /// </summary>");
                    sb_search.AppendLine("        public " + columnType + nullable + " " + columnName + "_Search { get; set; }");

                }
            }
            sb.AppendLine("");
            sb.AppendLine("        #endregion Model");

            #endregion Model

            StringBuilder sb_body = new StringBuilder();

            sb_body.AppendLine("using System;");
            sb_body.AppendLine("using System.Collections.Generic;");
            sb_body.AppendLine("using Lee.CQuery.Paging;");
            sb_body.AppendLine("");
            sb_body.AppendLine("namespace " + str_nameSpace + ".ViewModel." + tablePrefix + ".Filter");
            sb_body.AppendLine("{");
            sb_body.AppendLine("    /// <summary>");
            sb_body.AppendLine("    /// FilterViewModel：" + tableDesc);
            sb_body.AppendLine("    /// </summary>");
            sb_body.AppendLine("    public class " + tableName + "FilterViewModel: PagingFilter");
            sb_body.AppendLine("    {");
            sb_body.AppendLine("");


            SetData(tableName, sb);
            sb_body.Append(sb.ToString());

            sb_body.AppendLine("");
            sb_body.AppendLine("        #region Search");
            sb_body.Append(sb_search.ToString());
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion Search");
            sb_body.AppendLine("");

            sb_body.AppendLine("    }");
            sb_body.AppendLine("}");

            string file_Model = "C:\\Code\\" + str_nameSpace + ".ViewModel\\" + tablePrefix + "\\Filter";
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }
            CommonCode.Save(file_Model + "/" + tableName + "FilterViewModel" + ".cs", sb_body.ToString());
        }


        private static void SetData(string tableName, StringBuilder sb)
        {
            var listModel = CommonCode.GetTableModel(tableName);
            if (listModel != null)
            {
                foreach (var item in listModel)
                {
                    if (item.List != null)
                    {

                        foreach (var thisModel in item.List.Where(d => d.ListFilterVM != null))
                        {
                            sb.AppendLine("");
                            sb.AppendLine(@"        /// <summary>");
                            sb.AppendLine(@"        /// 扩展：" + thisModel.ListFilterVM[2]);
                            sb.AppendLine(@"        /// </summary>");
                            sb.AppendLine("        public " + thisModel.ListFilterVM[0] + " " + thisModel.ListFilterVM[1] + " { get; set; }");

                        }
                    }
                }
            }
        }
    }
}
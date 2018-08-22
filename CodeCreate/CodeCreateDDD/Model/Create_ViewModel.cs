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
    public class Create_ViewModel
    {
        public void Create(string str_nameSpace, DataTable dt_tables, string tableName)
        {
            string tablePrefix = CommonCode.GetTablePrefix(tableName); tableName = CommonCode.GetTableName(tableName);

            bool isPrimeKey = false;
            string primaryKey = "";

            StringBuilder sb = new StringBuilder();

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

                sb.AppendLine("");

                sb.AppendLine(@"        /// <summary>");
                sb.AppendLine(@"        /// " + columnComment);
                sb.AppendLine(@"        /// </summary>");
                //if (dr["nullable"].ToString().ToUpper().Trim() == "Y")//不為空
                //{
                //    sb.AppendLine(@"        [Required]");
                //}
                //if (!string.IsNullOrEmpty(data_maxLength))//最大長度
                //{
                //    sb.AppendLine(@"        [StringLength(" + data_maxLength + ")]");
                //}
                sb.AppendLine("        public " + columnType + nullable + " " + columnName + " { get; set; }");
            }
            sb.AppendLine("");
            sb.AppendLine("        #endregion Model");

            #endregion Model

            SetData(tableName, sb);

            StringBuilder sb_body = new StringBuilder();

            sb_body.AppendLine("using System;");
            sb_body.AppendLine("using System.Collections.Generic;");
            sb_body.AppendLine("using Lee.CQuery.Paging;");
            sb_body.AppendLine("");
            sb_body.AppendLine("namespace " + str_nameSpace + ".ViewModel." + tablePrefix + "");
            sb_body.AppendLine("{");
            sb_body.AppendLine("    public class " + tableName + "ViewModel");
            sb_body.AppendLine("    {");
            sb_body.AppendLine("");

            sb_body.Append(sb.ToString());

            sb_body.AppendLine("");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("}");

            string file_Model = "C:\\Code\\" + str_nameSpace + ".ViewModel\\" + tablePrefix + "";
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }
            CommonCode.Save(file_Model + "/" + tableName + "ViewModel" + ".cs", sb_body.ToString());
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

                        foreach (var thisModel in item.List.Where(d => !string.IsNullOrEmpty(d.NewColumnName_VM)))
                        {

                            if (string.IsNullOrEmpty(thisModel.NewColumnType_VM))
                            {
                                thisModel.NewColumnType_VM = thisModel.NewColumnType;
                            }
                            sb.AppendLine("");
                            sb.AppendLine(@"        /// <summary>");
                            sb.AppendLine(@"        /// 扩展：" + thisModel.NewColumnComment);
                            sb.AppendLine(@"        /// </summary>");
                            sb.AppendLine("        public " + thisModel.NewColumnType_VM + " " + thisModel.NewColumnName_VM + " { get; set; }");

                        }
                    }
                }
            }
        }

    }
}
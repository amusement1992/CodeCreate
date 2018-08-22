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
    public class Create_CmdDto
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
                sb.AppendLine("        public " + columnType + nullable + " " + columnName + " { get; set; }");
            }
            sb.AppendLine("");
            sb.AppendLine("        #endregion Model");

            #endregion Model

            StringBuilder sb_body = new StringBuilder();

            sb_body.AppendLine("using System;");
            sb_body.AppendLine("using System.Collections.Generic;");
            sb_body.AppendLine("using BigDataAnalysis.DTO.Sys.Cmd;");
            sb_body.AppendLine("");
            sb_body.AppendLine("namespace " + str_nameSpace + ".DTO." + tablePrefix + ".Cmd");
            sb_body.AppendLine("{");
            sb_body.AppendLine("    public class " + tableName + "CmdDto");
            sb_body.AppendLine("    {");
            sb_body.AppendLine("");

            SetData(tableName, sb);

            sb_body.Append(sb.ToString());

            sb_body.AppendLine("");
            sb_body.AppendLine("");
            sb_body.AppendLine("    }");

            SetDeleteCmdDto(tableName, sb_body);

            SetSaveCmdDto(tableName, sb_body);

            sb_body.AppendLine("}");


            string file_Model = "C:\\Code\\" + str_nameSpace + ".DTO\\" + tablePrefix + "\\Cmd";
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }
            CommonCode.Save(file_Model + "/" + tableName + "CmdDto" + ".cs", sb_body.ToString());
        }

        private static void SetDeleteCmdDto(string tableName, StringBuilder sb_body)
        {
            sb_body.AppendLine("");
            sb_body.AppendLine("    /// <summary>");
            sb_body.AppendLine("    /// 删除");
            sb_body.AppendLine("    /// </summary>");
            sb_body.AppendLine("    public class Delete" + tableName + "CmdDto");
            sb_body.AppendLine("    {");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 编号");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        public List<Guid> " + tableName + "Ids");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            get; set;");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 修改人");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        public Guid UpdateUserID { get; set; }");
            
            StringBuilder sb = new StringBuilder();
            SetDeleteData(tableName, sb);

            sb_body.Append(sb.ToString());

            sb_body.AppendLine("    }");
            sb_body.AppendLine("");
        }

        private static void SetSaveCmdDto(string tableName, StringBuilder sb_body)
        {
            sb_body.AppendLine("");
            sb_body.AppendLine("    /// <summary>");
            sb_body.AppendLine("    /// 保存信息");
            sb_body.AppendLine("    /// </summary>");
            sb_body.AppendLine("    public class Save" + tableName + "CmdDto");
            sb_body.AppendLine("    {");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 信息");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        public " + tableName + "CmdDto " + tableName + "");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            get; set;");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("");
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

                        foreach (var thisModel in item.List.Where(d => d.ListCmdDto != null))
                        {
                            sb.AppendLine("");
                            sb.AppendLine(@"        /// <summary>");
                            sb.AppendLine(@"        /// 扩展：" + thisModel.ListCmdDto[2]);
                            sb.AppendLine(@"        /// </summary>");
                            sb.AppendLine("        public " + thisModel.ListCmdDto[0] + " " + thisModel.ListCmdDto[1] + " { get; set; }");

                        }
                    }
                }
            }
        }

        private static void SetDeleteData(string tableName, StringBuilder sb)
        {
            var listModel = CommonCode.GetTableModel(tableName);
            if (listModel != null)
            {
                foreach (var item in listModel)
                {
                    if (item.List != null)
                    {

                        foreach (var thisModel in item.List.Where(d => d.ListDeleteCmdDto != null))
                        {
                            sb.AppendLine("");
                            sb.AppendLine(@"        /// <summary>");
                            sb.AppendLine(@"        /// 扩展：" + thisModel.ListDeleteCmdDto[2]);
                            sb.AppendLine(@"        /// </summary>");
                            sb.AppendLine("        public " + thisModel.ListDeleteCmdDto[0] + " " + thisModel.ListDeleteCmdDto[1] + " { get; set; }");

                        }
                    }
                }
            }
        }

    }
}
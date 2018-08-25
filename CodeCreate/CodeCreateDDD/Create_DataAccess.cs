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
    public class Create_DataAccess
    {
        public void Create(string str_nameSpace, DataTable dt_tables, string tableName)
        {
            string tablePrefix = CommonCode.GetTablePrefix(tableName);
            tableName = CommonCode.GetTableName(tableName);
            string tableDesc = CommonCode.GetTableDesc(tableName);

            bool isPrimeKey = false;
            string primaryKey = "";

            StringBuilder sb = new StringBuilder();

            #region Model

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

                sb.Append("\"" + columnName + "\", ");
            }
            string str = sb.ToString().Substring(0, sb.ToString().Length - 2);

            #endregion Model

            StringBuilder sb_body = new StringBuilder();


            sb_body.AppendLine("using Lee.Command;");
            sb_body.AppendLine("using Lee.Command.RDB;");
            sb_body.AppendLine("using " + str_nameSpace + ".DataAccessInterface." + tablePrefix + ";");
            sb_body.AppendLine("using " + str_nameSpace + ".Entity." + tablePrefix + ";");
            sb_body.AppendLine("using System;");
            sb_body.AppendLine("using System.Collections.Generic;");
            sb_body.AppendLine("using System.Linq;");
            sb_body.AppendLine("using System.Text;");
            sb_body.AppendLine("using System.Threading.Tasks;");
            sb_body.AppendLine("using Lee.Utility.Extension;");
            sb_body.AppendLine("using Lee.Command.UnitOfWork;");
            sb_body.AppendLine("using " + str_nameSpace + ".Entity;");
            sb_body.AppendLine("");
            sb_body.AppendLine("namespace " + str_nameSpace + ".DataAccess." + tablePrefix + "");
            sb_body.AppendLine("{");
            sb_body.AppendLine("    /// <summary>");
            sb_body.AppendLine("    /// 数据访问：" + tableDesc);
            sb_body.AppendLine("    /// </summary>");
            sb_body.AppendLine("    public class " + tableName + "DataAccess : RdbDataAccess<" + tableName + "Entity>, I" + tableName + "DbAccess");
            sb_body.AppendLine("    {");
            sb_body.AppendLine("");

            sb_body.AppendLine("        #region 获取添加字段");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 获取添加字段");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        protected override string[] GetEditFields()");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            return new string[] { " + str + " };");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 获取查询字段");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 获取查询字段");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        protected override string[] GetQueryFields()");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            return new string[] { " + str + " };");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");

            sb_body.AppendLine("    }");
            sb_body.AppendLine("}");


            string file_Model = "C:\\Code\\" + str_nameSpace + ".DataAccess\\" + tablePrefix + "";
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }
            CommonCode.Save(file_Model + "/" + tableName + "DataAccess" + ".cs", sb_body.ToString());
        }

    }
}
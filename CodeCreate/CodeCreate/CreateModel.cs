using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CodeCreate
{
    /// <summary>
    /// 创建Model文件
    /// </summary>
    public class CreateModel
    {
        public void Create(string file_Model, string str_nameSpace, DataTable dt_tables, string str_ModelName)
        {
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

                #region 数据类型判断

                switch (columnType.ToLower())
                {
                    case "bit":
                        columnType = "bool";
                        if (!string.IsNullOrEmpty(data_default))
                        {
                            data_default = " = " + data_default.TrimStart('(').TrimEnd(')');
                        }
                        break;

                    case "int":
                    case "tinyint":
                    case "bigint":
                        columnType = "int";
                        if (!string.IsNullOrEmpty(data_default))
                        {
                            data_default = " = " + data_default.TrimStart('(').TrimEnd(')');
                        }
                        break;

                    case "binary":
                    case "decimal":
                    case "float":
                    case "money":
                    case "numeric":
                    case "smallint":
                    case "smallmoney":
                    case "varbinary":
                        columnType = "decimal";
                        if (!string.IsNullOrEmpty(data_default))
                        {
                            data_default = " = " + data_default.TrimStart('(').TrimEnd(')');
                        }
                        break;

                    case "date":
                    case "datetime":
                    case "datetime2":
                    case "smalldatetime":
                    case "time":
                        columnType = "DateTime";
                        if (!string.IsNullOrEmpty(data_default))
                        {
                            if (data_default.ToLower() == "(getdate())")
                            {
                                data_default = " = DateTime.Now";
                            }
                        }
                        break;

                    default:
                        columnType = "string";
                        if (!string.IsNullOrEmpty(data_default))
                        {
                            data_default = " = \"" + data_default.Trim('(', ')').Trim('\'') + "\"";
                        }
                        break;
                }

                #endregion 数据类型判断

                if (nullable.ToUpper().Trim() == "N" && (columnType.ToLower() == "decimal" || columnType.ToLower() == "int"))
                {
                    nullable = "?";
                }
                else
                {
                    nullable = "";
                }

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
            sb.AppendLine("        #endregion Model");

            #endregion Model

            string st_prime_tip = "";
            if (!isPrimeKey)
            {
                st_prime_tip = "没有主键，默认主键为：" + primaryKey;
            }
            else
            {
                st_prime_tip = "有主键，主键为：" + primaryKey;
            }
            StringBuilder sb_body = new StringBuilder();

            sb_body.AppendLine("using System;");
            sb_body.AppendLine("");
            sb_body.AppendLine("namespace " + str_nameSpace + ".Model");
            sb_body.AppendLine("{");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// Model实体类：" + str_ModelName);
            sb_body.AppendLine("        /// 创建时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("    public partial class " + str_ModelName);
            sb_body.AppendLine("    {");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// " + st_prime_tip);
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        public " + str_ModelName + "() { }");
            sb_body.AppendLine("");

            sb_body.Append(sb.ToString());

            sb_body.AppendLine("");
            sb_body.AppendLine("");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("}");

            CommonCode.Save(file_Model + "/" + str_ModelName + ".cs", sb_body.ToString());
        }
    }
}
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
    public class Create_DomainModel
    {
        public void Create(string file_Model, string str_nameSpace, DataTable dt_tables, string tableName)
        {
            tableName = tableName.Replace("Data_", "");

            bool isPrimeKey = false;
            string primaryKey = "";

            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();

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
                sb.AppendLine("        protected " + columnType + nullable + " _" + columnName + ";");


                sb2.AppendLine("        /// <summary>");
                sb2.AppendLine("        /// " + columnComment);
                sb2.AppendLine("        /// </summary>");
                sb2.AppendLine("        public " + columnType + nullable + " " + columnName);
                sb2.AppendLine("        {");
                sb2.AppendLine("            get");
                sb2.AppendLine("            {");
                sb2.AppendLine("                return _" + columnName + ";");
                sb2.AppendLine("            }");
                sb2.AppendLine("            protected set");
                sb2.AppendLine("            {");
                sb2.AppendLine("                _" + columnName + " = value;");
                sb2.AppendLine("            }");
                sb2.AppendLine("        }");
                sb2.AppendLine("        ");

            }

            #endregion Model

            StringBuilder sb_body = new StringBuilder();

            sb_body.AppendLine("using System;");
            sb_body.AppendLine("using Lee.Domain.Aggregation;");
            sb_body.AppendLine("using Lee.Utility;");
            sb_body.AppendLine("using BigDataAnalysis.Domain.Data.Repository;");
            sb_body.AppendLine("using Lee.Utility.Extension;");
            sb_body.AppendLine("");
            sb_body.AppendLine("namespace BigDataAnalysis.Domain.Data.Model");
            sb_body.AppendLine("{");
            sb_body.AppendLine("    /// <summary>");
            sb_body.AppendLine("    /// ");
            sb_body.AppendLine("    /// </summary>");
            sb_body.AppendLine("    public class " + tableName + " : AggregationRoot<" + tableName + ">");
            sb_body.AppendLine("    {");
            sb_body.AppendLine("        I" + tableName + "Repository " + tableName + "Repository = null;");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region	字段");

            sb_body.Append(sb.ToString());

            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 构造方法");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 实例化对象");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"SysNo\">编号</param>");
            sb_body.AppendLine("        internal " + tableName + "(Guid? SysNo = null)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            _SysNo = SysNo ?? Guid.Empty;");
            sb_body.AppendLine("            " + tableName + "Repository = this.Instance<I" + tableName + "Repository>();");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region	属性");
            sb_body.AppendLine("");

            sb_body.Append(sb2.ToString());

            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 方法");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 功能方法");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 保存");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 保存");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        public override void Save()");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            " + tableName + "Repository.Save(this);");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region	移除");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 移除");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        public override void Remove()");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            " + tableName + "Repository.Remove(this);");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 初始化标识信息");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 初始化标识信息");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        public override void InitPrimaryValue()");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            base.InitPrimaryValue();");
            sb_body.AppendLine("            _SysNo = Generate" + tableName + "Id();");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 内部方法");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 判断标识对象值是否为空");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 判断标识对象值是否为空");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        protected override bool PrimaryValueIsNone()");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            return _SysNo == Guid.Empty;");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 静态方法");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 生成一个编号");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 生成一个编号");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        public static Guid Generate" + tableName + "Id()");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            return Guid.NewGuid();");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 创建");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 创建一个对象");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"SysNo\">编号</param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        public static " + tableName + " Create" + tableName + "(Guid? SysNo = null)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            SysNo = SysNo == Guid.Empty ? Generate" + tableName + "Id() : SysNo;");
            sb_body.AppendLine("            return new " + tableName + "(SysNo);");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("}");




            file_Model = "C:\\Code\\BigDataAnalysis.Domain\\Data\\Model";
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }
            CommonCode.Save(file_Model + "/" + tableName + "" + ".cs", sb_body.ToString());
        }

    }
}
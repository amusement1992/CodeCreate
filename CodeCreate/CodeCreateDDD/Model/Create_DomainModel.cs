using CodeCreate.Model;
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
        public void Create(string str_nameSpace, DataTable dt_tables, string tableName)
        {
            string tablePrefix = CommonCode.GetTablePrefix(tableName);
            tableName = CommonCode.GetTableName(tableName);
            string tableDesc = CommonCode.GetTableDesc(tableName);

            bool isPrimeKey = false;
            string primaryKey = "";

            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            StringBuilder sb3 = new StringBuilder();

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
                sb2.AppendLine("            get { return _" + columnName + "; }");
                sb2.AppendLine("            protected set { _" + columnName + " = value; }");
                sb2.AppendLine("        }");
                sb2.AppendLine("");
                SetExcludePropertys(tableName, sb3, columnName);

            }

            StringBuilder sb_instance = new StringBuilder();
            StringBuilder sb_instanceMethod = new StringBuilder();
            StringBuilder sb_SetLazyMember = new StringBuilder();
            StringBuilder sb_attribute = new StringBuilder();

            var listModel = CommonCode.GetTableModel(tableName);
            if (listModel != null)
            {
                foreach (var item in listModel)
                {
                    SetSB(sb, item);
                    SetInstance(tableName, sb_instance, sb_instanceMethod, sb_SetLazyMember, item);
                    SetAttribute(sb_attribute, item);
                }
            }



            #endregion Model

            StringBuilder sb_body = new StringBuilder();

            sb_body.AppendLine("using System;");
            sb_body.AppendLine("using Lee.Domain.Aggregation;");
            sb_body.AppendLine("using Lee.Utility;");
            sb_body.AppendLine("using " + str_nameSpace + ".Domain." + tablePrefix + ".Repository;");
            sb_body.AppendLine("using Lee.Utility.Extension;");
            sb_body.AppendLine("using System.Collections.Generic;");
            sb_body.AppendLine("using System.Linq;");
            sb_body.AppendLine("using Lee.Utility.ValueType;");
            sb_body.AppendLine("using " + str_nameSpace + ".Domain.Model;");
            sb_body.AppendLine("using " + str_nameSpace + ".Domain.Service;");

            sb_body.AppendLine("using " + str_nameSpace + ".Domain.Contract.Service;");
            sb_body.AppendLine("using Lee.CQuery;");
            sb_body.AppendLine("using " + str_nameSpace + ".Query.Contract;");
            sb_body.AppendLine("using " + str_nameSpace + ".Domain.Data.Model;");
            sb_body.AppendLine("using " + str_nameSpace + ".Domain.Data.Service;");
            sb_body.AppendLine("using " + str_nameSpace + ".Domain.Repository;");
            sb_body.AppendLine("using " + str_nameSpace + ".Query;");
            sb_body.AppendLine("using " + str_nameSpace + ".Domain.Data.Repository;");
            sb_body.AppendLine("using " + str_nameSpace + ".Query.Data;");


            sb_body.AppendLine("");
            sb_body.AppendLine("namespace " + str_nameSpace + ".Domain." + tablePrefix + ".Model");
            sb_body.AppendLine("{");
            sb_body.AppendLine("    /// <summary>");
            sb_body.AppendLine("    /// DomainModel：" + tableDesc);
            sb_body.AppendLine("    /// </summary>");
            sb_body.AppendLine("    public class " + tableName + " : AggregationRoot<" + tableName + ">");
            sb_body.AppendLine("    {");
            sb_body.AppendLine("        I" + tableName + "Repository " + tableName + "Repository = null;");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region	字段");
            sb_body.AppendLine("");

            sb_body.AppendLine(sb.ToString());

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
            sb_body.AppendLine("");
            sb_body.AppendLine(sb_instance.ToString());
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine(sb_instanceMethod.ToString());
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 设置LazyMember");
            sb_body.AppendLine("");
            sb_body.AppendLine(sb_SetLazyMember.ToString());
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");


            sb_body.AppendLine("");
            sb_body.AppendLine("        #region	属性");
            sb_body.AppendLine("");

            sb_body.AppendLine(sb2.ToString());

            sb_body.AppendLine("");
            sb_body.AppendLine(sb_attribute.ToString());
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
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 根据给定的对象更新当前信息");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 根据给定的对象更新当前信息");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"" + tableName + "\">信息</param>");
            sb_body.AppendLine("        /// <param name=\"excludePropertys\">排除更新的属性</param>");
            sb_body.AppendLine("        public virtual void ModifyFromOther" + tableName + "(" + tableName + " " + tableName + ", IEnumerable<string> excludePropertys = null)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            if (" + tableName + " == null)");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                return;");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("");
            sb_body.AppendLine("            CopyDataFromSimilarObject(" + tableName + ", excludePropertys);");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 从指定对象复制值");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 从指定对象复制值");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <typeparam name=\"DT\">数据类型</typeparam>");
            sb_body.AppendLine("        /// <param name=\"similarObject\">数据对象</param>");
            sb_body.AppendLine("        /// <param name=\"excludePropertys\">排除不复制的属性</param>");
            sb_body.AppendLine("        protected override void CopyDataFromSimilarObject<DT>(DT similarObject, IEnumerable<string> excludePropertys = null)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            base.CopyDataFromSimilarObject<DT>(similarObject, excludePropertys);");
            sb_body.AppendLine("            if (similarObject == null)");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                return;");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("            excludePropertys = excludePropertys ?? new List<string>(0);");
            sb_body.AppendLine("");
            sb_body.AppendLine("            #region 复制值");
            sb_body.AppendLine("");

            sb_body.AppendLine(sb3.ToString());

            sb_body.AppendLine("");
            sb_body.AppendLine("            #endregion");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 设置默认值");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        public void SetDefaultValue()");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            CreateDate = DateTime.Now;");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("}");
            sb_body.AppendLine("");





            string file_Model = "C:\\Code\\" + str_nameSpace + ".Domain\\" + tablePrefix + "\\Model";
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }
            CommonCode.Save(file_Model + "/" + tableName + "" + ".cs", sb_body.ToString());
        }

        private static void SetExcludePropertys(string tableName, StringBuilder sb3, string columnName)
        {
            var tableModel2 = CommonCode.GetTableModel(tableName);
            List<string> ExcludePropertys = new List<string>();
            foreach (var item in tableModel2)
            {
                if (item.ExcludePropertys != null)
                {
                    ExcludePropertys.AddRange(item.ExcludePropertys);
                }
            }

            if (ExcludePropertys.Contains(columnName))
            {
                sb3.AppendLine("            //if (!excludePropertys.Contains(\"" + columnName + "\"))");
                sb3.AppendLine("            //{");
                sb3.AppendLine("            //    " + columnName + " = similarObject." + columnName + ";");
                sb3.AppendLine("            //}");
            }
            else
            {
                sb3.AppendLine("            if (!excludePropertys.Contains(\"" + columnName + "\"))");
                sb3.AppendLine("            {");
                sb3.AppendLine("                " + columnName + " = similarObject." + columnName + ";");
                sb3.AppendLine("            }");
            }
        }

        private static void SetAttribute(StringBuilder sb_attribute, TableModel tableModel)
        {
            if (tableModel.List != null)
            {
                foreach (var thisModel in tableModel.List.Where(d => !string.IsNullOrEmpty(d.NewColumnName)))
                {
                    sb_attribute.AppendLine("        /// <summary>");
                    sb_attribute.AppendLine("        /// 扩展：" + thisModel.NewColumnComment + "");
                    sb_attribute.AppendLine("        /// </summary>");
                    sb_attribute.AppendLine("        public " + thisModel.NewColumnType + " " + thisModel.NewColumnName + "");
                    sb_attribute.AppendLine("        {");
                    sb_attribute.AppendLine("");
                    sb_attribute.AppendLine("            get { return _" + thisModel.NewColumnName + ".Value; }");
                    sb_attribute.AppendLine("            protected set { _" + thisModel.NewColumnName + ".SetValue(value, false); }");
                    sb_attribute.AppendLine("        }");
                    sb_attribute.AppendLine(" ");
                }
            }
        }

        private static void SetInstance(string tableName, StringBuilder sb_instance, StringBuilder sb_instanceMethod, StringBuilder sb_SetLazyMember, TableModel tableModel)
        {
            if (tableModel.List != null)
            {

                foreach (var thisModel in tableModel.List.Where(d => !string.IsNullOrEmpty(d.NewColumnName)))
                {
                    sb_instance.AppendLine("            _" + thisModel.NewColumnName + " = new LazyMember<" + thisModel.NewColumnType + ">(Load" + thisModel.NewColumnName + ");");

                    sb_instanceMethod.AppendLine("        /// <summary>");
                    sb_instanceMethod.AppendLine("        /// 加载：" + thisModel.NewColumnComment);
                    sb_instanceMethod.AppendLine("        /// </summary>");
                    sb_instanceMethod.AppendLine("        /// <returns></returns>");

                    if (thisModel.ColumnType == "Guid" || thisModel.ColumnType == "Guid?")
                    {
                        sb_instanceMethod.AppendLine("        " + thisModel.NewColumnType + " Load" + thisModel.NewColumnName + "()");
                        sb_instanceMethod.AppendLine("        {");
                        sb_instanceMethod.AppendLine("            if (!AllowLazyLoad(d => d." + thisModel.NewColumnName + "))");
                        sb_instanceMethod.AppendLine("            {");
                        sb_instanceMethod.AppendLine("                return _" + thisModel.NewColumnName + ".CurrentValue;");
                        sb_instanceMethod.AppendLine("            }");
                        sb_instanceMethod.AppendLine("            return " + thisModel.NewColumnType + "Service.Get" + thisModel.NewColumnType + "(" + thisModel.ColumnName + ");");
                        sb_instanceMethod.AppendLine("        }");
                    }
                    else
                    {
                        sb_instanceMethod.AppendLine("        " + thisModel.NewColumnType + " Load" + thisModel.NewColumnName + "()");
                        sb_instanceMethod.AppendLine("        {");
                        sb_instanceMethod.AppendLine("            if (!AllowLazyLoad(d => d." + thisModel.NewColumnName + "))");
                        sb_instanceMethod.AppendLine("            {");
                        sb_instanceMethod.AppendLine("                return _" + thisModel.NewColumnName + ".CurrentValue;");
                        sb_instanceMethod.AppendLine("            }");
                        sb_instanceMethod.AppendLine("            return " + tableName + "Service.Get" + thisModel.NewColumnName + "(SysNo);");
                        sb_instanceMethod.AppendLine("        }");
                    }


                    sb_SetLazyMember.AppendLine("        /// <summary>");
                    sb_SetLazyMember.AppendLine("        /// 设置LazyMember：" + thisModel.NewColumnComment);
                    sb_SetLazyMember.AppendLine("        /// </summary>");
                    sb_SetLazyMember.AppendLine("        public void Set" + thisModel.NewColumnName + "(" + thisModel.NewColumnType + " value, bool init = true)");
                    sb_SetLazyMember.AppendLine("        {");
                    sb_SetLazyMember.AppendLine("            _" + thisModel.NewColumnName + ".SetValue(value, init);");
                    sb_SetLazyMember.AppendLine("        }");
                    sb_SetLazyMember.AppendLine("");

                }
            }
        }

        private static void SetSB(StringBuilder sb, TableModel tableModel)
        {
            if (tableModel.List != null)
            {
                foreach (var thisModel in tableModel.List.Where(d => !string.IsNullOrEmpty(d.NewColumnName)))
                {
                    //if (string.IsNullOrEmpty(thisModel.NewColumnType))
                    //{
                    //    thisModel.NewColumnType = thisModel.NewColumnName;
                    //}

                    sb.AppendLine("");
                    sb.AppendLine(@"        /// <summary>");
                    sb.AppendLine(@"        /// 扩展：" + thisModel.NewColumnComment);
                    sb.AppendLine(@"        /// </summary>");
                    sb.AppendLine("        protected LazyMember<" + thisModel.NewColumnType + "> _" + thisModel.NewColumnName + ";");
                }
            }
        }
    }
}
using CodeCreate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeCreate
{
    public class CreateDAL
    {
        public void Create(string file_DAL, string str_table, string str_DALName, string str_IDALName, string str_ModelName, StringBuilder sb_column1, StringBuilder sb_column2, StringBuilder sb_column3, StringBuilder sb_column4, StringBuilder sb_column5, ConfigModel configModel)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Text;");
            sb.AppendLine("using System.Data.SqlClient;");
            sb.AppendLine("using " + configModel.str_nameSpace + ".IDAL;");
            sb.AppendLine("using " + configModel.str_nameSpace + ".DBUtility;");
            sb.AppendLine("using " + configModel.str_nameSpace + ".Common;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using " + configModel.str_nameSpace + ".Model;");
            sb.AppendLine("");
            sb.AppendLine("namespace " + configModel.str_nameSpace + ".SqlDAL");
            sb.AppendLine("{");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// 数据访问类：" + str_DALName);
            sb.AppendLine("    /// </summary>");
            sb.AppendLine("    public partial class " + str_DALName + " : " + str_IDALName);
            sb.AppendLine("    {");
            sb.AppendLine("        public " + str_DALName + "() { }");
            sb.AppendLine("");

            #region BasicMethod

            sb.AppendLine("        #region  BasicMethod");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 是否存在该记录");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public bool Exists(string ST_CODE)");
            sb.AppendLine("        {");
            sb.AppendLine("            StringBuilder sb = new StringBuilder();");
            sb.AppendLine("            sb.Append(\"select count(1) from " + str_table.ToUpper() + "\");");
            sb.AppendLine("            sb.Append(\" where ST_CODE=" + configModel.MARK + "ST_CODE \");");
            sb.AppendLine("            SqlParameter[] parameters ={");
            sb.AppendLine("                new SqlParameter(\"" + configModel.MARK + "ST_CODE\",ST_CODE)");
            sb.AppendLine("            };");
            sb.AppendLine("            return " + configModel.str_SqlHelper + "." + configModel.str_ExecuteScalar + "(sb.ToString(), parameters);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 增加一条数据");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public bool Add(" + str_ModelName + " model)");
            sb.AppendLine("        {");
            sb.AppendLine("            StringBuilder strSql;");
            sb.AppendLine("            SqlParameter[] parameters;");
            sb.AppendLine("            AddModel(model, out strSql, out parameters);");
            sb.AppendLine("");
            sb.AppendLine("            return new SqlCode().ReturnBool(strSql.ToString(), parameters);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 增加多条数据");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public int AddList(List<" + str_ModelName + "> list_model)");
            sb.AppendLine("        {");
            sb.AppendLine("            StringBuilder strSql;");
            sb.AppendLine("            SqlParameter[] parameters;");
            sb.AppendLine("            List<" + configModel.str_nameSpace + ".DBUtility.CommandInfo> ci = new List<CommandInfo>();");
            sb.AppendLine("");
            sb.AppendLine("            foreach (" + str_ModelName + " item in list_model)");
            sb.AppendLine("            {");
            sb.AppendLine("                strSql = new StringBuilder();");
            sb.AppendLine("                parameters = null;");
            sb.AppendLine("                AddModel(item, out strSql, out parameters);");
            sb.AppendLine("");
            sb.AppendLine("                " + configModel.str_nameSpace + ".DBUtility.CommandInfo subci = new CommandInfo();");
            sb.AppendLine("                subci.CommandText = strSql.ToString();");
            sb.AppendLine("                subci.Parameters = parameters;");
            sb.AppendLine("                subci.Type = CommandType.Text;");
            sb.AppendLine("                ci.Add(subci);");
            sb.AppendLine("            }");
            sb.AppendLine("            return " + configModel.str_SqlHelper + "." + configModel.str_UpdateDatabase + "(ci);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 更新一条数据");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public bool Update(" + str_ModelName + " model)");
            sb.AppendLine("        {");
            sb.AppendLine("            StringBuilder strSql;");
            sb.AppendLine("            SqlParameter[] parameters;");
            sb.AppendLine("            UpdateModel(model, out strSql, out parameters);");
            sb.AppendLine("");
            sb.AppendLine("            return new SqlCode().ReturnBool(strSql.ToString(), parameters);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 更新多条数据");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public int UpdateList(List<" + str_ModelName + "> list_model)");
            sb.AppendLine("        {");
            sb.AppendLine("            StringBuilder strSql;");
            sb.AppendLine("            SqlParameter[] parameters;");
            sb.AppendLine("            List<" + configModel.str_nameSpace + ".DBUtility.CommandInfo> ci = new List<CommandInfo>();");
            sb.AppendLine("");
            sb.AppendLine("            foreach (" + str_ModelName + " item in list_model)");
            sb.AppendLine("            {");
            sb.AppendLine("                strSql = new StringBuilder();");
            sb.AppendLine("                parameters = null;");
            sb.AppendLine("                UpdateModel(item, out strSql, out parameters);");
            sb.AppendLine("");
            sb.AppendLine("                " + configModel.str_nameSpace + ".DBUtility.CommandInfo subci = new CommandInfo();");
            sb.AppendLine("                subci.CommandText = strSql.ToString();");
            sb.AppendLine("                subci.Parameters = parameters;");
            sb.AppendLine("                subci.Type = CommandType.Text;");
            sb.AppendLine("                ci.Add(subci);");
            sb.AppendLine("");
            sb.AppendLine("            }");
            sb.AppendLine("            return " + configModel.str_SqlHelper + "." + configModel.str_UpdateDatabase + "(ci);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 删除一条数据");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public bool Delete(string ST_CODE)");
            sb.AppendLine("        {");
            sb.AppendLine("            //该表无主键信息，请自定义主键/条件字段");
            sb.AppendLine("            StringBuilder strSql = new StringBuilder();");
            sb.AppendLine("            strSql.Append(\"delete from " + str_table.ToUpper() + " \");");
            sb.AppendLine("            strSql.Append(\" where \");");
            sb.AppendLine("            SqlParameter[] parameters = {");
            sb.AppendLine("			};");
            sb.AppendLine("");
            sb.AppendLine("            return new SqlCode().ReturnBool(strSql.ToString(), parameters);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 删除多条数据");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        /// <param name=\"ST_CODElist\"></param>");
            sb.AppendLine("        /// <returns></returns>");
            sb.AppendLine("        public bool DeleteList(List<string> ST_CODElist)");
            sb.AppendLine("        {");
            sb.AppendLine("            List<string> list_sql = new List<string>();");
            sb.AppendLine("            foreach (string item in ST_CODElist)");
            sb.AppendLine("            {");
            sb.AppendLine("                list_sql.Add(\"delete from " + str_table.ToUpper() + " where ST_CODE ='\" + item + \"'\");");
            sb.AppendLine("            }");
            sb.AppendLine("            int rows = " + configModel.str_SqlHelper + "." + configModel.str_UpdateDatabase + "(list_sql);");
            sb.AppendLine("            if (rows > 0)");
            sb.AppendLine("            {");
            sb.AppendLine("                return true;");
            sb.AppendLine("            }");
            sb.AppendLine("            else");
            sb.AppendLine("            {");
            sb.AppendLine("                return false;");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 获得实体对象");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public " + str_ModelName + " GetModel(string ST_CODE)");
            sb.AppendLine("        {");
            sb.AppendLine("            //该表无主键信息，请自定义主键/条件字段");
            sb.AppendLine("            StringBuilder strSql = new StringBuilder();");
            sb.AppendLine("            strSql.Append(\"select " + sb_column1 + " from " + str_table.ToUpper() + " \");");
            sb.AppendLine("            strSql.Append(\" where \");");
            sb.AppendLine("            SqlParameter[] parameters = {");
            sb.AppendLine("			};");
            sb.AppendLine("            return " + configModel.str_SqlHelper + "." + configModel.str_GetModel + "<" + str_ModelName + ">(strSql.ToString(), parameters);");
            sb.AppendLine("");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 获得实体列表");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public List<" + str_ModelName + "> GetModelList(string strWhere)");
            sb.AppendLine("        {");
            sb.AppendLine("            StringBuilder strSql = new StringBuilder();");
            sb.AppendLine("            strSql.Append(\"select " + sb_column1 + " \");");
            sb.AppendLine("            strSql.Append(\" FROM " + str_table.ToUpper() + " \");");
            sb.AppendLine("            if (!string.IsNullOrEmpty(strWhere.Trim()))");
            sb.AppendLine("            {");
            sb.AppendLine("                strSql.Append(\" where \" + strWhere);");
            sb.AppendLine("            }");
            sb.AppendLine("            return " + configModel.str_SqlHelper + "." + configModel.str_GetModelList + "<" + str_ModelName + ">(strSql.ToString());");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 获得数据列表");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public DataTable GetList(string strWhere)");
            sb.AppendLine("        {");
            sb.AppendLine("            StringBuilder strSql = new StringBuilder();");
            sb.AppendLine("            strSql.Append(\"select " + sb_column1 + " \");");
            sb.AppendLine("            strSql.Append(\" FROM " + str_table.ToUpper() + " \");");
            sb.AppendLine("            if (!string.IsNullOrEmpty(strWhere.Trim()))");
            sb.AppendLine("            {");
            sb.AppendLine("                strSql.Append(\" where \" + strWhere);");
            sb.AppendLine("            }");
            sb.AppendLine("            return " + configModel.str_SqlHelper + "." + configModel.str_ExecuteNonQuery + "(strSql.ToString());");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 获取记录总数");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public int GetRecordCount(string strWhere)");
            sb.AppendLine("        {");
            sb.AppendLine("            StringBuilder strSql = new StringBuilder();");
            sb.AppendLine("            strSql.Append(\"select count(1) FROM " + str_table.ToUpper() + " \");");
            sb.AppendLine("            if (!string.IsNullOrEmpty(strWhere.Trim()))");
            sb.AppendLine("            {");
            sb.AppendLine("                strSql.Append(\" where \" + strWhere);");
            sb.AppendLine("            }");
            sb.AppendLine("            object obj = " + configModel.str_SqlHelper + "." + configModel.str_ExecuteScalar + "(strSql.ToString());");
            sb.AppendLine("            if (obj == null)");
            sb.AppendLine("            {");
            sb.AppendLine("                return 0;");
            sb.AppendLine("            }");
            sb.AppendLine("            else");
            sb.AppendLine("            {");
            sb.AppendLine("                return Convert.ToInt32(obj);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 分页获取数据列表");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public DataTable GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)");
            sb.AppendLine("        {");
            sb.AppendLine("            StringBuilder strSql = new StringBuilder();");
            sb.AppendLine("            strSql.Append(\"SELECT * FROM ( \");");
            sb.AppendLine("            strSql.Append(\" SELECT ROW_NUMBER() OVER (\");");
            sb.AppendLine("            if (!string.IsNullOrEmpty(orderby.Trim()))");
            sb.AppendLine("            {");
            sb.AppendLine("                strSql.Append(\"order by T.\" + orderby);");
            sb.AppendLine("            }");
            sb.AppendLine("            else");
            sb.AppendLine("            {");
            sb.AppendLine("                strSql.Append(\"order by T.ST_CODE desc\");");
            sb.AppendLine("            }");
            sb.AppendLine("            strSql.Append(\")AS Row, T.*  from " + str_table.ToUpper() + " T \");");
            sb.AppendLine("            if (!string.IsNullOrEmpty(strWhere.Trim()))");
            sb.AppendLine("            {");
            sb.AppendLine("                strSql.Append(\" WHERE \" + strWhere);");
            sb.AppendLine("            }");
            sb.AppendLine("            strSql.Append(\" ) TT\");");
            sb.AppendLine("            strSql.AppendFormat(\" WHERE TT.Row between {0} and {1}\", startIndex, endIndex);");
            sb.AppendLine("            return " + configModel.str_SqlHelper + "." + configModel.str_ExecuteNonQuery + "(strSql.ToString());");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /*");
            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 分页获取数据列表");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public DataTable GetList(int PageSize,int PageIndex,string strWhere)");
            sb.AppendLine("        {");
            sb.AppendLine("            SqlParameter[] parameters = {");
            sb.AppendLine("                    new SqlParameter(\"" + configModel.MARK + "tblName\", SqlType.VarChar, 255),");
            sb.AppendLine("                    new SqlParameter(\"" + configModel.MARK + "fldName\", SqlType.VarChar, 255),");
            sb.AppendLine("                    new SqlParameter(\"" + configModel.MARK + "PageSize\", SqlType.Number),");
            sb.AppendLine("                    new SqlParameter(\"" + configModel.MARK + "PageIndex\", SqlType.Number),");
            sb.AppendLine("                    new SqlParameter(\"" + configModel.MARK + "IsReCount\", SqlType.Clob),");
            sb.AppendLine("                    new SqlParameter(\"" + configModel.MARK + "OrderType\", SqlType.Clob),");
            sb.AppendLine("                    new SqlParameter(\"" + configModel.MARK + "strWhere\", SqlType.VarChar,1000),");
            sb.AppendLine("                    };");
            sb.AppendLine("            parameters[0].Value = \"" + str_table.ToUpper() + "\";");
            sb.AppendLine("            parameters[1].Value = \"ST_CODE\";");
            sb.AppendLine("            parameters[2].Value = PageSize;");
            sb.AppendLine("            parameters[3].Value = PageIndex;");
            sb.AppendLine("            parameters[4].Value = 0;");
            sb.AppendLine("            parameters[5].Value = 0;");
            sb.AppendLine("            parameters[6].Value = strWhere;	");
            sb.AppendLine("            return " + configModel.str_SqlHelper + ".RunProcedure(\"UP_GetRecordByPage\",parameters,\"ds\");");
            sb.AppendLine("        }*/");
            sb.AppendLine("");

            sb.AppendLine("        #endregion  BasicMethod");
            sb.AppendLine("");

            #endregion BasicMethod

            #region HelperMethod

            sb.AppendLine("        #region HelperMethod");
            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 添加一个Model");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        /// <param name=\"model\">Model类型</param>");
            sb.AppendLine("        /// <param name=\"strSql\">给sql语句赋值</param>");
            sb.AppendLine("        /// <param name=\"parameters\">给参数赋值</param>");
            sb.AppendLine("        private static void AddModel(" + str_ModelName + " model, out StringBuilder strSql, out SqlParameter[] parameters)");
            sb.AppendLine("        {");
            sb.AppendLine("            strSql = new StringBuilder();");
            sb.AppendLine("            strSql.Append(\"insert into " + str_table.ToUpper() + "(\");");
            sb.AppendLine("            strSql.Append(\"" + sb_column1 + ")\");");
            sb.AppendLine("            strSql.Append(\" values (\");");
            sb.AppendLine("            strSql.Append(\"" + sb_column2 + "\");");
            sb.AppendLine("            strSql.Append(\")\");");
            sb.AppendLine("            parameters = SetParameters(model);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 修改一个Model");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        /// <param name=\"model\">Model类型</param>");
            sb.AppendLine("        /// <param name=\"strSql\">给sql语句赋值</param>");
            sb.AppendLine("        /// <param name=\"parameters\">给参数赋值</param>");
            sb.AppendLine("        private static void UpdateModel(" + str_ModelName + " model, out StringBuilder strSql, out SqlParameter[] parameters)");
            sb.AppendLine("        {");
            sb.AppendLine("            strSql = new StringBuilder();");
            sb.AppendLine("            strSql.Append(\"update " + str_table.ToUpper() + " set \");");
            sb.AppendLine(sb_column5.ToString());
            sb.AppendLine("            strSql.Append(\" where \");");
            sb.AppendLine("            strSql.Append(\"NO_ID=" + configModel.MARK + "NO_ID\");");
            sb.AppendLine("            parameters = SetParameters(model);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 给参数列表赋值");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        /// <param name=\"model\">Model类型</param>");
            sb.AppendLine("        /// <returns>SqlParameter[]</returns>");
            sb.AppendLine("        private static SqlParameter[] SetParameters(" + str_ModelName + " model)");
            sb.AppendLine("        {");
            sb.AppendLine("            SqlParameter[] parameters;");
            sb.AppendLine("            parameters = new SqlParameter[]{");
            sb.AppendLine(sb_column3.ToString());
            sb.AppendLine("");
            sb.AppendLine(sb_column4.ToString());
            sb.AppendLine("            return parameters;");
            sb.AppendLine("        }");
            sb.AppendLine("        #endregion HelperMethod");

            #endregion HelperMethod

            sb.AppendLine("");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            CommonCode.Save(file_DAL + "/" + str_DALName + ".cs", sb.ToString());
        }
    }
}
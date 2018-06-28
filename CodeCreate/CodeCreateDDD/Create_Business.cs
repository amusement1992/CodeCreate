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
    public class Create_Business
    {
        public void Create(string file_Model, string str_nameSpace, DataTable dt_tables, string tableName)
        {
            tableName = tableName.Replace("Data_", "");


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

                if (columnType == "string")
                {
                    sb.AppendLine("            if (!filter."+ columnName + ".IsNullOrEmpty())");
                }
                else
                {
                    sb.AppendLine("            if (!filter." + columnName + ".HasValue)");

                }
                sb.AppendLine("            {");
                sb.AppendLine("                query.Equal<" + tableName + "Query>(c => c." + columnName + ", filter." + columnName + ");");
                sb.AppendLine("            }");
            }

            #endregion Model


            StringBuilder sb_body = new StringBuilder();

            sb_body.AppendLine("using BigDataAnalysis.BusinessInterface.Data;");
            sb_body.AppendLine("using BigDataAnalysis.Domain.Data.Model;");
            sb_body.AppendLine("using BigDataAnalysis.Domain.Data.Service;");
            sb_body.AppendLine("using BigDataAnalysis.DTO.Data.Query.Filter;");
            sb_body.AppendLine("using BigDataAnalysis.DTO.Data.Cmd;");
            sb_body.AppendLine("using BigDataAnalysis.DTO.Data.Query;");
            sb_body.AppendLine("using BigDataAnalysis.DTO.Query;");
            sb_body.AppendLine("using BigDataAnalysis.Query.Data;");
            sb_body.AppendLine("using Lee.Command.UnitOfWork;");
            sb_body.AppendLine("using Lee.CQuery;");
            sb_body.AppendLine("using Lee.CQuery.Paging;");
            sb_body.AppendLine("using Lee.Utility;");
            sb_body.AppendLine("using Lee.Utility.Extension;");
            sb_body.AppendLine("using System;");
            sb_body.AppendLine("using System.Collections.Generic;");
            sb_body.AppendLine("using System.Linq;");
            sb_body.AppendLine("");
            sb_body.AppendLine("namespace BigDataAnalysis.Business.Data");
            sb_body.AppendLine("{");
            sb_body.AppendLine("    /// <summary>");
            sb_body.AppendLine("    /// 业务");
            sb_body.AppendLine("    /// </summary>");
            sb_body.AppendLine("    public class " + tableName + "Business : I" + tableName + "Business");
            sb_body.AppendLine("    {");
            sb_body.AppendLine("        public " + tableName + "Business()");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 保存");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 保存");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"saveInfo\">保存信息</param>");
            sb_body.AppendLine("        /// <returns>执行结果</returns>");
            sb_body.AppendLine("        public Result<" + tableName + "Dto> Save" + tableName + "(Save" + tableName + "CmdDto saveInfo)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            if (saveInfo == null)");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                return Result<" + tableName + "Dto>.ErrorResult(\"没有指定任何要保持的信息\");");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("            using (var businessWork = UnitOfWork.Create())");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                var saveResult = " + tableName + "Service.Save" + tableName + "(saveInfo." + tableName + ".MapTo<" + tableName + ">());");
            sb_body.AppendLine("                if (!saveResult.Success)");
            sb_body.AppendLine("                {");
            sb_body.AppendLine("                    return Result<" + tableName + "Dto>.ErrorResult(saveResult.Message);");
            sb_body.AppendLine("                }");
            sb_body.AppendLine("                var commitResult = businessWork.Commit();");
            sb_body.AppendLine("                Result<" + tableName + "Dto> result = null;");
            sb_body.AppendLine("                if (commitResult.ExecutedSuccess)");
            sb_body.AppendLine("                {");
            sb_body.AppendLine("                    result = Result<" + tableName + "Dto>.SuccessResult(\"保存成功\");");
            sb_body.AppendLine("                    result.Data = saveResult.Data.MapTo<" + tableName + "Dto>();");
            sb_body.AppendLine("                }");
            sb_body.AppendLine("                else");
            sb_body.AppendLine("                {");
            sb_body.AppendLine("                    result = Result<" + tableName + "Dto>.ErrorResult(\"保存失败\");");
            sb_body.AppendLine("                }");
            sb_body.AppendLine("                return result;");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion 保存");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 获取");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 获取");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"filter\">查询条件</param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        public " + tableName + "Dto Get" + tableName + "(" + tableName + "FilterDto filter)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            var " + tableName + " = " + tableName + "Service.Get" + tableName + "(CreateQueryObject(filter));");
            sb_body.AppendLine("            return " + tableName + ".MapTo<" + tableName + "Dto>();");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion 获取");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 获取列表");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 获取列表");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"filter\">查询条件</param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        public List<" + tableName + "Dto> Get" + tableName + "List(" + tableName + "FilterDto filter)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            var " + tableName + "List = " + tableName + "Service.Get" + tableName + "List(CreateQueryObject(filter));");
            sb_body.AppendLine("            return " + tableName + "List.Select(c => c.MapTo<" + tableName + "Dto>()).ToList();");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion 获取列表");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 获取分页");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 获取分页");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"filter\">查询条件</param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        public IPaging<" + tableName + "Dto> Get" + tableName + "Paging(" + tableName + "FilterDto filter)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            var " + tableName + "Paging = " + tableName + "Service.Get" + tableName + "Paging(CreateQueryObject(filter));");
            sb_body.AppendLine("            return " + tableName + "Paging.Convert<" + tableName + ", " + tableName + "Dto>();");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion 获取分页");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 删除");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 删除");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"deleteInfo\">删除信息</param>");
            sb_body.AppendLine("        /// <returns>执行结果</returns>");
            sb_body.AppendLine("        public Result Delete" + tableName + "(Delete" + tableName + "CmdDto deleteInfo)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            #region 参数判断");
            sb_body.AppendLine("");
            sb_body.AppendLine("            if (deleteInfo == null || deleteInfo." + tableName + "Ids.IsNullOrEmpty())");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                return Result.ErrorResult(\"没有指定要删除的\");");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("");
            sb_body.AppendLine("            #endregion 参数判断");
            sb_body.AppendLine("");
            sb_body.AppendLine("            using (var businessWork = UnitOfWork.Create())");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                var " + tableName + "s = deleteInfo." + tableName + "Ids.Select(c => " + tableName + ".Create" + tableName + "(c));");
            sb_body.AppendLine("                var deleteResult = " + tableName + "Service.Delete" + tableName + "(" + tableName + "s);");
            sb_body.AppendLine("                if (!deleteResult.Success)");
            sb_body.AppendLine("                {");
            sb_body.AppendLine("                    return deleteResult;");
            sb_body.AppendLine("                }");
            sb_body.AppendLine("                var commitResult = businessWork.Commit();");
            sb_body.AppendLine("                return commitResult.ExecutedSuccess ? Result.SuccessResult(\"删除成功\") : Result.ErrorResult(\"删除失败\");");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion 删除");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 根据查询条件生成查询对象");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 根据查询条件生成查询对象");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"filter\">查询条件</param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        private IQuery CreateQueryObject(" + tableName + "FilterDto filter)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            if (filter == null)");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                return null;");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("            IQuery query = QueryFactory.Create<" + tableName + "Query>(filter);");
            sb_body.AppendLine("            if (!filter.SysNos.IsNullOrEmpty())");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                query.In<" + tableName + "Query>(c => c.SysNo, filter.SysNos);");
            sb_body.AppendLine("            }");


            sb_body.AppendLine(sb.ToString());


            sb_body.AppendLine("            return query;");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion 根据查询条件生成查询对象");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("}");
            sb_body.AppendLine("");



            file_Model = "C:\\Code\\BigDataAnalysis.Business\\Data";
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }
            CommonCode.Save(file_Model + "/" + tableName + "Business.cs", sb_body.ToString());
        }

    }
}
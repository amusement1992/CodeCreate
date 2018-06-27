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
    public class Create_DomainService
    {
        public void Create(string file_Model, string str_nameSpace, DataTable dt_tables, string tableName)
        {
            tableName = tableName.Replace("Data_", "");
            StringBuilder sb_body = new StringBuilder();
            
            sb_body.AppendLine("using Lee.CQuery;");
            sb_body.AppendLine("using Lee.CQuery.Paging;");
            sb_body.AppendLine("using Lee.Utility;");
            sb_body.AppendLine("using Lee.Utility.IoC;");
            sb_body.AppendLine("using System;");
            sb_body.AppendLine("using System.Collections.Generic;");
            sb_body.AppendLine("using System.Linq;");
            sb_body.AppendLine("using System.Text;");
            sb_body.AppendLine("using System.Threading.Tasks;");
            sb_body.AppendLine("using Lee.Utility.Extension;");
            sb_body.AppendLine("using BigDataAnalysis.Domain.Data.Repository;");
            sb_body.AppendLine("using BigDataAnalysis.Domain.Data.Model;");
            sb_body.AppendLine("");
            sb_body.AppendLine("namespace BigDataAnalysis.Domain.Data.Service");
            sb_body.AppendLine("{");
            sb_body.AppendLine("    /// <summary>");
            sb_body.AppendLine("    /// 服务");
            sb_body.AppendLine("    /// </summary>");
            sb_body.AppendLine("    public static class " + tableName + "Service");
            sb_body.AppendLine("    {");
            sb_body.AppendLine("        static I" + tableName + "Repository " + tableName + "Repository = ContainerManager.Container.Resolve<I" + tableName + "Repository>();");
            sb_body.AppendLine("        ");
            sb_body.AppendLine("        #region 保存");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 保存");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"" + tableName + "\">信息</param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        public static Result<" + tableName + "> Save" + tableName + "(" + tableName + " " + tableName + ")");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            return null;");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 获取");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 获取");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"query\">查询条件</param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        public static " + tableName + " Get" + tableName + "(IQuery query)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            var " + tableName + " = " + tableName + "Repository.Get(query);");
            sb_body.AppendLine("            return " + tableName + ";");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 获取列表");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 获取列表");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"query\">查询条件</param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        public static List<" + tableName + "> Get" + tableName + "List(IQuery query)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            var " + tableName + "List = " + tableName + "Repository.GetList(query);");
            sb_body.AppendLine("            return " + tableName + "List;");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 获取分页");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 获取分页");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"query\">查询条件</param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        public static IPaging<" + tableName + "> Get" + tableName + "Paging(IQuery query)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            var " + tableName + "Paging = " + tableName + "Repository.GetPaging(query);");
            sb_body.AppendLine("            return " + tableName + "Paging;");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 删除");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 删除");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"" + tableName + "s\">要删出的信息</param>");
            sb_body.AppendLine("        /// <returns>执行结果</returns>");
            sb_body.AppendLine("        public static Result Delete" + tableName + "(IEnumerable<" + tableName + "> " + tableName + "s)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            #region 参数判断");
            sb_body.AppendLine("");
            sb_body.AppendLine("            if (" + tableName + "s.IsNullOrEmpty())");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                return Result.ErrorResult(\"没有指定要删除的\");");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("");
            sb_body.AppendLine("            #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("            //删除逻辑");
            sb_body.AppendLine("            return Result.SuccessResult(\"删除成功\");");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("}");
            
            file_Model = "C:\\Code\\BigDataAnalysis.Domain\\Data\\Service";
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }
            CommonCode.Save(file_Model + "/" + tableName + "Service.cs", sb_body.ToString());
        }

    }
}
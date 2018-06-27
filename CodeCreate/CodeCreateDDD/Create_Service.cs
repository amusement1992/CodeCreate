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
    public class Create_Service
    {
        public void Create(string file_Model, string str_nameSpace, DataTable dt_tables, string tableName)
        {
            tableName = tableName.Replace("Data_", "");
            StringBuilder sb_body = new StringBuilder();

            sb_body.AppendLine("using Lee.CQuery;");
            sb_body.AppendLine("using Lee.DTO.Data.Query;");
            sb_body.AppendLine("using Lee.DTO.Data.Query.Filter;");
            sb_body.AppendLine("using Lee.CQuery.Paging;");
            sb_body.AppendLine("using BigDataAnalysis.ServiceInterface.Data;");
            sb_body.AppendLine("using System;");
            sb_body.AppendLine("using System.Collections.Generic;");
            sb_body.AppendLine("using System.Linq;");
            sb_body.AppendLine("using System.Text;");
            sb_body.AppendLine("using System.Threading.Tasks;");
            sb_body.AppendLine("using Lee.Utility;");
            sb_body.AppendLine("using BigDataAnalysis.BusinessInterface.Data;");
            sb_body.AppendLine("using BigDataAnalysis.DTO.Data.Query;");
            sb_body.AppendLine("using BigDataAnalysis.DTO.Data.Cmd;");
            sb_body.AppendLine("using BigDataAnalysis.DTO.Data.Query.Filter;");
            sb_body.AppendLine("");
            sb_body.AppendLine("namespace BigDataAnalysis.Service.Data");
            sb_body.AppendLine("{");
            sb_body.AppendLine("    /// <summary>");
            sb_body.AppendLine("    /// 服务");
            sb_body.AppendLine("    /// </summary>");
            sb_body.AppendLine("    public class " + tableName + "Service : I" + tableName + "Service");
            sb_body.AppendLine("    {");
            sb_body.AppendLine("        I" + tableName + "Business " + tableName + "Business = null;");
            sb_body.AppendLine("");
            sb_body.AppendLine("        public " + tableName + "Service(I" + tableName + "Business " + tableName + "Business)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            this." + tableName + "Business = " + tableName + "Business;");
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
            sb_body.AppendLine("            return " + tableName + "Business.Save" + tableName + "(saveInfo);");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
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
            sb_body.AppendLine("            return " + tableName + "Business.Get" + tableName + "(filter);");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
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
            sb_body.AppendLine("            return " + tableName + "Business.Get" + tableName + "List(filter);");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
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
            sb_body.AppendLine("            return " + tableName + "Business.Get" + tableName + "Paging(filter);");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
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
            sb_body.AppendLine("            return " + tableName + "Business.Delete" + tableName + "(deleteInfo);");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("}");



            file_Model = "C:\\Code\\BigDataAnalysis.Service\\Data";
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }
            CommonCode.Save(file_Model + "/" + tableName + "Service.cs", sb_body.ToString());
        }

    }
}
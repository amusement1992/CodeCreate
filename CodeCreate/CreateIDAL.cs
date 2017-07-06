using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeCreate
{
    public class CreateIDAL
    {

        public void Create(string file_IDAL, string str_nameSpace, string str_IDALName, string str_ModelName)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using " + str_nameSpace + ".Model;");
            sb.AppendLine("");
            sb.AppendLine("namespace " + str_nameSpace + ".IDAL");
            sb.AppendLine("{");
            sb.AppendLine("    public interface " + str_IDALName);
            sb.AppendLine("    {");

            #region BasicMethod
            sb.AppendLine("        #region BasicMethod");
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// 是否存在该记录");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		bool Exists(string ST_CODE);");
            sb.AppendLine("");

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// 增加一条数据");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		bool Add(" + str_ModelName + " model);");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 增加多条数据");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        int AddList(List<" + str_ModelName + "> list_model);");
            sb.AppendLine("");

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// 更新一条数据");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		bool Update(" + str_ModelName + " model);");
            sb.AppendLine("");

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// 更新多条数据");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		int UpdateList(List<" + str_ModelName + "> list_model);");
            sb.AppendLine("");

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// 删除一条数据");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		bool Delete(string ST_CODE);");
            sb.AppendLine("");

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// 删除多条数据");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		bool DeleteList(List<string> ST_CODElist);");
            sb.AppendLine("");

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// 得到实体对象");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		" + str_ModelName + " GetModel(string ST_CODE);");
            sb.AppendLine("");

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// 得到实体列表");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("        List<" + str_ModelName + "> GetModelList(string strWhere);");
            sb.AppendLine("");

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// 获得数据列表");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		DataTable GetList(string strWhere);");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 获取记录总数");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        int GetRecordCount(string strWhere);");
            sb.AppendLine("");

            sb.AppendLine("		///// <summary>");
            sb.AppendLine("		///// 分页获取数据列表");
            sb.AppendLine("		///// </summary>");
            sb.AppendLine("		//public int GetRecordCount(string strWhere);");
            sb.AppendLine("");

            sb.AppendLine("		///// <summary>");
            sb.AppendLine("		///// 根据分页获得数据列表");
            sb.AppendLine("		///// </summary>");
            sb.AppendLine("		//DataTable GetList(int PageSize,int PageIndex,string strWhere);");
            sb.AppendLine("		#endregion  BasicMethod");
            #endregion

            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            CommonCode.Save(file_IDAL + "/" + str_IDALName + ".cs", sb.ToString());
        }

    }
}

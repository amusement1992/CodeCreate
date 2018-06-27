using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeCreate
{
    public class CreateBLL
    {
        public void Create(string file_BLL, string str_nameSpace, string str_table, string str_BLLName, string str_IDALName, string str_ModelName)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using " + str_nameSpace + ".Model;");
            sb.AppendLine("using " + str_nameSpace + ".IDAL;");
            sb.AppendLine("using " + str_nameSpace + ".DALFactory;");
            sb.AppendLine("");
            sb.AppendLine("namespace " + str_nameSpace + ".BLL");
            sb.AppendLine("{");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// " + str_BLLName);
            sb.AppendLine("    /// </summary>");
            sb.AppendLine("    public partial class " + str_BLLName);
            sb.AppendLine("    {");
            sb.AppendLine("        private readonly " + str_IDALName + " dal = DataAccess.Create" + str_table.ToUpper() + "();");
            sb.AppendLine("        public " + str_BLLName + "() { }");
            sb.AppendLine("");

            #region BasicMethod
            sb.AppendLine("        #region  BasicMethod");
            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 是否存在该记录");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public bool Exists(string ST_CODE)");
            sb.AppendLine("        {");
            sb.AppendLine("            return dal.Exists(ST_CODE);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 增加一条数据");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public bool Add(" + str_ModelName + " model)");
            sb.AppendLine("        {");
            sb.AppendLine("            return dal.Add(model);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 增加多条数据");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public int AddList(List<" + str_ModelName + "> list_model)");
            sb.AppendLine("        {");
            sb.AppendLine("            return dal.AddList(list_model);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 更新一条数据");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public bool Update(" + str_ModelName + " model)");
            sb.AppendLine("        {");
            sb.AppendLine("            return dal.Update(model);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 更新多条数据");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public int UpdateList(List<" + str_ModelName + "> list_model)");
            sb.AppendLine("        {");
            sb.AppendLine("            return dal.UpdateList(list_model);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 删除一条数据");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public bool Delete(string ST_CODE)");
            sb.AppendLine("        {");
            sb.AppendLine("            return dal.Delete(ST_CODE);");
            sb.AppendLine("        }");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 删除多条数据");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public bool DeleteList(List<string> ST_CODElist)");
            sb.AppendLine("        {");
            sb.AppendLine("            return dal.DeleteList(ST_CODElist);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 得到实体对象");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public " + str_ModelName + " GetModel(string ST_CODE)");
            sb.AppendLine("        {");
            sb.AppendLine("            return dal.GetModel(ST_CODE);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 获得实体列表");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public List<" + str_ModelName + "> GetModelList(string strWhere)");
            sb.AppendLine("        {");
            sb.AppendLine("            return dal.GetModelList(strWhere);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 获得数据列表");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public DataTable GetList(string strWhere)");
            sb.AppendLine("        {");
            sb.AppendLine("            return dal.GetList(strWhere);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 获得数据列表");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public DataTable GetAllList()");
            sb.AppendLine("        {");
            sb.AppendLine("            return GetList(\"\");");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 获取记录总数");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public int GetRecordCount(string strWhere)");
            sb.AppendLine("        {");
            sb.AppendLine("            return dal.GetRecordCount(strWhere);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        ///// <summary>");
            sb.AppendLine("        ///// 分页获取数据列表");
            sb.AppendLine("        ///// </summary>");
            sb.AppendLine("        //public int GetRecordCount(string strWhere)");
            sb.AppendLine("        //{");
            sb.AppendLine("        //    return dal.GetRecordCount(strWhere);");
            sb.AppendLine("        //}");
            sb.AppendLine("");

            sb.AppendLine("        ///// <summary>");
            sb.AppendLine("        ///// 分页获取数据列表");
            sb.AppendLine("        ///// </summary>");
            sb.AppendLine("        //public DataTable GetList(int PageSize,int PageIndex,string strWhere)");
            sb.AppendLine("        //{");
            sb.AppendLine("        //return dal.GetList(PageSize,PageIndex,strWhere);");
            sb.AppendLine("        //}");
            sb.AppendLine("");
            sb.AppendLine("        #endregion  BasicMethod");
            #endregion

            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            CommonCode.Save(file_BLL + "/" + str_BLLName + ".cs", sb.ToString());
        }
    }
}

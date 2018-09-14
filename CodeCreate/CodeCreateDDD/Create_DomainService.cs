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
    public class Create_DomainService
    {
        public void Create(string str_nameSpace, DataTable dt_tables, string tableName)
        {
            string tablePrefix = CommonCode.GetTablePrefix(tableName);
            tableName = CommonCode.GetTableName(tableName);
            string tableDesc = CommonCode.GetTableDesc(tableName);

            StringBuilder sb_body = new StringBuilder();

            StringBuilder sb_List = new StringBuilder();
            StringBuilder sb_AllowLoad = new StringBuilder();
            StringBuilder sb_SetData = new StringBuilder();

            var listModel = CommonCode.GetTableModel(tableName);
            if (listModel != null)
            {
                foreach (var item in listModel)
                {
                    SetInstance(tableName, sb_List, sb_AllowLoad, sb_SetData, item);
                }
            }

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
            sb_body.AppendLine("using Lee.Utility.ExpressionUtil;");
            sb_body.AppendLine("using " + str_nameSpace + ".Domain." + tablePrefix + ".Repository;");
            sb_body.AppendLine("using " + str_nameSpace + ".Domain." + tablePrefix + ".Model;");
            sb_body.AppendLine("using " + str_nameSpace + ".Query." + tablePrefix + ";");
            sb_body.AppendLine("using " + str_nameSpace + ".Domain.Model;");
            sb_body.AppendLine("using " + str_nameSpace + ".Domain.Service;");
            sb_body.AppendLine("using " + str_nameSpace + ".Domain.Data.Model;");
            sb_body.AppendLine("using " + str_nameSpace + ".Domain.Data.Service;");
            sb_body.AppendLine("");
            sb_body.AppendLine("namespace " + str_nameSpace + ".Domain." + tablePrefix + ".Service");
            sb_body.AppendLine("{");
            sb_body.AppendLine("    /// <summary>");
            sb_body.AppendLine("    /// 服务：" + tableDesc);
            sb_body.AppendLine("    /// </summary>");
            sb_body.AppendLine("    public static class " + tableName + "Service");
            sb_body.AppendLine("    {");
            sb_body.AppendLine("        static I" + tableName + "Repository " + tableName + "Repository = ContainerManager.Container.Resolve<I" + tableName + "Repository>();");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 保存");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 保存" + tableDesc);
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"" + tableName + "\">信息</param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        public static Result<" + tableName + "> Save" + tableName + "(" + tableName + " " + tableName + ")");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            if (" + tableName + " == null)");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                return Result<" + tableName + ">.ErrorResult(\"信息为空\");");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("            Result<" + tableName + "> result = null;");
            sb_body.AppendLine("            if (" + tableName + ".SysNo == Guid.Empty)");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                result = Add" + tableName + "(" + tableName + ");");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("            else");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                result = Update" + tableName + "(" + tableName + ");");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("");
            sb_body.AppendLine("            return result;");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 批量保存" + tableDesc);
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"" + tableName + "\">信息</param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        public static Result SaveList" + tableName + "(IEnumerable<" + tableName + "> List" + tableName + ")");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            if (List" + tableName + ".IsNullOrEmpty())");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                return Result.ErrorResult(\"信息为空\");");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("");
            sb_body.AppendLine("            foreach (var item in List" + tableName + ")");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                Save" + tableName + "(item);");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("");
            sb_body.AppendLine("            return Result.SuccessResult(\"修改成功\");");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 添加");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 添加" + tableDesc);
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"" + tableName + "\">信息</param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        static Result<" + tableName + "> Add" + tableName + "(" + tableName + " " + tableName + ")");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            " + tableName + ".SetDefaultValue();");
            sb_body.AppendLine("            " + tableName + ".Save();");
            sb_body.AppendLine("            var result = Result<" + tableName + ">.SuccessResult(\"保存成功\");");
            sb_body.AppendLine("            result.Data = " + tableName + ";");
            sb_body.AppendLine("            return result;");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 修改");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 修改" + tableDesc);
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"" + tableName + "\">信息</param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        static Result<" + tableName + "> Update" + tableName + "(" + tableName + " " + tableName + ")");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            var this" + tableName + " = Get" + tableName + "(" + tableName + ".SysNo);");
            sb_body.AppendLine("            if (this" + tableName + " == null)");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                return Result<" + tableName + ">.ErrorResult(\"请指定要编辑的数据\");");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("            var excludeModifyPropertys = ExpressionHelper.GetExpressionPropertyNames<" + tableName + ">(u => u.CreateDate);");
            sb_body.AppendLine("");
            sb_body.AppendLine("            this" + tableName + ".ModifyFromOther" + tableName + "(" + tableName + ", excludeModifyPropertys);//更新");
            sb_body.AppendLine("            this" + tableName + ".Save();");
            sb_body.AppendLine("            var result = Result<" + tableName + ">.SuccessResult(\"更新成功\");");
            sb_body.AppendLine("            result.Data = this" + tableName + ";");
            sb_body.AppendLine("            return result;");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 获取");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 获取" + tableDesc);
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"query\">查询条件</param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        public static " + tableName + " Get" + tableName + "(IQuery query)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            var " + tableName + " = " + tableName + "Repository.Get(query);");
            sb_body.AppendLine("            return " + tableName + ";");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 获取" + tableDesc);
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"SysNo\">编号</param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        public static " + tableName + " Get" + tableName + "(Guid? SysNo)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            return Get" + tableName + "(SysNo ?? Guid.Empty);");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 获取" + tableDesc);
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"SysNo\">编号</param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        public static " + tableName + " Get" + tableName + "(Guid SysNo)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            if (SysNo == Guid.Empty)");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                return null;");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("            IQuery query = QueryFactory.Create<" + tableName + "Query>(c => c.SysNo == SysNo);");
            sb_body.AppendLine("            return Get" + tableName + "(query);");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 获取列表");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 获取" + tableDesc + "列表");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"query\">查询条件</param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        public static List<" + tableName + "> Get" + tableName + "List(IQuery query)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            var " + tableName + "List = " + tableName + "Repository.GetList(query);");
            sb_body.AppendLine("            " + tableName + "List = LoadOtherObjectData(" + tableName + "List, query);");
            sb_body.AppendLine("            return " + tableName + "List;");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 获取" + tableDesc + "列表");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"listID\"></param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        public static List<" + tableName + "> Get" + tableName + "List(IEnumerable<Guid> listID)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            if (listID.IsNullOrEmpty())");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                return new List<" + tableName + ">(0);");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("            IQuery query = QueryFactory.Create<" + tableName + "Query>(c => listID.Contains(c.SysNo));");
            sb_body.AppendLine("            return Get" + tableName + "List(query);");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 获取分页");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 获取" + tableDesc + "分页");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"query\">查询条件</param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        public static IPaging<" + tableName + "> Get" + tableName + "Paging(IQuery query)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            var " + tableName + "Paging = " + tableName + "Repository.GetPaging(query);");
            sb_body.AppendLine("            var list = LoadOtherObjectData(" + tableName + "Paging, query);");
            sb_body.AppendLine("            return new Paging<" + tableName + ">(" + tableName + "Paging.Page, " + tableName + "Paging.PageSize, " + tableName + "Paging.TotalCount, list);");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 删除");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 删除" + tableDesc);
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"" + tableName + "s\">要删除的信息</param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        public static Result Delete" + tableName + "(IEnumerable<" + tableName + "> " + tableName + "s)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            if (" + tableName + "s.IsNullOrEmpty())");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                return Result.ErrorResult(\"没有指定任何要删除的数据\");");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("            " + tableName + "Repository.Remove(" + tableName + "s.ToArray());");
            sb_body.AppendLine("            return Result.SuccessResult(\"删除成功\");");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 删除" + tableDesc);
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"" + tableName + "Ids\">编号</param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        public static Result Delete" + tableName + "(IEnumerable<Guid> " + tableName + "Ids)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            if (" + tableName + "Ids.IsNullOrEmpty())");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                return Result.ErrorResult(\"没有指定要删除的数据\");");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("            IEnumerable<" + tableName + "> " + tableName + "s = " + tableName + "Ids.Select(c => " + tableName + ".Create" + tableName + "(c)).ToList();");
            sb_body.AppendLine("            return Delete" + tableName + "(" + tableName + "s);");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("        ");
            sb_body.AppendLine("        #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 加载其它数据");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 加载其它数据");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"" + tableName + "s\">数据</param>");
            sb_body.AppendLine("        /// <param name=\"query\">筛选条件</param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        private static List<" + tableName + "> LoadOtherObjectData(IEnumerable<" + tableName + "> " + tableName + "s, IQuery query)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            if (" + tableName + "s.IsNullOrEmpty())");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                return new List<" + tableName + ">(0);");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("            if (query == null)");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                return " + tableName + "s.ToList();");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("");
            sb_body.AppendLine("            #region 获取数据");
            sb_body.AppendLine("");
            sb_body.AppendLine(sb_List.ToString());
            sb_body.AppendLine(sb_AllowLoad.ToString());
            sb_body.AppendLine("");
            sb_body.AppendLine("            #endregion");
            sb_body.AppendLine("");
            sb_body.AppendLine("            foreach (var item in " + tableName + "s)");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                if (item == null)");
            sb_body.AppendLine("                {");
            sb_body.AppendLine("                    continue;");
            sb_body.AppendLine("                }");
            sb_body.AppendLine("");
            sb_body.AppendLine(sb_SetData.ToString());
            sb_body.AppendLine("            }");
            sb_body.AppendLine("");
            sb_body.AppendLine("            return " + tableName + "s.ToList();");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion");

            sb_body.AppendLine("");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("}");
            sb_body.AppendLine("");


            string file_Model = "C:\\Code\\" + str_nameSpace + ".Domain\\" + tablePrefix + "\\Service";
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }
            CommonCode.Save(file_Model + "/" + tableName + "Service.cs", sb_body.ToString());
        }

        private static void SetInstance(string tableName, StringBuilder sb_List, StringBuilder sb_AllowLoad, StringBuilder sb_SetData, TableModel tableModel)
        {
            if (tableModel.List != null)
            {
                foreach (var thisModel in tableModel.List.Where(d => !string.IsNullOrEmpty(d.NewColumnName)))
                {
                    sb_List.AppendLine("            List<" + thisModel.NewColumnType + "> list" + thisModel.NewColumnName + " = null;");


                    sb_AllowLoad.AppendLine("            if (query.AllowLoad<" + tableName + ">(c => c." + thisModel.NewColumnName + "))");
                    sb_AllowLoad.AppendLine("            {");
                    if (thisModel.ColumnType == "Guid")
                    {
                        sb_AllowLoad.AppendLine("                var listID_" + thisModel.NewColumnName + " = " + tableName + "s.Select(c => c." + thisModel.NewColumnName + "ID).Distinct().ToList();");
                    }
                    else
                    {
                        sb_AllowLoad.AppendLine("                var listID_" + thisModel.NewColumnName + " = " + tableName + "s.Select(c => c." + thisModel.NewColumnName + "ID ?? Guid.Empty).Distinct().ToList();");
                    }
                    sb_AllowLoad.AppendLine("                list" + thisModel.NewColumnName + " = " + thisModel.NewColumnType + "Service.Get" + thisModel.NewColumnType + "List(listID_" + thisModel.NewColumnName + ");");
                    sb_AllowLoad.AppendLine("            }");


                    sb_SetData.AppendLine("                if (!list" + thisModel.NewColumnName + ".IsNullOrEmpty())");
                    sb_SetData.AppendLine("                {");
                    sb_SetData.AppendLine("                    item.Set" + thisModel.NewColumnName + "(list" + thisModel.NewColumnName + ".FirstOrDefault(c => c.SysNo == item." + thisModel.NewColumnName + "ID));");
                    sb_SetData.AppendLine("                }");

                }
            }
        }

    }
}
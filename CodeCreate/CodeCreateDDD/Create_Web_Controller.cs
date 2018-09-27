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
    public class Create_Web_Controller
    {
        public void Create(string str_nameSpace, DataTable dt_tables, string tableName )
        {
            string tablePrefix = CommonCode.GetTablePrefix(tableName);
            tableName = CommonCode.GetTableName(tableName);
            string tableDesc = CommonCode.GetTableDesc(tableName);

            bool isPrimeKey = false;
            string primaryKey = "";

            StringBuilder sb = new StringBuilder();

            StringBuilder sb_load = new StringBuilder();
            var listModel = CommonCode.GetTableModel(tableName);
            if (listModel != null)
            {
                foreach (var item in listModel)
                {
                    SetLoad(sb_load, item);
                }
            }

            StringBuilder sb_body = new StringBuilder();
            sb_body.AppendLine("using Lee.Web.Mvc;");
            sb_body.AppendLine("using System;");
            sb_body.AppendLine("using System.Collections.Generic;");
            sb_body.AppendLine("using System.Linq;");
            sb_body.AppendLine("using System.Web;");
            sb_body.AppendLine("using System.Web.Mvc;");
            sb_body.AppendLine("using Lee.Utility.Extension;");
            sb_body.AppendLine("using Lee.Utility.Serialize;");
            sb_body.AppendLine("using Lee.CQuery;");
            sb_body.AppendLine("using Lee.CQuery.Paging;");
            sb_body.AppendLine("using Lee.Utility;");
            sb_body.AppendLine("using Lee.Application.Identity.User;");
            sb_body.AppendLine("using Lee.Application.Identity.Auth;");
            sb_body.AppendLine("using " + str_nameSpace + ".Web.Controllers.Base;");
            sb_body.AppendLine("using " + str_nameSpace + ".ServiceInterface;");
            sb_body.AppendLine("using " + str_nameSpace + ".ViewModel;");
            sb_body.AppendLine("using " + str_nameSpace + ".DTO.Cmd;");
            sb_body.AppendLine("using " + str_nameSpace + ".DTO.Query.Filter;");
            sb_body.AppendLine("using " + str_nameSpace + ".ViewModel.Common;");
            sb_body.AppendLine("using " + str_nameSpace + ".ViewModel.Filter;");
            sb_body.AppendLine("using " + str_nameSpace + ".DTO.Query;");
            sb_body.AppendLine("using " + str_nameSpace + ".ServiceInterface." + tablePrefix + ";");
            sb_body.AppendLine("using Lee.Utility.IoC;");
            sb_body.AppendLine("using " + str_nameSpace + ".DTO." + tablePrefix + ".Query.Filter;");
            sb_body.AppendLine("using " + str_nameSpace + ".DTO." + tablePrefix + ".Query;");
            sb_body.AppendLine("using " + str_nameSpace + ".ViewModel." + tablePrefix + ";");
            sb_body.AppendLine("using " + str_nameSpace + ".ViewModel." + tablePrefix + ".Filter;");
            sb_body.AppendLine("using " + str_nameSpace + ".DTO." + tablePrefix + ".Cmd;");
            sb_body.AppendLine("using " + str_nameSpace + ".Web.Helper;");
            sb_body.AppendLine("");
            sb_body.AppendLine("namespace " + str_nameSpace + ".Web.Controllers");
            sb_body.AppendLine("{");
            sb_body.AppendLine("    /// <summary>");
            sb_body.AppendLine("    /// Controller：" + tableDesc);
            sb_body.AppendLine("    /// </summary>");

            sb_body.AppendLine("    public partial class " + tablePrefix + "Controller : WebBaseController");
            sb_body.AppendLine("    {");
            sb_body.AppendLine("        I" + tableName + "Service " + tableName + "Service = ContainerManager.Container.Resolve<I" + tableName + "Service>();");
            sb_body.AppendLine("        //Guid LoginUserId = UserHelper.GetLoginUserId();");
            sb_body.AppendLine("");
            //sb_body.AppendLine("        public " + tableName + "Controller() { }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region " + tableDesc + "管理");
            sb_body.AppendLine("        public ActionResult " + tableName + "()");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            return View();");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// " + tableDesc + "列表");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"filter\"></param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        [HttpGet]");
            sb_body.AppendLine("        public JsonResult Get" + tableName + "Paging(" + tableName + "FilterViewModel filter)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("");
            sb_body.AppendLine("            filter.PageSize = filter.rows;");
            sb_body.AppendLine(sb_load.ToString());
            sb_body.AppendLine("");
            sb_body.AppendLine("            var pager = " + tableName + "Service.Get" + tableName + "Paging(filter.MapTo<" + tableName + "FilterDto>()).Convert<" + tableName + "Dto, " + tableName + "ViewModel>();");
            sb_body.AppendLine("            object objResult = new");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                total = pager.TotalCount,");
            sb_body.AppendLine("                rows = pager,");
            sb_body.AppendLine("            };");
            sb_body.AppendLine("            return Json(objResult, JsonRequestBehavior.AllowGet);");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 下拉框：" + tableDesc + "列表");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"filter\"></param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        [HttpGet]");
            sb_body.AppendLine("        public JsonResult Get" + tableName + "List(" + tableName + "FilterViewModel filter)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            var list = " + tableName + "Service.Get" + tableName + "List(filter.MapTo<" + tableName + "FilterDto>());");
            sb_body.AppendLine("            list.Insert(0, new " + tableName + "Dto { SysNo = Guid.Empty, " + tableName + "Name = \"\" });");
            sb_body.AppendLine("            var result = list.Select(d => new { d.SysNo, d." + tableName + "Name });");
            sb_body.AppendLine("            return Json(result, JsonRequestBehavior.AllowGet);");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 添加、修改" + tableDesc);
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"user\"></param>");
            sb_body.AppendLine("        /// <param name=\"userRoles\"></param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        [HttpPost]");
            sb_body.AppendLine("        public JsonResult Edit" + tableName + "(" + tableName + "ViewModel vm)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            #region 验证是否存在");
            sb_body.AppendLine("");
            sb_body.AppendLine("            var model = " + tableName + "Service.Get" + tableName + "(new " + tableName + "FilterDto()");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                " + tableName + "Name = vm." + tableName + "Name,");
            sb_body.AppendLine("            });");
            sb_body.AppendLine("            if (model != null && (vm.SysNo == Guid.Empty || model.SysNo != vm.SysNo))");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                var result2 = Result<" + tableName + "ViewModel>.ErrorResult(\"" + tableDesc + "名称已存在！\");");
            sb_body.AppendLine("                return Json(result2);");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("");
            sb_body.AppendLine("            #endregion 验证是否存在");

            sb_body.AppendLine("            vm.UpdateDate = DateTime.Now;");
            sb_body.AppendLine("            vm.UpdateUserID = LoginUserId;");
            sb_body.AppendLine("            if (vm.SysNo == Guid.Empty)");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                vm.CreateUserID = LoginUserId;");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("            var saveInfo = new Save" + tableName + "CmdDto()");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                " + tableName + " = vm.MapTo<" + tableName + "CmdDto>()");
            sb_body.AppendLine("            };");
            sb_body.AppendLine("            var result = " + tableName + "Service.Save" + tableName + "(saveInfo);");
            sb_body.AppendLine("            return Json(result);");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 批量添加、修改" + tableDesc);
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"user\"></param>");
            sb_body.AppendLine("        /// <param name=\"userRoles\"></param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        [HttpPost]");
            sb_body.AppendLine("        public JsonResult EditList" + tableName + "(List<" + tableName + "ViewModel> list_vm)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            list_vm.ForEach(d => d.UpdateDate = DateTime.Now);");
            sb_body.AppendLine("            list_vm.ForEach(d => d.UpdateUserID = LoginUserId);");
            sb_body.AppendLine("            list_vm.Where(d => d.SysNo == Guid.Empty).ToList().ForEach(d => d.CreateUserID = LoginUserId);");
            sb_body.AppendLine("            list_vm.Where(d => d.SysNo == Guid.Empty).ToList().ForEach(d => d.CreateDate = DateTime.Now);");
            sb_body.AppendLine("");
            sb_body.AppendLine("            Result result = " + tableName + "Service.SaveList" + tableName + "(new Save" + tableName + "CmdDto()");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                List" + tableName + " = list_vm.Select(d => d.MapTo<" + tableName + "CmdDto>()).ToList(),");
            sb_body.AppendLine("            });");
            sb_body.AppendLine("            return Json(result);");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 删除" + tableDesc);
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"user\"></param>");
            sb_body.AppendLine("        /// <param name=\"userRoles\"></param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        [HttpPost]");
            sb_body.AppendLine("        public JsonResult Delete" + tableName + "(Delete" + tableName + "CmdDto vm)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            vm.UpdateUserID = LoginUserId;");
            sb_body.AppendLine("            vm.IsRealDeleted = false;");
            sb_body.AppendLine("            var result = " + tableName + "Service.Delete" + tableName + "(vm);");
            sb_body.AppendLine("            return Json(result);");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion " + tableDesc + "管理");
            sb_body.AppendLine("");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("}");


            string file_Model = "C:\\Code\\" + str_nameSpace + ".Web\\Controllers\\" + tablePrefix;
          
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }
            CommonCode.Save(file_Model + "/" + tableName + "Controller.cs", sb_body.ToString());
        }


        private static void SetLoad(StringBuilder sb_load, TableModel tableModel)
        {
            if (tableModel.List != null)
            {
                foreach (var thisModel in tableModel.List.Where(d => !string.IsNullOrEmpty(d.NewColumnName)))
                {
                    sb_load.AppendLine("            filter.IsLoad" + thisModel.NewColumnName + " = true;");
                }
            }
        }

    }
}
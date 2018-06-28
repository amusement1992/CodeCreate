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
        public void Create(string str_nameSpace, DataTable dt_tables, string tableName)
        {
            string tablePrefix = CommonCode.GetTablePrefix(tableName); tableName = CommonCode.GetTableName(tableName);
            string tableDesc = "";
            switch (tableName)
            {
                case "Brand":
                    tableDesc = "品牌";
                    break;
                case "Category":
                    tableDesc = "品类";
                    break;
                case "Platform":
                    tableDesc = "" + tableDesc + "";
                    break;
                case "Company":
                    tableDesc = "公司";
                    break;
                case "Shop":
                    tableDesc = "店铺";
                    break;
                default:
                    tableDesc = "【名称】";
                    break;
            }

            bool isPrimeKey = false;
            string primaryKey = "";

            StringBuilder sb = new StringBuilder();
             

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
            sb_body.AppendLine("");
            sb_body.AppendLine("namespace " + str_nameSpace + ".Web.Controllers");
            sb_body.AppendLine("{");
            sb_body.AppendLine("    public class " + tableName + "Controller : WebBaseController");
            sb_body.AppendLine("    {");
            sb_body.AppendLine("        I" + tableName + "Service " + tableName + "Service = ContainerManager.Container.Resolve<I" + tableName + "Service>();");
            sb_body.AppendLine("");
            sb_body.AppendLine("        public " + tableName + "Controller() { }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region " + tableDesc + "管理");
            sb_body.AppendLine("        public ActionResult " + tableName + "()");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            return View();");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 列表");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"filter\"></param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        [HttpGet]");
            sb_body.AppendLine("        public JsonResult Get" + tableName + "Paging(" + tableName + "FilterViewModel filter)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("");
            sb_body.AppendLine("            filter.PageSize = filter.rows;");
            sb_body.AppendLine("");
            sb_body.AppendLine("            var userPager = " + tableName + "Service.Get" + tableName + "Paging(filter.MapTo<" + tableName + "FilterDto>()).Convert<" + tableName + "Dto, " + tableName + "ViewModel>();");
            sb_body.AppendLine("            object objResult = new");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                total = userPager.TotalCount,");
            sb_body.AppendLine("                rows = userPager,");
            sb_body.AppendLine("            };");
            sb_body.AppendLine("            return Json(objResult, JsonRequestBehavior.AllowGet);");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 添加、修改");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"user\"></param>");
            sb_body.AppendLine("        /// <param name=\"userRoles\"></param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        [HttpPost]");
            sb_body.AppendLine("        public ActionResult Edit" + tableName + "(" + tableName + "ViewModel vm)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            var saveInfo = new Save" + tableName + "CmdDto()");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                " + tableName + " = vm.MapTo<" + tableName + "CmdDto>()");
            sb_body.AppendLine("            };");
            sb_body.AppendLine("            var result = " + tableName + "Service.Save" + tableName + "(saveInfo);");
            sb_body.AppendLine("            return Content(JsonSerialize.ObjectToJson(result));");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 删除");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"user\"></param>");
            sb_body.AppendLine("        /// <param name=\"userRoles\"></param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        [HttpPost]");
            sb_body.AppendLine("        public ActionResult Delete" + tableName + "(Delete" + tableName + "CmdDto vm)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            var result = " + tableName + "Service.Delete" + tableName + "(vm);");
            sb_body.AppendLine("            return Content(JsonSerialize.ObjectToJson(result));");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion " + tableDesc + "管理");
            sb_body.AppendLine("");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("}");


            string file_Model = "C:\\Code\\" + str_nameSpace + ".Web\\Controllers";
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }
            CommonCode.Save(file_Model + "/" + tableName + "Controller.cs", sb_body.ToString());
        }

    }
}
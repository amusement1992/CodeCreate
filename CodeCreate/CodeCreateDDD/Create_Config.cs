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
    public class Create_Config
    {
        public string GetStr_CreateMap(string str_nameSpace, DataTable dt_tables, string tableName)
        {
            string temp = tableName;
            string tablePrefix = CommonCode.GetTablePrefix(tableName); tableName = CommonCode.GetTableName(tableName);
            StringBuilder sb_body = new StringBuilder();

            sb_body.AppendLine("using " + str_nameSpace + ".Entity." + tablePrefix + ";");
            sb_body.AppendLine("using " + str_nameSpace + ".Domain." + tablePrefix + ".Model;");
            sb_body.AppendLine("using " + str_nameSpace + ".DTO." + tablePrefix + ".Query;");
            sb_body.AppendLine("using " + str_nameSpace + ".ViewModel." + tablePrefix + ".Filter;");
            sb_body.AppendLine("using " + str_nameSpace + ".ViewModel." + tablePrefix + ";");
            sb_body.AppendLine("using " + str_nameSpace + ".DTO." + tablePrefix + ".Cmd;");
            sb_body.AppendLine("using " + str_nameSpace + ".DTO." + tablePrefix + ".Query.Filter;");
            sb_body.AppendLine("");
            sb_body.AppendLine("");
            sb_body.AppendLine("");


            sb_body.AppendLine("            #region " + tableName + "");
            sb_body.AppendLine("");
            sb_body.AppendLine("            cfg.CreateMap<" + tableName + "Entity, " + tableName + ">();");
            sb_body.AppendLine("            cfg.CreateMap<" + tableName + ", " + tableName + "Entity>();");
            sb_body.AppendLine("");
            sb_body.AppendLine("            cfg.CreateMap<" + tableName + ", " + tableName + "Dto>();");
            sb_body.AppendLine("            cfg.CreateMap<" + tableName + "Dto, " + tableName + ">();");
            sb_body.AppendLine("");
            sb_body.AppendLine("            cfg.CreateMap<" + tableName + "Dto, " + tableName + "ViewModel>();");
            sb_body.AppendLine("");
            sb_body.AppendLine("            cfg.CreateMap<" + tableName + "FilterViewModel, " + tableName + "FilterDto>();");
            sb_body.AppendLine("");
            sb_body.AppendLine("            cfg.CreateMap<" + tableName + "CmdDto, " + tableName + ">();");
            sb_body.AppendLine("            cfg.CreateMap<" + tableName + "ViewModel, " + tableName + "CmdDto>();");
            sb_body.AppendLine("");
            sb_body.AppendLine("");
            sb_body.AppendLine("            #endregion");

            sb_body.AppendLine("");

            return sb_body.ToString();

        }

        public string GetStr_SetObjectName(string str_nameSpace, DataTable dt_tables, string tableName)
        {
            string temp = tableName;
            string tablePrefix = CommonCode.GetTablePrefix(tableName); tableName = CommonCode.GetTableName(tableName);
            StringBuilder sb_body = new StringBuilder();
            sb_body.AppendLine("            QueryConfig.SetObjectName(\"" + temp + "\", typeof(" + tableName + "Entity), typeof(" + tableName + "Query));");

            return sb_body.ToString();

        }
        public string GetStr_SetPrimaryKey(string str_nameSpace, DataTable dt_tables, string tableName)
        {
            string temp = tableName;
            string tablePrefix = CommonCode.GetTablePrefix(tableName); tableName = CommonCode.GetTableName(tableName);
            StringBuilder sb_body = new StringBuilder();

            sb_body.AppendLine("            QueryConfig.SetPrimaryKey<" + tableName + "Entity>(u => u.SysNo);");

            return sb_body.ToString();

        }

    }
}
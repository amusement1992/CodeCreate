﻿using System;
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
        public string GetStr_CreateMap(string file_Model, string str_nameSpace, DataTable dt_tables, string tableName)
        {
            string temp = tableName;
            tableName = tableName.Replace("Data_", "");
            StringBuilder sb_body = new StringBuilder();

            sb_body.AppendLine("            #region " + tableName + "");
            sb_body.AppendLine("");
            sb_body.AppendLine("            cfg.CreateMap<" + tableName + ", " + tableName + "Entity>();");
            sb_body.AppendLine("            cfg.CreateMap<" + tableName + "Entity, " + tableName + ">();");
            sb_body.AppendLine("            cfg.CreateMap<" + tableName + ", " + tableName + "Dto>();");
            sb_body.AppendLine("            cfg.CreateMap<" + tableName + "CmdDto, " + tableName + ">();");
            sb_body.AppendLine("            cfg.CreateMap<" + tableName + "Dto, " + tableName + "ViewModel>();");
            sb_body.AppendLine("            cfg.CreateMap<" + tableName + "ViewModel, " + tableName + "CmdDto>();");
            sb_body.AppendLine("");
            sb_body.AppendLine("            #endregion");
            sb_body.AppendLine("");

            return sb_body.ToString();

        }

        public string GetStr_SetObjectName(string file_Model, string str_nameSpace, DataTable dt_tables, string tableName)
        {
            string temp = tableName;
            tableName = tableName.Replace("Data_", "");
            StringBuilder sb_body = new StringBuilder();
            sb_body.AppendLine("            QueryConfig.SetObjectName(\"" + temp + "\", typeof(" + tableName + "Entity), typeof(" + tableName + "Query));");

            return sb_body.ToString();

        }
        public string GetStr_SetPrimaryKey(string file_Model, string str_nameSpace, DataTable dt_tables, string tableName)
        {
            string temp = tableName;
            tableName = tableName.Replace("Data_", "");
            StringBuilder sb_body = new StringBuilder();

            sb_body.AppendLine("            QueryConfig.SetPrimaryKey<" + tableName + "Entity>(u => u.SysNo);");

            return sb_body.ToString();

        }

    }
}
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
        public void Create(string file_Model, string str_nameSpace, DataTable dt_tables, string tableName)
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
            sb_body.AppendLine("");
            sb_body.AppendLine("            QueryConfig.SetObjectName(\""+ temp + "\", typeof(" + tableName + "Entity), typeof(" + tableName + "Query));");
            sb_body.AppendLine("");
            sb_body.AppendLine("");
            sb_body.AppendLine("            QueryConfig.SetPrimaryKey<" + tableName + "Entity>(u => u.SysNo);");

            file_Model = "C:\\Code\\BigDataAnalysis.Mapper";
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }
            CommonCode.Save(file_Model + "/" + tableName + "Mapper.cs", sb_body.ToString());
        }

    }
}
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

            string tablePrefix = CommonCode.GetTablePrefix(tableName);
            tableName = CommonCode.GetTableName(tableName);
            string tableDesc = CommonCode.GetTableDesc(tableName);

            StringBuilder sb_body = new StringBuilder();
            StringBuilder sb = new StringBuilder();

            //sb_body.AppendLine("using " + str_nameSpace + ".Entity." + tablePrefix + ";");
            //sb_body.AppendLine("using " + str_nameSpace + ".Domain." + tablePrefix + ".Model;");
            //sb_body.AppendLine("using " + str_nameSpace + ".DTO." + tablePrefix + ".Query;");
            //sb_body.AppendLine("using " + str_nameSpace + ".ViewModel." + tablePrefix + ".Filter;");
            //sb_body.AppendLine("using " + str_nameSpace + ".ViewModel." + tablePrefix + ";");
            //sb_body.AppendLine("using " + str_nameSpace + ".DTO." + tablePrefix + ".Cmd;");
            //sb_body.AppendLine("using " + str_nameSpace + ".DTO." + tablePrefix + ".Query.Filter;");
            //sb_body.AppendLine("");
            //sb_body.AppendLine("");
            //sb_body.AppendLine("");


            sb_body.AppendLine("            #region " + tableName + "");
            sb_body.AppendLine("");
            sb_body.AppendLine("            cfg.CreateMap<" + tableName + "Entity, " + tableName + ">();");
            sb_body.AppendLine("            cfg.CreateMap<" + tableName + ", " + tableName + "Entity>();");
            sb_body.AppendLine("");
            sb_body.AppendLine("            cfg.CreateMap<" + tableName + ", " + tableName + "Dto>();");
            sb_body.AppendLine("            cfg.CreateMap<" + tableName + "Dto, " + tableName + ">();");
            sb_body.AppendLine("");

            SetData(tableName, sb);

            sb_body.AppendLine(sb.ToString());


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


        private static void SetData(string tableName, StringBuilder sb)
        {

            var listModel = CommonCode.GetTableModel(tableName);
            if (listModel != null)
            {
                sb.AppendLine("            cfg.CreateMap<" + tableName + "Dto, " + tableName + "ViewModel>()");
                foreach (var item in listModel)
                {
                    if (item.List != null)
                    {
                        foreach (var thisModel in item.List.Where(d => d.IsMapper))
                        {
                            if (string.IsNullOrEmpty(thisModel.MapperModel))
                            {
                                thisModel.MapperModel = thisModel.NewColumnName;
                            }

                            sb.AppendLine("                .ForMember(r => r." + thisModel.NewColumnName_VM + ", re => re.MapFrom(rs => rs." + thisModel.MapperModel + "." + thisModel.MapperName + "))");

                        }
                    }

                }
                sb.Append(";");
            }
        }


        public static void SaveAutoMapMapper(string str_CreateMap)
        {
            string src = CommonCode.projSrc + "/BigDataAnalysis.Mapper/AutoMapMapper.cs";

            var str = JsonHelper.GetFile(src);

            string[] arr = JsonHelper.SplitString(str, "\r\n");
            StringBuilder sb = new StringBuilder();
            foreach (var item in arr)
            {
                if (item.Contains("AutoMapper.Mapper.Initialize(cfg);"))
                {
                    sb.AppendLine(str_CreateMap);
                }
                sb.AppendLine(item);
            }

            string file_Model = "C:\\Code\\BigDataAnalysis.Mapper";
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }

            CommonCode.Save(file_Model + "/AutoMapMapper.cs", sb.ToString());
        }

        public static void SaveDBConfig(string str_SetObjectName, string str_SetPrimaryKey)
        {
            string src = CommonCode.projSrc + "/BigDataAnalysis.Config/DbConfig.cs";

            var str = JsonHelper.GetFile(src);

            string[] arr = JsonHelper.SplitString(str, "\r\n");
            StringBuilder sb = new StringBuilder();
            foreach (var item in arr)
            {
                sb.AppendLine(item);
                if (item.Contains("#region 数据库表名配置"))
                {
                    sb.AppendLine(str_SetObjectName);
                }else if (item.Contains("#region 数据表主键配置"))
                {
                    sb.AppendLine(str_SetPrimaryKey);
                }
            }

            string file_Model = "C:\\Code\\BigDataAnalysis.Config";
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }

            CommonCode.Save(file_Model + "/DbConfig.cs", sb.ToString());


        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace CodeCreate
{
    /// <summary>
    /// 创建csproj
    /// </summary>
    public class Create_csproj
    {
        public void Create(string str_nameSpace, DataTable dt_tables, string tableName, string type)
        {
            string tablePrefix = CommonCode.GetTablePrefix(tableName);
            tableName = CommonCode.GetTableName(tableName);
            string tableDesc = CommonCode.GetTableDesc(tableName);

            string src = CommonCode.projSrc + "/" + str_nameSpace + "." + type + "/" + str_nameSpace + "." + type + ".csproj";
            var str = JsonHelper.GetFile(src);
            var suffix = type;

            string[] arr = JsonHelper.SplitString(str, "\r\n");
            StringBuilder sb2 = new StringBuilder();
            foreach (var item in arr)
            {
                sb2.AppendLine(item);
                if (item.Contains("SystemConfig") && !item.Contains(tableName))
                {
                    string pref = "";
                    if (type.Contains("Interface"))
                    {
                        pref = "I";
                        suffix = suffix.Replace("Interface", "");
                    }


                    if (type == "Domain")
                    {
                        if (item.Contains("Model\\"))
                        {
                            pref = "Model\\";
                            suffix = "";
                        }
                        else if (item.Contains("Repository\\"))
                        {
                            pref = "Repository\\I";
                            suffix = "Repository";
                        }
                        else if (item.Contains("Service\\"))
                        {
                            pref = "Service\\";
                            suffix = "Service";
                        }
                    }
                    else if (type == "ViewModel")
                    {
                        if (item.Contains("Filter\\"))
                        {
                            pref = "Filter\\";
                            suffix = "FilterViewModel";
                        }
                        else
                        {
                            suffix = "ViewModel";
                        }
                    }
                    else if (type == "Dto")
                    {
                        if (item.Contains("Cmd\\"))
                        {
                            pref = "Cmd\\";
                            suffix = "CmdDto";
                        }
                        else if (item.Contains("Query\\Filter\\"))
                        {
                            pref = "Query\\Filter\\";
                            suffix = "FilterDto";
                        }
                        else if (item.Contains("Query\\"))
                        {
                            pref = "Query\\";
                            suffix = "Dto";
                        }

                    }
                    suffix += ".cs";

                    if (type == "Web")
                    {
                        tablePrefix = "Views\\" + tablePrefix;
                        suffix = ".cshtml";
                    }

                    SaveData(tablePrefix, pref, tableName, suffix, sb2);
                }
            }

            string file_Model = "C:\\Code\\" + str_nameSpace + "." + type + "";
            CommonCode.Save(file_Model + "/" + str_nameSpace + "." + type + ".csproj", sb2.ToString());
        }

        private static void SaveData(string tablePrefix, string pref, string tableName, string suffix, StringBuilder sb2)
        {
            sb2.AppendLine("    <Compile Include=\"" + tablePrefix + "\\" + pref + tableName + "" + suffix + "\" />");
        }
    }
}
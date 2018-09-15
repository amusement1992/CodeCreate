using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace CodeCreate
{
    public class Create_Enum
    {
        public void Create(string str_nameSpace, List<string> listTableName)
        {

            string src = CommonCode.projSrc + "/" + str_nameSpace + ".Enum" + "/OperationTypeEnum.cs";
            var str = JsonHelper.GetFile(src);

            StringBuilder sb = new StringBuilder();


            string[] arr = JsonHelper.SplitString(str, "\r\n");
            for (int j = 0; j < arr.Length; j++)
            {
                var item = arr[j];

                sb.AppendLine(item);
                for (int i = 0; i < listTableName.Count; i++)
                {
                    string tableName = listTableName[i];

                    string tablePrefix = CommonCode.GetTablePrefix(tableName); tableName = CommonCode.GetTableName(tableName);


                    if (item.Contains("SystemConfig") && !item.Contains(tableName))
                    {
                        SaveData(tableName, sb);
                    }
                }
            }
            string sss = sb.ToString();
            string file_Model = "C:\\Code\\" + str_nameSpace + ".Enum";
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }
            CommonCode.Save(file_Model + "//OperationTypeEnum.cs", sb.ToString());
        }

        private static void SaveData(string tableName, StringBuilder sb_body)
        {
            string desc = CommonCode.GetTableDesc(tableName);
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// " + desc);
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        " + tableName + ",");

        }
    }
}
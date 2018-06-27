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
    public class Create_SaveCmdDto
    {
        public void Create(string file_Model, string str_nameSpace, DataTable dt_tables, string tableName)
        {
            tableName = tableName.Replace("Data_", "");
               StringBuilder sb_body = new StringBuilder();

            sb_body.AppendLine("using BigDataAnalysis.DTO.Cmd;");
            sb_body.AppendLine("using System;");
            sb_body.AppendLine("using System.Collections.Generic;");
            sb_body.AppendLine("using System.Linq;");
            sb_body.AppendLine("using System.Text;");
            sb_body.AppendLine("using System.Threading.Tasks;");
            sb_body.AppendLine("");
            sb_body.AppendLine("namespace BigDataAnalysis.DTO.Data.Cmd");
            sb_body.AppendLine("{");
            sb_body.AppendLine("    /// <summary>");
            sb_body.AppendLine("    /// 保存信息");
            sb_body.AppendLine("    /// </summary>");
            sb_body.AppendLine("    public class Save" + tableName + "CmdDto");
            sb_body.AppendLine("    {");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 信息");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        public " + tableName + "CmdDto " + tableName + "");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            get; set;");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("}");
            
            file_Model = "C:\\Code\\BigDataAnalysis.DTO\\Data\\Cmd\\Save";
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }
            CommonCode.Save(file_Model + "/Save" + tableName + "CmdDto.cs", sb_body.ToString());
        }
         
    }
}
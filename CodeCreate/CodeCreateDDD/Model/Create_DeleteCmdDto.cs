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
    public class Create_DeleteCmdDto
    {
        public void Create(string str_nameSpace, DataTable dt_tables, string tableName)
        {
            string tablePrefix = CommonCode.GetTablePrefix(tableName); tableName = CommonCode.GetTableName(tableName);
            StringBuilder sb_body = new StringBuilder();

            sb_body.AppendLine("using System;");
            sb_body.AppendLine("using System.Collections.Generic;");
            sb_body.AppendLine("using System.Linq;");
            sb_body.AppendLine("using System.Text;");
            sb_body.AppendLine("using System.Threading.Tasks;");
            sb_body.AppendLine("");
            sb_body.AppendLine("namespace " + str_nameSpace + ".DTO." + tablePrefix + ".Cmd");
            sb_body.AppendLine("{");
            sb_body.AppendLine("    /// <summary>");
            sb_body.AppendLine("    /// 删除");
            sb_body.AppendLine("    /// </summary>");
            sb_body.AppendLine("    public class Delete" + tableName + "CmdDto");
            sb_body.AppendLine("    {");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 编号");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        public IEnumerable<Guid> " + tableName + "Ids");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            get; set;");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("}");

            string file_Model = "C:\\Code\\" + str_nameSpace + ".DTO\\" + tablePrefix + "\\Cmd\\Delete";
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }
            CommonCode.Save(file_Model + "/Delete" + tableName + "CmdDto.cs", sb_body.ToString());
        }

    }
}
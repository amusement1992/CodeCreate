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
    public class Create_Repository
    {
        public void Create(string str_nameSpace, DataTable dt_tables, string tableName)
        {
            string tablePrefix = CommonCode.GetTablePrefix(tableName);
            tableName = CommonCode.GetTableName(tableName);
            string tableDesc = CommonCode.GetTableDesc(tableName);
            
            StringBuilder sb_body = new StringBuilder();

            sb_body.AppendLine("using System;");
            sb_body.AppendLine("using System.Collections.Generic;");
            sb_body.AppendLine("using Lee.Domain.Repository;");
            sb_body.AppendLine("using " + str_nameSpace + ".Domain." + tablePrefix + ".Model;");
            sb_body.AppendLine("using " + str_nameSpace + ".Domain." + tablePrefix + ".Repository;");
            sb_body.AppendLine("using " + str_nameSpace + ".Entity." + tablePrefix + ";");
            sb_body.AppendLine("using " + str_nameSpace + ".DataAccessInterface." + tablePrefix + ";");

            sb_body.AppendLine("");
            sb_body.AppendLine("");
            sb_body.AppendLine("namespace " + str_nameSpace + ".Repository." + tablePrefix + "");
            sb_body.AppendLine("{");
            sb_body.AppendLine("    /// <summary>");
            sb_body.AppendLine("    /// 存储：" + tableDesc);
            sb_body.AppendLine("    /// </summary>");
            sb_body.AppendLine("    public class " + tableName + "Repository : DefaultRepository<" + tableName + ", " + tableName + "Entity, I" + tableName + "DataAccess>, I" + tableName + "Repository");
            sb_body.AppendLine("    {");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("}");


            string file_Model = "C:\\Code\\" + str_nameSpace + ".Repository\\" + tablePrefix + "";
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }
            CommonCode.Save(file_Model + "/" + tableName + "Repository.cs", sb_body.ToString());
        }

    }
}
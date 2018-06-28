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
            string[] arrTemp = tableName.Split('_');
            string prefix = arrTemp[0];
            tableName = arrTemp[1];

            StringBuilder sb_body = new StringBuilder();

            sb_body.AppendLine("using System;");
            sb_body.AppendLine("using System.Collections.Generic;");
            sb_body.AppendLine("using System.Linq;");
            sb_body.AppendLine("using System.Text;");
            sb_body.AppendLine("using System.Threading.Tasks;");
            sb_body.AppendLine("using " + str_nameSpace + ".Domain." + prefix + ".Model;");
            sb_body.AppendLine("using Lee.Command.UnitOfWork;");
            sb_body.AppendLine("using " + str_nameSpace + ".Domain." + prefix + ".Repository;");
            sb_body.AppendLine("using " + str_nameSpace + ".Entity." + prefix + ";");
            sb_body.AppendLine("using Lee.Utility.Extension;");
            sb_body.AppendLine("using " + str_nameSpace + ".DataAccessInterface." + prefix + ";");
            sb_body.AppendLine("using Lee.CQuery;");
            sb_body.AppendLine("using Lee.CQuery.Paging;");
            sb_body.AppendLine("using Lee.Command;");
            sb_body.AppendLine("using " + str_nameSpace + ".Query;");
            sb_body.AppendLine("using Lee.Domain.Repository;");
            sb_body.AppendLine("");
            sb_body.AppendLine("");
            sb_body.AppendLine("namespace " + str_nameSpace + ".Repository." + prefix + "");
            sb_body.AppendLine("{");
            sb_body.AppendLine("    /// <summary>");
            sb_body.AppendLine("    /// 存储");
            sb_body.AppendLine("    /// </summary>");
            sb_body.AppendLine("    public class " + tableName + "Repository : DefaultRepository<" + tableName + ", " + tableName + "Entity, I" + tableName + "DataAccess>, I" + tableName + "Repository");
            sb_body.AppendLine("    {");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("}");


            string file_Model = "C:\\Code\\" + str_nameSpace + ".Repository\\" + prefix + "";
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }
            CommonCode.Save(file_Model + "/" + tableName + "Repository.cs", sb_body.ToString());
        }

    }
}
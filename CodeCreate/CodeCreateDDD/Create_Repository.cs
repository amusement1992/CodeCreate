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
        public void Create(string file_Model, string str_nameSpace, DataTable dt_tables, string tableName)
        {
            tableName = tableName.Replace("Data_", "");
            StringBuilder sb_body = new StringBuilder();

            sb_body.AppendLine("using System;");
            sb_body.AppendLine("using System.Collections.Generic;");
            sb_body.AppendLine("using System.Linq;");
            sb_body.AppendLine("using System.Text;");
            sb_body.AppendLine("using System.Threading.Tasks;");
            sb_body.AppendLine("using BigDataAnalysis.Domain.Data.Model;");
            sb_body.AppendLine("using Lee.Command.UnitOfWork;");
            sb_body.AppendLine("using BigDataAnalysis.Domain.Data.Repository;");
            sb_body.AppendLine("using BigDataAnalysis.Entity.Data;");
            sb_body.AppendLine("using Lee.Utility.Extension;");
            sb_body.AppendLine("using BigDataAnalysis.DataAccessInterface.Data;");
            sb_body.AppendLine("using Lee.CQuery;");
            sb_body.AppendLine("using Lee.CQuery.Paging;");
            sb_body.AppendLine("using Lee.Command;");
            sb_body.AppendLine("using BigDataAnalysis.Query;");
            sb_body.AppendLine("using Lee.Domain.Repository;");
            sb_body.AppendLine("");
            sb_body.AppendLine("");
            sb_body.AppendLine("namespace BigDataAnalysis.Repository.Data");
            sb_body.AppendLine("{");
            sb_body.AppendLine("    /// <summary>");
            sb_body.AppendLine("    /// 存储");
            sb_body.AppendLine("    /// </summary>");
            sb_body.AppendLine("    public class " + tableName + "Repository : DefaultRepository<" + tableName + ", " + tableName + "Entity, I" + tableName + "DataAccess>, I" + tableName + "Repository");
            sb_body.AppendLine("    {");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("}");


            file_Model = "C:\\Code\\BigDataAnalysis.Repository\\Data";
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }
            CommonCode.Save(file_Model + "/" + tableName + "Repository.cs", sb_body.ToString());
        }

    }
}
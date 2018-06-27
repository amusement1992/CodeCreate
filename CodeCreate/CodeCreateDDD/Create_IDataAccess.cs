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
    public class Create_IDataAccess
    {
        public void Create(string file_Model, string str_nameSpace, DataTable dt_tables, string tableName)
        {
            tableName = tableName.Replace("Data_", "");
            StringBuilder sb_body = new StringBuilder();

            sb_body.AppendLine("using Lee.Command;");
            sb_body.AppendLine("using BigDataAnalysis.Entity.Data;");
            sb_body.AppendLine("using System;");
            sb_body.AppendLine("using System.Collections.Generic;");
            sb_body.AppendLine("using System.Linq;");
            sb_body.AppendLine("using System.Text;");
            sb_body.AppendLine("using System.Threading.Tasks;");
            sb_body.AppendLine("using BigDataAnalysis.Entity;");
            sb_body.AppendLine("");
            sb_body.AppendLine("namespace BigDataAnalysis.DataAccessInterface.Data");
            sb_body.AppendLine("{");
            sb_body.AppendLine("    /// <summary>");
            sb_body.AppendLine("    /// 数据访问接口");
            sb_body.AppendLine("    /// </summary>");
            sb_body.AppendLine("    public interface I" + tableName + "DataAccess : IDataAccess<" + tableName + "Entity>");
            sb_body.AppendLine("    {");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("");
            sb_body.AppendLine("    /// <summary>");
            sb_body.AppendLine("    /// 数据库接口");
            sb_body.AppendLine("    /// </summary>");
            sb_body.AppendLine("    public interface I" + tableName + "DbAccess : I" + tableName + "DataAccess");
            sb_body.AppendLine("    {");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("}");
                        
            file_Model = "C:\\Code\\BigDataAnalysis.DataAccessInterface\\Data";
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }
            CommonCode.Save(file_Model + "/I" + tableName + "DataAccess.cs", sb_body.ToString());
        }

    }
}
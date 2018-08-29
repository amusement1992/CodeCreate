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
    public class Create_Json
    {
        public void Create(string str_nameSpace, DataTable dt_tables, string tableName)
        {
            string tablePrefix = CommonCode.GetTablePrefix(tableName);
            tableName = CommonCode.GetTableName(tableName);
            string tableDesc = CommonCode.GetTableDesc(tableName);

            bool isPrimeKey = false;
            string primaryKey = "";

            StringBuilder sb = new StringBuilder();


            #region Model

            //遍历每个字段
            foreach (DataRow dr in dt_tables.Rows)
            {
                string columnName = dr["columnName"].ToString().Trim();//字段名
                string columnType = dr["columnType"].ToString().Trim();//字段类型
                string columnComment = dr["columnComment"].ToString().Trim();//字段注释
                string nullable = dr["nullable"].ToString().Trim();//是否可空（Y是不为空，N是为空）
                string data_default = dr["data_default"].ToString().Trim();//默認值
                string data_maxLength = dr["char_col_decl_length"].ToString().Trim();//最大長度
                string bool_primaryKey = dr["primaryKey"].ToString().Trim();//主键 值为Y或N

                if (bool_primaryKey.ToUpper() == "Y")//存在主键
                {
                    isPrimeKey = true;
                    primaryKey = columnName.ToUpper();
                }
                if (string.IsNullOrEmpty(columnComment))
                {
                    columnComment = columnName;
                }

                CommonCode.GetColumnType(ref columnType, ref data_default);

                nullable = CommonCode.GetNullable(columnType, nullable);

                sb.AppendLine("			{");
                sb.AppendLine("				\"ColumnName\": \"" + columnName + "\",");
                sb.AppendLine("				\"ColumnDesc\": \"" + columnComment + "\"");
                sb.AppendLine("			},");
            }


            #endregion Model

            StringBuilder sb_body = new StringBuilder();
            sb_body.AppendLine("[");
            sb_body.AppendLine("	{");
            sb_body.AppendLine("		\"TableName\": \"" + tableName + "\",");
            sb_body.AppendLine("		\"List\": [");

            sb_body.Append(sb.ToString());

            sb_body.AppendLine("		]");
            sb_body.AppendLine("	}");
            sb_body.AppendLine("]");


            string file_Model = "C:\\Code\\Json";
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }
            CommonCode.Save(file_Model + "/" + tableName + ".json", sb_body.ToString());
        }

    }
}
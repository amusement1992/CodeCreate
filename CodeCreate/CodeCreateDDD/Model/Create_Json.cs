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
            int index = 0;

            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            StringBuilder sb3 = new StringBuilder();
            StringBuilder sb4 = new StringBuilder();
            StringBuilder sb5 = new StringBuilder();


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

                var arr = new List<string> {
                    "SysNo",
                    "OutsideCode",
                    "ShopID",
                    "StatisticalDate",

                    "Remark",
                    "CreateDate",
                    "UpdateDate",
                    "CreateUserID",
                    "UpdateUserID",
                };
                if (!arr.Contains(columnName))
                {
                    if (columnType == "string")
                    {
                        sb2.AppendLine("model." + columnName + " = GetColumnValue(listColumn, listDesc, row, \"" + columnName + "\");");
                        sb4.AppendLine("                        model." + columnName + " = GetColumnValue(listColumn, dr, \"" + columnName + "\");");

                    }
                    else if (columnType == "DateTime")
                    {
                        string col = columnType[0].ToString().ToUpper();
                        col += columnType.Substring(1, columnType.Length - 1);
                        sb2.AppendLine("model." + columnName + " = GetColumnValue(listColumn, listDesc, row, \"" + columnName + "\").StrTo" + col + "();");
                        sb4.AppendLine("                        model." + columnName + " = GetColumnValue(listColumn, dr, \"" + columnName + "\").StrTo" + col + "();");
                    }
                    else
                    {
                        string col = columnType[0].ToString().ToUpper();
                        col += columnType.Substring(1, columnType.Length - 1);
                        sb2.AppendLine("model." + columnName + " = GetColumnValue(listColumn, listDesc, row, \"" + columnName + "\").StrTo" + col + "(-1);");
                        sb4.AppendLine("                        model." + columnName + " = GetColumnValue(listColumn, dr, \"" + columnName + "\").StrTo" + col + "(-1);");
                    }

                    sb3.AppendLine("            Map(m => m." + columnName + ").Name(\"" + columnComment + "\");");

                    sb5.AppendLine("                ExcelHelper.SetCellValue(sheet, thisRow, " + index + ", item." + columnName + ", cellStyle);");

                    ++index;

                }

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


            StringBuilder sb_body3 = new StringBuilder();
            sb_body3.AppendLine("using BigDataAnalysis.DTO.Original.Cmd;");
            sb_body3.AppendLine("using System;");
            sb_body3.AppendLine("using System.Collections.Generic;");
            sb_body3.AppendLine("using System.Linq;");
            sb_body3.AppendLine("using System.Text;");
            sb_body3.AppendLine("using System.Threading.Tasks;");
            sb_body3.AppendLine("");
            sb_body3.AppendLine("namespace BigDataAnalysis.DTO.CSVMap");
            sb_body3.AppendLine("{");
            sb_body3.AppendLine("    /// <summary>");
            sb_body3.AppendLine("    /// CSVHeaderMap：" + tableDesc + "");
            sb_body3.AppendLine("    /// </summary>");
            sb_body3.AppendLine("    public class CSVHeaderMap_" + tableName + " : CsvHelper.Configuration.ClassMap<" + tableName + "CmdDto>");
            sb_body3.AppendLine("    {");
            sb_body3.AppendLine("        public CSVHeaderMap_" + tableName + "()");
            sb_body3.AppendLine("        {");
            sb_body3.AppendLine(sb3.ToString());
            sb_body3.AppendLine("        }");
            sb_body3.AppendLine("    }");
            sb_body3.AppendLine("}");
            sb_body3.AppendLine("");


            string file_Model = "C:\\Code\\Json";
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }
            CommonCode.Save(file_Model + "/" + tableName + ".json", sb_body.ToString());

            CommonCode.Save(file_Model + "/" + tableName + "_GetColumnValue.txt", sb2.ToString());

            CommonCode.Save(file_Model + "/CSVHeaderMap_" + tableName + ".cs", sb_body3.ToString());

            CommonCode.Save(file_Model + "/DT_" + tableName + ".txt", sb4.ToString());

            CommonCode.Save(file_Model + "/Export_" + tableName + ".txt", sb5.ToString());
        }

    }
}
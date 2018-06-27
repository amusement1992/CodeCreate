using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CodeCreate
{
    public class CommonCode
    {

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <param name="fileContent">文件内容</param>
        public static void Save(string fileName, string fileContent)
        {
            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);

            byte[] buffer = Encoding.Default.GetBytes(fileContent);

            fs.Write(buffer, 0, buffer.Length);

            fs.Flush();
            fs.Close();
        }


        public static void GetColumnType(ref string columnType, ref string data_default)
        {

            #region 数据类型判断

            switch (columnType.ToLower())
            {
                case "bit":
                    columnType = "bool";
                    if (!string.IsNullOrEmpty(data_default))
                    {
                        data_default = " = " + data_default.TrimStart('(').TrimEnd(')');
                    }
                    break;
                case "uniqueidentifier":
                    columnType = "Guid";
                    if (!string.IsNullOrEmpty(data_default))
                    {
                        data_default = " = " + data_default.TrimStart('(').TrimEnd(')');
                    }
                    break;

                case "int":
                case "tinyint":
                case "bigint":
                    columnType = "int";
                    if (!string.IsNullOrEmpty(data_default))
                    {
                        data_default = " = " + data_default.TrimStart('(').TrimEnd(')');
                    }
                    break;

                case "binary":
                case "decimal":
                case "float":
                case "money":
                case "numeric":
                case "smallint":
                case "smallmoney":
                case "varbinary":
                    columnType = "decimal";
                    if (!string.IsNullOrEmpty(data_default))
                    {
                        data_default = " = " + data_default.TrimStart('(').TrimEnd(')');
                    }
                    break;

                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                case "time":
                    columnType = "DateTime";
                    if (!string.IsNullOrEmpty(data_default))
                    {
                        if (data_default.ToLower() == "(getdate())")
                        {
                            data_default = " = DateTime.Now";
                        }
                    }
                    break;

                default:
                    columnType = "string";
                    if (!string.IsNullOrEmpty(data_default))
                    {
                        data_default = " = \"" + data_default.Trim('(', ')').Trim('\'') + "\"";
                    }
                    break;
            }

            #endregion 数据类型判断
        }


        /// <summary>
        /// 获取字段的数据类型
        /// </summary>
        /// <param name="colType"></param>
        /// <returns></returns>
        public static string GetColType(string colType)
        {
            colType = colType.Trim().ToLower();
            switch (colType)
            {
                case "int":
                case "bigint":
                case "binary":
                case "bit":
                case "decimal":
                case "float":
                case "money":
                case "numeric":
                case "smallint":
                case "smallmoney":
                case "tinyint":
                case "varbinary":
                    colType = "SqlType.Number";
                    break;

                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                case "time":
                    colType = "SqlType.DateTime";
                    break;

                case "text":
                case "ntext":
                    colType = "SqlType.Clob";
                    break;

                default:
                    colType = "SqlType.NVarChar";
                    break;
            }
            return colType;
        }

    }
}

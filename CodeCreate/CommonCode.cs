﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CodeCreate.Helper;
using CodeCreate.Model;

namespace CodeCreate
{
    public class CommonCode
    {
        public static string projSrc = "E:\\MyWeb\\Web_TFS\\BigDataAnalysis\\BigDataAnalysisDev";

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <param name="fileContent">文件内容</param>
        public static void Save(string fileName, string fileContent)
        {
            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
            fs.SetLength(0);
            var gb2312 = Encoding.GetEncoding("GB2312");

            byte[] buffer = gb2312.GetBytes(fileContent);

            fs.Write(buffer, 0, buffer.Length);

            fs.Flush();
            fs.Close();
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <param name="fileContent">文件内容</param>
        public static void Save2(string fileName, string fileContent)
        {
            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
            fs.SetLength(0);
            var gb2312 = Encoding.GetEncoding("UTF-8");

            byte[] buffer = gb2312.GetBytes(fileContent);

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
        /// 是否可空
        /// </summary>
        /// <param name="nullable"></param>
        /// <param name="columnType"></param>
        /// <returns></returns>
        public static string GetNullable(string columnType, string nullable)
        {
            bool isNullable = false;
            if (nullable.ToUpper().Trim() == "N")
            {
                var temp = columnType.ToLower();
                if (temp != "string")
                {
                    isNullable = true;
                }
            }
            return isNullable ? "?" : "";
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

        public static string GetTablePrefix(string tableName)
        {
            string[] arrTemp = tableName.Split('_');
            return arrTemp[0];
        }
        public static string GetTableName(string tableName)
        {
            string[] arrTemp = tableName.Split('_');
            return arrTemp[1];
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static List<TableModel> GetListTable()
        {
            string filepath = "table.json";
            string json = JsonHelper.GetFile(filepath);

            List<TableModel> tableModels = JsonConvertHelper.DeserializeObject<List<TableModel>>(json) as List<TableModel>;

            return tableModels;
        }


        /// <summary>
        /// 获取表的描述
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string GetTableDesc(string tableName)
        {
            var list = GetListTable();
            var model = list.Where(d => d.TableName == tableName).FirstOrDefault();
            if (model == null)
            {
                model = list.Where(d => d.TableName == "All").FirstOrDefault();
            }

            return model.TableDesc;
        }

        public static List<TableModel> GetTableModel(string TableName)
        {
            var list = GetListTable().Where(d => d.TableName == TableName || d.TableName == "All").ToList();
            return list;

        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sources">源路径</param>
        /// <param name="dest">新路径</param>
        public static void CopyFile(string sources, string dest, List<string> listExcept)
        {
            DirectoryInfo dinfo = new DirectoryInfo(sources);

            //注，这里面传的是路径，并不是文件，所以不能保含带后缀的文件                
            foreach (FileSystemInfo f in dinfo.GetFileSystemInfos())
            {
                //目标路径destName = 目标文件夹路径 + 原文件夹下的子文件(或文件夹)名字                
                //Path.Combine(string a ,string b) 为合并两个字符串                     
                String destName = Path.Combine(dest, f.Name);
                if (f is FileInfo)
                {
                    //如果是文件就复制       
                    File.Copy(f.FullName, destName, true);//true代表可以覆盖同名文件                     
                }
                else if(!listExcept.Contains(f.Name))
                {

                    //如果是文件夹就创建文件夹然后复制然后递归复制              
                    Directory.CreateDirectory(destName);
                    CopyFile(f.FullName, destName, listExcept);
                }
            }
        }

    }
}

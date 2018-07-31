using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CodeCreate.Model;

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
            fs.SetLength(0);
            var gb2312 = Encoding.GetEncoding("GB2312");

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
        /// 获取表的描述
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string GetTableDesc(string tableName)
        {
            string tableDesc = "";
            switch (tableName)
            {
                case "History":
                    tableDesc = "历史记录";
                    break;
                case "IPConfig":
                    tableDesc = "IP授权";
                    break;
                case "SystemConfig":
                    tableDesc = "系统配置";
                    break;



                case "Brand":
                    tableDesc = "品牌";
                    break;
                case "Category":
                    tableDesc = "类目";
                    break;
                case "Platform":
                    tableDesc = "平台";
                    break;
                case "Company":
                    tableDesc = "公司";
                    break;
                case "Shop":
                    tableDesc = "店铺";
                    break;
                case "Department":
                    tableDesc = "部门";
                    break;
                case "DataSource":
                    tableDesc = "数据来源";
                    break;
                case "Customer":
                    tableDesc = "客户";
                    break;



                case "Contracts":
                    tableDesc = "合同";
                    break;
                case "ContractsValue":
                    tableDesc = "合同值";
                    break;
                case "Template":
                    tableDesc = "模板";
                    break;
                case "TemplateValue":
                    tableDesc = "模板值";
                    break;
                case "TemplateType":
                    tableDesc = "模板类型";
                    break;
                case "ContractsType":
                    tableDesc = "合同类型";
                    break;
                default:
                    tableDesc = "【名称】";
                    break;
            }
            return tableDesc;
        }

        public static List<TableModel> GetTableModel(List<TableModel> domainModels, string TableName)
        {
            var list = domainModels.Where(d => d.TableName == TableName || d.TableName == "All").ToList();
            return list;

        }

        public static List<TableModel> GetData()
        {


            List<TableModel> domainModels = new List<TableModel>();

            TableModel domainModel = new TableModel()
            {
                TableName = "All",
                ExcludePropertys = new List<string>()
                {
                    "CreateDate",
                    "CreateUserID",
                },
                List = new List<ColumnModel>
                {
                    new ColumnModel()
                    {
                        ColumnName="CreateUserID",
                        ColumnType="Guid",

                        NewColumnName="CreateUser",
                        NewColumnType="User",
                        NewColumnComment="用户",

                        NewColumnType_Dto="UserDto",

                        NewColumnName_VM ="CreateUserName",
                        NewColumnType_VM ="string",

                        IsMapper=true,
                        MapperName="UserName",
                    },
                }
            };
            domainModels.Add(domainModel);
            
            domainModel = new TableModel()
            {
                TableName = "Template",
                ExcludePropertys = new List<string>()
                {
                    "ParentID",
                },
                List = new List<ColumnModel>
                {
                    new ColumnModel()
                    {
                        ColumnName="PartyA_CompanyID",
                        ColumnType="Guid?",

                        NewColumnName="PartyA_Company",
                        NewColumnType="Company",
                        NewColumnComment="甲方公司",

                        NewColumnType_Dto="CompanyDto",

                        NewColumnName_VM ="PartyA_CompanyName",
                        NewColumnType_VM ="string",

                        IsMapper=true,
                        MapperName="CompanyName",
                    },
                    new ColumnModel()
                    {
                        ColumnName="PartyB_CompanyID",
                        ColumnType="Guid?",

                        NewColumnType="Company",
                        NewColumnName="PartyB_Company",
                        NewColumnComment="乙方公司",

                        NewColumnType_Dto="CompanyDto",

                        NewColumnName_VM ="PartyB_CompanyName",
                        NewColumnType_VM ="string",

                        IsMapper=true,
                        MapperName="CompanyName",
                    },
                    new ColumnModel()
                    {
                        ColumnName="New",
                        ColumnType="int",

                        NewColumnType="int",
                        NewColumnName="AttachmentCount",
                        NewColumnComment="附件个数",

                        NewColumnName_VM ="AttachmentCount",
                    },
                    new ColumnModel()
                    {
                        NewColumnComment ="生效日期",

                        NewColumnName_VM ="StartTimeFormatter",
                        NewColumnType_VM ="string",
                    },
                    new ColumnModel()
                    {
                        NewColumnComment ="失效日期",

                        NewColumnName_VM ="EndTimeFormatter",
                        NewColumnType_VM ="string",
                    },
                }
            };
            domainModels.Add(domainModel);
            
            domainModel = new TableModel()
            {
                TableName = "Contract",
                ExcludePropertys = new List<string>()
                {
                    "ParentID",
                },
                List = new List<ColumnModel>
                {
                    new ColumnModel()
                    {
                        ColumnName="PartyA_CompanyID",
                        ColumnType="Guid?",

                        NewColumnName="PartyA_Company",
                        NewColumnType="Company",
                        NewColumnComment="甲方公司",

                        NewColumnType_Dto="CompanyDto",

                        NewColumnName_VM ="PartyA_CompanyName",
                        NewColumnType_VM ="string",

                        IsMapper=true,
                        MapperName="CompanyName",
                    },
                    new ColumnModel()
                    {
                        ColumnName="PartyB_CompanyID",
                        ColumnType="Guid?",

                        NewColumnType="Company",
                        NewColumnName="PartyB_Company",
                        NewColumnComment="乙方公司",

                        NewColumnType_Dto="CompanyDto",

                        NewColumnName_VM ="PartyB_CompanyName",
                        NewColumnType_VM ="string",

                        IsMapper=true,
                        MapperName="CompanyName",
                    },
                    new ColumnModel()
                    {
                        ColumnName="New",
                        ColumnType="int",

                        NewColumnType="int",
                        NewColumnName="AttachmentCount",
                        NewColumnComment="附件个数",

                        NewColumnName_VM ="AttachmentCount",
                    },
                    new ColumnModel()
                    {
                        NewColumnComment ="生效日期",

                        NewColumnName_VM ="StartTimeFormatter",
                        NewColumnType_VM ="string",
                    },
                    new ColumnModel()
                    {
                        NewColumnComment ="失效日期",

                        NewColumnName_VM ="EndTimeFormatter",
                        NewColumnType_VM ="string",
                    },
                }
            };
            domainModels.Add(domainModel);


            domainModel = new TableModel()
            {
                TableName = "Company",
                ExcludePropertys = new List<string>()
                {
                },
                List = new List<ColumnModel>
                {
                    new ColumnModel()
                    {
                        ColumnName="BrandID",
                        ColumnType="Guid?",

                        NewColumnName="Brand",
                        NewColumnType="Brand",
                        NewColumnComment="品牌",

                        NewColumnType_Dto="BrandDto",

                        NewColumnName_VM ="BrandName",
                        NewColumnType_VM ="string",

                        IsMapper=true,
                        MapperName="BrandName",
                    },
                    new ColumnModel()
                    {
                        ColumnName="Legal_CustomerID",
                        ColumnType="Guid?",

                        NewColumnName="Legal_Customer",
                        NewColumnType="Customer",
                        NewColumnComment="法定代表人",

                        NewColumnType_Dto="CustomerDto",

                        NewColumnName_VM ="Legal_CustomerName",
                        NewColumnType_VM ="string",

                        IsMapper=true,
                        MapperName="CustomerName",
                    },
                    new ColumnModel()
                    {
                        ColumnName="Actual_CustomerID",
                        ColumnType="Guid?",

                        NewColumnName="Actual_Customer",
                        NewColumnType="Customer",
                        NewColumnComment="实际负责人",

                        NewColumnType_Dto="CustomerDto",

                        NewColumnName_VM ="Actual_CustomerName",
                        NewColumnType_VM ="string",

                        IsMapper=true,
                        MapperName="CustomerName",
                    },
                }
            };
            domainModels.Add(domainModel);
            return domainModels;
        }
    }
}

using CodeCreate.Model;
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
    public class Create_Business
    {
        public void Create(string str_nameSpace, DataTable dt_tables, string tableName)
        {
            string tablePrefix = CommonCode.GetTablePrefix(tableName);
            tableName = CommonCode.GetTableName(tableName);
            string tableDesc = CommonCode.GetTableDesc(tableName);

            bool isPrimeKey = false;
            string primaryKey = "";
            bool isSortIndex = false;

            StringBuilder sb = new StringBuilder();
            StringBuilder sb_search = new StringBuilder();

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

                if (columnType == "string")
                {
                    sb.AppendLine("            if (!filter." + columnName + ".IsNullOrEmpty())");
                    sb.AppendLine("            {");
                    sb.AppendLine("                query.Equal<" + tableName + "Query>(c => c." + columnName + ", filter." + columnName + ");");
                    sb.AppendLine("            }");

                    sb_search.AppendLine("            if (!filter." + columnName + "_Search.IsNullOrEmpty())");
                    sb_search.AppendLine("            {");
                    sb_search.AppendLine("                query.Like<" + tableName + "Query>(c => c." + columnName + ", filter." + columnName + "_Search);");
                    sb_search.AppendLine("            }");
                }
                else if (columnName == "IsDelete")
                {
                    sb_search.AppendLine("");
                    sb_search.AppendLine("            query.Equal<" + tableName + "Query>(c => c." + columnName + ", false);");
                    sb_search.AppendLine("");
                }
                else
                {
                    sb.AppendLine("            if (filter." + columnName + ".HasValue)");
                    sb.AppendLine("            {");
                    sb.AppendLine("                query.Equal<" + tableName + "Query>(c => c." + columnName + ", filter." + columnName + ");");
                    sb.AppendLine("            }");

                }

                if (columnName == "SortIndex")
                {
                    isSortIndex = true;
                }
            }

            #endregion Model

            bool IsSetFilter = false;
            bool IsSaveHistory = true;

            StringBuilder sb2 = new StringBuilder();
            var listModel = CommonCode.GetTableModel(tableName);
            if (listModel != null)
            {
                foreach (var item in listModel)
                {
                    SetSB(sb2, item, tableName);

                    if (item.IsSetFilter.HasValue)
                    {
                        IsSetFilter = item.IsSetFilter.Value;
                    }
                    if (item.IsSaveHistory.HasValue)
                    {
                        IsSaveHistory = item.IsSaveHistory.Value;
                    }
                }
            }

            var tableModels = CommonCode.GetTableModel(tableName);
            foreach (var item in tableModels)
            {
            }

            StringBuilder sb_body = new StringBuilder();

            sb_body.AppendLine("using System;");
            sb_body.AppendLine("using System.Collections.Generic;");
            sb_body.AppendLine("using System.Linq;");
            sb_body.AppendLine("using System.Data.SqlClient;");
            sb_body.AppendLine("using System.Transactions;");
            sb_body.AppendLine("using Lee.Command.UnitOfWork;");
            sb_body.AppendLine("using Lee.CQuery;");
            sb_body.AppendLine("using Lee.CQuery.Paging;");
            sb_body.AppendLine("using Lee.Utility;");
            sb_body.AppendLine("using Lee.Utility.Extension;");
            sb_body.AppendLine("using " + str_nameSpace + ".BusinessInterface." + tablePrefix + ";");
            sb_body.AppendLine("using " + str_nameSpace + ".Domain." + tablePrefix + ".Model;");
            sb_body.AppendLine("using " + str_nameSpace + ".Domain." + tablePrefix + ".Service;");
            sb_body.AppendLine("using " + str_nameSpace + ".DTO." + tablePrefix + ".Query.Filter;");
            sb_body.AppendLine("using " + str_nameSpace + ".DTO." + tablePrefix + ".Cmd;");
            sb_body.AppendLine("using " + str_nameSpace + ".DTO." + tablePrefix + ".Query;");
            sb_body.AppendLine("using " + str_nameSpace + ".DTO.Query;");
            sb_body.AppendLine("using " + str_nameSpace + ".Query." + tablePrefix + ";");
            sb_body.AppendLine("using " + str_nameSpace + ".DTO.Bcl.Cmd;");
            sb_body.AppendLine("using " + str_nameSpace + ".Enum;");
            sb_body.AppendLine("using " + str_nameSpace + ".Domain.Bcl.Service;");
            sb_body.AppendLine("using " + str_nameSpace + ".Tools;");
            sb_body.AppendLine("using " + str_nameSpace + ".Tools.Helper;");
            sb_body.AppendLine("using " + str_nameSpace + ".Business.Common;");
            sb_body.AppendLine("");
            sb_body.AppendLine("namespace " + str_nameSpace + ".Business." + tablePrefix + "");
            sb_body.AppendLine("{");
            sb_body.AppendLine("    /// <summary>");
            sb_body.AppendLine("    /// 业务：" + tableDesc);
            sb_body.AppendLine("    /// </summary>");
            sb_body.AppendLine("    public class " + tableName + "Business : I" + tableName + "Business");
            sb_body.AppendLine("    {");
            sb_body.AppendLine("        public " + tableName + "Business()");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 保存");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 保存" + tableDesc);
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"saveInfo\">保存信息</param>");
            sb_body.AppendLine("        /// <returns>执行结果</returns>");
            sb_body.AppendLine("        public Result<" + tableName + "Dto> Save" + tableName + "(Save" + tableName + "CmdDto saveInfo)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            if (saveInfo == null)");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                return Result<" + tableName + "Dto>.ErrorResult(\"没有指定任何要保存的信息\");");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("            using (var businessWork = UnitOfWork.Create())");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                var saveResult = " + tableName + "Service.Save" + tableName + "(saveInfo." + tableName + ".MapTo<" + tableName + ">());");
            sb_body.AppendLine("                if (!saveResult.Success)");
            sb_body.AppendLine("                {");
            sb_body.AppendLine("                    return Result<" + tableName + "Dto>.ErrorResult(saveResult.Message);");
            sb_body.AppendLine("                }");
            sb_body.AppendLine("");
            sb_body.AppendLine("                SaveHistory(saveInfo." + tableName + ", saveResult.Data.SysNo);");
            sb_body.AppendLine("");
            sb_body.AppendLine("                var commitResult = businessWork.Commit();");
            sb_body.AppendLine("                Result<" + tableName + "Dto> result = null;");
            sb_body.AppendLine("                if (commitResult.ExecutedSuccess)");
            sb_body.AppendLine("                {");
            sb_body.AppendLine("                    result = Result<" + tableName + "Dto>.SuccessResult(\"保存成功！\");");
            sb_body.AppendLine("                    result.Data = saveResult.Data.MapTo<" + tableName + "Dto>();");
            sb_body.AppendLine("                }");
            sb_body.AppendLine("                else");
            sb_body.AppendLine("                {");
            sb_body.AppendLine("                    result = Result<" + tableName + "Dto>.ErrorResult(\"保存失败！\");");
            sb_body.AppendLine("                }");
            sb_body.AppendLine("                return result;");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 批量保存" + tableDesc);
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"saveInfo\">保存信息</param>");
            sb_body.AppendLine("        /// <returns>执行结果</returns>");
            sb_body.AppendLine("        public Result SaveList" + tableName + "(Save" + tableName + "CmdDto saveInfo)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            if (saveInfo == null || saveInfo.List" + tableName + ".IsNullOrEmpty())");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                return Result.ErrorResult(\"没有指定任何要保存的信息\");");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("");
            sb_body.AppendLine("            try");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                saveInfo.List" + tableName + ".Where(d => d.SysNo == Guid.Empty).ToList().ForEach(d => d.SysNo = Guid.NewGuid());");
            sb_body.AppendLine("                var bulk = new SqlBulkTools.BulkOperations();");
            sb_body.AppendLine("");
            sb_body.AppendLine("                using (TransactionScope trans = new TransactionScope())");
            sb_body.AppendLine("                {");
            sb_body.AppendLine("                    using (SqlConnection conn = new SqlConnection(Keys.connectionString))");
            sb_body.AppendLine("                    {");
            sb_body.AppendLine("                        bulk.Setup<" + tableName + "CmdDto>()");
            sb_body.AppendLine("                            .ForCollection(saveInfo.List" + tableName + ")");
            sb_body.AppendLine("                            .WithTable(\"" + tablePrefix + "_" + tableName + "\")");
            sb_body.AppendLine("                            .AddAllColumns()");
            sb_body.AppendLine("                            .BulkInsertOrUpdate()");
            sb_body.AppendLine("                            .SetIdentityColumn(x => x.SysNo)");
            sb_body.AppendLine("                            .MatchTargetOn(x => x.SysNo)");
            sb_body.AppendLine("                            .Commit(conn);");
            sb_body.AppendLine("                    }");
            sb_body.AppendLine("                    trans.Complete();");
            sb_body.AppendLine("                }");
            sb_body.AppendLine("");

            if (IsSaveHistory)
            {
                sb_body.AppendLine("                using (var businessWork = UnitOfWork.Create())");
                sb_body.AppendLine("                {");
                sb_body.AppendLine("                    foreach (var item in saveInfo.List" + tableName + ")");
                sb_body.AppendLine("                    {");
                sb_body.AppendLine("                        SaveHistory(item, item.SysNo);");
                sb_body.AppendLine("                    }");
                sb_body.AppendLine("                    var commitResult = businessWork.Commit();");
                sb_body.AppendLine("                }");
            }
            sb_body.AppendLine("                return Result.SuccessResult(\"修改成功！\");");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("            catch (Exception ex)");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                LogHelper.WriteError(ex);");
            sb_body.AppendLine("                return Result.ErrorResult(\"修改失败！\" + ex.Message);");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            //sb_body.AppendLine("        /// <summary>");
            //sb_body.AppendLine("        /// 批量保存" + tableDesc);
            //sb_body.AppendLine("        /// </summary>");
            //sb_body.AppendLine("        /// <param name=\"saveInfo\">保存信息</param>");
            //sb_body.AppendLine("        /// <returns>执行结果</returns>");
            //sb_body.AppendLine("        public Result SaveList" + tableName + "(Save" + tableName + "CmdDto saveInfo)");
            //sb_body.AppendLine("        {");
            //sb_body.AppendLine("            if (saveInfo == null || saveInfo.List" + tableName + ".IsNullOrEmpty())");
            //sb_body.AppendLine("            {");
            //sb_body.AppendLine("                return Result.ErrorResult(\"没有指定任何要保存的信息\");");
            //sb_body.AppendLine("            }");
            //sb_body.AppendLine("            using (var businessWork = UnitOfWork.Create())");
            //sb_body.AppendLine("            {");
            //sb_body.AppendLine("                var saveResult = " + tableName + "Service.SaveList" + tableName + "(saveInfo.List" + tableName + ".Select(c => c.MapTo<" + tableName + ">()));");
            //sb_body.AppendLine("                if (!saveResult.Success)");
            //sb_body.AppendLine("                {");
            //sb_body.AppendLine("                    return saveResult;");
            //sb_body.AppendLine("                }");
            //sb_body.AppendLine("");
            //sb_body.AppendLine("                foreach (var item in saveInfo.List" + tableName + ")");
            //sb_body.AppendLine("                {");
            //sb_body.AppendLine("                    SaveHistory(item, item.SysNo);");
            //sb_body.AppendLine("                }");
            //sb_body.AppendLine("");
            //sb_body.AppendLine("                var commitResult = businessWork.Commit();");
            //sb_body.AppendLine("                return commitResult.ExecutedSuccess ? Result.SuccessResult(\"修改成功\") : Result.ErrorResult(\"修改失败\");");
            //sb_body.AppendLine("");
            //sb_body.AppendLine("            }");
            //sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion 保存");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 获取");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 获取" + tableDesc);
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"filter\">查询条件</param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        public " + tableName + "Dto Get" + tableName + "(" + tableName + "FilterDto filter)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            var " + tableName + " = " + tableName + "Service.Get" + tableName + "(CreateQueryObject(filter));");
            sb_body.AppendLine("            return " + tableName + ".MapTo<" + tableName + "Dto>();");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion 获取");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 获取列表");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 获取" + tableDesc + "列表");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"filter\">查询条件</param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        public List<" + tableName + "Dto> Get" + tableName + "List(" + tableName + "FilterDto filter)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            var " + tableName + "List = " + tableName + "Service.Get" + tableName + "List(CreateQueryObject(filter));");
            sb_body.AppendLine("            return " + tableName + "List.Select(c => c.MapTo<" + tableName + "Dto>()).ToList();");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion 获取列表");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 获取分页");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 获取" + tableDesc + "分页");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"filter\">查询条件</param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        public IPaging<" + tableName + "Dto> Get" + tableName + "Paging(" + tableName + "FilterDto filter)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            var " + tableName + "Paging = " + tableName + "Service.Get" + tableName + "Paging(CreateQueryObject(filter));");
            sb_body.AppendLine("            return " + tableName + "Paging.Convert<" + tableName + ", " + tableName + "Dto>();");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion 获取分页");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 删除");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 删除" + tableDesc);
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"deleteInfo\">删除信息</param>");
            sb_body.AppendLine("        /// <returns>执行结果</returns>");
            sb_body.AppendLine("        public Result Delete" + tableName + "(Delete" + tableName + "CmdDto deleteInfo)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            if (!deleteInfo.IsRealDeleted)");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                return Delete" + tableName + "2(deleteInfo);");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("");
            sb_body.AppendLine("            #region 参数判断");
            sb_body.AppendLine("");
            sb_body.AppendLine("            if (deleteInfo == null || deleteInfo." + tableName + "Ids.IsNullOrEmpty())");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                return Result.ErrorResult(\"没有指定要删除的\");");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("");
            sb_body.AppendLine("            #endregion 参数判断");
            sb_body.AppendLine("");
            sb_body.AppendLine("            using (var businessWork = UnitOfWork.Create())");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                var " + tableName + "s = deleteInfo." + tableName + "Ids.Select(c => " + tableName + ".Create" + tableName + "(c));");
            sb_body.AppendLine("                var deleteResult = " + tableName + "Service.Delete" + tableName + "(" + tableName + "s);");
            sb_body.AppendLine("                if (!deleteResult.Success)");
            sb_body.AppendLine("                {");
            sb_body.AppendLine("                    return deleteResult;");
            sb_body.AppendLine("                }");
            sb_body.AppendLine("");
            sb_body.AppendLine("                SaveDeleteHistory(deleteInfo);");
            sb_body.AppendLine("");
            sb_body.AppendLine("                var commitResult = businessWork.Commit();");
            sb_body.AppendLine("                return commitResult.ExecutedSuccess ? Result.SuccessResult(\"删除成功\") : Result.ErrorResult(\"删除失败\");");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 删除客户（假删除）");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"deleteInfo\"></param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        public Result Delete" + tableName + "2(Delete" + tableName + "CmdDto deleteInfo)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            #region 参数判断");
            sb_body.AppendLine("");
            sb_body.AppendLine("            if (deleteInfo == null || deleteInfo." + tableName + "Ids.IsNullOrEmpty())");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                return Result.ErrorResult(\"没有指定要删除的\");");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("");
            sb_body.AppendLine("            #endregion 参数判断");
            sb_body.AppendLine("");
            sb_body.AppendLine("            try");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                var List" + tableName + " = deleteInfo." + tableName + "Ids.Select(c => " + tableName + ".Create" + tableName + "(c)).MapTo<List<" + tableName + "CmdDto>>();");
            sb_body.AppendLine("                List" + tableName + ".ForEach(d => d.UpdateDate = DateTime.Now);");
            sb_body.AppendLine("                List" + tableName + ".ForEach(d => d.UpdateUserID = deleteInfo.UpdateUserID);");
            sb_body.AppendLine("                List" + tableName + ".ForEach(d => d.IsDelete = true);");
            sb_body.AppendLine("");
            sb_body.AppendLine("                var bulk = new SqlBulkTools.BulkOperations();");
            sb_body.AppendLine("");
            sb_body.AppendLine("                using (TransactionScope trans = new TransactionScope())");
            sb_body.AppendLine("                {");
            sb_body.AppendLine("                    using (SqlConnection conn = new SqlConnection(Keys.connectionString))");
            sb_body.AppendLine("                    {");
            sb_body.AppendLine("                        bulk.Setup<" + tableName + "CmdDto>()");
            sb_body.AppendLine("                            .ForCollection(List" + tableName + ")");
            sb_body.AppendLine("                            .WithTable(\"" + tablePrefix + "_" + tableName + "\")");
            sb_body.AppendLine("                            .AddColumn(d => d.UpdateDate)");
            sb_body.AppendLine("                            .AddColumn(d => d.UpdateUserID)");
            sb_body.AppendLine("                            .AddColumn(d => d.IsDelete)");
            sb_body.AppendLine("                            .BulkUpdate()");
            sb_body.AppendLine("                            .SetIdentityColumn(x => x.SysNo)");
            sb_body.AppendLine("                            .MatchTargetOn(x => x.SysNo)");
            sb_body.AppendLine("                            .Commit(conn);");
            sb_body.AppendLine("                    }");
            sb_body.AppendLine("                    trans.Complete();");
            sb_body.AppendLine("                }");
            sb_body.AppendLine("");
            if (IsSaveHistory)
            {
                sb_body.AppendLine("                using (var businessWork = UnitOfWork.Create())");
                sb_body.AppendLine("                {");
                sb_body.AppendLine("                    SaveDeleteHistory(deleteInfo);");
                sb_body.AppendLine("                    var commitResult = businessWork.Commit();");
                sb_body.AppendLine("                }");
            }
            sb_body.AppendLine("");
            sb_body.AppendLine("                return Result.SuccessResult(\"删除成功！\");");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("            catch (Exception ex)");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                LogHelper.WriteError(ex);");
            sb_body.AppendLine("                return Result.ErrorResult(\"删除失败！\" + ex.Message);");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("        }");

            sb_body.AppendLine("        #endregion 删除");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #region 历史记录");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 保存时，记录历史记录");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"saveInfo\"></param>");
            sb_body.AppendLine("        /// <param name=\"SysNo\"></param>");
            sb_body.AppendLine("        private static void SaveHistory(" + tableName + "CmdDto saveInfo, Guid SysNo)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            SaveHistoryCmdDto saveHistoryCmdDto = new SaveHistoryCmdDto()");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                History = new HistoryCmdDto()");
            sb_body.AppendLine("                {");
            sb_body.AppendLine("                    OperationID = SysNo,");
            sb_body.AppendLine("                    OperationType = OperationTypeEnum." + tableName + ".ToString(),");
            sb_body.AppendLine("                    OperationName = saveInfo.SysNo == Guid.Empty ? OperationNameEnum.Add.ToString() : OperationNameEnum.Edit.ToString(),");
            sb_body.AppendLine("                    CreateUserID = saveInfo.UpdateUserID ?? Guid.Empty,");
            sb_body.AppendLine("                }");
            sb_body.AppendLine("            };");
            sb_body.AppendLine("            HistoryService.SaveHistory(saveHistoryCmdDto.History.MapTo<Domain.Bcl.Model.History>());");
            sb_body.AppendLine("");
            sb_body.AppendLine("            LogHelper.WriteHistory(\"Save" + tableName + "\", JsonConvertHelper.SerializeObject(saveInfo), saveInfo.UpdateUserID);");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 删除时，记录历史记录");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"deleteInfo\"></param>");
            sb_body.AppendLine("        private void SaveDeleteHistory(Delete" + tableName + "CmdDto deleteInfo)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            foreach (var item in deleteInfo." + tableName + "Ids)");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                SaveHistoryCmdDto saveHistoryCmdDto = new SaveHistoryCmdDto()");
            sb_body.AppendLine("                {");
            sb_body.AppendLine("                    History = new HistoryCmdDto()");
            sb_body.AppendLine("                    {");
            sb_body.AppendLine("                        OperationID = item,");
            sb_body.AppendLine("                        OperationType = OperationTypeEnum." + tableName + ".ToString(),");
            sb_body.AppendLine("                        OperationName = OperationNameEnum.Delete.ToString(),");
            sb_body.AppendLine("                        CreateUserID = deleteInfo.UpdateUserID,");
            sb_body.AppendLine("                    }");
            sb_body.AppendLine("                };");
            sb_body.AppendLine("                HistoryService.SaveHistory(saveHistoryCmdDto.History.MapTo<Domain.Bcl.Model.History>());");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("");
            sb_body.AppendLine("            LogHelper.WriteHistory(\"Delete" + tableName + "\", JsonConvertHelper.SerializeObject(deleteInfo), deleteInfo.UpdateUserID);");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion 历史记录");
            sb_body.AppendLine("");

            sb_body.AppendLine("        #region 根据查询条件生成查询对象");
            sb_body.AppendLine("");
            sb_body.AppendLine("        /// <summary>");
            sb_body.AppendLine("        /// 根据查询条件生成查询对象");
            sb_body.AppendLine("        /// </summary>");
            sb_body.AppendLine("        /// <param name=\"filter\">查询条件</param>");
            sb_body.AppendLine("        /// <returns></returns>");
            sb_body.AppendLine("        private IQuery CreateQueryObject(" + tableName + "FilterDto filter)");
            sb_body.AppendLine("        {");
            sb_body.AppendLine("            if (filter == null)");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                return null;");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("            IQuery query = QueryFactory.Create<" + tableName + "Query>(filter);");
            sb_body.AppendLine("            if (!filter.SysNos.IsNullOrEmpty())");
            sb_body.AppendLine("            {");
            sb_body.AppendLine("                query.In<" + tableName + "Query>(c => c.SysNo, filter.SysNos);");
            sb_body.AppendLine("            }");

            sb_body.AppendLine(sb.ToString());
            sb_body.AppendLine("");

            if (IsSetFilter)
            {
                sb_body.AppendLine("            CommonSetFilter.SetFilter_" + tableName + "(filter, query);");
            }

            sb_body.AppendLine("");
            sb_body.AppendLine("            #region Search");
            sb_body.AppendLine("");
            sb_body.AppendLine(sb_search.ToString());
            sb_body.AppendLine("");
            sb_body.AppendLine("            #endregion");

            sb_body.AppendLine("");
            sb_body.AppendLine("            #region 数据加载");


            sb_body.AppendLine(sb2.ToString());
            sb_body.AppendLine("            #endregion");
            sb_body.AppendLine("");

            if (isSortIndex)
            {
                sb_body.AppendLine("            query.Desc<" + tableName + "Query>(c => c.SortIndex);");
            }
            sb_body.AppendLine("            query.Desc<" + tableName + "Query>(c => c.CreateDate);");
            sb_body.AppendLine("            return query;");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        #endregion 根据查询条件生成查询对象");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("}");
            sb_body.AppendLine("");



            string file_Model = "C:\\Code\\" + str_nameSpace + ".Business\\" + tablePrefix + "";
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }
            CommonCode.Save(file_Model + "/" + tableName + "Business.cs", sb_body.ToString());
        }

        private static void SetSB(StringBuilder sb, TableModel tableModel, string tableName)
        {
            if (tableModel.List != null)
            {
                foreach (var thisModel in tableModel.List.Where(d => !string.IsNullOrEmpty(d.NewColumnName)))
                {
                    sb.AppendLine("");
                    sb.AppendLine("            if (filter.IsLoad" + thisModel.NewColumnName + ")");
                    sb.AppendLine("            {");
                    sb.AppendLine("                query.SetLoadPropertys<" + tableName + ">(true, c => c." + thisModel.NewColumnName + ");");
                    sb.AppendLine("            }");
                }
            }
        }

    }

}
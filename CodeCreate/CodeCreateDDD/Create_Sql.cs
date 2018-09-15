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
    public class Create_Sql
    {
        public void Create(string str_nameSpace, DataTable dt_tables, string tableName)
        {
            string tablePrefix = CommonCode.GetTablePrefix(tableName); tableName = CommonCode.GetTableName(tableName);

            string tableDesc = CommonCode.GetTableDesc(tableName);

            StringBuilder sb_body = new StringBuilder();
            SetData(tableName, tablePrefix, tableDesc, sb_body);

            string filePath = "C:\\Code\\Sql";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            CommonCode.Save(filePath + "/" + tableName + ".txt", sb_body.ToString());

            CommonCode.Save(filePath + "/" + tableName + "_Delete.txt", GetSql_Delete(tableDesc));

        }

        public static void SetData(string tableName, string tablePrefix, string tableDesc, StringBuilder sb_body)
        {

            //授权
            sb_body.AppendLine(GetSql_Sys_AuthorityOperationGroup(tableDesc, 10, "基础配置"));
            sb_body.AppendLine(GetSql_Sys_AuthorityOperation(tablePrefix, tableName, tableDesc + "列表", tableDesc));
            sb_body.AppendLine(GetSql_Sys_AuthorityOperation(tablePrefix, "Get" + tableName + "Paging", tableDesc + "列表数据", tableDesc));

            sb_body.AppendLine(GetSql_Sys_AuthorityOperation(tablePrefix, "Edit" + tableName, "添加、修改" + tableDesc, tableDesc));
            sb_body.AppendLine(GetSql_Sys_AuthorityOperation(tablePrefix, "Delete" + tableName, "删除" + tableDesc, tableDesc));
            sb_body.AppendLine(GetSql_Sys_AuthorityOperation(tablePrefix, "Get" + tableName + "List", tableDesc + "下拉框", tableDesc));

            //权限
            sb_body.AppendLine(GetSql_Sys_AuthorityGroup(tableDesc, 10, "基础配置"));

            sb_body.AppendLine(GetSql_Sys_Authority(tableDesc + "列表", tableDesc));
            sb_body.AppendLine(GetSql_Sys_Authority("添加、修改" + tableDesc, tableDesc));
            sb_body.AppendLine(GetSql_Sys_Authority("删除" + tableDesc, tableDesc));

            //添加关联
            sb_body.AppendLine(GetSql_Sys_AuthorityBindOperation(tableDesc + "列表"));
            sb_body.AppendLine(GetSql_Sys_AuthorityBindOperation(tableDesc + "列表数据", tableDesc + "列表"));
            //sb_body.AppendLine(GetSql_Sys_AuthorityBindOperation(tableDesc + "下拉框", tableDesc + "列表"));

            sb_body.AppendLine(GetSql_Sys_AuthorityBindOperation("添加、修改" + tableDesc));
            sb_body.AppendLine(GetSql_Sys_AuthorityBindOperation("删除" + tableDesc));
        }

        public static void SetData2(string tableName, string tablePrefix, string desc, StringBuilder sb_body)
        {

            //授权
            sb_body.AppendLine(GetSql_Sys_AuthorityOperationGroup(desc, 10, "基础配置"));
            sb_body.AppendLine(GetSql_Sys_AuthorityOperation(tablePrefix, tableName, desc, desc));

            //权限
            sb_body.AppendLine(GetSql_Sys_AuthorityGroup(desc, 10, "基础配置"));

            sb_body.AppendLine(GetSql_Sys_Authority(desc, desc));

            //添加关联
            sb_body.AppendLine(GetSql_Sys_AuthorityBindOperation(desc));
        }

        private static string GetSql_Sys_AuthorityOperationGroup(string Name, int Sort, string ParentName)
        {
            StringBuilder sb_body = new StringBuilder();
            sb_body.AppendLine("");
            sb_body.AppendLine("  --插入授权组  ");
            sb_body.AppendLine("  INSERT INTO [dbo].[Sys_AuthorityOperationGroup]([SysNo],[Name],[Sort],[Parent],[Root],[Level],[Status],[Application],[Remark],[CreateDate])");
            sb_body.AppendLine("     VALUES (newid(),'" + Name + "'," + Sort + ",");
            sb_body.AppendLine("	 (select [SysNo] FROM [dbo].[Sys_AuthorityOperationGroup] where [name]='" + ParentName + "' and Parent='00000000-0000-0000-0000-000000000000' ),");
            sb_body.AppendLine("	 (select [SysNo] FROM [dbo].[Sys_AuthorityOperationGroup] where [name]='" + ParentName + "' and Parent='00000000-0000-0000-0000-000000000000' ),");
            sb_body.AppendLine("	 2,310,1,null,getdate()");
            sb_body.AppendLine("  );");
            sb_body.AppendLine("");
            return sb_body.ToString();
        }

        private static string GetSql_Sys_AuthorityOperation(string ControllerCode, string ActionCode, string Name, string GroupName)
        {
            StringBuilder sb_body = new StringBuilder();
            sb_body.AppendLine("  --插入授权");
            sb_body.AppendLine("  insert into [dbo].[Sys_AuthorityOperation] ([SysNo],[ControllerCode],[ActionCode],[Method],[Name],[Status],[Sort],[Group],[AuthorizeType],[Application],[Remark],[CreateDate]) ");
            sb_body.AppendLine("  values(newid(),'" + ControllerCode + "','" + ActionCode.ToUpper() + "',430,'" + Name + "',310,0,");
            sb_body.AppendLine("  (select [SysNo] FROM [dbo].[Sys_AuthorityOperationGroup] where [name]='" + GroupName + "'),");
            sb_body.AppendLine("  520,1,null,getdate()) ");
            sb_body.AppendLine("");
            return sb_body.ToString();
        }

        private static string GetSql_Sys_AuthorityGroup(string Name, int Sort, string ParentName)
        {
            StringBuilder sb_body = new StringBuilder();
            sb_body.AppendLine("");
            sb_body.AppendLine("  --插入权限组  ");
            sb_body.AppendLine("  INSERT INTO [dbo].[Sys_AuthorityGroup]([SysNo],[Name],[SortIndex],[Status],[Parent],[Level],[Application],[Remark],[CreateDate])");
            sb_body.AppendLine("     VALUES (newid(),'" + Name + "'," + Sort + ",310,");
            sb_body.AppendLine("	 (select [SysNo] FROM [dbo].[Sys_AuthorityGroup] where [name]='" + ParentName + "' and Parent='00000000-0000-0000-0000-000000000000' ),");
            sb_body.AppendLine("	 2,1,null,getdate()");
            sb_body.AppendLine("  );");

            return sb_body.ToString();
        }

        private static string GetSql_Sys_Authority(string Name, string ParentName)
        {
            StringBuilder sb_body = new StringBuilder();
            sb_body.AppendLine("");
            sb_body.AppendLine("  --插入权限");
            sb_body.AppendLine("  insert into [dbo].[Sys_Authority] ([Code],[Name],[AuthType],[Status],[Sort],[AuthGroup],[CreateDate],[Application],[Remark]) ");
            sb_body.AppendLine("  values(newid(),'" + Name + "',410,310,0,");
            sb_body.AppendLine("  (select [SysNo] FROM [BigDataAnalysis].[dbo].[Sys_AuthorityGroup] where [name]='" + ParentName + "'),");
            sb_body.AppendLine("  getdate(),1,null) ");

            return sb_body.ToString();
        }

        private static string GetSql_Sys_AuthorityBindOperation(string Name_AuthorityOperation, string Name_Authority = "")
        {
            if (Name_Authority == "")
            {
                Name_Authority = Name_AuthorityOperation;
            }
            StringBuilder sb_body = new StringBuilder();
            sb_body.AppendLine("    --插入授权、权限关系");
            sb_body.AppendLine("  INSERT INTO [dbo].[Sys_AuthorityBindOperation]([AuthorithOperation],[AuthorityCode],[CreateDate])");
            sb_body.AppendLine("     VALUES (");
            sb_body.AppendLine("	 (select [SysNo] FROM [dbo].[Sys_AuthorityOperation] where [name]='" + Name_AuthorityOperation + "' ),");
            sb_body.AppendLine("	 (select [Code] FROM [dbo].[Sys_Authority] where [name]='" + Name_Authority + "' ),");
            sb_body.AppendLine("	 getdate()");
            sb_body.AppendLine(");");

            return sb_body.ToString();
        }


        public static string GetSql_Delete(string Name)
        {
            StringBuilder sb_body = new StringBuilder();
            sb_body.AppendLine("");
            sb_body.AppendLine("  --删除");
            sb_body.AppendLine("  delete from [Sys_AuthorityBindOperation] where [AuthorithOperation] in(select [SysNo] FROM [dbo].[Sys_AuthorityOperation] where [name] like '%" + Name + "%' )");
            sb_body.AppendLine("  delete from [dbo].[Sys_AuthorityOperationGroup] where [Name]='" + Name + "'");
            sb_body.AppendLine("  delete from [dbo].[Sys_AuthorityOperation] where [Name] like '%" + Name + "%'");
            sb_body.AppendLine("  delete from [dbo].[Sys_AuthorityGroup] where [Name]='" + Name + "'");
            sb_body.AppendLine("  delete from [dbo].[Sys_Authority] where [Name] like '%" + Name + "%'");

            return sb_body.ToString();
        }
    }
}
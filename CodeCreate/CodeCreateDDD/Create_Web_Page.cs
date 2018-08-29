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
    public class Create_Web_Page
    {
        public void Create(string str_nameSpace, DataTable dt_tables, string tableName)
        {
            string tablePrefix = CommonCode.GetTablePrefix(tableName); tableName = CommonCode.GetTableName(tableName);
            string tableDesc = CommonCode.GetTableDesc(tableName);

            bool isPrimeKey = false;
            string primaryKey = "";

            StringBuilder sb_GetEditData = new StringBuilder();
            StringBuilder sb_SetEditData = new StringBuilder();
            StringBuilder sb_Table_Th = new StringBuilder();
            StringBuilder sb_EditDiv = new StringBuilder();
            StringBuilder sb_Formatter = new StringBuilder();

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

                string required = "true";
                if (columnName == "Remark")
                {
                    required = "false";
                }

                var arr = new List<string> {
                    "SysNo",
                    "CreateDate",
                    "UpdateDate",
                    "Status",
                    "Enable",
                    "IsDeleted",
                    "CreateUserID",
                    "UpdateUserID",
                };
                if (!arr.Contains(columnName))
                {
                    if (columnType == "bool")
                    {
                        sb_GetEditData.AppendLine("            " + columnName + ": $(\"." + columnName + "\").combobox(\"getValue\"),");

                        sb_SetEditData.AppendLine("        $(\"." + columnName + "\").combobox(\"select\", row." + columnName + ");");

                        sb_Formatter.AppendLine("    function formatter" + columnName + "(value, row, index) {");
                        sb_Formatter.AppendLine("        if (value == true) {");
                        sb_Formatter.AppendLine("            return \"是\";");
                        sb_Formatter.AppendLine("        } else {");
                        sb_Formatter.AppendLine("            return \"否\";");
                        sb_Formatter.AppendLine("        }");
                        sb_Formatter.AppendLine("        return value;");
                        sb_Formatter.AppendLine("    }");

                        sb_Table_Th.AppendLine("                <th data-options=\"field:'" + columnName + "',width:100,align:'center',formatter:formatter" + columnName + "\">" + columnComment + "</th>");

                        sb_EditDiv.AppendLine("                <div style=\"margin-top: 10px;\">");
                        sb_EditDiv.AppendLine("                    <input class=\"easyui-combobox " + columnName + "\" name=\"" + columnName + "\" style=\"width: 300px;\" data-options=\"label:'" + columnComment + "',required:" + required + ",editable:false,valueField:'value',textField:'label',data:[{'value':true,label:'是',selected:true},{'value':false,label:'否'}],panelHeight:true\" />");
                        sb_EditDiv.AppendLine("                </div>");
                    }
                    else
                    {
                        if (columnName.Contains("ID"))
                        {
                            sb_GetEditData.AppendLine("            " + columnName + ": $(\"." + columnName + "\").combobox(\"getValue\"),");
                            sb_SetEditData.AppendLine("        $(\"." + columnName + "\").combobox(\"select\", row." + columnName + ");");

                            sb_EditDiv.AppendLine("            <div style=\"margin-top: 10px;\">");
                            sb_EditDiv.AppendLine("                <input class=\"easyui-combobox " + columnName + "\" name=\"" + columnName + "\" data-options=\"label:'" + columnComment + "',editable:false,valueField:'SysNo',textField:'" + columnName.Replace("ID", "Name") + "',method:'get',url:'/Data/Get" + columnName.Replace("ID", "") + "List',panelHeight:true\" style=\"width: 300px;\" />");
                            sb_EditDiv.AppendLine("            </div>");
                        }
                        else
                        {
                            sb_GetEditData.AppendLine("            " + columnName + ": $(\"." + columnName + "\").textbox(\"getText\"),");
                            sb_SetEditData.AppendLine("        $(\"." + columnName + "\").textbox(\"setText\", row." + columnName + ");");

                            sb_EditDiv.AppendLine("            <div style=\"margin-top: 10px;\">");
                            sb_EditDiv.AppendLine("                <input class=\"easyui-textbox " + columnName + "\" name=\"" + columnName + "\" data-options=\"label:'" + columnComment + "',prompt:'请输入" + columnComment + "',required:" + required + ",validType:['length[1,"+ data_maxLength + "]'],missingMessage:'请输入" + columnComment + "'\" style=\"width: 300px;\" />");
                            sb_EditDiv.AppendLine("            </div>");
                        }

                        sb_Table_Th.AppendLine("                <th data-options=\"field:'" + columnName + "',width:100,align:'center'\">" + columnComment + "</th>");
                    }
                }
            }

            #endregion Model

            StringBuilder sb_body = new StringBuilder();
            sb_body.AppendLine("@{");
            sb_body.AppendLine("    ViewBag.Title = \"" + tableDesc + "详情\";");
            sb_body.AppendLine("    Layout = \"~/Views/Shared/_Layout.cshtml\";");
            sb_body.AppendLine("}");
            sb_body.AppendLine("");
            sb_body.AppendLine("<script>");
            sb_body.AppendLine("");
            sb_body.AppendLine("    $(function () {");
            sb_body.AppendLine("");
            sb_body.AppendLine("        //加载表格");
            sb_body.AppendLine("        grid(\"#dtGrid\", \"#dtGridToolber\");");
            sb_body.AppendLine("");
            sb_body.AppendLine("        //添加");
            sb_body.AppendLine("        $(\"#add\").on(\"click\", function () {");
            sb_body.AppendLine("            content(\"添加" + tableDesc + "\", \"insertRow\", true);");
            sb_body.AppendLine("        });");
            sb_body.AppendLine("");
            sb_body.AppendLine("        //搜索");
            sb_body.AppendLine("        $(\"#searchList\").on(\"click\", function () {");
            sb_body.AppendLine("            $('#dtGrid').datagrid('load', GetQueryParams());");
            sb_body.AppendLine("        });");
            sb_body.AppendLine("");
            sb_body.AppendLine("        //删除选中");
            sb_body.AppendLine("        $('#remove').linkbutton({");
            sb_body.AppendLine("            onClick: function () {");
            sb_body.AppendLine("                var gridID = \"#dtGrid\";");
            sb_body.AppendLine("                var rows = $(gridID).datagrid('getSelections');");
            sb_body.AppendLine("");
            sb_body.AppendLine("                if (rows.length > 0) {");
            sb_body.AppendLine("                    layer.alert(\"确定删除选中的" + tableDesc + "吗\", {");
            sb_body.AppendLine("                        icon: 3,");
            sb_body.AppendLine("                        btn: ['确定', '取消'],");
            sb_body.AppendLine("                        yes: function (j) {");
            sb_body.AppendLine("                            deleSelected(gridID, rows);");
            sb_body.AppendLine("                        }");
            sb_body.AppendLine("                    })");
            sb_body.AppendLine("");
            sb_body.AppendLine("                } else {");
            sb_body.AppendLine("                    layer.alert('请选择要删除的" + tableDesc + "', { icon: 0, time: 2000 });");
            sb_body.AppendLine("                }");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("        })");
            sb_body.AppendLine("    });");
            sb_body.AppendLine("");
            sb_body.AppendLine("    //删除选中");
            sb_body.AppendLine("    function deleSelected(gridID, rows) {");
            sb_body.AppendLine("        var " + tableName + "Ids = new Array();");
            sb_body.AppendLine("        for (var i = 0; i < rows.length; i++) {");
            sb_body.AppendLine("            " + tableName + "Ids.push(rows[i].SysNo);");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        var parm = {");
            sb_body.AppendLine("            " + tableName + "Ids: " + tableName + "Ids,");
            sb_body.AppendLine("        }");
            sb_body.AppendLine("");
            sb_body.AppendLine("        $.ajax({");
            sb_body.AppendLine("            type: \"Post\",");
            sb_body.AppendLine("            url: \"Delete" + tableName + "\",");
            sb_body.AppendLine("            dataType: \"json\",");
            sb_body.AppendLine("            data: JSON.stringify(parm),");
            sb_body.AppendLine("            contentType: \"application/json\",");
            sb_body.AppendLine("            success: function (data) {");
            sb_body.AppendLine("                if (data.Success) {");
            sb_body.AppendLine("                    for (var i = 0; i < rows.length; i++) {");
            sb_body.AppendLine("                        var _index = $(gridID).datagrid(\"getRowIndex\", rows[i]);");
            sb_body.AppendLine("                        $(gridID).datagrid('deleteRow', _index);");
            sb_body.AppendLine("                    }");
            sb_body.AppendLine("                    layer.close(closeIndex);");
            sb_body.AppendLine("                } else {");
            sb_body.AppendLine("                    layer.alert(data.Message);");
            sb_body.AppendLine("                }");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("        });");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("");
            sb_body.AppendLine("    //实例化表格");
            sb_body.AppendLine("    function grid(id, toolbar) {");
            sb_body.AppendLine("        $(id).datagrid({");
            sb_body.AppendLine("            url: 'Get" + tableName + "Paging',");
            sb_body.AppendLine("            method: 'get',");
            sb_body.AppendLine("            height: \"100%\",");
            sb_body.AppendLine("            width: \"100%\",");
            sb_body.AppendLine("            autoRowHeight: true,");
            sb_body.AppendLine("            fitColumns: true,");
            sb_body.AppendLine("            collapsible: true,");
            sb_body.AppendLine("            pagination: true,");
            sb_body.AppendLine("            pageSize: 10,");
            sb_body.AppendLine("            queryParams: GetQueryParams(),");
            sb_body.AppendLine("            striped: true,");
            sb_body.AppendLine("            loading: true,");
            sb_body.AppendLine("            singleSelect: false,");
            sb_body.AppendLine("            //nowrap:true,");
            sb_body.AppendLine("            scrollbarSize: 0,");
            sb_body.AppendLine("            toolbar: toolbar,");
            sb_body.AppendLine("            onLoadSuccess: function (data) {");
            sb_body.AppendLine("                if (data.total > 0) {");
            sb_body.AppendLine("                    //加载完成 实例化按钮");
            sb_body.AppendLine("                    handle_edit(id);");
            sb_body.AppendLine("                }");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("        })");
            sb_body.AppendLine("    };");
            sb_body.AppendLine("");
            sb_body.AppendLine("    //弹框");
            sb_body.AppendLine("    function content(title, type, check, _index) {");
            sb_body.AppendLine("        layer.open({");
            sb_body.AppendLine("            type: 1,");
            sb_body.AppendLine("            area: [],");
            sb_body.AppendLine("            title: title,");
            sb_body.AppendLine("            zIndex: 100,");
            sb_body.AppendLine("            content: $(\"#Layer_" + tableName + "\"),");
            sb_body.AppendLine("            btn: ['保存', '取消'],");
            sb_body.AppendLine("            btn1: function (closeIndex) {");
            sb_body.AppendLine("                submit(type, closeIndex);");
            sb_body.AppendLine("            },");
            sb_body.AppendLine("            btn2: function (closeIndex) {");
            sb_body.AppendLine("                layer.close(closeIndex);");
            sb_body.AppendLine("            },");
            sb_body.AppendLine("            success: function () {");
            sb_body.AppendLine("                if (check) {");
            sb_body.AppendLine("                    $(\"#Layer_Form_" + tableName + "\").form(\"reset\");");
            sb_body.AppendLine("                } else {");
            sb_body.AppendLine("                    var rows = $('#dtGrid').datagrid('getRows');");
            sb_body.AppendLine("                    var row = rows[_index];");
            sb_body.AppendLine("                    SetEditData(row);");
            sb_body.AppendLine("                }");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("        });");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("");
            sb_body.AppendLine("    //弹框 提交表单");
            sb_body.AppendLine("    function submit(type, closeIndex) {");
            sb_body.AppendLine("");
            sb_body.AppendLine("        $(\"#Layer_Form_" + tableName + "\").form(\"submit\", {");
            sb_body.AppendLine("            onSubmit: function () {");
            sb_body.AppendLine("                if ($(this).form(\"validate\")) {");
            sb_body.AppendLine("                    $.ajax({");
            sb_body.AppendLine("                        url: \"Edit" + tableName + "\",");
            sb_body.AppendLine("                        type: 'post',");
            sb_body.AppendLine("                        contentType: \"application/json\",");
            sb_body.AppendLine("                        data: JSON.stringify(GetEditData(type)),");
            sb_body.AppendLine("                        success: function (data) {");
            sb_body.AppendLine("                            data = $.parseJSON(data);");
            sb_body.AppendLine("                            if (!data.Success) {");
            sb_body.AppendLine("                                layer.alert(data.Message, { icon: 0, tiem: 1500 });");
            sb_body.AppendLine("                                return;");
            sb_body.AppendLine("                            }");
            sb_body.AppendLine("                            var rows = $(\"#dtGrid\").datagrid(\"getRows\");");
            sb_body.AppendLine("                            $(\"#dtGrid\").datagrid('loadData', rows);");
            sb_body.AppendLine("                            layer.close(closeIndex);//关闭面板");
            sb_body.AppendLine("                            $('#dtGrid').datagrid('reload');");
            sb_body.AppendLine("                        },");
            sb_body.AppendLine("                        error: function (s) {");
            sb_body.AppendLine("");
            sb_body.AppendLine("                        }");
            sb_body.AppendLine("                    });");
            sb_body.AppendLine("                } else {");
            sb_body.AppendLine("                    layer.alert(\"信息填写不完整\", { icon: 0, tiem: 1500 });");
            sb_body.AppendLine("                }");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("        });");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("");
            sb_body.AppendLine("    //查询条件");
            sb_body.AppendLine("    function GetQueryParams() {");
            sb_body.AppendLine("        var parm = {");
            sb_body.AppendLine("            " + tableName + "Name: $('#" + tableName + "Name').val(),");
            sb_body.AppendLine("        };");
            sb_body.AppendLine("        return parm;");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("");
            sb_body.AppendLine("    //提交数据");
            sb_body.AppendLine("    function GetEditData(type) {");
            sb_body.AppendLine("        var parm = {");
            sb_body.AppendLine("            SysNo: type == \"updateRow\" ? $(\".SysNo\").textbox(\"getText\") : \"00000000-0000-0000-0000-000000000000\",");

            sb_body.AppendLine(sb_GetEditData.ToString());

            sb_body.AppendLine("");
            sb_body.AppendLine("        };");
            sb_body.AppendLine("        return parm;");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("");
            sb_body.AppendLine("    //编辑页面赋值");
            sb_body.AppendLine("    function SetEditData(row) {");
            sb_body.AppendLine("        $(\".SysNo\").textbox(\"setText\", row.SysNo);");

            sb_body.AppendLine(sb_SetEditData.ToString());

            sb_body.AppendLine("");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("");
            sb_body.AppendLine("    //格式化操作栏");
            sb_body.AppendLine("    function formatterHandle(val, row, index) {");
            sb_body.AppendLine("        var ops = \"\";");
            sb_body.AppendLine("        ops += \"<a class='easyui-linkbutton style-primary edit'>编辑</a>\";");
            sb_body.AppendLine("        return ops;");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("");
            sb_body.AppendLine("    //编辑");
            sb_body.AppendLine("    function handle_edit(id) {");
            sb_body.AppendLine("        $(id).datagrid(\"getPanel\").find(\"tr .edit\").linkbutton({");
            sb_body.AppendLine("            iconCls: 'icon iconfont icon-edit',");
            sb_body.AppendLine("            onClick: function (item) {");
            sb_body.AppendLine("                var EditIndex = $(this).parents(\"tr\").index();");
            sb_body.AppendLine("                $(id).datagrid(\"selectRow\", EditIndex);");
            sb_body.AppendLine("");
            sb_body.AppendLine("                //var rows = $(id).datagrid('getRows');");
            sb_body.AppendLine("                //var row = rows[EditIndex];");
            sb_body.AppendLine("");
            sb_body.AppendLine("                content(\"编辑" + tableDesc + "\", \"updateRow\", false, EditIndex);");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("        });");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("</script>");
            sb_body.AppendLine("");
            sb_body.AppendLine("<body class=\"easyui-layout\">");
            sb_body.AppendLine("    <div style=\"padding: 10px;\" id=\"dtGridToolber\">");
            sb_body.AppendLine("        <input class=\"easyui-textbox\" style=\"width: 400px;\" id=\"" + tableName + "Name\" data-options=\"label:'" + tableDesc + "名称',prompt:'请输入" + tableDesc + "名称'\" />");
            sb_body.AppendLine("");
            sb_body.AppendLine("        <a id=\"searchList\" class=\"easyui-linkbutton style-primary\" style=\"margin: 0\" data-options=\"iconCls:'icon iconfont icon-sousuo'\">搜索</a>");
            sb_body.AppendLine("        <a id=\"remove\" class=\"easyui-linkbutton style-red\" style=\"float: right;margin: 0 5px 0 0;\" data-options=\"iconCls:'icon iconfont icon-delete2'\">删除选中</a>");
            sb_body.AppendLine("        <a id=\"add\" class=\"easyui-linkbutton style-blue\" style=\"float: right;margin: 0 5px 0 0;\" data-options=\"iconCls:'icon iconfont icon-tianjia1'\">添加" + tableDesc + "</a>");
            sb_body.AppendLine("");
            sb_body.AppendLine("    </div>");
            sb_body.AppendLine("    <table id=\"dtGrid\" scroll=\"on\">");
            sb_body.AppendLine("        <thead>");
            sb_body.AppendLine("            <tr>");
            sb_body.AppendLine("                <th data-options=\"field:'ck',checkbox:true,fixed:true\"></th>");
            sb_body.AppendLine("                <th data-options=\"field:'SysNo',width:100,align:'center',hidden:true\">编号</th>");

            sb_body.AppendLine(sb_Table_Th.ToString());

            sb_body.AppendLine("");
            sb_body.AppendLine("                <th data-options=\"field:'CreateDate',width:120,align:'center',formatter:fmDate\">添加时间</th>");
            sb_body.AppendLine("                <th data-options=\"field:'UpdateDate',width:120,align:'center',formatter:fmDate\">更新时间</th>");
            sb_body.AppendLine("                <th data-options=\"field:'handle',width:100,align:'center',fixed:true,formatter:formatterHandle\">操作</th>");
            sb_body.AppendLine("            </tr>");
            sb_body.AppendLine("        </thead>");
            sb_body.AppendLine("    </table>");
            sb_body.AppendLine("    <div id=\"menu\" style=\"background: #fff;\">");
            sb_body.AppendLine("        <div data-options=\"iconCls:'icon iconfont icon-bianji1',name:'edit'\">");
            sb_body.AppendLine("            <a href=\"###\" class=\"easyui-linkbutton style-purple\">编辑</a>");
            sb_body.AppendLine("        </div>");
            sb_body.AppendLine("    </div>");
            sb_body.AppendLine("");
            sb_body.AppendLine("    <div id=\"Layer_" + tableName + "\" style=\"width: 600px;height: 400px;display: none;\">");
            sb_body.AppendLine("        <form class=\"easyui-form\" id=\"Layer_Form_" + tableName + "\" style=\"width: 80%;margin: 20px auto;\">");
            sb_body.AppendLine("            <div style=\"margin-top: 10px;display:none\">");
            sb_body.AppendLine("                <input class=\"easyui-textbox SysNo\" name=\"SysNo\" data-options=\"label:'编号'\" />");
            sb_body.AppendLine("            </div>");

            sb_body.AppendLine(sb_EditDiv.ToString());

            sb_body.AppendLine("");
            sb_body.AppendLine("        </form>");
            sb_body.AppendLine("");
            sb_body.AppendLine("    </div>");
            sb_body.AppendLine("");
            sb_body.AppendLine("</body>");
            sb_body.AppendLine("");

            string file_Model = "C:\\Code\\" + str_nameSpace + ".Web\\Views\\" + tablePrefix + "\\";
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }
            CommonCode.Save(file_Model + "/" + tableName + ".cshtml", sb_body.ToString());
        }
    }
}
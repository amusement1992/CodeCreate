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

                if (columnType == "string")
                {
                    sb.AppendLine("            if (!filter."+ columnName + ".IsNullOrEmpty())");
                }
                else
                {
                    sb.AppendLine("            if (!filter." + columnName + ".HasValue)");

                }
                sb.AppendLine("            {");
                sb.AppendLine("                query.Equal<" + tableName + "Query>(c => c." + columnName + ", filter." + columnName + ");");
                sb.AppendLine("            }");
            }

            #endregion Model


            StringBuilder sb_body = new StringBuilder();
            sb_body.AppendLine("@{");
            sb_body.AppendLine("    ViewBag.Title = \"" + tableDesc + "详情\";");
            sb_body.AppendLine("    Layout = \"~/Views/Shared/_LayoutUser.cshtml\";");
            sb_body.AppendLine("}");
            sb_body.AppendLine("");
            sb_body.AppendLine("<script>");
            sb_body.AppendLine("");
            sb_body.AppendLine("    $(function () {");
            sb_body.AppendLine("        grid(\"#dtGrid\", \"#dtGridToolber\");");
            sb_body.AppendLine("        $(\"#add\").on(\"click\", function () {");
            sb_body.AppendLine("            content(\"添加" + tableDesc + "\", \"insertRow\", true);");
            sb_body.AppendLine("        })");
            sb_body.AppendLine("");
            sb_body.AppendLine("");
            sb_body.AppendLine("        $(\"#searchList\").on(\"click\", function () {");
            sb_body.AppendLine("            $('#dtGrid').datagrid('load', {");
            sb_body.AppendLine("                " + tableDesc + "Name: $('#" + tableDesc + "Name').val()");
            sb_body.AppendLine("            });");
            sb_body.AppendLine("        });");
            sb_body.AppendLine("");
            sb_body.AppendLine("        //删除选中");
            sb_body.AppendLine("        $('#remove').linkbutton({");
            sb_body.AppendLine("            iconCls: 'icon iconfont icon-icon_delete',");
            sb_body.AppendLine("            onClick: function () {");
            sb_body.AppendLine("                var row = $(\"#dtGrid\").datagrid('getSelections');");
            sb_body.AppendLine("");
            sb_body.AppendLine("                if (row.length > 0) {");
            sb_body.AppendLine("                    layer.alert(\"确定删除选中的" + tableDesc + "吗\", {");
            sb_body.AppendLine("                        icon: 3,");
            sb_body.AppendLine("                        btn: ['确定', '取消'],");
            sb_body.AppendLine("                        yes: function (j) {");
            sb_body.AppendLine("                            var " + tableName + "Ids = new Array()");
            sb_body.AppendLine("                            for (var i = 0; i < row.length; i++) {");
            sb_body.AppendLine("                                " + tableName + "Ids.push(row[i].SysNo);");
            sb_body.AppendLine("                            }");
            sb_body.AppendLine("");
            sb_body.AppendLine("                            var parm = {");
            sb_body.AppendLine("                                " + tableName + "Ids: " + tableName + "Ids,");
            sb_body.AppendLine("                            }");
            sb_body.AppendLine("");
            sb_body.AppendLine("                            $.ajax({");
            sb_body.AppendLine("                                type: \"Post\",");
            sb_body.AppendLine("                                url: \"Delete" + tableName + "\",");
            sb_body.AppendLine("                                dataType: \"json\",");
            sb_body.AppendLine("                                " + tablePrefix + ": JSON.stringify(parm),");
            sb_body.AppendLine("                                contentType: \"application/json\",");
            sb_body.AppendLine("                                success: function (" + tablePrefix + ") {");
            sb_body.AppendLine("                                    if (" + tablePrefix + ".Success) {");
            sb_body.AppendLine("                                        for (var i = 0; i < row.length; i++) {");
            sb_body.AppendLine("                                            var _index = $(\"#dtGrid\").datagrid(\"getRowIndex\", row[i]);");
            sb_body.AppendLine("                                            $(\"#dtGrid\").datagrid('deleteRow', _index);");
            sb_body.AppendLine("                                        }");
            sb_body.AppendLine("                                        layer.close(j);");
            sb_body.AppendLine("                                    } else {");
            sb_body.AppendLine("                                        layer.alert(" + tablePrefix + ".Message);");
            sb_body.AppendLine("                                    }");
            sb_body.AppendLine("                                }");
            sb_body.AppendLine("                            });");
            sb_body.AppendLine("");
            sb_body.AppendLine("                        }");
            sb_body.AppendLine("                    })");
            sb_body.AppendLine("");
            sb_body.AppendLine("                } else {");
            sb_body.AppendLine("                    layer.alert('请选择要删除的" + tableDesc + "', { icon: 0, time: 2000 });");
            sb_body.AppendLine("                }");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("        })");
            sb_body.AppendLine("    });");
            sb_body.AppendLine("    ");
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
            sb_body.AppendLine("            striped: true,");
            sb_body.AppendLine("            loading: true,");
            sb_body.AppendLine("            singleSelect: true,");
            sb_body.AppendLine("            //nowrap:true,");
            sb_body.AppendLine("            scrollbarSize: 0,");
            sb_body.AppendLine("            toolbar: toolbar,");
            sb_body.AppendLine("            onLoadSuccess: function (" + tablePrefix + ") {");
            sb_body.AppendLine("                $(id).datagrid(\"getPanel\").find(\"tr.datagrid-row td[field!=ck]\").unbind('click').bind('click', function () {//取消选中行事件");
            sb_body.AppendLine("                    return false;");
            sb_body.AppendLine("                });");
            sb_body.AppendLine("");
            sb_body.AppendLine("                if (" + tablePrefix + ".total > 0) {");
            sb_body.AppendLine("                    //加载完成 实例化按钮");
            sb_body.AppendLine("                    var ddlMenu = $(\".menuBtn\").addClass(\"style-blue\").menubutton({");
            sb_body.AppendLine("                        menu: \"#menu\"");
            sb_body.AppendLine("                    });");
            sb_body.AppendLine("                    $(ddlMenu).on(\"click\", function () {");
            sb_body.AppendLine("                        var index = $(this).parents(\"tr\").index();");
            sb_body.AppendLine("                        $(id).datagrid(\"selectRow\", index);");
            sb_body.AppendLine("                        $(\".menu-item\").css({ height: 36 });");
            sb_body.AppendLine("                        $(\"#menu\").css({ height: 120 });");
            sb_body.AppendLine("                        $(\".menu-line\").css({ height: 124 });");
            sb_body.AppendLine("                    })");
            sb_body.AppendLine("                    $(ddlMenu).unbind('mouseenter').bind(\"mouseenter\", function () {");
            sb_body.AppendLine("                        return false");
            sb_body.AppendLine("                    });");
            sb_body.AppendLine("                    $(\"#menu\").bind(\"mouseenter\", function () {");
            sb_body.AppendLine("                        $(\".menuBtn\").css({ padding: 1 });");
            sb_body.AppendLine("                    });");
            sb_body.AppendLine("                    $(ddlMenu.menubutton('options').menu).menu({");
            sb_body.AppendLine("                        onClick: function (item) {");
            sb_body.AppendLine("                            //编辑");
            sb_body.AppendLine("                            if (item.name == 'edit') {");
            sb_body.AppendLine("                                var index = $(this).parents(\"tr\").index();");
            sb_body.AppendLine("                                $(id).datagrid(\"selectRow\", index);");
            sb_body.AppendLine("                                var row = $(id).datagrid('getSelected');");
            sb_body.AppendLine("                                content(\"编辑" + tableDesc + "\", \"updateRow\", false, index)");
            sb_body.AppendLine("");
            sb_body.AppendLine("                            }");
            sb_body.AppendLine("                        }");
            sb_body.AppendLine("                    })");
            sb_body.AppendLine("                }");
            sb_body.AppendLine("");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("        })");
            sb_body.AppendLine("    };");
            sb_body.AppendLine("");
            sb_body.AppendLine("    function content(title, type, check, _index) {");
            sb_body.AppendLine("        layer.open({");
            sb_body.AppendLine("            type: 1,");
            sb_body.AppendLine("            area: [],");
            sb_body.AppendLine("            title: title,");
            sb_body.AppendLine("            zIndex: 100,");
            sb_body.AppendLine("            content: $(\"#Add" + tableName + "\"),");
            sb_body.AppendLine("            btn: ['保存', '取消'],");
            sb_body.AppendLine("            btn1: function (j) {");
            sb_body.AppendLine("                var index = _index ? _index : 0;");
            sb_body.AppendLine("                //下一步");
            sb_body.AppendLine("                $(\"#Add" + tableName + "Temp\").form(\"submit\", {");
            sb_body.AppendLine("                    onSubmit: function () {");
            sb_body.AppendLine("                        if ($(this).form(\"validate\")) {");
            sb_body.AppendLine("");
            sb_body.AppendLine("                            var parm = {");
            sb_body.AppendLine("                                " + tableName + "Name: $(\"." + tableName + "Name\").textbox(\"getText\"),");
            sb_body.AppendLine("                                SysNo: type == \"updateRow\" ? $(\".SysNo\").textbox(\"getText\") : \"00000000-0000-0000-0000-000000000000\"");
            sb_body.AppendLine("                            }");
            sb_body.AppendLine("                            $.ajax({");
            sb_body.AppendLine("                                url: \"Edit" + tableName + "\",");
            sb_body.AppendLine("                                type: 'post',");
            sb_body.AppendLine("                                contentType: \"application/json\",");
            sb_body.AppendLine("                                " + tablePrefix + ": JSON.stringify(parm),");
            sb_body.AppendLine("                                success: function (" + tablePrefix + ") {");
            sb_body.AppendLine("                                    var rows = $(\"#dtGrid\").datagrid(\"getRows\");");
            sb_body.AppendLine("                                    $(\"#dtGrid\").datagrid('loadData', rows);");
            sb_body.AppendLine("                                    layer.close(j);//关闭面板");
            sb_body.AppendLine("                                    $('#dtGrid').datagrid('reload');");
            sb_body.AppendLine("                                },");
            sb_body.AppendLine("                                error: function (s) {");
            sb_body.AppendLine("");
            sb_body.AppendLine("                                }");
            sb_body.AppendLine("                            });");
            sb_body.AppendLine("                        } else {");
            sb_body.AppendLine("                            layer.alert(\"信息填写不完整\", { icon: 0, tiem: 1500 });");
            sb_body.AppendLine("                        }");
            sb_body.AppendLine("                    }");
            sb_body.AppendLine("                });");
            sb_body.AppendLine("            },");
            sb_body.AppendLine("            btn2: function (j) {");
            sb_body.AppendLine("                layer.close(j);//关闭面板");
            sb_body.AppendLine("            },");
            sb_body.AppendLine("            success: function () {");
            sb_body.AppendLine("                if (check) {");
            sb_body.AppendLine("                    $(\"#Add" + tableName + "Temp\").form(\"reset\");");
            sb_body.AppendLine("                } else {");
            sb_body.AppendLine("                    var row = $(\"#dtGrid\").datagrid(\"getSelected\");");
            sb_body.AppendLine("                    $(\".SysNo\").textbox(\"setText\", row.SysNo);");
            sb_body.AppendLine("                    $(\"." + tableName + "Name\").textbox(\"setText\", row." + tableName + "Name);");
            sb_body.AppendLine("                }");
            sb_body.AppendLine("            }");
            sb_body.AppendLine("        });");
            sb_body.AppendLine("    }");
            sb_body.AppendLine("</script>");
            sb_body.AppendLine("");
            sb_body.AppendLine("<body class=\"easyui-layout\">");
            sb_body.AppendLine("    <table id=\"dtGrid\" scroll=\"on\">");
            sb_body.AppendLine("        <thead>");
            sb_body.AppendLine("            <tr>");
            sb_body.AppendLine("                <th " + tablePrefix + "-options=\"field:'ck',checkbox:true,fixed:true\"></th>");
            sb_body.AppendLine("                <th " + tablePrefix + "-options=\"field:'SysNo',width:100,align:'center'\">编号</th>");
            sb_body.AppendLine("                <th " + tablePrefix + "-options=\"field:'" + tableName + "Name',width:100,align:'center'\">" + tableDesc + "名称</th>");
            sb_body.AppendLine("                <th " + tablePrefix + "-options=\"field:'CreateDate',width:160,align:'center',fixed:true,formatter:fmDate\">添加时间</th>");
            sb_body.AppendLine("                <th " + tablePrefix + "-options=\"field:'UpdateDate',width:160,align:'center',fixed:true,formatter:fmDate\">更新时间</th>");
            sb_body.AppendLine("                <th " + tablePrefix + "-options=\"field:'color',title:'操作',width:200,align:'center',fixed:true,formatter:function(){return '<a class=menuBtn>操作</a>'}\"></th>");
            sb_body.AppendLine("            </tr>");
            sb_body.AppendLine("        </thead>");
            sb_body.AppendLine("    </table>");
            sb_body.AppendLine("    <div id=\"menu\" style=\"background: #fff;\">");
            sb_body.AppendLine("        <div " + tablePrefix + "-options=\"iconCls:'icon iconfont icon-bianji1',name:'edit'\">");
            sb_body.AppendLine("            <a href=\"###\" class=\"easyui-linkbutton style-purple\">编辑</a>");
            sb_body.AppendLine("        </div>");
            sb_body.AppendLine("    </div>");
            sb_body.AppendLine("    <div style=\"padding: 10px;\" id=\"dtGridToolber\">");
            sb_body.AppendLine("        <input class=\"easyui-textbox\" style=\"width: 400px;\" id=\"" + tableName + "Name\" " + tablePrefix + "-options=\"label:'" + tableDesc + "名称',prompt:'请输入" + tableDesc + "名称'\" />");
            sb_body.AppendLine("        <a href=\"###\" class=\"easyui-linkbutton style-red\" id=\"searchList\" style=\"margin: 0;\">搜索</a>");
            sb_body.AppendLine("        <a class=\"easyui-linkbutton style-primary delete\" id=\"remove\" style=\"float: right;margin: 0\" " + tablePrefix + "-options=\"iconCls:'icon iconfont icon-delete2'\">删除选中</a>");
            sb_body.AppendLine("        <a class=\"easyui-linkbutton style-blue add\" style=\"float: right;margin: 0 5px 0 0;\" " + tablePrefix + "-options=\"iconCls:'icon iconfont icon-tianjia1'\" id=\"add\">添加" + tableDesc + "</a>");
            sb_body.AppendLine("");
            sb_body.AppendLine("");
            sb_body.AppendLine("    </div>");
            sb_body.AppendLine("");
            sb_body.AppendLine("    <div id=\"Add" + tableName + "\" style=\"width: 600px;height: 150px;display: none;\">");
            sb_body.AppendLine("        <form class=\"easyui-form\" id=\"Add" + tableName + "Temp\" style=\"width: 80%;margin: 20px auto;\">");
            sb_body.AppendLine("            <div style=\"margin-top: 10px;display:none\">");
            sb_body.AppendLine("                <input class=\"easyui-textbox SysNo\" name=\"SysNo\" " + tablePrefix + "-options=\"label:'编号'\" />");
            sb_body.AppendLine("            </div>");
            sb_body.AppendLine("            <div style=\"margin-top: 10px;\">");
            sb_body.AppendLine("                <input class=\"easyui-textbox " + tableName + "Name\" name=\"" + tableName + "Name\" " + tablePrefix + "-options=\"label:'" + tableDesc + "名称',prompt:'请输入" + tableDesc + "名称',required:true,missingMessage:'请输入" + tableDesc + "名称'\" style=\"width: 300px;\" />");
            sb_body.AppendLine("            </div>");
            sb_body.AppendLine("");
            sb_body.AppendLine("        </form>");
            sb_body.AppendLine("");
            sb_body.AppendLine("    </div>");
            sb_body.AppendLine("");
            sb_body.AppendLine("</body>");
            sb_body.AppendLine("");
            sb_body.AppendLine("");
            sb_body.AppendLine("");
            sb_body.AppendLine("");
            sb_body.AppendLine("");

            string file_Model = "C:\\Code\\" + str_nameSpace + ".Web\\Views\\" + tableName + "";
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }
            CommonCode.Save(file_Model + "/" + tableName + ".cshtml", sb_body.ToString());
        }

    }
}
using CodeCreate.Model;
using Prime.DBUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CodeCreate
{
    public partial class MSSqlCreate : Form
    {
        public MSSqlCreate()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            DataTable dt = SqlHelper.GetDataTable(" select * from dbo.sysobjects where xtype = 'U' order by name");

            List<string> list = new List<string>();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        listBox1.Items.Add(dr["name"]);
                        list.Add(dr["name"].ToString());
                    }
                }
            }
            else
            {
                MessageBox.Show("数据库连接失敗了！");
                this.Hide();
                Login f1 = new Login();
                f1.Show();
            }
        }

        #region 左右移動

        //全部右移
        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    listBox2.Items.Add(listBox1.Items[i]);
                }
                listBox1.Items.Clear();
            }
        }

        //选中右移
        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems != null)
            {
                foreach (var item in listBox1.SelectedItems)
                {
                    listBox2.Items.Add(item.ToString());
                }

                this.listBox2.SelectedIndex = this.listBox2.Items.Count - 1;
                for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                {
                    listBox1.Items.RemoveAt(listBox1.SelectedIndices[i]);
                    i--;
                }
            }
        }

        //选中左移
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItems != null)
            {
                foreach (var item in listBox2.SelectedItems)
                {
                    listBox1.Items.Add(item.ToString());
                }
                this.listBox1.SelectedIndex = this.listBox1.Items.Count - 1;
                for (int i = 0; i < listBox2.SelectedIndices.Count; i++)
                {
                    listBox2.Items.RemoveAt(listBox2.SelectedIndices[i]);
                    i--;
                }
            }
        }

        //全部左移
        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox2.Items.Count > 0)
            {
                for (int i = 0; i < listBox2.Items.Count; i++)
                {
                    listBox1.Items.Add(listBox2.Items[i]);
                }
                listBox2.Items.Clear();
            }
        }

        #endregion 左右移動

        //返回主窗体
        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login f1 = new Login();
            f1.Show();
        }

        //生成
        private void btn_save_Click(object sender, EventArgs e)
        {
            ConfigModel configModel = new ConfigModel();
            configModel.MARK = "@";//SQL的连接类型
            configModel.str_nameSpace = textBox1.Text.Trim();//命名空间
            configModel.str_SqlHelper = textBox2.Text.Trim();//数据访问类名

            configModel.str_ExecuteNonQuery = textBox5.Text.Trim();
            configModel.str_UpdateDatabase = textBox6.Text.Trim();
            configModel.str_ExecuteScalar = textBox4.Text.Trim();
            configModel.str_GetDataTable = textBox7.Text.Trim();
            configModel.str_Exists = textBox10.Text.Trim();
            configModel.str_GetModel = textBox8.Text.Trim();
            configModel.str_GetModelList = textBox9.Text.Trim();

            #region 生成文件夹

            //string file_BLL = textBox3.Text.Trim();
            //string file_DAL = textBox11.Text.Trim();
            //string file_IDAL = textBox12.Text.Trim();
            //string file_Model = textBox13.Text.Trim();

            //if (!Directory.Exists(file_Model))
            //{
            //    Directory.CreateDirectory(file_Model);
            //}
            //if (!Directory.Exists(file_IDAL))
            //{
            //    Directory.CreateDirectory(file_IDAL);
            //}
            //if (!Directory.Exists(file_BLL))
            //{
            //    Directory.CreateDirectory(file_BLL);
            //}
            //if (!Directory.Exists(file_DAL))
            //{
            //    Directory.CreateDirectory(file_DAL);
            //}

            //if (comboBox1.SelectedIndex == 1)
            //{
            //    string path = textBox3.Text.Trim();
            //    if (!string.IsNullOrEmpty(path))
            //    {
            //        if (path.Contains("\\BLL"))
            //        {
            //            path = path.Replace("\\BLL", "");

            //            string BaseBLLSrc = Directory.GetCurrentDirectory() + "/BaseBll.cs";
            //            string BaseDALSrc = Directory.GetCurrentDirectory() + "/BaseDAL.cs";
            //            string BaseIDALSrc = Directory.GetCurrentDirectory() + "/BaseIDAL.cs";

            //            File.Copy(BaseBLLSrc, path + "\\BaseBll.cs");
            //            File.Copy(BaseDALSrc, path + "\\BaseDAL.cs");
            //            File.Copy(BaseIDALSrc, path + "\\BaseIDAL.cs");
            //        }
            //    }
            //}

            #endregion 生成文件夹

            string str_CreateMap = "";
            string str_SetObjectName = "";
            string str_SetPrimaryKey = "";



            //遍历每个表
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                string str_table = listBox2.Items[i].ToString();//表名

                //string str_BLLName = str_table + textBox17.Text.Trim();
                //string str_DALName = str_table + textBox16.Text.Trim();
                //string str_IDALName = str_table + textBox15.Text.Trim();
                //string str_ModelName = str_table + textBox14.Text.Trim();

                string sql = @"
SELECT
    表名       = case when a.colorder=1 then d.name else '' end,
    表说明     = case when a.colorder=1 then isnull(f.value,'') else '' end,
    字段序号   = a.colorder,
    columnName     = a.name,
    标识       = case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then 'Y'else 'N' end,
    primaryKey       = case when exists(SELECT 1 FROM sysobjects where xtype='PK' and parent_obj=a.id and name in (
                     SELECT name FROM sysindexes WHERE indid in(
                        SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid))) then 'Y' else 'N' end,
    columnType       = b.name,
    占用字节数 = a.length,
    char_col_decl_length       = COLUMNPROPERTY(a.id,a.name,'PRECISION'),
    小数位数   = isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0),
    nullable     = case when a.isnullable=1 then 'N'else 'Y' end,
    data_default     = isnull(e.text,''),
    columnComment   = isnull(g.[value],'')
FROM  syscolumns a
left join systypes b on a.xusertype=b.xusertype
inner join sysobjects d on     a.id=d.id  and d.xtype='U' and  d.name<>'dtproperties'
left join syscomments e on     a.cdefault=e.id
left join sys.extended_properties g on     a.id=g.major_id and a.colid=g.minor_id
left join    sys. extended_properties f on     d.id=f.major_id and f.major_id=0
where     d.name=" + configModel.MARK + "a order by     a.id,a.colorder";

                SqlParameter[] sps = { new SqlParameter(configModel.MARK + "a", str_table) };
                DataTable dt_tables = SqlHelper.GetDataTable(sql, sps);

                if (dt_tables == null)
                {
                    return;
                }
                if (dt_tables.Rows.Count > 0)
                {
                    #region 生成内容

                    StringBuilder sb_column1 = new StringBuilder();//格式如 NO_ID,ST_NAME,ST_VALUES,NO_ORDER,ST_OTHER,ST_VALUES_ENG
                    StringBuilder sb_column2 = new StringBuilder();//格式如 @NO_ID,@ST_NAME,@ST_VALUES,@NO_ORDER,@ST_OTHER,@ST_VALUES_ENG
                    StringBuilder sb_column3 = new StringBuilder();//格式如 new SqlParameter("@NO_ID", SqlType.Number,4),
                    StringBuilder sb_column4 = new StringBuilder();//格式如 parameters[0].Value = model.NO_ID;
                    StringBuilder sb_column5 = new StringBuilder();//格式如 strSql.Append("ST_NAME=@ST_NAME,");
                    for (int j = 0; j < dt_tables.Rows.Count; j++)
                    {
                        string colName = dt_tables.Rows[j]["columnName"].ToString().Trim();//字段名
                        string colType = CommonCode.GetColType(dt_tables.Rows[j]["columnType"].ToString());//字段类型
                        if (j == dt_tables.Rows.Count - 1)
                        {
                            sb_column1.Append(colName);
                            sb_column2.Append(configModel.MARK + colName);
                            sb_column3.AppendLine("					new SqlParameter(\"" + configModel.MARK + colName + "\", model." + colName + ")};");
                            //sb_column4.AppendLine("            parameters[" + j + "].Value = model." + colName + ";");
                            sb_column4.AppendLine("item." + colName + "=model." + colName + ";");
                            sb_column5.AppendLine("            strSql.Append(\"" + colName + "=" + configModel.MARK + colName + "\");");
                        }
                        else
                        {
                            sb_column1.Append(colName + ",");
                            sb_column2.Append(configModel.MARK + colName + ",");
                            sb_column3.AppendLine("					new SqlParameter(\"" + configModel.MARK + colName + "\", model." + colName + "),");
                            //sb_column4.AppendLine("            parameters[" + j + "].Value = model." + colName + ";");
                            sb_column4.AppendLine("item." + colName + "=model." + colName + ";");
                            sb_column5.AppendLine("            strSql.Append(\"" + colName + "=" + configModel.MARK + colName + ",\");");
                        }
                    }

                    #endregion 生成内容

                    ////生成model
                    //new CreateModel().Create(file_Model, configModel.str_nameSpace, dt_tables, str_ModelName);

                    ////生成IDAL
                    //new CreateIDAL().Create(file_IDAL, configModel.str_nameSpace, str_IDALName, str_ModelName);

                    ////生成BLL
                    //new CreateBLL().Create(file_BLL, configModel.str_nameSpace, str_table, str_BLLName, str_IDALName, str_ModelName);

                    ////生成DAL
                    //new CreateDAL().Create(file_DAL, str_table, str_DALName, str_IDALName, str_ModelName, sb_column1, sb_column2, sb_column3, sb_column4, sb_column5, configModel);


                    new Create_DeleteCmdDto().Create(configModel.str_nameSpace, dt_tables, str_table);
                    new Create_SaveCmdDto().Create(configModel.str_nameSpace, dt_tables, str_table);
                    new Create_CmdDto().Create(configModel.str_nameSpace, dt_tables, str_table);
                    new Create_Dto().Create(configModel.str_nameSpace, dt_tables, str_table);
                    new Create_FilterDto().Create(configModel.str_nameSpace, dt_tables, str_table);
                    new Create_Query().Create(configModel.str_nameSpace, dt_tables, str_table);
                    new Create_ViewModel().Create(configModel.str_nameSpace, dt_tables, str_table);
                    new Create_FilterViewModel().Create(configModel.str_nameSpace, dt_tables, str_table);
                    new Create_Entity().Create(configModel.str_nameSpace, dt_tables, str_table);

                    new Create_Repository().Create(configModel.str_nameSpace, dt_tables, str_table);
                    new Create_IRepository().Create(configModel.str_nameSpace, dt_tables, str_table);

                    new Create_DomainService().Create(configModel.str_nameSpace, dt_tables, str_table);
                    new Create_Business().Create(configModel.str_nameSpace, dt_tables, str_table);
                    new Create_IDataAccess().Create(configModel.str_nameSpace, dt_tables, str_table);
                    new Create_DataAccess().Create(configModel.str_nameSpace, dt_tables, str_table);
                    new Create_IService().Create(configModel.str_nameSpace, dt_tables, str_table);
                    new Create_Service().Create(configModel.str_nameSpace, dt_tables, str_table);
                    new Create_IBusiness().Create(configModel.str_nameSpace, dt_tables, str_table);
                    new Create_DomainModel().Create(configModel.str_nameSpace, dt_tables, str_table);
                    new Create_Web_Page().Create(configModel.str_nameSpace, dt_tables, str_table);
                    new Create_Web_Controller().Create(configModel.str_nameSpace, dt_tables, str_table);

                    str_CreateMap += new Create_Config().GetStr_CreateMap(configModel.str_nameSpace, dt_tables, str_table);

                    str_SetObjectName += new Create_Config().GetStr_SetObjectName(configModel.str_nameSpace, dt_tables, str_table);

                    str_SetPrimaryKey += new Create_Config().GetStr_SetPrimaryKey(configModel.str_nameSpace, dt_tables, str_table);
                }
                label4.Text = "提示信息：" + (i + 1) + "个文件，全部生成成功！";
            }


            string filePath = "C:\\Code\\Config";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            CommonCode.Save(filePath + "/AutoMapMapper CreateMap.txt", str_CreateMap);
            CommonCode.Save(filePath + "/DbConfig SetObjectName.txt", str_SetObjectName);
            CommonCode.Save(filePath + "/DbConfig SetPrimaryKey.txt", str_SetPrimaryKey);

            //OpenFolder();
        }

        #region 生成路径


        #endregion 生成路径

        //窗口关闭
        private void MSSqlCreate_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.ExitThread();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            OpenFolder();
        }

        /// <summary>
        /// 打开文件夹
        /// </summary>
        private void OpenFolder()
        {
            System.Diagnostics.Process.Start("explorer.exe", "C:\\Code");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                Directory.Delete("C:\\Code", true);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
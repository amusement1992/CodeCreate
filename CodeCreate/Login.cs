using Prime.DBUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace CodeCreate
{
    public partial class Login : Form
    {
        private string configFileName = Directory.GetCurrentDirectory() + "/CodeCreate.xml";
        //private string configFileName = "D://CodeCreate.xml";

        public Login()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 1;
            try
            {
                if (File.Exists(configFileName))
                {
                    XmlDocument document = new XmlDocument();
                    document.Load(configFileName);
                    XmlNodeList xnl = document.SelectNodes("//CodeCreateConfig");
                    foreach (XmlNode item in xnl)
                    {
                        comboBox1.Text = item["DataSourceName"].InnerText;
                        comboBox2.Text = item["Type"].InnerText;
                        textBox1.Text = item["ServerName"].InnerText;
                        comboBox3.Text = item["DataSourceName"].InnerText;
                        textBox3.Text = item["UserName"].InnerText;
                        textBox4.Text = item["Pwd"].InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message + "1");
            }
        }

        //连接按钮
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button1.Text = "正在登陆中...";

            try
            {
                if (checkBox1.Checked)
                {
                    if (!File.Exists(configFileName))
                    {
                        File.Create(configFileName, 100);
                    }

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<?xml version='1.0' encoding='utf-8'?>");
                    sb.Append("<CodeCreateConfig>");
                    sb.Append("<DataSource>" + comboBox1.Text + "</DataSource>");
                    sb.Append("<Type>" + comboBox2.Text + "</Type>");
                    sb.Append("<ServerName>" + textBox1.Text.Trim() + "</ServerName>");
                    sb.Append("<DataSourceName>" + comboBox3.Text + "</DataSourceName>");
                    sb.Append("<UserName>" + textBox3.Text.Trim() + "</UserName>");
                    sb.Append("<Pwd>" + textBox4.Text.Trim() + "</Pwd>");
                    sb.Append("</CodeCreateConfig>");

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(sb.ToString());
                    doc.Save(configFileName);
                }
                else
                {
                    if (File.Exists(configFileName))
                    {
                        File.Delete(configFileName);
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message + "2");
            }

            //连接到数据库
            if (comboBox1.SelectedIndex == 0)//sql
            {
                if (comboBox2.SelectedIndex == 0)//windows身份验证
                {
                    //SqlHelper.connStr = "Data Source=.;Initial Catalog=Tennis;Integrated Security=True";
                    SqlHelper.connStr = "Data Source=" + textBox1.Text.Trim() + ";Initial Catalog=" + comboBox3.Text.Trim() + ";Integrated Security=True";
                }
                else if (comboBox2.SelectedIndex == 1)//sql身份验证
                {
                    SqlHelper.connStr = "server=" + textBox1.Text.Trim() + ";database=" + comboBox3.Text.Trim() + ";uid=" + textBox3.Text.Trim() + ";pwd=" + textBox4.Text.Trim();
                }

                DataTable dt = SqlHelper.GetDataTable(" select * from dbo.sysobjects where xtype = 'U' order by name");
                if (dt == null)
                {
                    MessageBox.Show("数据库连接失敗了！");
                    return;
                }

                OracleHelper.connStr = "";
                this.Hide();
                MSSqlCreate f3 = new MSSqlCreate();
                f3.StartPosition = FormStartPosition.CenterScreen;
                f3.Show();
            }
            else if (comboBox1.SelectedIndex == 1)//oracle
            {
                //OracleHelper.connStr = "Data Source=vipg;User ID=vipgtest; Password=vipg";
                OracleHelper.connStr = "Data Source=" + comboBox3.Text.Trim() + ";User ID=" + textBox3.Text.Trim() + ";Password=" + textBox4.Text.Trim();
                SqlHelper.connStr = "";

                DataTable dt = OracleHelper.GetDataTable(" select table_name name from user_tables order by table_name ");
                if (dt != null)
                {
                    MessageBox.Show("数据库连接失敗了！");
                    return;
                }

                this.Hide();
                OracleCreate f2 = new OracleCreate();
                f2.StartPosition = FormStartPosition.CenterScreen;
                f2.Show();
            }
        }

        //数据源下拉框
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                comboBox2.Enabled = true;
            }
            else
            {
                comboBox2.Enabled = false;
                textBox1.Enabled = false;
                comboBox3.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
            }
        }

        //身份验证方式下拉框
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0)
            {
                textBox1.Enabled = true;
                comboBox3.Enabled = true;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
            }
            else
            {
                textBox1.Enabled = true;
                comboBox3.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
            }
        }

        //关闭窗口
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.ExitThread();
        }

        private void comboBox3_DropDown(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" || comboBox3.Items.Count > 0)
            {
                return;
            }
            if (comboBox1.SelectedIndex == 0)//sql
            {
                if (comboBox2.SelectedIndex == 0)//windows身份验证
                {
                    SqlHelper.connStr = "Data Source=" + textBox1.Text.Trim() + ";Initial Catalog=" + comboBox3.Text.Trim() + ";Integrated Security=True";
                }
                else if (comboBox2.SelectedIndex == 1)//sql身份验证
                {
                    SqlHelper.connStr = "server=" + textBox1.Text.Trim() + ";database=" + comboBox3.Text.Trim() + ";uid=" + textBox3.Text.Trim() + ";pwd=" + textBox4.Text.Trim();
                }

                DataTable dt = SqlHelper.GetDataTable("select * from dbo.sysdatabases order by name");
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        comboBox3.Items.Add(dr["name"].ToString());
                    }
                }
                else
                {
                    MessageBox.Show("数据库连接失敗了！数据库连接的字符串：" + SqlHelper.connStr);
                    return;
                }
            }
        }
    }

    ///// <summary>
    ///// 登陆的配置文件的模板
    ///// </summary>
    //public class CodeCreateConfigModel
    //{
    //    /// <summary>
    //    /// 数据源
    //    /// </summary>
    //    public string DataSource { get; set; }

    //    /// <summary>
    //    /// 身份验证
    //    /// </summary>
    //    public string Type { get; set; }

    //    /// <summary>
    //    /// 服务器名
    //    /// </summary>
    //    public string ServerName { get; set; }

    //    /// <summary>
    //    /// 数据库名
    //    /// </summary>
    //    public string DataSourceName { get; set; }

    //    /// <summary>
    //    /// 用户名
    //    /// </summary>
    //    public string UserName { get; set; }

    //    /// <summary>
    //    /// 密码
    //    /// </summary>
    //    public string Pwd { get; set; }

    //}
}
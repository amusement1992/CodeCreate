using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Prime.DBUtility;
using System.IO;
using System.Data.OracleClient;

namespace CodeCreate
{
    public partial class OracleCreate : Form
    {
        public OracleCreate()
        {
            InitializeComponent();
        }
        #region 字段

        /// <summary>
        /// 命名空间
        /// </summary>
        public string str_nameSpace = "";

        /// <summary>
        /// 数据访问类名
        /// </summary>
        public string str_SqlHelper = "";

        /// <summary>
        /// 执行一条SQL语句方法名：
        /// </summary>
        public string str_ExecuteNonQuery = "";

        /// <summary>
        /// 执行多条SQL语句方法名：
        /// </summary>
        public string str_UpdateDatabase = "";

        /// <summary>
        /// 返回首行首列方法名：
        /// </summary>
        public string str_ExecuteScalar = "";

        /// <summary>
        /// 返回DataTable方法名：
        /// </summary>
        public string str_GetDataTable = "";

        /// <summary>
        /// 是否存在数据方法名：
        /// </summary>
        public string str_Exists = "";

        /// <summary>
        /// 返回一个Model方法名：
        /// </summary>
        public string str_GetModel = "";

        /// <summary>
        /// 返回多个Model方法名：
        /// </summary>
        public string str_GetModelList = "";
        #endregion

        private void Form2_Load(object sender, EventArgs e)
        {
            str_nameSpace = textBox1.Text.Trim();

            #region 獲取數據表列表，填充到ListBox中
            DataTable dt = OracleHelper.GetDataTable(" select table_name name from user_tables order by table_name ");
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        listBox1.Items.Add(dr["name"]);
                    }
                }
            }
            else
            {
                MessageBox.Show("失敗了！");
                this.Hide();
                Login f1 = new Login();
                f1.Show();
            }
            #endregion

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
        #endregion

        #region 其它

        //返回主窗体
        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login f1 = new Login();
            f1.Show();
        }

        //选择
        private void button6_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult dr = fbd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                textBox3.Text = fbd.SelectedPath;
            }
        }

        //查看
        private void button7_Click(object sender, EventArgs e)
        {
            string path = textBox3.Text.Trim();
            System.Diagnostics.Process.Start("explorer.exe", path);
        }

        //生成
        private void btn_save_Click(object sender, EventArgs e)
        {
            CodeCreate();
        }
        #endregion

        #region 核心代碼

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <param name="fileContent">文件内容</param>
        private void Save(string fileName, string fileContent)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);

            byte[] buffer = Encoding.Default.GetBytes(fileContent);

            fs.Write(buffer, 0, buffer.Length);

            fs.Flush();
            fs.Close();
        }



        /// <summary>
        /// 生成代码
        /// </summary>
        private void CodeCreate()
        {

            label4.Text = "";

            #region 生成文件夹
            string file_path = textBox3.Text.Trim();

            string file_Model = file_path + "/Model";
            string file_IDAL = file_path + "/IDAL";
            string file_BLL = file_path + "/BLL";
            string file_DAL = file_path + "/DAL";

            if (!Directory.Exists(file_path))
            {
                Directory.CreateDirectory(file_path);
            }
            if (!Directory.Exists(file_Model))
            {
                Directory.CreateDirectory(file_Model);
            }
            if (!Directory.Exists(file_IDAL))
            {
                Directory.CreateDirectory(file_IDAL);
            }
            if (!Directory.Exists(file_BLL))
            {
                Directory.CreateDirectory(file_BLL);
            }
            if (!Directory.Exists(file_DAL))
            {
                Directory.CreateDirectory(file_DAL);
            }

            #endregion

            #region 获取表名集合
            List<string> list_tables = new List<string>();//表名集合
            if (listBox2.Items.Count > 0)
            {
                for (int i = 0; i < listBox2.Items.Count; i++)
                {
                    string str_table = listBox2.Items[i].ToString().ToUpper().Trim();//表名
                    list_tables.Add(str_table);
                }
            }
            string str_tables = txt_tables.Text.ToUpper().Trim();
            if (str_tables != "")
            {
                if (!str_tables.Contains(' '))
                {
                    list_tables.Add(str_tables);
                }
                else
                {
                    string[] arr = str_tables.Split(' ');
                    foreach (string item in arr)
                    {
                        list_tables.Add(item);
                    }
                }
            }
            #endregion

            if (list_tables.Count > 0)
            {
                str_nameSpace = textBox1.Text.Trim();

                str_SqlHelper = textBox2.Text.Trim();

                str_ExecuteNonQuery = textBox5.Text.Trim();
                str_UpdateDatabase = textBox6.Text.Trim();
                str_ExecuteScalar = textBox4.Text.Trim();
                str_GetDataTable = textBox7.Text.Trim();
                str_Exists = textBox10.Text.Trim();
                str_GetModel = textBox8.Text.Trim();
                str_GetModelList = textBox9.Text.Trim();

                int successCount = 0, failCount = 0;//成功生成0个文件，失败生成0个文件
                string failTables = "";//失败生成的表名


                //遍历每个表
                foreach (string item in list_tables)
                {
                    string str_table = item.ToString().ToUpper();//表名

                    #region 获取主键
                    string sql_primeKey = "select column_name pk_name from user_cons_columns where constraint_name = (select constraint_name from user_constraints where table_name = :a  and constraint_type ='P')";

                    OracleParameter[] parameters_primeKey = { new OracleParameter(":a", str_table) };
                    DataTable dt_primeKey = OracleHelper.GetDataTable(sql_primeKey, parameters_primeKey);

                    bool b_primeCode = false;//false:没有主键
                    string prime_key = "";//主键变量
                    if (dt_primeKey != null)
                    {
                        if (dt_primeKey.Rows.Count > 0)
                        {
                            b_primeCode = true;
                            prime_key = dt_primeKey.Rows[0]["pk_name"].ToString();
                        }
                    }
                    #endregion

                    string sql = @" select a.column_name,a.data_type,a.nullable,a.data_default,b.char_col_decl_length,c.comments
                                from user_tab_columns a
                                left join user_tab_cols b on a.column_name=b.column_name
                                left join user_col_comments c on a.column_name=c.column_name
                                where a.table_name =:a  and b.table_name =:a and c.table_name =:a";//order by a.column_name
                    OracleParameter[] parameters = { new OracleParameter(":a", str_table) };
                    DataTable dt_tables = OracleHelper.GetDataTable(sql, parameters);

                    if (dt_tables == null)
                    {
                        return;
                    }
                    if (dt_tables.Rows.Count > 0)
                    {
                        #region 生成内容
                        string str_ModelName = str_table.ToUpper() + "_Model";
                        string str_IDALName = str_table.ToUpper() + "_IDAL";
                        string str_BLLName = str_table.ToUpper() + "_BLL";
                        string str_DALName = str_table.ToUpper() + "_DAL";

                        List<string> list_columnName = new List<string>();//字段名集合
                        for (int j = 0; j < dt_tables.Rows.Count; j++)
                        {
                            string colName = dt_tables.Rows[j]["column_name"].ToString().Trim().ToUpper();//字段名
                        }

                        if (!b_primeCode)//不存在主键
                        {
                            if (list_columnName.Contains("NO_ID"))
                            {
                                prime_key = "NO_ID";
                            }
                            else if (list_columnName.Contains("ST_CODE"))
                            {
                                prime_key = "ST_CODE";
                            }
                            else if (list_columnName.Contains("ST_PREFIXNO"))
                            {
                                prime_key = "ST_PREFIXNO";
                            }
                            else
                            {
                                prime_key = dt_tables.Rows[0]["column_name"].ToString().Trim().ToUpper();
                            }
                        }
                        #endregion

                        string st_prime_tip = "";
                        if (!b_primeCode)
                        {
                            st_prime_tip = "没有主键，默认主键为：" + prime_key;
                        }
                        else
                        {
                            st_prime_tip = "有主键，主键为：" + prime_key;
                        }

                        StringBuilder sb = new StringBuilder();
                        #region Model生成
                        sb.AppendLine("using System;");
                        sb.AppendLine("using System.ComponentModel.DataAnnotations;");
                        sb.AppendLine("");
                        sb.AppendLine("namespace " + str_nameSpace + ".Model");
                        sb.AppendLine("{");
                        sb.AppendLine("    /// <summary>");
                        sb.AppendLine("    /// Model实体类：" + str_ModelName);
                        sb.AppendLine("    /// </summary>");
                        sb.AppendLine("    public partial class " + str_ModelName);
                        sb.AppendLine("    {");
                        sb.AppendLine("        /// <summary>");
                        sb.AppendLine("        /// " + st_prime_tip);
                        sb.AppendLine("        /// </summary>");
                        sb.AppendLine("        public " + str_ModelName + "() { }");
                        sb.AppendLine("");

                        #region Model
                        sb.AppendLine("        #region Model");
                        //遍历每个字段
                        foreach (DataRow dr in dt_tables.Rows)
                        {
                            string columnName = dr["column_name"].ToString().Trim();//字段名
                            string columnType = dr["data_type"].ToString().Trim();//字段类型
                            string columnComment = dr["comments"].ToString().Trim();//字段注释
                            string nullable = dr["nullable"].ToString().Trim();//是否可空
                            string data_default = dr["data_default"].ToString().Trim();//默認值
                            string data_maxLength = dr["char_col_decl_length"].ToString().Trim();//最大長度

                            #region 数据类型判断
                            if (data_default == "null")
                            {
                                data_default = "";
                            }
                            switch (columnType.ToUpper())
                            {
                                case "VARCHAR":
                                case "NVARCHAR2":
                                case "CLOB":
                                    columnType = "string";
                                    if (!string.IsNullOrEmpty(data_default))
                                    {
                                        data_default = " = \"" + data_default + "\"";
                                    }
                                    break;
                                case "NUMBER":
                                    columnType = "decimal";
                                    if (!string.IsNullOrEmpty(data_default))
                                    {
                                        data_default = " = " + data_default;
                                    }
                                    break;
                                case "DATE":
                                    columnType = "DateTime";
                                    if (!string.IsNullOrEmpty(data_default))
                                    {
                                        if (data_default == "sysdate")
                                        {
                                            data_default = " = DateTime.Now";
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                            #endregion

                            if (nullable.ToUpper().Trim() == "Y" && columnType.ToUpper() == "NUMBER")
                            {
                                nullable = "?";
                            }
                            else
                            {
                                nullable = "";
                            }
                            if (columnName.ToUpper() == prime_key)
                            {
                                columnComment = "主键 " + columnComment;
                            }
                            sb.AppendLine("");
                            sb.AppendLine("        private " + columnType + nullable + " _" + columnName.ToLower() + data_default + ";");
                            sb.AppendLine(@"        /// <summary>");
                            sb.AppendLine(@"        /// " + columnComment);
                            sb.AppendLine(@"        /// </summary>");
                            if (dr["nullable"].ToString().ToUpper().Trim() == "N")//不為空
                            {
                                sb.AppendLine(@"        [Required]");
                            }
                            if (!string.IsNullOrEmpty(data_maxLength))//最大長度
                            {
                                sb.AppendLine(@"        [StringLength(" + data_maxLength + ")]");
                            }
                            sb.AppendLine("        public " + columnType + nullable + " " + columnName.ToUpper());
                            sb.AppendLine("        {");
                            sb.AppendLine("            set { " + "_" + columnName.ToLower() + " = value; }");
                            sb.AppendLine("            get { return " + "_" + columnName.ToLower() + "; }");
                            sb.AppendLine("        }");

                        }
                        sb.AppendLine("        #endregion Model");
                        #endregion

                        sb.AppendLine("");
                        sb.AppendLine("");
                        sb.AppendLine("        #region UserModel");
                        sb.AppendLine("");
                        sb.AppendLine("        #endregion UserModel");
                        sb.AppendLine("");

                        sb.AppendLine("");
                        sb.AppendLine("");
                        sb.AppendLine("    }");
                        sb.AppendLine("}");
                        Save(file_Model + "/" + str_ModelName + ".cs", sb.ToString());
                        #endregion Model生成

                        #region IDAL生成
                        sb.Clear();

                        sb.AppendLine("using System;");
                        sb.AppendLine("using System.Collections.Generic;");
                        sb.AppendLine("using System.Data;");
                        sb.AppendLine("using " + str_nameSpace + ".Model;");
                        sb.AppendLine("");
                        sb.AppendLine("namespace " + str_nameSpace + ".IDAL");
                        sb.AppendLine("{");
                        sb.AppendLine("    /// <summary>");
                        sb.AppendLine("    /// 接口访问层：" + str_IDALName + "。" + st_prime_tip);
                        sb.AppendLine("    /// </summary>");
                        sb.AppendLine("    public interface " + str_IDALName + " : BaseIDAL<" + str_ModelName + ">");
                        sb.AppendLine("    {");
                        sb.AppendLine("        #region UserMethod");
                        sb.AppendLine("");
                        sb.AppendLine("");
                        sb.AppendLine("        #endregion UserMethod");
                        sb.AppendLine("");
                        sb.AppendLine("    }");
                        sb.AppendLine("}");
                        Save(file_IDAL + "/" + str_IDALName + ".cs", sb.ToString());
                        #endregion IDAL生成

                        #region BLL生成
                        sb.Clear();

                        sb.AppendLine("using System;");
                        sb.AppendLine("using System.Data;");
                        sb.AppendLine("using System.Collections.Generic;");
                        sb.AppendLine("using " + str_nameSpace + ".Model;");
                        sb.AppendLine("using " + str_nameSpace + ".IDAL;");
                        sb.AppendLine("using " + str_nameSpace + ".DALFactory;");
                        sb.AppendLine("");
                        sb.AppendLine("namespace " + str_nameSpace + ".BLL");
                        sb.AppendLine("{");
                        sb.AppendLine("    /// <summary>");
                        sb.AppendLine("    /// 业务逻辑层：" + str_BLLName);
                        sb.AppendLine("    /// </summary>");
                        sb.AppendLine("    public partial class " + str_BLLName + " : BaseBLL<" + str_ModelName + ", " + str_IDALName + ">");
                        sb.AppendLine("    {");
                        sb.AppendLine("        /// <summary>");
                        sb.AppendLine("        /// " + st_prime_tip);
                        sb.AppendLine("        /// </summary>");
                        sb.AppendLine("        public " + str_BLLName + "() : base() { }");
                        sb.AppendLine("");
                        sb.AppendLine("");
                        sb.AppendLine("        #region UserMethod");
                        sb.AppendLine("");
                        sb.AppendLine("");
                        sb.AppendLine("        #endregion UserMethod");
                        sb.AppendLine("");
                        sb.AppendLine("    }");
                        sb.AppendLine("}");
                        Save(file_BLL + "/" + str_BLLName + ".cs", sb.ToString());
                        #endregion IDAL生成

                        #region DAL生成
                        sb.Clear();

                        sb.AppendLine("using System;");
                        sb.AppendLine("using System.Data;");
                        sb.AppendLine("using System.Text;");
                        sb.AppendLine("using System.Data.OracleClient;");
                        sb.AppendLine("using " + str_nameSpace + ".IDAL;");
                        sb.AppendLine("using " + str_nameSpace + ".DBUtility;");
                        sb.AppendLine("using " + str_nameSpace + ".Common;");
                        sb.AppendLine("using System.Collections.Generic;");
                        sb.AppendLine("using " + str_nameSpace + ".Model;");
                        sb.AppendLine("");
                        sb.AppendLine("namespace " + str_nameSpace + ".OracleDAL");
                        sb.AppendLine("{");
                        sb.AppendLine("    /// <summary>");
                        sb.AppendLine("    /// 数据访问层：" + str_DALName);
                        sb.AppendLine("    /// </summary>");
                        sb.AppendLine("    public partial class " + str_DALName + " : BaseDAL<" + str_ModelName + ">, " + str_IDALName);
                        sb.AppendLine("    {");
                        sb.AppendLine("        /// <summary>");
                        sb.AppendLine("        /// 数据访问层，" + st_prime_tip);
                        sb.AppendLine("        /// </summary>");
                        sb.AppendLine("        public " + str_DALName + "() : base(\"" + prime_key + "\") { }");
                        sb.AppendLine("");
                        sb.AppendLine("");
                        sb.AppendLine("        #region UserMethod");
                        sb.AppendLine("");
                        sb.AppendLine("");
                        sb.AppendLine("        #endregion UserMethod");
                        sb.AppendLine("");
                        sb.AppendLine("    }");
                        sb.AppendLine("}");
                        Save(file_DAL + "/" + str_DALName + ".cs", sb.ToString());
                        #endregion DAL生成
                        successCount++;
                    }
                    else
                    {
                        failCount++;
                        failTables += item;
                    }
                }

                #region 生成基類文件
                Save(file_IDAL + "/BaseIDAL.cs", Create_BaseIDAL());
                Save(file_BLL + "/BaseBLL.cs", Create_BaseBLL());
                Save(file_DAL + "/BaseDAL.cs", Create_BaseDAL());
                #endregion

                if (failTables != "")
                {
                    failTables = "失败的表名：" + failTables;
                }
                label4.Text = "成功生成：" + successCount + "个文件！失败生成：" + failCount + "个文件！" + failTables;
            }
            else
            {
                MessageBox.Show("请选择或输入表名！");
            }
        }

        #endregion

        #region 基類文件內容的拼接

        public string Create_BaseIDAL()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Text;");
            sb.AppendLine("");
            sb.AppendLine("namespace " + str_nameSpace + ".IDAL");
            sb.AppendLine("{");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// 父類IDAL。作用：封裝常用的調用方法");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine("    /// <typeparam name=\"T\">Model</typeparam>");
            sb.AppendLine("    public interface BaseIDAL<T> where T : class");
            sb.AppendLine("    {");

            #region BasicMethod
            sb.AppendLine("     #region BasicMethod");
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// 是否存在该记录");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		bool Exists(string st_code);");
            sb.AppendLine("");

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// 增加一条数据");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		int Add(T model);");
            sb.AppendLine("");

            sb.AppendLine("     /// <summary>");
            sb.AppendLine("     /// 增加多条数据");
            sb.AppendLine("     /// </summary>");
            sb.AppendLine("     int AddList(List<T> list_model);");
            sb.AppendLine("");

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// 更新一条数据");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		int Update(T model);");
            sb.AppendLine("");

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// 更新多条数据");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		int UpdateList(List<T> list_model);");
            sb.AppendLine("");

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// 删除一条数据");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		int Delete(string st_code);");
            sb.AppendLine("");

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// 删除多条数据");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		int DeleteList(List<string> st_codeList);");
            sb.AppendLine("");

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// 得到实体对象");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		T GetModel(string st_code);");
            sb.AppendLine("");

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// 得到实体列表");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("        List<T> GetModelList(string strWhere);");
            sb.AppendLine("");

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// 获得数据列表");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		DataTable GetList(string strWhere);");
            sb.AppendLine("");

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// 获取记录总数");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		int GetRecordCount(string strWhere);");
            sb.AppendLine("");

            sb.AppendLine("     /// <summary>");
            sb.AppendLine("     /// 根据分页获得数据列表");
            sb.AppendLine("     /// </summary>");
            sb.AppendLine("     /// <param name=\"strWhere\">筛选语句where</param>");
            sb.AppendLine("     /// <param name=\"orderby\">排序</param>");
            sb.AppendLine("     /// <param name=\"startIndex\">开始索引</param>");
            sb.AppendLine("     /// <param name=\"endIndex\">结束索引</param>");
            sb.AppendLine("     /// <returns></returns>");
            sb.AppendLine("     DataTable GetListByPage(string strWhere, string orderby, int startIndex, int endIndex);");
            sb.AppendLine("");

            sb.AppendLine("		#endregion  BasicMethod");

            #endregion

            sb.AppendLine("    }");
            sb.AppendLine("}");



            return sb.ToString();
        }

        public string Create_BaseBLL()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Text;");
            sb.AppendLine("using " + str_nameSpace + ".IDAL;");
            sb.AppendLine("");
            sb.AppendLine("namespace " + str_nameSpace + ".BLL");
            sb.AppendLine("{");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// 父類BLL。作用：封裝常用的調用方法");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine("    /// <typeparam name=\"T\">Model</typeparam>");
            sb.AppendLine("    /// <typeparam name=\"T2\">IDAL</typeparam>");
            sb.AppendLine("    public class BaseBLL<T, T2>");
            sb.AppendLine("        where T : class");
            sb.AppendLine("        where T2 : class ,BaseIDAL<T>");
            sb.AppendLine("    {");
            sb.AppendLine("        #region 給字段賦值");
            sb.AppendLine("");
            sb.AppendLine("        public T2 dal;");
            sb.AppendLine("");
            sb.AppendLine("        public BaseBLL()");
            sb.AppendLine("        {");
            sb.AppendLine("            dal = " + str_nameSpace + ".DALFactory.TDataAccess<T2>.CreateInstance();");
            sb.AppendLine("        }");
            sb.AppendLine("");
            sb.AppendLine("        #endregion");
            sb.AppendLine("");


            #region BasicMethod
            sb.AppendLine("        #region  BasicMethod");
            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 是否存在该记录");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public bool Exists(string st_code)");
            sb.AppendLine("        {");
            sb.AppendLine("            return dal.Exists(st_code);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 增加一条数据");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public int Add(T model)");
            sb.AppendLine("        {");
            sb.AppendLine("            return dal.Add(model);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 增加多条数据");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public int AddList(List<T> list_model)");
            sb.AppendLine("        {");
            sb.AppendLine("            return dal.AddList(list_model);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 更新一条数据");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public int Update(T model)");
            sb.AppendLine("        {");
            sb.AppendLine("            return dal.Update(model);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 更新多条数据");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public int UpdateList(List<T> list_model)");
            sb.AppendLine("        {");
            sb.AppendLine("            return dal.UpdateList(list_model);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 删除一条数据");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public int Delete(string st_code)");
            sb.AppendLine("        {");
            sb.AppendLine("            return dal.Delete(st_code);");
            sb.AppendLine("        }");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 删除多条数据");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public int DeleteList(List<string> st_codeList)");
            sb.AppendLine("        {");
            sb.AppendLine("            return dal.DeleteList(st_codeList);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 得到实体对象");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public T GetModel(string st_code)");
            sb.AppendLine("        {");
            sb.AppendLine("            return dal.GetModel(st_code);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 获得实体列表");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public List<T> GetModelList(string strWhere)");
            sb.AppendLine("        {");
            sb.AppendLine("            return dal.GetModelList(strWhere);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 获得数据列表");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public DataTable GetList(string strWhere)");
            sb.AppendLine("        {");
            sb.AppendLine("            return dal.GetList(strWhere);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 获得数据列表");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public DataTable GetAllList()");
            sb.AppendLine("        {");
            sb.AppendLine("            return GetList(\"\");");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 获取记录总数");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public int GetRecordCount(string strWhere)");
            sb.AppendLine("        {");
            sb.AppendLine("            return dal.GetRecordCount(strWhere);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 根据分页获得数据列表");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        /// <param name=\"strWhere\">筛选语句where</param>");
            sb.AppendLine("        /// <param name=\"orderby\">排序</param>");
            sb.AppendLine("        /// <param name=\"startIndex\">开始索引</param>");
            sb.AppendLine("        /// <param name=\"endIndex\">结束索引</param>");
            sb.AppendLine("        /// <returns></returns>");
            sb.AppendLine("        public DataTable GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)");
            sb.AppendLine("        {");
            sb.AppendLine("            return dal.GetListByPage(strWhere, orderby, startIndex, endIndex);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        #endregion  BasicMethod");
            #endregion

            sb.AppendLine("    }");
            sb.AppendLine("}");



            return sb.ToString();
        }

        public string Create_BaseDAL()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Data.OracleClient;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Reflection;");
            sb.AppendLine("using System.Text;");
            sb.AppendLine("using " + str_nameSpace + ".DBUtility;");
            sb.AppendLine("using " + str_nameSpace + ".Model;");
            sb.AppendLine("");
            sb.AppendLine("namespace " + str_nameSpace + ".OracleDAL");
            sb.AppendLine("{");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// 父類DAL。作用：封裝常用的調用方法");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine("    /// <typeparam name=\"T\">Model</typeparam>");
            sb.AppendLine("    public class BaseDAL<T> :IDAL.BaseIDAL<T> where T : class");
            sb.AppendLine("    {");

            #region 給字段賦值
            sb.AppendLine("        #region 給字段賦值");
            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 標誌");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        private const string MARK = \":\";");
            sb.AppendLine("");
            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 表名");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        private static string tableName = \"\";");
            sb.AppendLine("");
            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 主鍵名");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        private static string primeKey = \"\";");
            sb.AppendLine("");
            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 參數拼接。格式如 ST_CODE=:ST_CODE");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        private static string strParameter = \"\";");
            sb.AppendLine("");
            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 構造函數賦值");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        /// <param name=\"primekey\">主鍵名</param>");
            sb.AppendLine("        public BaseDAL(string primekey)");
            sb.AppendLine("        {");
            sb.AppendLine("            primeKey = primekey;");
            sb.AppendLine("            tableName = GetTabelName(typeof(T).Name);");
            sb.AppendLine("");
            sb.AppendLine("            strParameter = primeKey + \"=\" + MARK + primeKey;");
            sb.AppendLine("        }");
            sb.AppendLine("");
            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 獲取表名。去除_Model");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        /// <param name=\"name\"></param>");
            sb.AppendLine("        /// <returns></returns>");
            sb.AppendLine("        private string GetTabelName(string tablename)");
            sb.AppendLine("        {");
            sb.AppendLine("            return tablename.Substring(0, tablename.Length - \"_Model\".Length);");
            sb.AppendLine("        }");
            sb.AppendLine("        #endregion");
            sb.AppendLine("");
            #endregion

            #region BasicMethod
            sb.AppendLine("        #region  BasicMethod");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 是否存在该记录");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public bool Exists(string st_code)");
            sb.AppendLine("        {");
            sb.AppendLine("            StringBuilder strSql = new StringBuilder();");
            sb.AppendLine("            strSql.Append(\" select count(1) from \" + tableName);");
            sb.AppendLine("            strSql.Append(\" where \" + strParameter);");
            sb.AppendLine("            OracleParameter[] parameters ={");
            sb.AppendLine("                new OracleParameter(MARK + primeKey,st_code)");
            sb.AppendLine("            };");
            sb.AppendLine("            return " + str_SqlHelper + "." + str_Exists + "(strSql.ToString(), parameters);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 增加一条数据");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public int Add(T model)");
            sb.AppendLine("        {");
            sb.AppendLine("            StringBuilder strSql;");
            sb.AppendLine("            OracleParameter[] parameters;");
            sb.AppendLine("            AddModel(model, out strSql, out parameters);");
            sb.AppendLine("");
            sb.AppendLine("            return " + str_SqlHelper + "." + str_ExecuteNonQuery + "(strSql.ToString(), parameters);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 增加多条数据");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public int AddList(List<T> list_model)");
            sb.AppendLine("        {");
            sb.AppendLine("            StringBuilder strSql;");
            sb.AppendLine("            OracleParameter[] parameters;");
            sb.AppendLine("            List<" + str_nameSpace + ".DBUtility.CommandInfo> ci = new List<CommandInfo>();");
            sb.AppendLine("");
            sb.AppendLine("            foreach (T item in list_model)");
            sb.AppendLine("            {");
            sb.AppendLine("                strSql = new StringBuilder();");
            sb.AppendLine("                parameters = null;");
            sb.AppendLine("                AddModel(item, out strSql, out parameters);");
            sb.AppendLine("");
            sb.AppendLine("                " + str_nameSpace + ".DBUtility.CommandInfo subci = new CommandInfo(CommandType.Text, strSql.ToString(), parameters);");
            sb.AppendLine("                ci.Add(subci);");
            sb.AppendLine("            }");
            sb.AppendLine("            return " + str_SqlHelper + "." + str_UpdateDatabase + "(ci);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 更新一条数据");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public int Update(T model)");
            sb.AppendLine("        {");
            sb.AppendLine("            StringBuilder strSql;");
            sb.AppendLine("            OracleParameter[] parameters;");
            sb.AppendLine("            UpdateModel(model, out strSql, out parameters);");
            sb.AppendLine("");
            sb.AppendLine("            return " + str_SqlHelper + "." + str_ExecuteNonQuery + "(strSql.ToString(), parameters);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 更新多条数据");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public int UpdateList(List<T> list_model)");
            sb.AppendLine("        {");
            sb.AppendLine("            StringBuilder strSql;");
            sb.AppendLine("            OracleParameter[] parameters;");
            sb.AppendLine("            List<" + str_nameSpace + ".DBUtility.CommandInfo> ci = new List<CommandInfo>();");
            sb.AppendLine("");
            sb.AppendLine("            foreach (T item in list_model)");
            sb.AppendLine("            {");
            sb.AppendLine("                strSql = new StringBuilder();");
            sb.AppendLine("                parameters = null;");
            sb.AppendLine("                UpdateModel(item, out strSql, out parameters);");
            sb.AppendLine("");
            sb.AppendLine("                " + str_nameSpace + ".DBUtility.CommandInfo subci = new CommandInfo(CommandType.Text, strSql.ToString(), parameters);");
            sb.AppendLine("                ci.Add(subci);");
            sb.AppendLine("");
            sb.AppendLine("            }");
            sb.AppendLine("            return " + str_SqlHelper + "." + str_UpdateDatabase + "(ci);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 删除一条数据");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public int Delete(string st_code)");
            sb.AppendLine("        {");
            sb.AppendLine("            StringBuilder strSql = new StringBuilder();");
            sb.AppendLine("            strSql.Append(\" delete from \" + tableName);");
            sb.AppendLine("            strSql.Append(\" where \" + strParameter);");
            sb.AppendLine("            OracleParameter[] parameters ={");
            sb.AppendLine("                new OracleParameter(MARK + primeKey,st_code)");
            sb.AppendLine("            };");
            sb.AppendLine("");
            sb.AppendLine("            return " + str_SqlHelper + "." + str_ExecuteNonQuery + "(strSql.ToString(), parameters);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 删除多条数据");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        /// <returns></returns>");
            sb.AppendLine("        public int DeleteList(List<string> st_codeList)");
            sb.AppendLine("        {");
            sb.AppendLine("            StringBuilder strSql;");
            sb.AppendLine("            OracleParameter[] parameters;");
            sb.AppendLine("            List<" + str_nameSpace + ".DBUtility.CommandInfo> ci = new List<CommandInfo>();");
            sb.AppendLine("            foreach (string item in st_codeList)");
            sb.AppendLine("            {");
            sb.AppendLine("                strSql = new StringBuilder();");
            sb.AppendLine("                strSql.Append(\" delete from \" + tableName);");
            sb.AppendLine("                strSql.Append(\" where \" + strParameter);");
            sb.AppendLine("                parameters = new OracleParameter[1];");
            sb.AppendLine("                parameters[0] = new OracleParameter(MARK + primeKey, item);");
            sb.AppendLine("");
            sb.AppendLine("                " + str_nameSpace + ".DBUtility.CommandInfo subci = new CommandInfo(CommandType.Text, strSql.ToString(), parameters);");
            sb.AppendLine("                ci.Add(subci);");
            sb.AppendLine("            }");
            sb.AppendLine("            return " + str_SqlHelper + "." + str_UpdateDatabase + "(ci);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 获得实体对象");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public T GetModel(string st_code)");
            sb.AppendLine("        {");
            sb.AppendLine("            StringBuilder strSql = new StringBuilder();");
            sb.AppendLine("            strSql.Append(\" select \" + InsertStr() + \" from \" + tableName);");
            sb.AppendLine("            strSql.Append(\" where \" + strParameter);");
            sb.AppendLine("            OracleParameter[] parameters ={");
            sb.AppendLine("                new OracleParameter(MARK + primeKey,st_code)");
            sb.AppendLine("            };");
            sb.AppendLine("            return " + str_SqlHelper + ".GetModel<T>(strSql.ToString(), parameters);");
            sb.AppendLine("");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 获得实体列表");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public List<T> GetModelList(string strWhere)");
            sb.AppendLine("        {");
            sb.AppendLine("            StringBuilder strSql = new StringBuilder();");
            sb.AppendLine("            strSql.Append(\" select \" + InsertStr() + \" from \" + tableName);");
            sb.AppendLine("            if (!string.IsNullOrEmpty(strWhere.Trim()))");
            sb.AppendLine("            {");
            sb.AppendLine("                strSql.Append(\" where \" + strWhere);");
            sb.AppendLine("            }");
            sb.AppendLine("            return " + str_SqlHelper + ".GetModelList<T>(strSql.ToString());");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 获得数据列表");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public DataTable GetList(string strWhere)");
            sb.AppendLine("        {");
            sb.AppendLine("            StringBuilder strSql = new StringBuilder();");
            sb.AppendLine("            strSql.Append(\" select \" + InsertStr() + \" from \" + tableName);");
            sb.AppendLine("            if (!string.IsNullOrEmpty(strWhere.Trim()))");
            sb.AppendLine("            {");
            sb.AppendLine("                strSql.Append(\" where \" + strWhere);");
            sb.AppendLine("            }");
            sb.AppendLine("            return " + str_SqlHelper + "." + str_GetDataTable + "(strSql.ToString());");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 获取记录总数");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public int GetRecordCount(string strWhere)");
            sb.AppendLine("        {");
            sb.AppendLine("            StringBuilder strSql = new StringBuilder();");
            sb.AppendLine("            strSql.Append(\" select count(1) from \" + tableName);");
            sb.AppendLine("            if (!string.IsNullOrEmpty(strWhere.Trim()))");
            sb.AppendLine("            {");
            sb.AppendLine("                strSql.Append(\" where \" + strWhere);");
            sb.AppendLine("            }");
            sb.AppendLine("            object obj = " + str_SqlHelper + "." + str_ExecuteScalar + "(strSql.ToString());");
            sb.AppendLine("            if (obj == null)");
            sb.AppendLine("            {");
            sb.AppendLine("                return 0;");
            sb.AppendLine("            }");
            sb.AppendLine("            else");
            sb.AppendLine("            {");
            sb.AppendLine("                return Convert.ToInt32(obj);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine("");


            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 分页获取数据列表");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public DataTable GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)");
            sb.AppendLine("        {");
            sb.AppendLine("            StringBuilder strSql = new StringBuilder();");
            sb.AppendLine("            strSql.Append(\" SELECT * from ( \");");
            sb.AppendLine("            strSql.Append(\" SELECT ROW_NUMBER() OVER (\");");
            sb.AppendLine("            if (!string.IsNullOrEmpty(orderby.Trim()))");
            sb.AppendLine("            {");
            sb.AppendLine("                strSql.Append(\"order by T.\" + orderby);");
            sb.AppendLine("            }");
            sb.AppendLine("            else");
            sb.AppendLine("            {");
            sb.AppendLine("                strSql.Append(\"order by T.\" + primeKey + \" desc\");");
            sb.AppendLine("            }");
            sb.AppendLine("            strSql.Append(\")AS Row, T.*  from \" + tableName + \" T \");");
            sb.AppendLine("            if (!string.IsNullOrEmpty(strWhere.Trim()))");
            sb.AppendLine("            {");
            sb.AppendLine("                strSql.Append(\" WHERE \" + strWhere);");
            sb.AppendLine("            }");
            sb.AppendLine("            strSql.Append(\" ) TT\");");
            sb.AppendLine("            strSql.AppendFormat(\" WHERE TT.Row between {0} and {1}\", startIndex, endIndex);");
            sb.AppendLine("            return " + str_SqlHelper + "." + str_GetDataTable + "(strSql.ToString());");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        #endregion  BasicMethod");
            sb.AppendLine("");
            #endregion

            #region HelperMethod
            sb.AppendLine("        #region HelperMethod");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 拼接字符串。格式如 ST_NAME,ST_CODE");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        /// <returns></returns>");
            sb.AppendLine("        private static string InsertStr()");
            sb.AppendLine("        {");
            sb.AppendLine("            return InsertStr(\"\");");
            sb.AppendLine("        }");
            sb.AppendLine("");
            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 增加數據時的拼接字符串。格式如 :ST_NAME,:ST_CODE");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        /// <param name=\"mark\">參數。如 :</param>");
            sb.AppendLine("        /// <returns></returns>");
            sb.AppendLine("        private static string InsertStr(string mark)");
            sb.AppendLine("        {");
            sb.AppendLine("            StringBuilder sb = new StringBuilder();");
            sb.AppendLine("");
            sb.AppendLine("            foreach (PropertyInfo item in typeof(T).GetProperties())");
            sb.AppendLine("            {");
            sb.AppendLine("                string str = item.Name;");
            sb.AppendLine("                if (!string.IsNullOrEmpty(mark))");
            sb.AppendLine("                {");
            sb.AppendLine("                    str = mark + item.Name;");
            sb.AppendLine("                }");
            sb.AppendLine("                sb.Append(str + \",\");");
            sb.AppendLine("            }");
            sb.AppendLine("            return sb.ToString().TrimEnd(',');");
            sb.AppendLine("        }");
            sb.AppendLine("");
            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 更新數據時的拼接字符串。格式如 ST_NAME=:ST_NAME,ST_CODE=:ST_CODE");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        /// <returns></returns>");
            sb.AppendLine("        private static string UpdateStr()");
            sb.AppendLine("        {");
            sb.AppendLine("            StringBuilder sb = new StringBuilder();");
            sb.AppendLine("");
            sb.AppendLine("            foreach (PropertyInfo item in typeof(T).GetProperties())");
            sb.AppendLine("            {");
            sb.AppendLine("                sb.Append(item.Name + \"=\" + MARK + item.Name + \",\");");
            sb.AppendLine("            }");
            sb.AppendLine("            return sb.ToString().TrimEnd(',');");
            sb.AppendLine("        }");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 增加一个Model");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        /// <param name=\"model\">Model类型</param>");
            sb.AppendLine("        /// <param name=\"strSql\">给sql语句赋值</param>");
            sb.AppendLine("        /// <param name=\"parameters\">给参数赋值</param>");
            sb.AppendLine("        private static void AddModel(T model, out StringBuilder strSql, out OracleParameter[] parameters)");
            sb.AppendLine("            {");
            sb.AppendLine("                strSql = new StringBuilder();");
            sb.AppendLine("                strSql.Append(\" insert into \" + tableName);");
            sb.AppendLine("                strSql.Append(\"(\" + InsertStr() + \")\");");
            sb.AppendLine("                strSql.Append(\" values (\" + InsertStr(MARK) + \")\");");
            sb.AppendLine("                parameters = SetParameters(model);");
            sb.AppendLine("            }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 更新一个Model");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        /// <param name=\"model\">Model类型</param>");
            sb.AppendLine("        /// <param name=\"strSql\">给sql语句赋值</param>");
            sb.AppendLine("        /// <param name=\"parameters\">给参数赋值</param>");
            sb.AppendLine("        private static void UpdateModel(T model, out StringBuilder strSql, out OracleParameter[] parameters)");
            sb.AppendLine("        {");
            sb.AppendLine("            strSql = new StringBuilder();");
            sb.AppendLine("            strSql.Append(\" update \" + tableName);");
            sb.AppendLine("            strSql.Append(\" set \" + UpdateStr());");
            sb.AppendLine("            strSql.Append(\" where \");");
            sb.AppendLine("            strSql.Append(primeKey + \"=\" + MARK + primeKey);");
            sb.AppendLine("            parameters = SetParameters(model);");
            sb.AppendLine("        }");
            sb.AppendLine("");

            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// 给参数列表赋值");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        /// <param name=\"model\">Model类型</param>");
            sb.AppendLine("        /// <returns>OracleParameter[]</returns>");
            sb.AppendLine("        private static OracleParameter[] SetParameters(T model)");
            sb.AppendLine("        {");
            sb.AppendLine("            T t = (T)Activator.CreateInstance(typeof(T));");
            sb.AppendLine("            System.Reflection.PropertyInfo[] propertys = t.GetType().GetProperties();");
            sb.AppendLine("");
            sb.AppendLine("            int length = propertys.Length;");
            sb.AppendLine("            OracleParameter[] parameters = new OracleParameter[length];");
            sb.AppendLine("");
            sb.AppendLine("            for (int i = 0; i < length; i++)");
            sb.AppendLine("            {");
            sb.AppendLine("                string proName = propertys[i].Name;");
            sb.AppendLine("                object val = propertys[i].GetValue(model, null);");
            sb.AppendLine("                if (proName == primeKey)");
            sb.AppendLine("                {");
            sb.AppendLine("                    if (val == null)//如果是主鍵為null");
            sb.AppendLine("                    {");
            sb.AppendLine("                        return new OracleParameter[0];");
            sb.AppendLine("                    }");
            sb.AppendLine("                }");
            sb.AppendLine("                parameters[i] = new OracleParameter(MARK + proName, Common.CheckData.NullToDB(val));");
            sb.AppendLine("            }");
            sb.AppendLine("            return parameters;");
            sb.AppendLine("        }");
            sb.AppendLine("        #endregion HelperMethod");
            #endregion


            sb.AppendLine("    }");
            sb.AppendLine("}");



            return sb.ToString();
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace RWConfig
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //获取Configuration对象
        Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(".\\RWConfig.exe");//可执行程序exe路径

        #region 窗体加载事件
        private void Form1_Load(object sender, EventArgs e)
        {
            //根据Key读取<add>元素的Value
            string a = config.AppSettings.Settings["q1"].Value;//取AppSettings
            string c = config.AppSettings.Settings["q2"].Value;//取AppSettings
            string b = config.ConnectionStrings.ConnectionStrings["qqq"].ConnectionString;//取ConnectionStrings
            string d = config.ConnectionStrings.ConnectionStrings["aaa"].ConnectionString;//取ConnectionStrings
            textBox1.Text = a;//取出AppSettings放到textBox1
            textBox3.Text = b;//取出ConnectionStrings放到textBox3
            textBox4.Text = c;//取出ConnectionStrings放到textBox4
            textBox5.Text = d;//取出ConnectionStrings放到textBox5

            label1.Text = "修改dtp控件Format属性为Custom后下方代码才生效\nShowUpDown属性更换下拉按钮";
            dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm:ss";
        }
        #endregion

        #region 写配置按钮单击事件 更新appSettings 调用更新connectionStrings
        private void button1_Click(object sender, EventArgs e)
        {
            #region 更新appSettings
            //增加<add>元素
            //config.AppSettings.Settings.Add("qqq", "123");
            //删除<add>元素
            //config.AppSettings.Settings.Remove("qk");

            //写入<add>元素的Value
            config.AppSettings.Settings["q1"].Value = textBox1.Text.Trim();
            config.AppSettings.Settings["q2"].Value = textBox4.Text.Trim();
            //一定要记得保存，写不带参数的config.Save()也可以
            config.Save(ConfigurationSaveMode.Modified);
            //刷新，否则程序读取的还是之前的值（可能已装入内存）
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
            
            #endregion

            #region 调用更新connectionStrings
            UpdateConnectionStringsConfig("qqq", textBox3.Text);
            UpdateConnectionStringsConfig("aaa", textBox5.Text);
            #endregion

            MessageBox.Show("配置更新成功");
        }
        #endregion

        #region 更新connectionStrings
        ///<summary>
        ///更新连接字符串
        ///</summary>
        ///<param name="newName">连接字符串名称</param>
        ///<param name="newConString">连接字符串内容</param>
        public static void UpdateConnectionStringsConfig(string newName, string newConString)
        {
            //指定config文件读取
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(".\\RWConfig.exe");//可执行程序exe路径

            bool exist = false; //记录该连接串是否已经存在
            //如果要更改的连接串已经存在
            if (config.ConnectionStrings.ConnectionStrings[newName] != null)
            {
                exist = true;
            }
            // 如果连接串已存在，首先删除它
            if (exist)
            {
                config.ConnectionStrings.ConnectionStrings.Remove(newName);
            }
            //新建一个连接字符串实例
            ConnectionStringSettings mySettings = new ConnectionStringSettings(newName, newConString);
            // 将新的连接串添加到配置文件中.
            config.ConnectionStrings.ConnectionStrings.Add(mySettings);
            // 保存对配置文件所作的更改
            config.Save(ConfigurationSaveMode.Modified);
            // 强制重新载入配置文件的ConnectionStrings配置节
            ConfigurationManager.RefreshSection("connectionStrings");
        }
        #endregion

        #region cmd执行按钮单击事件
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false;     //是否使用操作系统shell启动
                process.StartInfo.CreateNoWindow = true;        //不显示程序窗口
                process.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
                process.StartInfo.RedirectStandardInput = true;  //接受来自调用程序的输入信息
                process.StartInfo.RedirectStandardError = true;  //重定向标准错误输出
                process.Start();
                //process.StandardInput.WriteLine(command + "&exit");   //向cmd窗口发送输入信息，&exit意思为不论command命令执行成功与否，接下来都执行exit这句
                process.StandardInput.WriteLine(textBox1.Text + "&exit");   //向cmd窗口发送输入信息，&exit意思为不论command命令执行成功与否，接下来都执行exit这句
                process.StandardInput.AutoFlush = true;

                string output = process.StandardOutput.ReadToEnd();  //获取cmd的输出信息
                process.WaitForExit();         //等待程序执行完退出进程
                process.Close();
                process.Dispose();

                MessageBox.Show("command命令：" + output);
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
            }
        }
        #endregion

        #region 两个dtp转格式
        private void button3_Click(object sender, EventArgs e)
        {
            string begintime = dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string endtime = dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss");
            textBox2.Text = "\"" + begintime + "\" \"" + endtime + "\"";
        }
        #endregion
    }
}
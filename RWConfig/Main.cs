using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RWConfig
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        string path = ".\\RWConfig.exe";//变量存放配置文件路径（不需要后缀）

        #region 窗体加载事件
        private void Main_Load(object sender, EventArgs e)
        {
            //radioButton1-connectionStrings
            //radioButton2-appSettings
            radioButton1.Checked = true;//运行程序默认选中第一个单选框
            textBox1.Text = "qqq";//文本框默认

            label1.Text = "修改dtp控件Format属性为Custom后下方代码才生效\nShowUpDown属性更换下拉按钮";
            dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";//自定义格式
            dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm:ss";//自定义格式
        }
        #endregion

        #region 根据key读value 条件为单选按钮
        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            string a;//取出的value 用来判空
            if (textBox1.Text == "")
            {
                MessageBox.Show("key不能为空");
            }
            else
            {
                if (radioButton1.Checked)
                {
                    a = Gaiokane.RWConfig.GetconnectionStringsValue(textBox1.Text, path);//头文件using Gaiokane;可省略此处Gaiokane
                    if (a != "")
                    {
                        textBox2.Text = a;
                    }
                    else
                    {
                        MessageBox.Show("配置文件中不存在该key");
                        textBox1.SelectAll();//全选
                        textBox1.Focus();//获取焦点
                    }
                }
                else
                {
                    a = Gaiokane.RWConfig.GetappSettingsValue(textBox1.Text, path);//头文件using Gaiokane;可省略此处Gaiokane
                    if (a != "")
                    {
                        textBox2.Text = a;
                    }
                    else
                    {
                        MessageBox.Show("配置文件中不存在该key");
                        textBox1.SelectAll();//全选
                        textBox1.Focus();//获取焦点
                    }
                }
            }
        }
        #endregion

        #region 写配置 根据key更新配置文件中value 若不存在该key 则新建此条键值对
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
                MessageBox.Show("key/value不能为空");
            else
            {
                if (radioButton1.Checked)
                    Gaiokane.RWConfig.SetconnectionStringsValue(textBox1.Text, textBox2.Text, path);//头文件using Gaiokane;可省略此处Gaiokane
                else
                    Gaiokane.RWConfig.SetappSettingsValue(textBox1.Text, textBox2.Text, path);//头文件using Gaiokane;可省略此处Gaiokane
            }
            MessageBox.Show("写入成功");
        }
        #endregion

        #region cmd执行value中的内容
        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("value不能为空");
            }
            else
            {
                try
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.UseShellExecute = false;     //是否使用操作系统shell启动
                    process.StartInfo.CreateNoWindow = true;        //不显示程序窗口
                    process.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
                    process.StartInfo.RedirectStandardInput = true;  //接受来自调用程序的输入信息
                    process.StartInfo.RedirectStandardError = true;  //重定向标准错误输出
                    process.Start();
                    //process.StandardInput.WriteLine(command + "&exit");   //向cmd窗口发送输入信息，&exit意思为不论command命令执行成功与否，接下来都执行exit这句
                    process.StandardInput.WriteLine(textBox2.Text + "&exit");   //向cmd窗口发送输入信息，&exit意思为不论command命令执行成功与否，接下来都执行exit这句
                    process.StandardInput.AutoFlush = true;

                    string output = process.StandardOutput.ReadToEnd();  //获取cmd的输出信息
                    process.WaitForExit();         //等待程序执行完退出进程
                    process.Close();
                    process.Dispose();

                    MessageBox.Show("command命令：" + output);
                }
                catch (Exception a)
                {
                    MessageBox.Show(a.Message);
                }
            }
        }
        #endregion

        #region 将两个dtp自定义格式并组合显示到文本框中
        private void button4_Click(object sender, EventArgs e)
        {
            string begintime = dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss");//自定义格式
            string endtime = dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss");//自定义格式
            textBox3.Text = "\"" + begintime + "\" \"" + endtime + "\"";//拼凑
        }
        #endregion

        #region 点击文本框全选并获取焦点
        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.SelectAll();//全选
            textBox1.Focus();//获取焦点
        }
        #endregion
    }
}
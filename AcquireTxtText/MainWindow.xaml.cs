using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AcquireTxtText
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        //定义文件选择按钮的功能
        private void FileChoose_Click(object sender, RoutedEventArgs e)
        {
            //选择文件操作
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                //设置不可选择多个文件夹
                Multiselect = false
            };
            //判断文件TextBox是否为空，不为空则清除在打开文件选择对话框。
            if (SourceFileText != null)
            {
                SourceFileText.Text = "";
                openFileDialog.ShowDialog();
            }
            else
            {
                openFileDialog.ShowDialog();
            }
            //提取与显示文件名
            string fileName = openFileDialog.FileName;
            FilePath.Text = fileName;
            //读取文本与行数
            FileReadMethod(fileName);

        }
        //读取文件内容并捕获用户没有选择文件时的操作，弹出提示。若用户有选择，显示文本并显示行数。
        private void FileReadMethod(string fileName)
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName, Encoding.Default))
                {
                    String line;
                    int i = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        SourceFileText.Text += line + "\n";
                        i++;
                    }
                    SourceTextLine.Text = i.ToString();
                }
            }
            catch (Exception)
            {
                //当用户选择文件时点击取消按钮提示无法在TextBox打印文本
                MessageBox.Show("路径为空");
            }
        }
        //定义的清除所有文本按钮的操作
        private void ClearAllText_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }
        //清除所有内容
        private void ClearAll()
        {
            FilePath.Text = "";
            Method.Text = "";
            SourceFileText.Text = "";
            AfterHandleText.Text = "";
            SourceTextLine.Text = "";
            AfterHandleTextLine.Text = "";
            SaveOtherChar.IsChecked = false;
            AcquireChar.Text = "";
        }

        //定义了保存文件的方法
        private void SaveFileMethod(TextBox textBox)
        {
            string localFilePath = "", fileNameExt = "", newFileName = "", FilePath = "";
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "txt files(*.txt)|*.txt|xls files(*.xls)|*.xls|All files(*.*)|*.*"

            };
            bool? result = saveFileDialog.ShowDialog();
            //点了保存按钮进入
            if (result == true)
            {
                //获得文件路径
                localFilePath = saveFileDialog.FileName.ToString();
                //获取文件名，不带路径
                fileNameExt = localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1);
                //获取文件路径，不带文件名
                FilePath = localFilePath.Substring(0, localFilePath.LastIndexOf("\\"));
                //给文件名前加上时间
                newFileName = fileNameExt;
                newFileName = FilePath + "\\" + newFileName;
                //在文件名里加字符
                //saveFileDialog.FileName.Insert(1,"dameng");
                //为用户使用 SaveFileDialog 选定的文件名创建读/写文件流。
                System.IO.File.WriteAllText(newFileName, textBox.Text, Encoding.Default); //这里的文件名其实是含有路径的。
            }
        }
        //定义了保存新文件按钮的功能
        private void NewFileSave(object sender, RoutedEventArgs e)
        {
            SaveFileMethod(AfterHandleText);
        }
        //判断字符串是否包含中文
        public static Boolean IsContainChinese(String str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }
        //判断字符串是否包含英文
        public static Boolean IsContainEnglish(String str)
        {
            return Regex.IsMatch(str, @"[a-zA-z]");
        }
        //判断字符串是否包含数字
        public static Boolean IsContainNumber(String str)
        {
            return Regex.IsMatch(str, @"[0-9]");
        }
        //判断字符串是否包含其他字符
        public static Boolean IsContainOtherChar(String str)
        {
            if (IsContainChinese(str) == false && IsContainEnglish(str) == false && IsContainNumber(str) == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //传入点击的提取方式，打印其方法
        private void SaveMenuAndPrintMethodText(MenuItem mi)
        {
            if (mi == OnlyCh)
            {
                Method.Text = "只保留中文";
            }
            else if (mi == ChAndEn)
            {
                Method.Text = "保留中文与英文";
            }
            else if (mi == ChAndNum)
            {
                Method.Text = "保留中文与数字";
            }
            else if (mi == ChAndEnAndNum)
            {
                Method.Text = "保留中英文与数字";
            }

            else if (mi == OnlyEn)
            {
                Method.Text = "只保留英文";
            }
            else if (mi == EnAndCh)
            {
                Method.Text = "保留英文与中文";
            }
            else if (mi == EnAndNum)
            {
                Method.Text = "保留英文与数字";
            }
            else if (mi == EnAndChAndNum)
            {
                Method.Text = "保留英文与中文与数字";
            }

            else if (mi == OnlyNum)
            {
                Method.Text = "只保留数字";
            }
            else if (mi == NumAndCh)
            {
                Method.Text = "保留数字与中文";
            }
            else if (mi == NumAndEn)
            {
                Method.Text = "保留数字与英文";
            }
            else if (mi == NumAndChAndEn)
            {
                Method.Text = "保留数字与中英文";
            }

            else if (mi == AcquireLineByInputSomething)
            {
                Method.Text = "按指定内容提取行";
            }
        }

        //定义提取中文子菜单的操作
        private void OnlyCh_Click(object sender, RoutedEventArgs e)
        {
            //Method.Text = "只保留中文";
            SaveMenuAndPrintMethodText(OnlyCh);
        }
        private void ChAndEn_Click(object sender, RoutedEventArgs e)
        {
            //Method.Text = "保留中文与英文";
            SaveMenuAndPrintMethodText(ChAndEn);
        }
        private void ChAndNum_Click(object sender, RoutedEventArgs e)
        {
            ////Method.Text = "保留中文与数字";
            SaveMenuAndPrintMethodText(ChAndNum);
        }
        private void ChAndEnAndNum_Click(object sender, RoutedEventArgs e)
        {
            //Method.Text = "保留中英文与数字";
            SaveMenuAndPrintMethodText(ChAndEnAndNum);
        }

        //定义提取英文子菜单的操作
        private void OnlyEn_Click(object sender, RoutedEventArgs e)
        {
            SaveMenuAndPrintMethodText(OnlyEn);
        }
        private void EnAndCh_Click(object sender, RoutedEventArgs e)
        {
            SaveMenuAndPrintMethodText(EnAndCh);
        }
        private void EnAndNum_Click(object sender, RoutedEventArgs e)
        {
            SaveMenuAndPrintMethodText(EnAndNum);
        }
        private void EnAndChAndNum_Click(object sender, RoutedEventArgs e)
        {
            SaveMenuAndPrintMethodText(EnAndChAndNum);
        }

        //定义提取数字子菜单的操作
        private void OnlyNum_Click(object sender, RoutedEventArgs e)
        {
            SaveMenuAndPrintMethodText(OnlyNum);
        }
        private void NumAndCh_Click(object sender, RoutedEventArgs e)
        {
            SaveMenuAndPrintMethodText(NumAndCh);
        }
        private void NumAndEn_Click(object sender, RoutedEventArgs e)
        {
            SaveMenuAndPrintMethodText(NumAndEn);
        }
        private void NumAndChAndEn_Click(object sender, RoutedEventArgs e)
        {
            SaveMenuAndPrintMethodText(NumAndChAndEn);
        }

        //返回字符串的中文字符(新方法)
        private void ReturnChinese(string s)
        {
            using (StreamReader sr = new StreamReader(FilePath.Text, Encoding.Default))
            {
                String line;
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if (SaveOtherChar.IsChecked == true)
                    {
                        if (IsContainChinese(line) || IsContainOtherChar(line))
                        {
                            for (int count = 0; count < line.Length; count++)
                            {
                                if (IsContainChinese(line[count] + "") || IsContainOtherChar(line[count] + ""))
                                {
                                    AfterHandleText.Text += line[count];
                                }
                            }
                            i++;
                            AfterHandleText.Text += "\r\n";
                        }
                    }
                    else
                    {
                        if (IsContainChinese(line))
                        {
                            for (int count = 0; count < line.Length; count++)
                            {
                                if (IsContainChinese(line[count] + ""))
                                {
                                    AfterHandleText.Text += line[count];
                                }
                            }
                            i++;
                            AfterHandleText.Text += "\r\n";
                        }
                    }
                }
                AfterHandleTextLine.Text = i.ToString();
            }
        }
        //返回字符串的英文字符(新方法)
        private void ReturnEnglish(string s)
        {
            using (StreamReader sr = new StreamReader(FilePath.Text, Encoding.Default))
            {
                String line;
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if (SaveOtherChar.IsChecked == true)
                    {
                        if (IsContainEnglish(line) || IsContainOtherChar(line))
                        {
                            for (int count = 0; count < line.Length; count++)
                            {
                                if (IsContainEnglish(line[count] + "") || IsContainOtherChar(line[count] + ""))
                                {
                                    AfterHandleText.Text += line[count];
                                }
                            }
                            i++;
                            AfterHandleText.Text += "\r\n";
                        }
                    }
                    else
                    {
                        if (IsContainEnglish(line))
                        {
                            for (int count = 0; count < line.Length; count++)
                            {
                                if (IsContainEnglish(line[count] + ""))
                                {
                                    AfterHandleText.Text += line[count];
                                }
                            }
                            i++;
                            AfterHandleText.Text += "\r\n";
                        }
                    }
                }
                AfterHandleTextLine.Text = i.ToString();
            }
        }
        //返回字符串的数字字符(新方法)
        private void ReturnNum(string s)
        {
            using (StreamReader sr = new StreamReader(FilePath.Text, Encoding.Default))
            {
                String line;
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if (SaveOtherChar.IsChecked == true)
                    {
                        if (IsContainNumber(line) || IsContainOtherChar(line))
                        {
                            for (int count = 0; count < line.Length; count++)
                            {
                                if (IsContainNumber(line[count] + "") || IsContainOtherChar(line[count] + ""))
                                {
                                    AfterHandleText.Text += line[count];
                                }
                            }
                            i++;
                            AfterHandleText.Text += "\r\n";
                        }
                    }
                    else
                    {
                        if (IsContainNumber(line))
                        {
                            for (int count = 0; count < line.Length; count++)
                            {
                                if (IsContainNumber(line[count] + ""))
                                {
                                    AfterHandleText.Text += line[count];
                                }
                            }
                            i++;
                            AfterHandleText.Text += "\r\n";
                        }
                    }
                }
                AfterHandleTextLine.Text = i.ToString();
            }
        }

        //返回字符串的中英字符(新方法)
        private void ReturnChAndEn(string s)
        {
            using (StreamReader sr = new StreamReader(FilePath.Text, Encoding.Default))
            {
                String line;
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if (SaveOtherChar.IsChecked == true)
                    {
                        if (IsContainChinese(line) || IsContainEnglish(line) || IsContainOtherChar(line))
                        {
                            for (int count = 0; count < line.Length; count++)
                            {
                                if (IsContainChinese(line[count] + "") || IsContainEnglish(line[count] + "") || IsContainOtherChar(line[count] + ""))
                                {
                                    AfterHandleText.Text += line[count];
                                }
                            }
                            i++;
                            AfterHandleText.Text += "\r\n";
                        }
                    }
                    else
                    {
                        if (IsContainChinese(line) || IsContainEnglish(line))
                        {
                            for (int count = 0; count < line.Length; count++)
                            {
                                if (IsContainChinese(line[count] + "") || IsContainEnglish(line[count] + ""))
                                {
                                    AfterHandleText.Text += line[count];
                                }
                            }
                            i++;
                            AfterHandleText.Text += "\r\n";
                        }
                    }
                }
                AfterHandleTextLine.Text = i.ToString();
            }
        }
        //返回字符串的中数字符(新方法)
        private void ReturnChAndNum(string s)
        {
            using (StreamReader sr = new StreamReader(FilePath.Text, Encoding.Default))
            {
                String line;
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if (SaveOtherChar.IsChecked == true)
                    {
                        if (IsContainChinese(line) || IsContainNumber(line) || IsContainOtherChar(line))
                        {
                            for (int count = 0; count < line.Length; count++)
                            {
                                if (IsContainChinese(line[count] + "") || IsContainNumber(line[count] + "") || IsContainOtherChar(line[count] + ""))
                                {
                                    AfterHandleText.Text += line[count];
                                }
                            }
                            i++;
                            AfterHandleText.Text += "\r\n";
                        }
                    }
                    else
                    {
                        if (IsContainChinese(line) || IsContainNumber(line))
                        {
                            for (int count = 0; count < line.Length; count++)
                            {
                                if (IsContainChinese(line[count] + "") || IsContainNumber(line[count] + ""))
                                {
                                    AfterHandleText.Text += line[count];
                                }
                            }
                            i++;
                            AfterHandleText.Text += "\r\n";
                        }
                    }
                }
                AfterHandleTextLine.Text = i.ToString();
            }
        }
        //返回字符串的英数字符(新方法)
        private void ReturnEnAndNum(string s)
        {
            using (StreamReader sr = new StreamReader(FilePath.Text, Encoding.Default))
            {
                String line;
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if (SaveOtherChar.IsChecked == true)
                    {
                        if (IsContainEnglish(line) || IsContainNumber(line) || IsContainOtherChar(line))
                        {
                            for (int count = 0; count < line.Length; count++)
                            {
                                if (IsContainEnglish(line[count] + "") || IsContainNumber(line[count] + "") || IsContainOtherChar(line[count] + ""))
                                {
                                    AfterHandleText.Text += line[count];
                                }
                            }
                            i++;
                            AfterHandleText.Text += "\r\n";
                        }
                    }
                    else
                    {
                        if (IsContainEnglish(line) || IsContainNumber(line))
                        {
                            for (int count = 0; count < line.Length; count++)
                            {
                                if (IsContainEnglish(line[count] + "") || IsContainNumber(line[count] + ""))
                                {
                                    AfterHandleText.Text += line[count];
                                }
                            }
                            i++;
                            AfterHandleText.Text += "\r\n";
                        }
                    }
                }
                AfterHandleTextLine.Text = i.ToString();
            }
        }

        //返回中英文与数字三种字符
        private void ReturnChAndEnAndNum(string s)
        {
            using (StreamReader sr = new StreamReader(FilePath.Text, Encoding.Default))
            {
                String line;
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if (SaveOtherChar.IsChecked == true)
                    {
                        if (IsContainEnglish(line) || IsContainNumber(line) || IsContainChinese(line) || IsContainOtherChar(line))
                        {
                            for (int count = 0; count < line.Length; count++)
                            {
                                if (IsContainEnglish(line[count] + "") || IsContainNumber(line[count] + "") || IsContainChinese(line[count] + "") || IsContainOtherChar(line[count] + ""))
                                {
                                    AfterHandleText.Text += line[count];
                                }
                            }
                            i++;
                            AfterHandleText.Text += "\r\n";
                        }
                    }
                    else
                    {
                        if (IsContainEnglish(line) || IsContainNumber(line) || IsContainChinese(line))
                        {
                            for (int count = 0; count < line.Length; count++)
                            {
                                if (IsContainEnglish(line[count] + "") || IsContainNumber(line[count] + "") || IsContainChinese(line[count] + ""))
                                {
                                    AfterHandleText.Text += line[count];
                                }
                            }
                            i++;
                            AfterHandleText.Text += "\r\n";
                        }
                    }
                }
                AfterHandleTextLine.Text = i.ToString();
            }
        }

        //定义点击按钮的操作
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (FilePath.Text == "")
            {
                MessageBox.Show("没有载入文件");
            }
            else if (Method.Text == "")
            {
                MessageBox.Show("没有选择提取方法");
            }
            AfterHandleText.Text = "";
            AfterHandleTextLine.Text = "";
            //只保留中文英文数字
            if (Method.Text == "只保留中文")
            {
                if (IsContainChinese(SourceFileText.Text))
                {
                    ReturnChinese(SourceFileText.Text);

                }
                else
                {
                    MessageBox.Show("文档中没有中文");
                }
            }
            else if (Method.Text == "只保留英文")
            {
                if (IsContainEnglish(SourceFileText.Text))
                {
                    ReturnEnglish(SourceFileText.Text);

                }
                else
                {
                    MessageBox.Show("文档中没有英文");
                }
            }
            else if (Method.Text == "只保留数字")
            {
                if (IsContainNumber(SourceFileText.Text))
                {
                    ReturnNum(SourceFileText.Text);

                }
                else
                {
                    MessageBox.Show("文档中没有数字");
                }
            }
            //保留两种类型
            else if (Method.Text == "保留中文与英文" || Method.Text == "保留英文与中文")
            {
                if (IsContainChinese(SourceFileText.Text) || IsContainEnglish(SourceFileText.Text))
                {
                    ReturnChAndEn(SourceFileText.Text);

                }
                else
                {
                    MessageBox.Show("文档中没有中文或英文");
                }
            }
            else if (Method.Text == "保留中文与数字" || Method.Text == "保留数字与中文")
            {
                if (IsContainChinese(SourceFileText.Text) || IsContainNumber(SourceFileText.Text))
                {
                    ReturnChAndNum(SourceFileText.Text);

                }
                else
                {
                    MessageBox.Show("文档中没有中文或数字");
                }
            }
            else if (Method.Text == "保留英文与数字" || Method.Text == "保留数字与英文")
            {
                if (IsContainEnglish(SourceFileText.Text) || IsContainNumber(SourceFileText.Text))
                {
                    ReturnEnAndNum(SourceFileText.Text);

                }
                else
                {
                    MessageBox.Show("文档中没有英文或数字");
                }
            }
            //保留三种类型
            else if (Method.Text == "保留中英文与数字" || Method.Text == "保留英文与中文与数字" || Method.Text == "保留数字与中英文")
            {
                if (IsContainChinese(SourceFileText.Text) || IsContainNumber(SourceFileText.Text) || IsContainNumber(SourceFileText.Text))
                {
                    ReturnChAndEnAndNum(SourceFileText.Text);

                }
                else
                {
                    MessageBox.Show("文档中没有中文或英文或数字");
                }
            }
            //按指定内容提取行
            else if (Method.Text == "按指定内容提取行")
            {
                if (AcquireChar.Text != "")
                {
                    ReturnAcquireLine(AcquireChar.Text);
                }
                else if (AcquireChar.Text == "")
                {
                    MessageBox.Show("没有指定内容");
                }
            }
        }

        private void ReturnAcquireLine(string text)
        {
            using (StreamReader sr = new StreamReader(FilePath.Text, Encoding.Default))
            {
                String line;
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains(text))
                    {
                        AfterHandleText.Text += line + "\r\n";
                        i++;
                    }
                    else { }
                }
                AfterHandleTextLine.Text = i.ToString();
            }
        }
        //定义点击按内容提取行时的操作
        private void AcquireLineByInputSomething_Click(object sender, RoutedEventArgs e)
        {
            SaveMenuAndPrintMethodText(AcquireLineByInputSomething);
        }
    }
}

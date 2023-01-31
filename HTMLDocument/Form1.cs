using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HTMLDocument
{
    public partial class Form1 : Form
    {   
        private bool checkVariable = false;
        private bool checkOne = false;
        private bool checkTwo = false;
        public Form1()
        {
            InitializeComponent();
            urlTextBox.Text = @"https://dzen.ru/news?issue_tld=ru";
            checkBox1.Text = "Получить список всех категорий";
            checkBox2.Text = "Получить список наименований последних новостей";
        }
        private void getCaptionCategories(string url)
        {   
            resultListBox.Items.Clear();
            progressBar1.Value = 0;
            string titleContent = "";

            // загружаем страничку в браузер
            WebBrowser browser = new WebBrowser();
            browser.Navigate(urlTextBox.Text);
            while (browser.ReadyState != WebBrowserReadyState.Complete)
            {
                while (progressBar1.Value != progressBar1.Maximum)
                {
                    progressBar1.Value++;
                }
                Application.DoEvents();
            }
                   
            //Верхнее меню, список категорий
            HtmlElementCollection elementsByTagName =
                    browser.Document.GetElementsByTagName("span");

            foreach (HtmlElement element in elementsByTagName)
            {
                titleContent += element.InnerHtml;
            }

            int startCut = titleContent.IndexOf("Ещё");
            int endCut = titleContent.LastIndexOf("Ещё");

            string removeFromTilteContentOne = titleContent.Substring(startCut, endCut);
            int cutTags = removeFromTilteContentOne.IndexOf("<");

            string newString = removeFromTilteContentOne.Remove(cutTags);
            newString = newString.Replace("Ещё", "_");
            newString = newString.Remove(0, 1);

            string[] array = newString.Split('_');
            for (int i = 0; i < array.Length; i++)
            {
                resultListBox.Items.Add(array[i]);
            }

            browser.Dispose();
        }
        private void getTitleAllNews(string url)
        {
            progressBar1.Value = 0;
            resultListBox.Items.Clear();
            string titleContent = "";

            // загружаем страничку в браузер
            WebBrowser browser = new WebBrowser();
            browser.Navigate(urlTextBox.Text);
            while (browser.ReadyState != WebBrowserReadyState.Complete)
            {
                while (progressBar1.Value != progressBar1.Maximum)
                {
                    progressBar1.Value++;
                }
                Application.DoEvents();
            }

            // получаем все теги <h2> и перебираем их
            HtmlElementCollection elementsByTagName =
                 browser.Document.GetElementsByTagName("h2");
            foreach (HtmlElement element in elementsByTagName)
            {
                resultListBox.Items.Add(element.OuterText);
                //titleContent += element.OuterText + "\n\n";
            }

            browser.Dispose();
        }
        private void button1_Click(object sender, EventArgs e)
        {   
            if (checkOne || checkTwo)
            {
                checkVariable = true;

                if (checkOne)
                {
                    getCaptionCategories(urlTextBox.Text);
                    while (progressBar1.Value != progressBar1.Maximum)
                        progressBar1.Value++;
                }
                if (checkTwo)
                {
                    getTitleAllNews(urlTextBox.Text);
                    while (progressBar1.Value != progressBar1.Maximum)
                        progressBar1.Value++;
                }
            }
            else
            {
                if (!checkVariable)
                {
                    MessageBox.Show("Необходимо сделать выбор");
                    return;
                }
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {   
            if (checkBox1.Checked)
            {
                checkBox2.Visible = false;
                checkOne = true;
                checkTwo = false;
            }
            else
            {
                checkBox2.Visible = true;
                checkTwo = true;
                checkOne = false;
            }
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox1.Visible = false;
                checkOne = false;
                checkTwo = true;
            }
            else
            {
                checkBox1.Visible = true;
                checkOne = true;
                checkTwo = false;
            }
        }
    }
}

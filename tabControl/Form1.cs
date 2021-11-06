using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tabControl
{


    public partial class Form1 : Form
    {
        public RichTextBox RichTB
        {
            get
            {

                foreach (var item in tabControl.SelectedTab.Controls.OfType<RichTextBox>())
                {
                    return item;
                }
                return null;
            }
        }
        public Form1()
        {
            InitializeComponent();
            tabControl.TabPages.Clear();
            richTextBox.AllowDrop = true;
            NewPage();
        }

        void NewPage()
        {
            TabPage page = new TabPage($"Page {tabControl.TabPages.Count + 1}");
            page.Padding = new Padding(3);
            page.UseVisualStyleBackColor = true;

            RichTextBox rTb = new RichTextBox();
            rTb.Location = new Point(6 / 2, 6 / 2);
            rTb.Name = "richTextBox";
            rTb.Size = new Size(7562, 3732);
            rTb.TabIndex = 0;
            rTb.Text = "";
            rTb.TextChanged += richTextBox_TextChanged;
            rTb.AllowDrop=true;
            rTb.DragEnter += (sender, e) => {
                if (e.Data.GetDataPresent(DataFormats.Text) || e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                    e.Effect = DragDropEffects.None;
            };
            rTb.DragDrop += (sender, e) =>
            {
                if (e.Data.GetDataPresent(DataFormats.Text))
                {
                    rTb.Text += e.Data.GetData(DataFormats.Text).ToString();
                }
                else if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    foreach (var item in (string[])e.Data.GetData(DataFormats.FileDrop))
                    {
                        using (StreamReader sr = new StreamReader(item))
                        {
                            rTb.Text = sr.ReadToEnd();
                          
                        }
                       
                    }
                }
            };
            page.Controls.Add(rTb);
            tabControl.TabPages.Add(page);
            tabControl.SelectTab(page);


        }

        private void newPageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            NewPage();
        }

        private void closePageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (tabControl.SelectedIndex != -1)
            {
                if (sd.ShowDialog() == DialogResult.OK)
                {
                    string filename = sd.FileName;
                    var richTextBox = RichTB;
                    using (StreamWriter sw = new StreamWriter(sd.FileName))
                    {
                        sw.WriteLine(richTextBox.Text);
                    }
                    tabControl.TabPages.RemoveAt(tabControl.SelectedIndex);
                }
                else
                {
                    tabControl.TabPages.RemoveAt(tabControl.SelectedIndex);
                }
            }
        }

        void Open()
        {
            if (tabControl.TabPages.Count > 0)
            {
                var result = MessageBox.Show("Do you want to open in new page?", "Question", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    if (tabControl.SelectedIndex != -1)
                    {
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            RichTB.Text = File.ReadAllText(ofd.FileName);
                        }
                    }
                }
                else
                {
                    NewPage();
                    OpenFileDialog ofd = new OpenFileDialog();
                    if (tabControl.SelectedIndex != -1)
                    {
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            RichTB.Text = File.ReadAllText(ofd.FileName);
                        }
                    }
                }
            }
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open();
        }
       
        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (tabControl.SelectedIndex != -1)
            {
                if (sd.ShowDialog() == DialogResult.OK)
                {
                    string filename = sd.FileName;
                    tabControl.TabPages[tabControl.SelectedIndex].Text = filename;
                    var richTextBox = tabControl.TabPages[tabControl.SelectedIndex].Controls[0];
                    using (StreamWriter sw = new StreamWriter(sd.FileName))
                    {
                        sw.WriteLine(richTextBox.Text);
                    }
                }
            }
        }

        private void openBtn_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void copyBtn_Click(object sender, EventArgs e)
        {
                RichTB.Copy();
        }

        private void pasteBtn_Click(object sender, EventArgs e)
        {
            RichTB.Paste();
        }

        private void cutBtn_Click(object sender, EventArgs e)
        {
            RichTB.Cut();
        }

        private void forecolorBtn_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog()==DialogResult.OK)
            {
                RichTB.SelectionColor = cd.Color;
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTB.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTB.Paste();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTB.Cut();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTB.Clear();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTB.SelectAll();
        }

        private void backgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                RichTB.SelectionBackColor = cd.Color;
            }
        }

        private void foregroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                RichTB.SelectionColor = cd.Color;
            }
        }

        void Stat()
        {
            symbolsLabel.Text = RichTB.Text.Length.ToString();
            linesLabel.Text = RichTB.Lines.Length.ToString();
            int wordCount = 0, index = 0;
            while (index < RichTB.Text.Length && char.IsWhiteSpace(RichTB.Text[index]))
                index++;

            while (index < RichTB.Text.Length)
            {
               
                while (index < RichTB.Text.Length && !char.IsWhiteSpace(RichTB.Text[index]))
                    index++;

                wordCount++;

                
                while (index < RichTB.TextLength && char.IsWhiteSpace(RichTB.Text[index]))
                    index++;
            }
            wordsLabel.Text = wordCount.ToString();
        }
        private void richTextBox_TextChanged(object sender, EventArgs e)
        {
            Stat(); 
        }

        private void tabControl_Selected(object sender, TabControlEventArgs e)
        {
            Stat();
        }
    }
}

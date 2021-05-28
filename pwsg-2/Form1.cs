using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace pwsg_2
{
    public partial class Form1 : Form
    {
        List<Block> blocks = new List<Block>();
        Block currentBlock;
        Block selectedBlock;
        bool moving=false; int rel_x, rel_y;
        bool linking=false,link_mode=false; MyPoint link_point;
        bool trash_mode=false;
        Graphics flagGraphics;
        Pen linePen=new Pen(Color.Black,1);
        //private void ChangeLanguage(string lang)
        //{
        //    foreach (Control c in this.Controls)
        //    {
        //        ComponentResourceManager resources = new ComponentResourceManager(typeof(Form1));
        //        resources.ApplyResources(c, c.Name, new CultureInfo(lang));
        //    }
        //}
        public Form1()
        {
            InitializeComponent();
        }
        

        //void ReadFromFile()
        //{
        //    ifstream myfile("2048.txt", ios::in);
        //    if (!myfile) return false;
        //    string str;
        //    int i = 0;
        //    int max = 0;
        //    getline(myfile, str); gameState = stoi(str);
        //    getline(myfile, str); score.val = stoi(str);
        //    getline(myfile, str); score.goal = stoi(str);
        //    while (getline(myfile, str))
        //    {
        //        if (tiles[i].val > max) max = tiles[i].val;
        //        tiles[i].val = stoi(str);
        //        i++;
        //    }

        //}
        private void ChangeLanguage(string lang)
        //https://stackoverflow.com/questions/7556367/how-do-i-change-the-culture-of-a-winforms-application-at-runtime
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
            ComponentResourceManager resources = new ComponentResourceManager(typeof(Form1));
            resources.ApplyResources(this, "$this");
            applyResources(resources, this.Controls);
            if (selectedBlock != null)
                textBox1.Enabled = true;
            //pictureBox1.Refresh();
        }

        private void applyResources(ComponentResourceManager resources, Control.ControlCollection ctls)
        {
            foreach (Control ctl in ctls)
            {
                resources.ApplyResources(ctl, ctl.Name);
                applyResources(resources, ctl.Controls);
            }
        }

        private void PrintAll(bool refresh = true)
        {
            foreach(Block b in blocks)
            {
                //b.SetGraphics(pictureBox1,flagGraphics);
                b.Print(false);
            }
            if (refresh)
            {
                flagGraphics.DrawImage(pictureBox1.Image, 0, 0);
                pictureBox1.Refresh();
            }
        }
        private void ReprintPicture(bool refresh=true)
        {
            flagGraphics.FillRectangle(Brushes.White,0, 0, pictureBox1.Image.Width, pictureBox1.Image.Height);
            PrintAll(refresh);
        }


        private  Block NearestBlock(int x,int y)
        {
            if (blocks.Count == 0) return null;
            Block nearest = new OperationBlock();
            nearest.SetCoordinates(int.MaxValue, int.MaxValue);

            foreach (Block b in blocks)
                if (b.IsInside(x, y))
                    if ((x - nearest.x) * (x - nearest.x) + (y - nearest.y) * (y - nearest.y) > (x - b.x) * (x - b.x) + (y - b.y) * (y - b.y))
                        nearest = b;
            if (nearest.x == int.MaxValue)
                return null;
            return nearest;
        }

    
        public void CreateClearBitmap(int width, int height)
        {


            pictureBox1.Image = new Bitmap(width, height);
            Graphics flagGraphics = Graphics.FromImage(pictureBox1.Image);
            this.flagGraphics = flagGraphics;
            flagGraphics.FillRectangle(Brushes.White, 0, 0, width, height);
            flagGraphics.DrawImage(pictureBox1.Image, width, height);
            pictureBox1.Refresh();
            //foreach (Block b in blocks)
            //    b.SetGraphics(pictureBox1,this.flagGraphics);
            foreach (Block b in blocks)
                b.Clear();
            blocks.Clear();
            //if (currentBlock != null) currentBlock.SetGraphics(pictureBox1);
        }
        public void CreateBitmapAtRuntime()
        {
            ///https://docs.microsoft.com/pl-pl/dotnet/framework/winforms/advanced/how-to-create-a-bitmap-at-run-time
            pictureBox1.Size = new Size(210, 110);
            this.Controls.Add(pictureBox1);

            Bitmap flag = new Bitmap(200, 100);
            Graphics flagGraphics = Graphics.FromImage(flag);
            int red = 0;
            int white = 11;
            while (white <= 100)
            {
                flagGraphics.FillRectangle(Brushes.Red, 0, red, 200, 10);
                flagGraphics.FillRectangle(Brushes.White, 0, white, 200, 10);
                red += 20;
                white += 20;
            }
            pictureBox1.Image = flag;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            CreateClearBitmap(1000, 1000);
            currentBlock = new StartBlock();
            ChangeLanguage("pl-PL");

            //CreateBitmapAtRuntime();
        }

        //private void button7_Click(object sender, EventArgs e)
        //{
        //    chainButton.BackColor = SystemColors.Control;
        //    stopBlockButton.BackColor = Color.FromArgb(192, 192, 255);
        //    chainButton.FlatAppearance.MouseOverBackColor = new Color();
        //    stopBlockButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255 / 2, 192, 192, 255);
        //    currentBlock = new DecisionBlock();
        //}


        //private void button6_Click(object sender, EventArgs e)
        //{
        //    chainButton.BackColor = Color.FromArgb(192, 192, 255);
        //    stopBlockButton.BackColor = SystemColors.Control;
        //    stopBlockButton.FlatAppearance.MouseOverBackColor = new Color();
        //    chainButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255 / 2, 192, 192, 255);
        //    currentBlock = new OperationBlock();
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 dialog = new Form2();
            if (dialog.ShowDialog() == DialogResult.OK)
                CreateClearBitmap(dialog.Szerokosc, dialog.Wysokosc);
            dialog.Dispose();

        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!moving)
                if (e.Button == MouseButtons.Middle)
                { if(!linking)
                    if (selectedBlock != null)
                    {
                        moving = true;
                        rel_x = selectedBlock.x - e.X;
                        rel_y = selectedBlock.y - e.Y;
                    }
                }
                else if (e.Button == MouseButtons.Left)
                {
                    if(trash_mode)
                    {
                        Block blok = NearestBlock(e.X, e.Y);
                        if (blok != null)
                        {
                            blok.Clear();
                            blocks.Remove(blok);
                            ReprintPicture();
                            if (blok == selectedBlock)
                                selectedBlock = null;
                        }
                    }
                    else if (link_mode)
                    {
                        foreach (Block b in blocks)
                            foreach (MyPoint p in b.outPoints)
                                if(p.other==null)
                                    if (p.IsInside(e.X, e.Y))
                                    {
                                        linking = true;
                                        link_point = p;
                                    }

                    }
                    else
                    {
                        currentBlock.SetGraphics(pictureBox1, flagGraphics);
                        Block blok = currentBlock.CloneNew();
                        if (blok != null)
                        {
                            blok.SetCoordinates(e.X, e.Y);
                            blok.Print();
                            blocks.Add(blok);
                        }
                    }

                    //System.Resources.ResourceManager rm = Properties.Resources.ResourceManager;
                    //rm.GetObject("rhombus");
                }
                else if (e.Button == MouseButtons.Right)
                {
                    if (selectedBlock != null)
                    {
                        selectedBlock.drawPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                        textBox1.Enabled = false;
                        textBox1.Text = string.Empty;
                    }
                    selectedBlock = NearestBlock(e.X, e.Y);
                    if (selectedBlock != null)
                    {
                        selectedBlock.drawPen.DashPattern = new float[] { 2.0F, 2.0F };
                        if (selectedBlock.editable)                         
                            textBox1.Enabled = true;
                        textBox1.Text = selectedBlock.text;
                    }
                    ReprintPicture();
                }
        }

        private void PLButton_Click(object sender, EventArgs e)
        {
            panel1.VerticalScroll.Value = 0;
            panel1.HorizontalScroll.Value = 0;
            ChangeLanguage("pl-PL");        
        }

        private void ENButton_Click(object sender, EventArgs e)
        {
            panel1.VerticalScroll.Value = 0;
            panel1.HorizontalScroll.Value = 0;
            ChangeLanguage("en-GB");
        }

       
        private void decisionBlockButton_Click(object sender, EventArgs e)
        {
            Color clrcolor = new Color();
            Button butt;            
            for(int i=0;i<3;i++)
                for(int j=0;j<2;j++)
                {
                    butt= (Button)tableLayoutPanel6.GetControlFromPosition(i, j);
                    butt.FlatAppearance.MouseOverBackColor = clrcolor; //Color.FromArgb(255 / 2, 192, 192, 255);
                    butt.BackColor = SystemColors.Control;
                }
            link_mode = false;
            trash_mode = false;
            decisionBlockButton.BackColor = Color.FromArgb(192, 192, 255);
            decisionBlockButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255 / 3, 192, 192, 255);
            currentBlock = new DecisionBlock();
            //currentButton = decisionBlockButton;
        }


        private void operationBlockButton_Click(object sender, EventArgs e)
        {
            Color clrcolor = new Color();
            Button butt;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 2; j++)
                {
                    butt = (Button)tableLayoutPanel6.GetControlFromPosition(i, j);
                    butt.FlatAppearance.MouseOverBackColor = clrcolor;
                    butt.BackColor = SystemColors.Control;
                }
            link_mode = false;
            trash_mode = false;
            operationBlockButton.BackColor = Color.FromArgb(192, 192, 255);
            operationBlockButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255 / 3, 192, 192, 255);
            currentBlock = new OperationBlock();
        }

        private void startBlockButton_Click(object sender, EventArgs e)
        {
            Color clrcolor = new Color();
            Button butt;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 2; j++)
                {
                    butt = (Button)tableLayoutPanel6.GetControlFromPosition(i, j);
                    butt.FlatAppearance.MouseOverBackColor = clrcolor;
                    butt.BackColor = SystemColors.Control;
                }
            link_mode = false;
            trash_mode = false;
            startBlockButton.BackColor = Color.FromArgb(192, 192, 255);
            startBlockButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255 / 3, 192, 192, 255);
            currentBlock = new StartBlock();

        }

        private void chainButton_Click(object sender, EventArgs e)
        {
            Color clrcolor = new Color();
            Button butt;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 2; j++)
                {
                    butt = (Button)tableLayoutPanel6.GetControlFromPosition(i, j);
                    butt.FlatAppearance.MouseOverBackColor = clrcolor;
                    butt.BackColor = SystemColors.Control;
                }
            trash_mode = false;
            chainButton.BackColor = Color.FromArgb(192, 192, 255);
            chainButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255 / 3, 192, 192, 255);
            link_mode = true;
        }

        private void trashButton_Click(object sender, EventArgs e)
        {
            Color clrcolor = new Color();
            Button butt;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 2; j++)
                {
                    butt = (Button)tableLayoutPanel6.GetControlFromPosition(i, j);
                    butt.FlatAppearance.MouseOverBackColor = clrcolor;
                    butt.BackColor = SystemColors.Control;
                }
            link_mode = false;
            trashButton.BackColor = Color.FromArgb(192, 192, 255);
            trashButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255 / 3, 192, 192, 255);
            trash_mode = true;
            //PrintAll();
        }

        private void stopBlockButton_Click(object sender, EventArgs e)
        {
            Color clrcolor = new Color();
            Button butt;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 2; j++)
                {
                    butt = (Button)tableLayoutPanel6.GetControlFromPosition(i, j);
                    butt.FlatAppearance.MouseOverBackColor = clrcolor;
                    butt.BackColor = SystemColors.Control;
                }
            link_mode = false;
            trash_mode = false;
            stopBlockButton.BackColor = Color.FromArgb(192, 192, 255);
            stopBlockButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(255 / 3, 192, 192, 255);
            currentBlock = new StopBlock();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Enabled)
            {
                selectedBlock.text = textBox1.Text;
                ReprintPicture();
            }
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Diagram files (*.diag)|*.diag";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                if (!fileDialog.ValidateNames) new Form3().ShowDialog();
                Stream file = fileDialog.OpenFile();

                //Stream stream = File.Open("EmployeeInfo.osl", FileMode.Create);
                BinaryFormatter bformatter = new BinaryFormatter();

                bformatter.Serialize(file, blocks);
                file.Close();
                //pictureBox2.Image = new Bitmap(fileDialog.FileName);
            }
            
            //    void SaveToFile()
            //    {
            //        FileStream file;
            //        ofstream myfile("2048.txt", ios::trunc);
            //    myfile << gameState << endl;
            //    myfile << score.val << endl;
            //    myfile << score.goal << endl;
            //    for (int i = 0; i < 16; i++)
            //    {
            //        myfile << tiles[i].val << endl;
            //    }
            //    myfile.close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Diagram files (*.diag)|*.diag";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                 blocks = null;

                //Open the file written above and read values from it.
                Stream file = File.Open(fileDialog.FileName, FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();

                blocks = (List<Block>)bformatter.Deserialize(file);
                file.Close();
                foreach (Block b in blocks)
                    b.SetGraphics(pictureBox1, flagGraphics);
                ReprintPicture();

            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                if (moving)
                {
                    moving = false;
                    if (selectedBlock.x < 0)
                        selectedBlock.SetCoordinates(0, selectedBlock.y);
                    if (selectedBlock.y < 0)
                        selectedBlock.SetCoordinates(selectedBlock.x, 0);
                    if (selectedBlock.x > pictureBox1.Image.Width)
                        selectedBlock.SetCoordinates(pictureBox1.Image.Width, selectedBlock.y);
                    if (selectedBlock.y > pictureBox1.Image.Height)
                        selectedBlock.SetCoordinates(selectedBlock.x, pictureBox1.Image.Height);
                    ReprintPicture();
                }

            }
            if(e.Button==MouseButtons.Left)
            {
                if (linking)
                {
                    linking = false;
                    foreach (Block b in blocks)
                        if(b!=link_point.parent)
                            if (b.inPoint != null)
                                if(b.inPoint.other==null)
                                    if (b.inPoint.IsInside(e.X, e.Y))
                                    {
                                        link_point.other = b.inPoint;
                                        b.inPoint.other = link_point;
                                        break;
                                    }

                    ReprintPicture();
                }
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if(moving)
            {
                selectedBlock.SetCoordinates(e.X + rel_x, e.Y + rel_y);
                ReprintPicture();
            }
            if (linking)
            {
                ReprintPicture(false);
                flagGraphics.DrawLine(linePen, link_point.GetPoint(), e.Location);
                flagGraphics.DrawImage(pictureBox1.Image, 0, 0);
                pictureBox1.Refresh();
            }


        }
    }
}

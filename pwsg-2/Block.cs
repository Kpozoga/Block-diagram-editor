using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Resources;
using System.Runtime.Serialization;

namespace pwsg_2
{
    [Serializable]
    abstract class Block : ISerializable
    {
        //protected (int x, int y, bool linked) inPoint;
        //protected List<(int x, int y, Block next)> outPoints;
        public MyPoint inPoint;
        public List<MyPoint> outPoints;
        public string text;
        protected Font drawFont;
        protected SolidBrush drawBrush;
        protected StringFormat drawFormat;
        public bool editable { get; protected set; }
        public Graphics flagGraphics { get; protected set; }
        public Pen drawPen;     
        public PictureBox picture { get; protected set; }
        public int x { get; protected set; }
        public int y {  get; protected set; }
        public Block()
        {
            drawFont = new Font("Microsoft Sans Serif", 8);
            drawBrush = new SolidBrush(Color.Black);
            drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;
            drawFormat.LineAlignment = StringAlignment.Center;
            drawPen = new Pen(Color.Black, 2);
        }
        public Block(PictureBox p, Graphics g) 
        {
            drawFont = new Font("Microsoft Sans Serif", 8);
            drawBrush = new SolidBrush(Color.Black);
            drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;
            drawFormat.LineAlignment = StringAlignment.Center;
            drawPen = new Pen(Color.Black, 2);
            flagGraphics = g;
            picture = p;
            
            editable = true;
        }
        public void SetGraphics(PictureBox p,Graphics g) { picture = p; flagGraphics = g; }
        public void SetCoordinates(int x, int y) { this.x = x; this.y = y; }
        protected void PrintPoints()
        {
            if (inPoint != null) inPoint.Print();
            foreach (MyPoint p in outPoints)
                p.Print();
        }
        public virtual void Clear()
        {
            if (inPoint != null)  {
                inPoint.Clear();
                inPoint = null;
            }
            foreach (MyPoint p in outPoints)
                p.Clear();
            outPoints.Clear();
            outPoints = null;
            if(drawFont!=null) drawFont.Dispose();
            if (drawBrush != null) drawBrush.Dispose();
            if (drawFormat != null) drawFormat.Dispose();
            if (drawPen != null) drawPen.Dispose();
            flagGraphics = null;
            picture = null;


        }
        public abstract void Print(bool refresh=true);
        public abstract void BaseText();
        public abstract Block CloneNew();
        public abstract bool IsInside(int x, int y);

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            
            info.AddValue("x", x);
            info.AddValue("y", y);
            info.AddValue("text", text);
            info.AddValue("inPoint", inPoint);
            info.AddValue("outPoints", outPoints);
            info.AddValue("editable", editable);       
            //info.AddValue("flagGraphics", flagGraphics);
            //info.AddValue("picture", picture);

    }
        public Block(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            x = (int)info.GetValue("x", typeof(int));
            y = (int)info.GetValue("y", typeof(int));
            text = (string)info.GetValue("text", typeof(string));
            editable = (bool)info.GetValue("editable", typeof(bool));        
            inPoint = (MyPoint)info.GetValue("inPoint", typeof(MyPoint));
            outPoints = (List<MyPoint>)info.GetValue("outPoints", typeof(List<MyPoint>));
            //flagGraphics = (Graphics)info.GetValue("flagGraphics", typeof(Graphics));
            //picture = (PictureBox)info.GetValue("picture", typeof(PictureBox));
        }
    }
    [Serializable]
    class OperationBlock:Block, ISerializable
    {
        public OperationBlock():base() { }
        public OperationBlock(PictureBox p,Graphics g) : base(p,g)
        {
            inPoint = new MyPoint(this, 0, -25);
            outPoints = new List<MyPoint>();
            outPoints.Add( new MyPoint(this, 0, 25, true));
        }
        public override void Print(bool refresh=true)
        {
            //Graphics flagGraphics = Graphics.FromImage(p.Image);
            //drawPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;       
            //drawPen.DashPattern = new float[] { 2.0F, 2.0F };
            //drawPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            Rectangle displayRectangle = new Rectangle(new Point(x - 50, y - 25), new Size(100, 50));
            flagGraphics.FillRectangle(Brushes.White, displayRectangle);
            flagGraphics.DrawRectangle(drawPen, displayRectangle);
            flagGraphics.DrawString(text, drawFont, drawBrush, displayRectangle, drawFormat);
            PrintPoints();

            
            if (refresh)
            {
                flagGraphics.DrawImage(picture.Image, 0, 0);
                picture.Refresh();
            }
        }
        public override void BaseText()
        {
            var resources = new ResourceManager(typeof(Form1));
            text = resources.GetString("operationBlockText");
        }
        public override Block CloneNew()
        {
            OperationBlock other = new OperationBlock(this.picture,this.flagGraphics);
            other.BaseText();
            return other;
        }
        public override bool IsInside(int x, int y)
        {
            if (x >= this.x + 50) return false;
            if (x <= this.x - 50) return false;
            if (y >= this.y + 25) return false;
            if (y <= this.y - 25) return false;
            return true;
        }
        public OperationBlock(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            x = (int)info.GetValue("x", typeof(int));
            y = (int)info.GetValue("y", typeof(int));
            text = (string)info.GetValue("text", typeof(string));
            editable = (bool)info.GetValue("editable", typeof(bool));
            inPoint = (MyPoint)info.GetValue("inPoint", typeof(MyPoint));
            outPoints = (List<MyPoint>)info.GetValue("outPoints", typeof(List<MyPoint>));

            //flagGraphics = (Graphics)info.GetValue("flagGraphics", typeof(Graphics));
            //picture = (PictureBox)info.GetValue("picture", typeof(PictureBox));
        }


    }
    [Serializable]
    class DecisionBlock : Block, ISerializable
    {
        public DecisionBlock() { }
        public DecisionBlock(PictureBox p,Graphics g) : base(p,g)
        {
            inPoint = new MyPoint(this, 0, -37);
            outPoints = new List<MyPoint>();
            outPoints.Add(new MyPoint(this, -50, 0, true));
            outPoints.Add(new MyPoint(this, 50, 0, true));
        }
        public override void BaseText()
        {
            var resources = new ResourceManager(typeof(Form1));
            text = resources.GetString("decisionBlockText");
        }
        public override void Print(bool refresh = true)
        {
            Point []rhomb= new Point[4] { new Point(x - 50, y), new Point(x, y - 37), new Point(x + 50, y), new Point(x, y + 37) };
            Rectangle displayRectangle = new Rectangle(new Point(x - 24, y - 18), new Size(48,36));
            flagGraphics.FillPolygon(Brushes.White, rhomb);
            flagGraphics.DrawPolygon(drawPen,rhomb);
            //flagGraphics.DrawEllipse(drawPen, new Rectangle(x - 50, y - 37, 100, 74));
            flagGraphics.DrawString(text, drawFont, drawBrush, displayRectangle, drawFormat);
            flagGraphics.DrawString("T", drawFont, drawBrush, x-50,y-15, drawFormat);
            flagGraphics.DrawString("F", drawFont, drawBrush, x + 50, y - 15, drawFormat);
            PrintPoints();

            if (refresh)
            {
                flagGraphics.DrawImage(picture.Image, 0, 0);
                picture.Refresh();
            }
        }
        public override Block CloneNew()
        {         
            DecisionBlock other = new DecisionBlock(this.picture,this.flagGraphics);
            other.BaseText();
            return other;
        }
        public override bool IsInside(int x, int y)
        {
            if (((x - this.x) *(x - this.x) / (50.0 * 50.0) + (y - this.y) * (y - this.y) / (37.0 * 37.0))<1)
                return true;
            else return false;
        }
        public DecisionBlock(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            x = (int)info.GetValue("x", typeof(int));
            y = (int)info.GetValue("y", typeof(int));
            text = (string)info.GetValue("text", typeof(string));
            editable = (bool)info.GetValue("editable", typeof(bool));
            inPoint = (MyPoint)info.GetValue("inPoint", typeof(MyPoint));
            outPoints = (List<MyPoint>)info.GetValue("outPoints", typeof(List<MyPoint>));

            //flagGraphics = (Graphics)info.GetValue("flagGraphics", typeof(Graphics));
            //picture = (PictureBox)info.GetValue("picture", typeof(PictureBox));
        }
    }
    [Serializable]
    class StartBlock : Block, ISerializable
    {
        static bool exists=false;
        public StartBlock():base()
        {
            drawPen = new Pen(Color.LimeGreen, 2);
            editable = false;
        }
        public StartBlock(PictureBox p, Graphics g) : base(p, g)
        {
            outPoints = new List<MyPoint>();
            outPoints.Add(new MyPoint(this, 0, 25, true));
            drawPen = new Pen(Color.LimeGreen, 2);
            editable = false;
        }
        public override void BaseText()
        {

            text = "START";
        }
        public override void Print(bool refresh = true)
        {
            Rectangle displayRectangle = new Rectangle(new Point(x - 24, y - 18), new Size(48, 36));
            flagGraphics.FillEllipse(Brushes.White, new Rectangle(x - 40, y - 25, 80, 50));
            flagGraphics.DrawEllipse(drawPen, new Rectangle(x - 40, y - 25, 80, 50));
            flagGraphics.DrawString(text, drawFont, drawBrush, displayRectangle, drawFormat);
            PrintPoints();

            if (refresh)
            {
                flagGraphics.DrawImage(picture.Image, 0, 0);
                picture.Refresh();
            }
        }
        public override Block CloneNew()
        {
            if(exists)
            {
                Form F = new Form3();
                F.ShowDialog();
                F.Dispose();
                return null;

            }
            StartBlock other = new StartBlock(this.picture, this.flagGraphics);
            other.BaseText();
            exists = true;
            return other;
        }
        public override bool IsInside(int x, int y)
        {
            if (((x - this.x) * (x - this.x) / (50.0 * 50.0) + (y - this.y) * (y - this.y) / (37.0 * 37.0)) < 1)
                return true;
            else return false;
        }
        public override void Clear()
        {
            base.Clear();
            exists = false;
        }
        public new void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {

            info.AddValue("x", x);
            info.AddValue("y", y);
            info.AddValue("text", text);
            info.AddValue("inPoint", inPoint);
            info.AddValue("outPoints", outPoints);
            info.AddValue("editable", editable);
            info.AddValue("exists", exists);
            //info.AddValue("flagGraphics", flagGraphics);
            //info.AddValue("picture", picture);

        }
        public StartBlock(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            x = (int)info.GetValue("x", typeof(int));
            y = (int)info.GetValue("y", typeof(int));
            text = (string)info.GetValue("text", typeof(string));
            editable = (bool)info.GetValue("editable", typeof(bool));
            inPoint = (MyPoint)info.GetValue("inPoint", typeof(MyPoint));
            outPoints = (List<MyPoint>)info.GetValue("outPoints", typeof(List<MyPoint>));
            exists = (bool)info.GetValue("exists", typeof(bool));

            //flagGraphics = (Graphics)info.GetValue("flagGraphics", typeof(Graphics));
            //picture = (PictureBox)info.GetValue("picture", typeof(PictureBox));
        }
    }



    [Serializable]
    class StopBlock : Block, ISerializable
    {
            public StopBlock() { }
            public StopBlock(PictureBox p, Graphics g) : base(p, g)
            {
                outPoints = new List<MyPoint>();
                inPoint=new MyPoint(this, 0, -25);
                drawPen = new Pen(Color.Red, 2);
                editable = false;
            }
            public override void BaseText()
            {

                text = "STOP";
            }
            public override void Print(bool refresh = true)
            {
                Rectangle displayRectangle = new Rectangle(new Point(x - 24, y - 18), new Size(48, 36));
                flagGraphics.FillEllipse(Brushes.White, new Rectangle(x - 40, y - 25, 80, 50));
                flagGraphics.DrawEllipse(drawPen, new Rectangle(x - 40, y - 25, 80, 50));
                flagGraphics.DrawString(text, drawFont, drawBrush, displayRectangle, drawFormat);
                PrintPoints();

                if (refresh)
                {
                    flagGraphics.DrawImage(picture.Image, 0, 0);
                    picture.Refresh();
                }
            }
            public override Block CloneNew()
            {
                StopBlock other = new StopBlock(this.picture, this.flagGraphics);
                other.BaseText();
                return other;
            }
            public override bool IsInside(int x, int y)
            {
                if (((x - this.x) * (x - this.x) / (50.0 * 50.0) + (y - this.y) * (y - this.y) / (37.0 * 37.0)) < 1)
                    return true;
                else return false;
            }
        public StopBlock(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            x = (int)info.GetValue("x", typeof(int));
            y = (int)info.GetValue("y", typeof(int));
            text = (string)info.GetValue("text", typeof(string));
            editable = (bool)info.GetValue("editable", typeof(bool));
            inPoint = (MyPoint)info.GetValue("inPoint", typeof(MyPoint));
            outPoints = (List<MyPoint>)info.GetValue("outPoints", typeof(List<MyPoint>));         

            //flagGraphics = (Graphics)info.GetValue("flagGraphics", typeof(Graphics));
            //picture = (PictureBox)info.GetValue("picture", typeof(PictureBox));
        }
    }


   
}

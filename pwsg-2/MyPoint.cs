using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace pwsg_2
{
    [Serializable]
    class MyPoint : ISerializable
    {
        int local_x, local_y;
        public MyPoint other;
        bool outPoint;
        public Block parent;
        Pen drawPen;
        Pen linePen;
        public MyPoint()
        {
            drawPen = new Pen(Color.Black, 2);
            linePen = new Pen(Color.Black, 1);
            linePen.CustomEndCap = new AdjustableArrowCap(6, 6);
        }
        public MyPoint(Block p, int x, int y,bool isOut= false): base()
        {
            drawPen = new Pen(Color.Black, 2);
            linePen = new Pen(Color.Black, 1);
            linePen.CustomEndCap = new AdjustableArrowCap(6, 6);
            outPoint = isOut;
            parent = p;
            local_x = x;
            local_y = y;
            
        }
        public void Print()
        {
            if (other == null)
            {
                if(outPoint)
                    parent.flagGraphics.FillEllipse(Brushes.Black,parent.x+local_x - 3,parent.y +local_y - 3, 6, 6);
                else parent.flagGraphics.FillEllipse(Brushes.White, parent.x + local_x - 3, parent.y + local_y - 3, 6, 6);
                parent.flagGraphics.DrawEllipse(drawPen, parent.x + local_x - 3, parent.y + local_y - 3, 6, 6);
                //parent.flagGraphics.DrawImage(parent.picture.Image, 0, 0);
                //parent.picture.Refresh();
            }
            else if(outPoint)
            {
                parent.flagGraphics.DrawLine(linePen, GetPoint(), other.GetPoint());
            }
        }
        public bool IsInside(int x, int y)
        {
            if (x > parent.x+local_x+4) return false;
            if (x < parent.x + local_x-4) return false;
            if (y > parent.y + local_y+4) return false;
            if (y < parent.y + local_y-4) return false;
            return true;
        }
        public Point GetPoint()
        {
            return new Point(parent.x + local_x, parent.y + local_y);
        }
        public void Clear()
        {
            if (other != null)
                other.other = null;
            other = null;
            parent = null;
            drawPen.Dispose();
            linePen.Dispose();
        }
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {

            info.AddValue("x", local_x);
            info.AddValue("y", local_y);
            info.AddValue("other", other);
            info.AddValue("outPoint", outPoint);
            info.AddValue("parent", parent);
    }
        public MyPoint(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            local_x= (int)info.GetValue("x", typeof(int));
            local_y = (int)info.GetValue("y", typeof(int));
            other = (MyPoint)info.GetValue("other", typeof(MyPoint));
            outPoint= (bool)info.GetValue("outPoint", typeof(bool));
            parent= (Block)info.GetValue("parent", typeof(Block));
        }
    }
    
}

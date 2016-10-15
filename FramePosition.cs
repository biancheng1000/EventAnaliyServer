using RigourTech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RigourTech
{
    /// <summary>
    /// 具有坐标的帧
    /// </summary>
    public class FramePosition
    {
        public FramePosition()
        {

        }

        public FramePosition(Point3D p,string f)
        {
            position = p;
            frameNumber = f;
        }


        string frameNumber;
        Point3D position;
        double radius;

        public string FrameNumber
        {
            get
            {
                return frameNumber;
            }

            set
            {
                frameNumber = value;
            }
        }

        public Point3D Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public double Radius
        {
            get
            {
                return radius;
            }

            set
            {
                radius = value;
            }
        }

        public static FramePosition Parse(string strvalue)
        {
            if (string.IsNullOrEmpty(strvalue))
            {
                return null;
            }

            int s = strvalue.IndexOf(",");

            if (s > 0)
            {
                string frame = strvalue.Substring(0, strvalue.IndexOf(","));
                Point3D p = Point3D.Prase(strvalue.Substring(strvalue.IndexOf(",") + 1));
                return new FramePosition(p, frame);
            }
            return null;
        } 
    }
}

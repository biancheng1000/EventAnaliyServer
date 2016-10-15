using RigourTech.Tennisball.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RigourTech
{
    /// <summary>
    /// 轨迹对象
    /// </summary>
    public class Track
    {
        #region 属性
        IList<FramePosition> trace = new List<FramePosition>();
        double radius;
        double beginTime;
        double endtime;
        Point3D touchdown_p1;
        Point3D touchdown_p2;
        string touchdonwMarkNumber;

        FramePosition firstPosition;
        FramePosition endPosition;
        double maxSpeed;
        double maxRotateSpeed;


        /// <summary>
        /// 轨迹
        /// </summary>
        public IList<FramePosition> Trace
        {
            get
            {
                return trace;
            }

            set
            {
                trace = value;
            }
        }

        /// <summary>
        /// 距离最近边半径
        /// </summary>
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

        /// <summary>
        /// 估计开始时间
        /// </summary>
        public double BeginTime
        {
            get
            {
                return beginTime;
            }

            set
            {
                beginTime = value;
            }
        }

        /// <summary>
        /// 轨迹结束时间
        /// </summary>
        public double Endtime
        {
            get
            {
                return endtime;
            }

            set
            {
                endtime = value;
            }
        }

        /// <summary>
        /// 落地点
        /// </summary>
        public Point3D Touchdown_P1
        {
            get
            {
                return touchdown_p1;
            }

            set
            {
                touchdown_p1 = value;
            }
        }
        /// <summary>
        /// 弹起点
        /// </summary>
        public Point3D Touchdown_P2
        {
            get
            {
                return touchdown_p2;
            }

            set
            {
                touchdown_p2 = value;
            }
        }
        /// <summary>
        /// 落地场地号
        /// </summary>
        public string TouchdonwMarkNumber
        {
            get
            {
                return touchdonwMarkNumber;
            }

            set
            {
                touchdonwMarkNumber = value;
            }
        }

        /// <summary>
        /// 开始点
        /// </summary>
        public FramePosition FirstPosition
        {
            get
            {
                return firstPosition;
            }

            set
            {
                firstPosition = value;
            }
        }

        /// <summary>
        /// 结束点
        /// </summary>
        public FramePosition EndPosition
        {
            get
            {
                return endPosition;
            }

            set
            {
                endPosition = value;
            }
        }

        /// <summary>
        /// 轨迹的最大速度
        /// </summary>
        public double MaxSpeed
        {
            get
            {
                if (maxSpeed == 0)
                {
                    maxSpeed = GetHightSpeedFromTrace();
                }
                return maxSpeed;
            }

        }

        /// <summary>
        /// 最大旋转速度
        /// </summary>
        public double MaxRotateSpeed
        {
            get
            {
                return maxRotateSpeed;
            }

            set
            {
                maxRotateSpeed = value;
            }
        }
        #endregion

        /// <summary>
        /// 从字符串中解析出对象
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static Track Parse(string strValue)
        {
            Track t = new RigourTech.Track();

            if (string.IsNullOrEmpty(strValue))
            {
                return null;
            }

            string[] items= strValue.Split('|');
            if (items.Length != 6)
            {
                LogText.Error("Track.pase", "数据格式错误：" + strValue);
            }


            MatchCollection results = Regex.Matches(items[1], "[0-9]*,-?[0-9]*\\.[0-9]*,-?[0-9]*\\.[0-9]*,-?[0-9]*\\.[0-9]*");
            foreach (Match m in results)
            {
                t.Trace.Add(FramePosition.Parse(m.Value.Substring(0, m.Value.LastIndexOf(","))));
            }
            MatchCollection mtouchdownpoints = Regex.Matches(items[1], "-?[0-9]*\\.[0-9]*,-?[0-9]*\\.[0-9]*,-?[0-9]*\\.[0-9]*");
            if (mtouchdownpoints.Count == 2)
            {
                t.Touchdown_P1 = Point3D.Prase(mtouchdownpoints[0].Value);
                t.Touchdown_P2 = Point3D.Prase(mtouchdownpoints[1].Value);
            }
            else
            {
                LogText.Error("Track.pase", "数据格式错误：" + strValue);
            }
            MatchCollection mtimes = Regex.Matches(items[2], "[0-9]*");
            if (mtimes.Count == 2)
            {
                t.BeginTime = long.Parse(mtimes[0].Value);
                t.Endtime = long.Parse(mtimes[1].Value);
            }
            else
            {
                LogText.Error("Track.pase", "数据格式错误：" + strValue);
            }

            t.TouchdonwMarkNumber = Regex.Match(items[3], "[0-9]?").Value;

            t.Radius = double.Parse(Regex.Match(items[3], "[0-9]*\\.[0-9]*").Value);

            t.MaxRotateSpeed = double.Parse(items[4]);
            return t;
        }

        private double GetHightSpeedFromTrace()
        {
            int index = 0;
            double dis = 0;
            Point3D p1 = null;
            Point3D p2 = null;
            long f1 = 0;
            long f2 = 0;
            while (index * 5 < Trace.Count)
            {
                p1 = p2;
                f1 = f2;

                p2 = Trace[index].Position;
                f2 =long.Parse(Trace[index].FrameNumber);

                if (p1 != null)
                {
                    dis = Math.Max(dis, getDistancebettwenTowPoint(p1, p2));
                }

                index++;
            }

            //计算最终速度
            return (dis * 18 * 6 * 6) / (1000.0 * 5.0);
        }

        /// <summary>
        /// 两点间距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private double getDistancebettwenTowPoint(Point3D p1, Point3D p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y) + (p1.Z - p2.Z) * (p1.Z - p2.Z));
        }

    }
}

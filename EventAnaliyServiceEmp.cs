using RigourTech;
using RigourTech.Tennisball.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TennisDataAnaliy;

namespace DataProviderService
{
    public class EventAnaliyServiceEmp: TennisDataAnaliy.TennisDataAnaliy.Iface
    {

        #region 属性

        IList<UserMgr> users = new List<UserMgr>();
        /// <summary>
        /// 当前登录的用户信息
        /// </summary>
        public IList<UserMgr> Users
        {
            get
            {
                return users;
            }

            set
            {
                users = value;
            }
        }

        /// <summary>
        /// 运行模式
        /// </summary>
        public RunningMode Model
        {
            get
            {
                return model;
            }

            set
            {
                model = value;
            }
        }

        RunningMode model;
        #endregion

        #region Iface


        /// <summary>
        /// 接收来自底层的数据
        /// </summary>
        /// <param name="path"></param>
        public void InsertPathToDB(string path)
        {
            if (path.StartsWith("0|"))
            {
                MatchPerson(path);
            }
            else if (path.StartsWith("1|"))
            {
                Track t = Track.Parse(path);

            }
            else
            {
                matchSpeed(path);
                Console.WriteLine("速度");
            }
        }

        /// <summary>
        /// 接收来自界面传递来的用户信息
        /// </summary>
        /// <param name="useinfo"></param>
        public void UpdatePlayers(List<userInfo> _users)
        {
            Array.ForEach(_users.ToArray(), (n) => 
            {
                bool find=false;
                foreach (UserMgr m in Users)
                {
                    if (m.User.UserID == n.UserID)
                    {
                        find = true;
                        m.User = n;
                        break;
                    }
                }
                if (!find)
                {
                    Users.Add(new UserMgr(n));
                }
            });
        }

        /// <summary>
        /// 接收来自客户端当前的模式
        /// 比赛 训练（需要目标区域号码）
        /// </summary>
        /// <param name="model"></param>
        public void SetRunningModel(RunningMode model)
        {
            Console.WriteLine("收到工作模式为：{0}，参数为：{1}的模式设置",model.Model,model.TargetMarkNumber);
        }
        #endregion

        #region 接收到底层坐标信息进行事件分析
        /// <summary>
        /// 匹配人员坐标
        /// </summary>
        /// <param name="SubjectString"></param>
        private void MatchPerson(string SubjectString)
        {
            try
            {
                string strPosition = Regex.Match(SubjectString, "\\|[0-9]*\\|-?[0-9]*\\.[0-9]*,-?[0-9]*\\.[0-9]*,-?[0-9]*\\.[0-9]*").Value;
                if (!string.IsNullOrEmpty(strPosition))
                {
                    FramePosition position = FramePosition.Parse(strPosition);
                    if (position != null)
                    {
                        AddPosition(position);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                LogText.Error("MatchPerson",ex.Message);
            }
        }

        private void AddPosition(FramePosition fr)
        {
            Parallel.ForEach(Users, (n) => { n.AddPosition(fr); });
        }

        private void AddTrack(Track t)
        {
            Parallel.ForEach(Users, (n) => { n.AddTracks(t); });
        }
       

        /// <summary>
        /// 匹配数据
        /// </summary>
        /// <param name="SubjectString"></param>
        private void matchSpeed(string SubjectString)
        {
            string ResultString = null;
            try
            {
                ResultString = Regex.Match(SubjectString, "[0-9]*\\.[0-9]*").Value;
                Console.WriteLine(ResultString);
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
        }

        /// <summary>
        /// 匹配轨迹数据
        /// </summary>
        /// <param name="subjectstring"></param>
        private void MatchTracks(string subjectstring)
        {
            string ResultString = null;
            try
            {
                MatchCollection results = Regex.Matches(subjectstring, "[0-9]*,[0-9]*\\.[0-9]*,[0-9]*\\.[0-9]*,[0-9]*\\.[0-9]*,[0-9]*\\.[0-9]*(;¦|)");
            }
            catch (ArgumentException ex)
            {
                
            }
        }



        #endregion

    }
}

using System;

namespace DispatchApp
{
    public class switchInfo
    {
        private string _callno;
        private string _id;
        private int _grade;

        public string callno
        {
            get { return _callno; }
            set { _callno = value; }
        }

        public string id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int grade
        {
            get { return _grade; }
            set { _grade = value; }
        }

        public switchInfo()
        {
        }

        public switchInfo(string callno, string id, int grade)
        {
            _callno = callno;
            _id = id;
            _grade = grade;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using static System.Console;

namespace TodoAppClassLib
{
    public class Todo : DbEntity
    {
        private string _title;
        public string Title {
            get {
                return this._title;
            } set {
                this._title = value;
            }
        
        }
        private Status _status;
        public Status Status {
            get {
                return this._status;
            } 
            set {
                this._status = value;
                this.ChangeState();
            }
        }
        private Priority _priority;
        public Priority Priority { 
            get {
                return this._priority;
            } 
            set {
                this._priority = value;
                this.ChangeState();
            }
        }
        private DateTime _deadlineDate;
        public DateTime DeadlineDate { 
            get {
                return this._deadlineDate;
            } 
            set {
                this._deadlineDate = value;
                this.ChangeState();
            }
        }
        private DateTime _deadlineTime;
        public DateTime DeadlineTime { 
            get {
                return this._deadlineTime;
            } 
            set {
                this._deadlineTime = value;
                this.ChangeState();
            }
        }
        private string prevTitle;
        private string prevPriority;
        private DateTime prevDeadlineDate;
        private DateTime prevDeadlineTime;
        public Todo(DataAccess dt, string title, Priority priority, DateTime deadlineDate, DateTime deadlineTime, Status status):base(dt)
        {
            this._title = title;
            this._status = status;
            this._priority = priority;
            this._deadlineDate = deadlineDate;
            this._deadlineTime = deadlineTime;
        }

        public static List<Todo> Select(DataAccess dt)
        {
            var todosList = new List<Todo>();
            SqlDataReader reader = null;
            string query = "SELECT * from todos";
            try
            {
                reader = dt.Select(query);
                while(reader.Read())
                {
                    todosList.Add(new Todo(dt,reader.GetString(1),(Priority)reader.GetInt32(2),reader.GetDateTime(3),reader.GetDateTime(4),(Status)reader.GetInt32(5)){
                        State = ObjectState.Unchanged,
                        prevTitle = reader.GetString(1),
                        prevPriority = reader.GetString(2),
                        prevDeadlineDate = reader.GetDateTime(3),
                        prevDeadlineTime = reader.GetDateTime(4),
                    });
                }
            }
            finally
            {
                reader?.Close();
                
            }
            return todosList;
        }
        internal override void Insert()
        {
            Dictionary<string,Object> dict = new Dictionary<string,Object>();
            string query = $@"INSERT INTO dbo.todos(title,priority,deadline_date,deadline_time,status) VALUES(@title,@priority,@deadlineDate,@deadlineTime,@status)";
            dict.Add("@title",this._title);
            dict.Add("@priority",this._priority);
            dict.Add("@deadlineDate",this._deadlineDate);
            dict.Add("@deadlineTime",this._deadlineTime);
            dict.Add("@status",this._status);
            _dt.PerformOperation(query,dict);

        }
        internal override void Update()
        {
            Dictionary<string,Object> dict = new Dictionary<string,Object>();
            string updateTodo = @"UPDATE dbo.todos SET title=@newTitle, priority=@newPriority,deadline_date=@newDeadlineDate,deadline_time=@newDeadlineTime,status=@newStatus WHERE title=@title AND priority=@priority AND deadline_date=@deadlineDate AND deadline_time=@deadlineTime";
            dict.Add("@title",this.prevTitle);
            dict.Add("@newTitle",this._title);
            dict.Add("@priority",this.prevPriority);
            dict.Add("@newPriority",this._priority);
            dict.Add("@deadline_date",this.prevDeadlineDate);
            dict.Add("@deadline_time",this.prevDeadlineTime);

            _dt.PerformOperation(updateTodo,dict);
        }
        internal override void Delete()
        {
            
        }
        public override string ToString() {
            return $"{_title}\t{_deadlineDate}\t{_deadlineTime}";
        }
        
    }
}

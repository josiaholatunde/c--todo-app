using System;
using TodoAppClassLib;
using static System.Console;
using System.Collections.Generic;

namespace TodoAppConsole
{
    class Program
    {
        private DataAccess _dt;
        static void Main(string[] args)
        {
          List<Todo> todos = new List<Todo>();
          var program = new Program();
          program._dt = new DataAccess("Data Source=.\\SQLEXPRESS;Database=Todos;Integrated Security=True;");
          while(true)
          {
              todos = Todo.Select(program._dt);
              for(int i = 0; i < todos.Count; i++)
              {
                  WriteLine($"{i} {todos[i].ToString()}");
              }
              WriteLine("Select 1 to perform an insert operation or 2 to update or 3 to Exit");
            int userInput = Convert.ToInt32(ReadLine());
            if (userInput == 1)
            {
                var todo = new Todo(program._dt,"Study Python",Priority.Urgent,new DateTime(2019,10,29),new DateTime(2018,10,29,20,30,34),Status.NotDone);
                todo.Save();
            } else if (userInput == 2) {
                WriteLine("Enter the index of the todo you wish to update");
                userInput = Convert.ToInt32(ReadLine());
                Todo todo = todos[userInput - 1];
                WriteLine("Enter the property of the todo you wish to update");
                string userResponse = ReadLine();
                if (userResponse.ToLower() == "title")
                {
                    WriteLine("Enter the new value of the title");
                    string title = ReadLine();
                    todo.Title = title;
                } else if (userResponse.ToLower() == "priority")
                {
                    WriteLine("Enter 1 Urgent\n2 Important\n3 Normal\n4 NotUrgent");
                    int resp = Convert.ToInt32(ReadLine());
                    todo.Priority = (Priority) resp;
                } else if(userResponse.ToLower() == "status")
                {
                    WriteLine("Enter 1 Not Done\n2 Done");
                     int resp = Convert.ToInt32(ReadLine());
                     todo.Status = (Status) resp;
                } else {
                    WriteLine("Invalid method");
                }

            } else {
                break;
            }
          }
          

        }
    }
}

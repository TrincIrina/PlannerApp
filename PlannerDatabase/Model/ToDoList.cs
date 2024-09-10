using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerDatabase.Model
{
    // ToDoList - класс, описывающий списки дел
    public class ToDoList
    {
        public int Id { get; set; }        // первичный ключ
        public string Title { get; set; }  // заголовок
        public List<Deal> Deals { get; set; } = new();
        public ToDoList()
        {
            Title = string.Empty;
        }
        //public ToDoList(string title)
        //{
        //    Title = title;
        //}
        public override string ToString()
        {
            return $"{Title}/";
        }
    }
}

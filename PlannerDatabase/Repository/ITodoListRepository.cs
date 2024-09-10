using PlannerDatabase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerDatabase.Repository
{
    // ITodoListRepository - интерфейс сервиса для выполнения операций с сущностями ToDoList
    internal interface ITodoListRepository
    {
        // добавление новой записи
        public ToDoList Add(ToDoList todo);
        // получение всех записей
        public List<ToDoList> ListAll();
        // получение записи по id
        public ToDoList? GetByName(string title);
        // удаление записи по id
        public ToDoList? DeleteByName(string title);
        // обновление записи
        public ToDoList? Update(string oldTitle, string newTitle);
    }
}

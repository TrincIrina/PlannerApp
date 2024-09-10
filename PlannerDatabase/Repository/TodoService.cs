using PlannerDatabase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerDatabase.Repository
{
    // TodoService - сервис для выполнения операций с сущностями ToDoList
    public class TodoService : ITodoListRepository
    {
        // добавление новой записи
        public ToDoList Add(ToDoList todo)
        {
            using (ApplicationDbContext db = new())
            {
                db.ToDoLists.Add(todo);
                db.SaveChanges();
                return todo;
            }
        }
        // получение всех записей
        public List<ToDoList> ListAll()
        {
            using (ApplicationDbContext db = new())
            {
                return db.ToDoLists.ToList();
            }
        }

        // получение записи по id
        public ToDoList? GetByName(string title)
        {
            using (ApplicationDbContext db = new())
            {
                return db.ToDoLists.FirstOrDefault(todo => todo.Title == title);
            }
        }

        // удаление записи по id
        public ToDoList? DeleteByName(string title)
        {
            ToDoList? deletedTodo = GetByName(title);
            if (deletedTodo != null)
            {
                using (ApplicationDbContext db = new())
                {
                    db.ToDoLists.Remove(deletedTodo);
                    db.SaveChanges();
                }
            }
            return deletedTodo;
        }

        // обновление записи
        public ToDoList? Update(string oldTitle, string newTitle)
        {
            using (ApplicationDbContext db = new())
            {
                ToDoList? updatedTodo = db.ToDoLists.FirstOrDefault(todo => todo.Title == oldTitle);
                if (updatedTodo != null)
                {
                    updatedTodo.Title = newTitle;
                    db.SaveChanges();
                }
                return updatedTodo;
            }
        }
    }
}

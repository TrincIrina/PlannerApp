using PlannerDatabase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerDatabase.Repository
{
    // DealService - сервис для выполнения операций с сущностями Deal
    public class DealService : IDealRepository
    {
        // добавление новой записи
        public Deal Add(Deal deal)
        {
            using (ApplicationDbContext db = new())
            {
                db.Deals.Add(deal);
                db.SaveChanges();
                return deal;
            }
        }
        // получение записи по id
        public Deal? GetByName(string name)
        {
            using (ApplicationDbContext db = new())
            {
                return db.Deals.FirstOrDefault(deal => deal.Name == name);
            }
        }
        // получение одного списка дел (всех записей с одинаковым вторичным ключём)
        public List<Deal> ListAllByToDoList(string title)
        {
            using (ApplicationDbContext db = new())
            {
                ToDoList? toDoList = db.ToDoLists.FirstOrDefault(tl => tl.Title == title);
                if (toDoList == null)
                {
                    return new List<Deal>();
                }
                int ToDoListId = toDoList.Id;
                List<Deal> list = new();
                List<Deal> deals = db.Deals.ToList();
                foreach(Deal deal in deals)
                {
                    if(deal.ToDoListId == ToDoListId)
                    {
                        list.Add(deal);
                    }
                }
                return list;
            }
        }
        // удаление записи по id
        public Deal? DeleteByName(string name)
        {
            Deal? deletedDeal = GetByName(name);
            if (deletedDeal != null)
            {
                using (ApplicationDbContext db = new())
                {
                    db.Deals.Remove(deletedDeal);
                    db.SaveChanges();
                }
            }
            return deletedDeal;
        }

        // обновление записи
        public Deal? Update(Deal deal)
        {
            using (ApplicationDbContext db = new())
            {
                Deal? updatedDeal = db.Deals.FirstOrDefault(d => d.Id == deal.Id);
                if (updatedDeal != null)
                {
                    updatedDeal.Name = deal.Name;
                    updatedDeal.Priority = deal.Priority;
                    updatedDeal.DateCreation = deal.DateCreation;
                    updatedDeal.Deadline = deal.Deadline;
                    updatedDeal.ToDoListId = deal.ToDoListId;
                    db.SaveChanges();
                }
                return updatedDeal;
            }
        }
    }
}

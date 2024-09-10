using PlannerDatabase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerDatabase.Repository
{
    // ItemService - сервис для выполнения операций с сущностями Item
    public class ItemService : IItemRepository
    {
        // добавление новой записи
        public Item Add(Item item)
        {
            using (ApplicationDbContext db = new())
            {
                db.Items.Add(item);
                db.SaveChanges();
                return item;
            }
        }
        // получение записи по id
        public Item? GetById(int id)
        {
            using (ApplicationDbContext db = new())
            {
                return db.Items.FirstOrDefault(item => item.Id == id);
            }
        }
        // удаление записи по id
        public Item? DeleteById(int id)
        {
            Item? deletedItem = GetById(id);
            if (deletedItem != null)
            {
                using (ApplicationDbContext db = new())
                {
                    db.Items.Remove(deletedItem);
                    db.SaveChanges();
                }
            }
            return deletedItem;
        }
        // получение одного чек-листа (всех записей с одинаковым вторичным ключём)
        public List<Item> ListAllByDeal(int DealId)
        {
            using (ApplicationDbContext db = new())
            {
                List<Item> list = new();
                List<Item> items = db.Items.ToList();
                foreach (Item item in items)
                {
                    if (item.DealId == DealId)
                    {
                        list.Add(item);
                    }
                }
                return list;
            }
        }
        // обновление записи
        public Item? Update(Item item)
        {
            using (ApplicationDbContext db = new())
            {
                Item? updatedItem = db.Items.FirstOrDefault(it => it.Id == item.Id);
                if (updatedItem != null)
                {
                    updatedItem.Description = item.Description;
                    updatedItem.DealId = item.DealId;
                    db.SaveChanges();
                }
                return updatedItem;
            }
        }
    }   
}

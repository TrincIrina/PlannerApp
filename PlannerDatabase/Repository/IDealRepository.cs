using PlannerDatabase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerDatabase.Repository
{
    // IDealRepository - интерфейс сервиса для выполнения операций с сущностями Deal
    internal interface IDealRepository
    {
        // добавление новой записи
        public Deal Add(Deal deal);
        // получение записи по id
        public Deal? GetByName(string name);
        // получение одного списка дел (всех записей с одинаковым вторичным ключём)
        public List<Deal> ListAllByToDoList(string title);
        // удаление записи по id
        public Deal? DeleteByName(string name);
        // обновление записи
        public Deal? Update(Deal deal);
    }
}

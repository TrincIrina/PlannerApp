using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerDatabase.Model
{
    // Item - класс, описывающий пункт чек-листа
    public class Item
    {
        public int Id { get; set; }                 // первичный ключ
        public string Description { get; set; }     // описание
        public bool IsDone { get; set; } = false;   // выполнено
        public int DealId { get; set; }             // внешний ключ
        public Deal? Deal { get; set; }             // навигационное свойство
        public Item()
        {
            Description = string.Empty;
        }
        public override string ToString()
        {
            return $"{Description}/";
        }
    }
}

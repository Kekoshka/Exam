using Exam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.DataSeed
{
    public static class RoleSeed
    {
        public static readonly List<Role> Roles = new()
        {
            new()
            {
               Id = 1,
               Name = "Клиент"
            },
            new()
            {
               Id = 2,
               Name = "Менеджер"
            },
            new()
            {
               Id = 3,
               Name = "Администратор"
            }
        };
    }
    public static class CategorySeed
    {
        public static readonly List<Category> Categories = new()
        {
            new()
            {
               Id = 1,
               Name = "Женская обувь"
            },
            new()
            {
               Id = 2,
               Name = "Мужская обувь"
            }
        };
    }
    public static class OrderStatusSeed
    {
        public static readonly List<OrderStatus> OrderStatuses = new()
        {
            new()
            {
               Id = 1,
               Name = "Завершен"
            },
            new()
            {
               Id = 2,
               Name = "Новый"
            }
        };
    }
    public static class UnitSeed
    {
        public static readonly List<Unit> Units = new()
        {
            new()
            {
               Id = 1,
               Name = "шт."
            }
        };
    }
}

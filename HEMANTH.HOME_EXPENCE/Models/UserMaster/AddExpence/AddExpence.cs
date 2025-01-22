    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

    namespace HEMANTH.HOME_EXPENCE.Models.UserMaster.AddExpence
    {
        public class AddExpence
        {
        public string FormattedExpenceDate { get; set; }
        public List<SelectListItem> Categories { get; set; }
            public List<User> Users { get; set; }
            public List<Expense> Expenses { get; set; }
            public int SelectedCategory { get; set; }
            public string Description { get; set; }
            public decimal Amount { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ExpenceDate { get; set; }

        public int ExpenceMasterId { get; set; }

        public bool UpdateSaveStatus { get; set; }
        public int UserId { get; set; }
            public AddExpence()
            {
                Categories = new List<SelectListItem>();
                Users = new List<User>();
                Expenses = new List<Expense>();
            }
        }

        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Amount { get; set; }

        public int ExpDetailsId { get; set; }

        }

        public class Expense
        {
        public int UserId { get; set; }

        public int ExpDetailsId { get; set; }

        public string Category { get; set; }
            public string Description { get; set; }
            public decimal Amount { get; set; }
            public string UserName { get; set; }
            public DateTime Date { get; set; } 
             
            public int ExpenseMasterId { get; set; }
        }

    }

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalExpensesTracker.Models
{

    public class Category
    {
        public int CategoryId { get; set; }  // Unique identifier for the category
        public string CategoryType { get; set; } // "Expense" or "Income"
        public string CategoryName { get; set; } // Name of the category (e.g., Food, Salary)
        public DateTime DateCreated { get; set; } // Date when the category was created
    }
}

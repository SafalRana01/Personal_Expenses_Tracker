using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalExpensesTracker.Models
{
    public class Debt
    {

        public string DebtSource { get; set; } 
        public DateTime? DueDate { get; set; } 
        public decimal Amount { get; set; } // Amount of debt
        public string Status { get; set; } // Debt status (e.g., "pending", "paid")
        public DateTime? DateCreated { get; set; } // Date the debt was created (nullable)
        public string Note { get; set; } // Optional note about the debt
    }
}

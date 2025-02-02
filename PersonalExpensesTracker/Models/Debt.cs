using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalExpensesTracker.Models
{
    public class Debt
    {
        public string Id { get; set; } // Unique identifier for each debt
        public string DebtSource { get; set; } // Source of debt
        public DateTime? DueDate { get; set; } // Due date for debt payment
        public decimal Amount { get; set; } // Amount of debt
        public string Status { get; set; } // Debt status (e.g., "pending", "cleared")
        public DateTime? DateCreated { get; set; } // Date when debt was created
        public string Note { get; set; } // Optional note about the debt
        public DateTime? ClearedDate { get; set; } // Date when debt is cleared (new property)
    }
}

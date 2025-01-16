using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalExpensesTracker.Models
{
    public class Transaction
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
    }
}

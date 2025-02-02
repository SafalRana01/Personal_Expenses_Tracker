using Newtonsoft.Json;
using PersonalExpensesTracker.Models;

namespace PersonalExpensesTracker.Services
{
    // Service for managing transactions (adding, retrieving, deleting, etc.)
    public class TransactionService
    {
        private readonly string _folderPath = Path.Combine("C:\\Users\\Safal\\source\\repos\\PersonalExpensesTracker\\PersonalExpensesTracker\\Data");

        // Constructor to ensure the 'Data' folder exists for storing transaction data
        public TransactionService()
        {
            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }
        }

        // Adds a new transaction to the list and saves it to a file
        public async Task AddTransactionAsync(Transaction transaction)
        {
            try
            {
                var filePath = Path.Combine(_folderPath, "transaction.json");

                List<Transaction> transactions = await GetTransactionsAsync();

                // Ensure the transaction has a unique ID if it doesn't have one
                if (string.IsNullOrEmpty(transaction.Id))
                {
                    transaction.Id = Guid.NewGuid().ToString();
                }

                transactions.Add(transaction);

                var json = JsonConvert.SerializeObject(transactions, Formatting.Indented);
                await File.WriteAllTextAsync(filePath, json);

                Console.WriteLine($"Transaction saved to: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving transaction: {ex.Message}");
            }
        }

        // Retrieves all transactions from the saved file
        public async Task<List<Transaction>> GetTransactionsAsync()
        {
            try
            {
                var filePath = Path.Combine(_folderPath, "transaction.json");

                if (File.Exists(filePath))
                {
                    var json = await File.ReadAllTextAsync(filePath);
                    var transactions = JsonConvert.DeserializeObject<List<Transaction>>(json);
                    return transactions ?? new List<Transaction>();
                }
                return new List<Transaction>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading transactions: {ex.Message}");
                return new List<Transaction>();
            }
        }

        // Retrieves a specific transaction by its unique ID
        public async Task<Transaction> GetTransactionByIdAsync(string transactionId)
        {
            try
            {
                var transactions = await GetTransactionsAsync();
                var transaction = transactions.FirstOrDefault(t => t.Id == transactionId);

                if (transaction == null)
                {
                    Console.WriteLine("Transaction not found.");
                }

                return transaction;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving transaction by ID: {ex.Message}");
                return null;
            }
        }

        // Deletes a specific transaction by its ID
        public async Task<bool> DeleteTransactionAsync(string transactionId)
        {
            try
            {
                var filePath = Path.Combine(_folderPath, "transaction.json");
                var transactions = await GetTransactionsAsync();

                var transactionToDelete = transactions.FirstOrDefault(t => t.Id == transactionId);

                if (transactionToDelete == null)
                {
                    Console.WriteLine("Transaction not found.");
                    return false;
                }

                transactions.Remove(transactionToDelete);

                var json = JsonConvert.SerializeObject(transactions, Formatting.Indented);
                await File.WriteAllTextAsync(filePath, json);

                Console.WriteLine("Transaction deleted successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting transaction: {ex.Message}");
                return false;
            }
        }

        // Calculates the user's current balance by subtracting debits from credits
        public async Task<decimal> GetUserBalanceAsync()
        {
            try
            {
                var transactions = await GetTransactionsAsync();

                // Summing the amounts of credit and debit transactions
                decimal creditTotal = transactions
                    .Where(t => t.TransactionType.Equals("credit", StringComparison.OrdinalIgnoreCase))
                    .Sum(t => t.Amount);

                decimal debitTotal = transactions
                    .Where(t => t.TransactionType.Equals("debit", StringComparison.OrdinalIgnoreCase))
                    .Sum(t => t.Amount);

                return creditTotal - debitTotal;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating balance: {ex.Message}");
                return 0;
            }
        }

        // Performs a debit transaction, checking if there is enough balance first
        public async Task<bool> PerformDebitTransactionAsync(decimal amount, string title, string note)
        {
            try
            {
                if (amount <= 0)
                {
                    Console.WriteLine("Amount should be greater than zero for debit transactions.");
                    return false;
                }

                decimal currentBalance = await GetUserBalanceAsync();

                if (currentBalance < amount)
                {
                    Console.WriteLine("Insufficient balance for the debit transaction.");
                    return false;
                }

                // Creating the debit transaction
                var transaction = new Transaction
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = title,
                    Amount = amount,
                    TransactionType = "debit",
                    Date = DateTime.Now,
                    Note = note
                };

                // Adding the transaction and saving it
                await AddTransactionAsync(transaction);
                Console.WriteLine("Debit transaction successful.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error performing debit transaction: {ex.Message}");
                return false;
            }
        }
    }
}

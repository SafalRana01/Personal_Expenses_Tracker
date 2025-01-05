using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using PersonalExpensesTracker.Models;

namespace PersonalExpensesTracker.Services
{
    public class TransactionService
    {
        // Path to save JSON files in the 'Data' folder of the project
        private readonly string _folderPath = Path.Combine("C:\\Users\\Safal\\source\\repos\\PersonalExpensesTracker\\PersonalExpensesTracker\\Data");


       
        public TransactionService()
        {
            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath); 
            }
        }

       
        public async Task SaveTransactionAsync(Transaction transaction)
        {
            try
            {
                var filePath = Path.Combine(_folderPath, "transaction.json");

                // Read existing transactions or create a new list if file doesn't exist
                List<Transaction> transactions = await GetTransactionsAsync();

                // Add the new transaction to the list
                transactions.Add(transaction);

                // Serialize the updated list to JSON
                var json = JsonConvert.SerializeObject(transactions, Formatting.Indented);

                // Write the JSON to the file asynchronously
                await File.WriteAllTextAsync(filePath, json);

                Console.WriteLine($"Transaction saved to: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving transaction: {ex.Message}");
            }
        }

        // Method to read all transactions from the JSON file
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
                else
                {
                    return new List<Transaction>(); // Return empty list if file doesn't exist
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading transactions: {ex.Message}");
                return new List<Transaction>();
            }
        }
    }
}

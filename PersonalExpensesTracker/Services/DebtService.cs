using Newtonsoft.Json;
using PersonalExpensesTracker.Models;

namespace PersonalExpensesTracker.Services
{
    public class DebtService
    {
        private readonly string _folderPath = Path.Combine("C:\\Users\\Safal\\source\\repos\\PersonalExpensesTracker\\PersonalExpensesTracker\\Data");
        private readonly TransactionService _transactionService;

        public DebtService(TransactionService transactionService)
        {
            _transactionService = transactionService;

            // Checking if the folder for storing data exists, if not, creating it
            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }
        }

        // Method to get the file path for saving debt data
        private string GetFilePath() => Path.Combine(_folderPath, "debts.json");

        // Saving debt data to a file
        public async Task SaveDebtAsync(Debt debt)
        {
            try
            {
                // Assigning a unique ID to the debt if it's not provided
                if (string.IsNullOrEmpty(debt.Id))
                {
                    debt.Id = Guid.NewGuid().ToString();
                }

                // Setting the default status of the debt to "Pending"
                if (string.IsNullOrEmpty(debt.Status))
                {
                    debt.Status = "Pending";
                }

                var filePath = GetFilePath();
                List<Debt> debts = await GetDebtsAsync();
                debts.Add(debt);

                // Serializing the updated list of debts to JSON format and saving it
                var json = JsonConvert.SerializeObject(debts, Formatting.Indented);
                await File.WriteAllTextAsync(filePath, json);
            }
            catch (Exception ex)
            {
                // Printing error if something goes wrong while saving the debt
                Console.WriteLine($"Error saving debt: {ex.Message}");
            }
        }

        // Retrieving all debts from the saved file
        public async Task<List<Debt>> GetDebtsAsync()
        {
            try
            {
                var filePath = GetFilePath();
                if (File.Exists(filePath))
                {
                    // Reading and deserializing debt data from the file
                    var json = await File.ReadAllTextAsync(filePath);
                    var debts = JsonConvert.DeserializeObject<List<Debt>>(json);
                    return debts ?? new List<Debt>();
                }
            }
            catch (Exception ex)
            {
                // Printing error if something goes wrong while reading debts
                Console.WriteLine($"Error reading debts: {ex.Message}");
            }
            return new List<Debt>();
        }

        // Clearing a debt by making a payment
        public async Task<string> ClearDebtAsync(string debtId)
        {
            try
            {
                // Fetching the list of all debts
                List<Debt> debts = await GetDebtsAsync();
                var debtToClear = debts.FirstOrDefault(d => d.Id == debtId && d.Status == "Pending");

                // Checking if the debt is found and if it is in "Pending" status
                if (debtToClear == null)
                {
                    return "Debt not found or already cleared.";
                }

                // Checking if the user has enough balance to clear the debt
                decimal currentBalance = await _transactionService.GetUserBalanceAsync();
                if (currentBalance < debtToClear.Amount)
                {
                    return "Insufficient balance to clear the debt.";
                }

                // Creating a transaction to pay off the debt
                var debtPaymentTransaction = new Transaction
                {
                    Title = $"Debt Payment for {debtToClear.DebtSource}",
                    Amount = debtToClear.Amount,
                    TransactionType = "debit",
                    Category = "Debt",
                    Date = DateTime.Now,
                    Note = $"Cleared debt from source: {debtToClear.DebtSource}"
                };

                // Adding the transaction to the user's transaction history
                await _transactionService.AddTransactionAsync(debtPaymentTransaction);

                // Marking the debt as "Cleared" and updating the cleared date
                debtToClear.Status = "Cleared";
                debtToClear.ClearedDate = DateTime.Now;

                // Saving the updated debt data back to the file
                var json = JsonConvert.SerializeObject(debts, Formatting.Indented);
                await File.WriteAllTextAsync(GetFilePath(), json);

                return "Debt cleared successfully.";
            }
            catch (Exception ex)
            {
                // Returning error if something goes wrong while clearing the debt
                return $"Error clearing debt: {ex.Message}";
            }
        }

        // Updating the details of an existing debt
        public async Task UpdateDebtAsync(Debt updatedDebt)
        {
            try
            {
                var filePath = GetFilePath();
                var debts = await GetDebtsAsync();
                var index = debts.FindIndex(d => d.Id == updatedDebt.Id);

                // If the debt is found, replace it with the updated version
                if (index != -1)
                {
                    debts[index] = updatedDebt;
                }
                else
                {
                    Console.WriteLine($"Debt with ID {updatedDebt.Id} not found.");
                }

                // Saving the updated list of debts back to the file
                var json = JsonConvert.SerializeObject(debts, Formatting.Indented);
                await File.WriteAllTextAsync(filePath, json);
            }
            catch (Exception ex)
            {
                // Printing error if something goes wrong while updating the debt
                Console.WriteLine($"Error updating debt: {ex.Message}");
            }
        }
    }
}

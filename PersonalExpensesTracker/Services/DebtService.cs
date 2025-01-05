using Newtonsoft.Json;
using PersonalExpensesTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalExpensesTracker.Services
{
    public class DebtService
    {
        // Path to save JSON files in the 'Data' folder of the project
        private readonly string _folderPath = Path.Combine("C:\\Users\\Safal\\source\\repos\\PersonalExpensesTracker\\PersonalExpensesTracker\\Data");


        // Constructor to ensure the 'Data' folder exists
        public DebtService()
        {
            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath); // Create folder if it doesn't exist
            }
        }

        // Method to save a new debt to the JSON file
        public async Task SaveDebtAsync(Debt debt)
        {
            try
            {
                var filePath = Path.Combine(_folderPath, "debts.json");

                // Read existing debts or create a new list if file doesn't exist
                List<Debt> debts = await GetDebtsAsync();

                // Add the new debt to the list
                debts.Add(debt);

                // Serialize the updated list to JSON
                var json = JsonConvert.SerializeObject(debts, Formatting.Indented);

                // Write the JSON to the file asynchronously
                await File.WriteAllTextAsync(filePath, json);

                Console.WriteLine($"Debt saved to: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving debt: {ex.Message}");
            }
        }

        // Method to read all debts from the JSON file
        public async Task<List<Debt>> GetDebtsAsync()
        {
            try
            {
                var filePath = Path.Combine(_folderPath, "debts.json");

                if (File.Exists(filePath))
                {
                    var json = await File.ReadAllTextAsync(filePath);
                    var debts = JsonConvert.DeserializeObject<List<Debt>>(json);
                    return debts ?? new List<Debt>();
                }
                else
                {
                    return new List<Debt>(); // Return empty list if file doesn't exist
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading debts: {ex.Message}");
                return new List<Debt>();
            }
        }
    }
}

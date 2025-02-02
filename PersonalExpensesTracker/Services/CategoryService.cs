using PersonalExpensesTracker.Models;

namespace PersonalExpensesTracker.Services
{

    // Service: CategoryService
    public class CategoryService
    {
        private readonly List<Category> _categories = new List<Category>();

        public CategoryService()
        {
            // Adding some initial categories for demonstration
            _categories.Add(new Category { CategoryId = 1, CategoryType = "Expense", CategoryName = "Food", DateCreated = DateTime.Now });
            _categories.Add(new Category { CategoryId = 2, CategoryType = "Income", CategoryName = "Salary", DateCreated = DateTime.Now });
        }

        // Get all categories
        public Task<List<Category>> GetCategoriesAsync()
        {
            return Task.FromResult(_categories);
        }

        // Get categories based on type (Expense or Income)
        public Task<List<Category>> GetCategoriesByTypeAsync(string type)
        {
            var categories = _categories.Where(c => c.CategoryType.Equals(type, StringComparison.OrdinalIgnoreCase)).ToList();
            return Task.FromResult(categories);
        }

        // Add a new category
        public Task AddCategoryAsync(Category category)
        {
            category.DateCreated = DateTime.Now; // Set creation date to now
            _categories.Add(category);
            return Task.CompletedTask;
        }
    }
}

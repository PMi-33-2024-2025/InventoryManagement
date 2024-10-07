using Microsoft.Data.SqlClient;

namespace InventoryManagementApp.Seeder
{
    public static class DatabaseSeeder
    {
        const string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=InventoryManagement;" +
                "Integrated Security=True;";
        public static void FillDatabaseWithTestData()
        {
            var users = GenerateUsers(30);
            var categories = GenerateCategories(10);
            var suppliers = GenerateSuppliers(10);
            var products = GenerateProducts();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var user in users)
                {
                    string query = "INSERT INTO dbo.Users (Name, Password, Role) VALUES (@Name, @Password, @Role)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", user.Name);
                        command.Parameters.AddWithValue("@Password", user.Password);
                        command.Parameters.AddWithValue("@Role", user.Role);
                        command.ExecuteNonQuery();
                    }
                }

                foreach (var category in categories)
                {
                    string query = "INSERT INTO dbo.Categories (Name) VALUES (@Name)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", category);
                        command.ExecuteNonQuery();
                    }
                }

                foreach (var supplier in suppliers)
                {
                    string query = "INSERT INTO dbo.Suppliers (Name) VALUES (@Name)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", supplier);
                        command.ExecuteNonQuery();
                    }
                }

                Random rand = new Random();
                for (int i = 0; i < products.Count; i++)
                {
                    var product = products[i];
                    string query = "INSERT INTO dbo.Products (Title, Amount, Price, Description, CategoryId, SupplierId, LastUpdated) VALUES (@Title, @Amount, @Price, @Description, @CategoryId, @SupplierId, @LastUpdated)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Title", product.Title);
                        command.Parameters.AddWithValue("@Amount", rand.Next(1, 100));
                        command.Parameters.AddWithValue("@Price", Math.Round(rand.NextDouble() * 100, 2));
                        command.Parameters.AddWithValue("@Description", product.Description);
                        command.Parameters.AddWithValue("@CategoryId", rand.Next(1, categories.Count + 1));
                        command.Parameters.AddWithValue("@SupplierId", rand.Next(1, suppliers.Count + 1));
                        command.Parameters.AddWithValue("@LastUpdated", DateTime.Now);
                        command.ExecuteNonQuery();
                    }
                }

            }
        }

        private static List<User> GenerateUsers(int count)
        {
            var users = new List<User>();
            for (int i = 1; i <= 5; i++)
            {
                users.Add(new User
                {
                    Name = $"Admin{i}",
                    Password = "password",
                    Role = "admin"
                });
            }

            for (int i = 1; i <= count - 5; i++)
            {
                users.Add(new User
                {
                    Name = $"User{i}",
                    Password = "password",
                    Role = "user"
                });
            }
            return users;
        }

        private static List<string> GenerateCategories(int count)
        {
            var categories = new List<string>
            {
                "Electronics", "Furniture", "Clothing", "Sports", "Food & Beverages",
                "Books", "Toys", "Beauty & Personal Care", "Automotive", "Health"
            };
            return categories.GetRange(0, Math.Min(count, categories.Count));
        }

        private static List<string> GenerateSuppliers(int count)
        {
            var suppliers = new List<string>
            {
                "Supplier A", "Supplier B", "Supplier C", "Supplier D", "Supplier E",
                "Supplier F", "Supplier G", "Supplier H", "Supplier I", "Supplier J"
            };
            return suppliers.GetRange(0, Math.Min(count, suppliers.Count));
        }

        private static List<Product> GenerateProducts()
        {
            var products = new List<Product>
            {
                new Product { Title = "Apple iPhone 13", Description = "Latest model with advanced features." },
                new Product { Title = "Samsung Galaxy S21", Description = "High-performance smartphone with a stunning display." },
                new Product { Title = "Sony WH-1000XM4", Description = "Noise-canceling over-ear headphones." },
                new Product { Title = "Dell XPS 13", Description = "Premium ultrabook with high-resolution display." },
                new Product { Title = "Logitech MX Master 3", Description = "Advanced wireless mouse with customizable buttons." },
                new Product { Title = "Nike Air Max 270", Description = "Stylish and comfortable sneakers." },
                new Product { Title = "Adidas Ultraboost", Description = "High-performance running shoes with excellent cushioning." },
                new Product { Title = "HP Spectre x360", Description = "Versatile 2-in-1 laptop with sleek design." },
                new Product { Title = "Amazon Echo Dot", Description = "Smart speaker with Alexa." },
                new Product { Title = "Sony PlayStation 5", Description = "Next-generation gaming console." },
                new Product { Title = "Samsung QLED TV", Description = "4K Ultra HD Smart TV with stunning picture quality." },
                new Product { Title = "Bose SoundLink Mini", Description = "Portable Bluetooth speaker with deep sound." },
                new Product { Title = "Kindle Paperwhite", Description = "Waterproof e-reader with built-in light." },
                new Product { Title = "Instant Pot Duo 7-in-1", Description = "Multi-cooker for fast and convenient meals." },
                new Product { Title = "Fitbit Charge 5", Description = "Health and fitness tracker with GPS." },
                new Product { Title = "Nikon D3500", Description = "Beginner-friendly DSLR camera." },
                new Product { Title = "GoPro HERO9 Black", Description = "Action camera with 5K video capabilities." },
                new Product { Title = "Microsoft Surface Pro 7", Description = "Powerful tablet with laptop performance." },
                new Product { Title = "Apple MacBook Air M1", Description = "Lightweight laptop with impressive battery life." },
                new Product { Title = "JBL Flip 5", Description = "Portable waterproof Bluetooth speaker." },
                new Product { Title = "Oculus Quest 2", Description = "All-in-one VR headset." },
                new Product { Title = "Ring Video Doorbell", Description = "Smart doorbell with video and two-way audio." },
                new Product { Title = "Nest Thermostat", Description = "Smart thermostat that learns your habits." },
                new Product { Title = "Samsung Galaxy Tab S7", Description = "Premium Android tablet with S Pen." },
                new Product { Title = "Razer Blade 15", Description = "High-performance gaming laptop." },
                new Product { Title = "Tiffany & Co. Bracelet", Description = "Classic and elegant silver bracelet." },
                new Product { Title = "Levi's 501 Jeans", Description = "Timeless denim jeans." },
                new Product { Title = "Ray-Ban Sunglasses", Description = "Stylish sunglasses with UV protection." },
                new Product { Title = "KitchenAid Stand Mixer", Description = "Versatile kitchen appliance for baking." },
                new Product { Title = "Dyson V11 Vacuum Cleaner", Description = "Powerful cordless vacuum with smart features." },
                new Product { Title = "Apple AirPods Pro", Description = "Wireless earbuds with noise cancellation." },
                new Product { Title = "Bose QuietComfort 35 II", Description = "Wireless headphones with noise cancellation." },
                new Product { Title = "Samsung Galaxy Watch", Description = "Smartwatch with health tracking features." },
                new Product { Title = "Anker PowerCore Portable Charger", Description = "High-capacity portable battery charger." },
                new Product { Title = "GoPro HERO8 Black", Description = "Compact action camera for adventure." },
                new Product { Title = "Bose Soundbar 700", Description = "Premium soundbar with voice control." },
                new Product { Title = "Raspberry Pi 4 Model B", Description = "Compact computer for DIY projects." },
                new Product { Title = "Nespresso Vertuo Coffee Maker", Description = "Coffee machine for brewing espresso." },
                new Product { Title = "Sony WH-CH710N", Description = "Wireless noise-canceling headphones." }
            };
            return products;
        }

        public static void DisplayData(string tableName, string query)
        {
            Console.WriteLine($"\n--- {tableName} ---");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string output = "";
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            output += $"{reader[i]}\t";
                        }
                        Console.WriteLine(output);
                    }
                    reader.Close();
                }
            }
        }

        public class User
        {
            public string Name { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }
        }

        public class Product
        {
            public string Title { get; set; }
            public string Description { get; set; }
        }
    }
}

namespace WebAPIDemo.Models.Repositories
{
    public static class ShirtRepository
    {
        private static List<Shirt> shirts = new List<Shirt>
        {
            new Shirt { ShirtId = 1, Brand = "Polo", Color = "Blue", Size = 'M', Gender = "Men", Price = 200 },
            new Shirt { ShirtId = 2, Brand = "Polo", Color = "Red", Size = 'L', Gender = "Men", Price = 300 },
            new Shirt { ShirtId = 3, Brand = "Polo", Color = "Green", Size = 'S', Gender = "Women", Price = 100 },
            new Shirt { ShirtId = 4, Brand = "Polo", Color = "Black", Size = 'M', Gender = "Men", Price = 500 }
        };
        public static bool ShirtExists(int id)
        {
            return shirts.Any(s => s.ShirtId == id);
        }
        public static Shirt? GetShirtById(int id)
        {
            return shirts.FirstOrDefault(s => s.ShirtId == id);
        }
        public static List<Shirt> GetAllShirts()
        {
            return shirts;
        }

        public static Shirt? GetShirtByProperties(string? brand, string? gender, string? color, char? size)
        {
            return shirts.FirstOrDefault(x => !string.IsNullOrWhiteSpace(brand) 
                                                && !string.IsNullOrWhiteSpace(x.Brand) 
                                                && brand.Equals(x.Brand)
                                                && !string.IsNullOrWhiteSpace(gender)
                                                && !string.IsNullOrWhiteSpace(x.Gender)
                                                && gender.Equals(x.Gender)
                                                && !string.IsNullOrWhiteSpace(color)
                                                && !string.IsNullOrWhiteSpace(x.Color)
                                                && color.Equals(x.Color)
                                                && size.HasValue
                                                && x.Size.HasValue
                                                && size.Value== x.Size.Value);
        }

        public static void CreateShirt(Shirt shirt)
        {
            int maxId = shirts.Max(x => x.ShirtId);
            shirt.ShirtId = maxId + 1;
            shirts.Add(shirt);
        }
        public static void UpdateShirt(Shirt shirt)
        {
            var shirtToUpdate = shirts.First(x => x.ShirtId == shirt.ShirtId);

            shirtToUpdate.Brand = shirt.Brand;
            shirtToUpdate.Price = shirt.Price;
            shirtToUpdate.Gender = shirt.Gender;
            shirtToUpdate.Color = shirt.Color;
            shirtToUpdate.Size = shirt.Size;
        }

        public static void DeleteShirt(int id)
        {
            var shirt = GetShirtById(id);
            if(shirt != null)
                shirts.Remove(shirt);
        }
    }
}

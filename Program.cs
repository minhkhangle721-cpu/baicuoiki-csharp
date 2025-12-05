using System;
using System.Collections.Generic;
using System.Linq;

namespace FashionStoreManager
{
    class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        public override string ToString()
        {
            return $"{Id,-6} | {Name,-15} | {Type,-10} | {Price,8:C} | SL: {Quantity}";
        }
    }

    class ProductStore
    {
        private List<Product> products = new List<Product>();


        public void Add(Product p) => products.Add(p);

        public Product Find(string id) => products.FirstOrDefault(p => p.Id == id);

        public void Update(string id, Product newP)
        {
            var p = Find(id);
            if (p != null)
            {
                p.Name = newP.Name;
                p.Type = newP.Type;
                p.Price = newP.Price;
                p.Quantity = newP.Quantity;
                Console.WriteLine("✅ Đã cập nhật sản phẩm!");
            }
            else
                Console.WriteLine("❌ Không tìm thấy sản phẩm!");
        }

        public void Remove(string id)
        {
            int removed = products.RemoveAll(p => p.Id == id);
            Console.WriteLine(removed > 0 ? "🗑️ Đã xóa sản phẩm!" : "❌ Không tìm thấy sản phẩm!");
        }

        public void ShowAll()
        {
            Console.WriteLine("\n--- DANH SÁCH SẢN PHẨM ---");
            foreach (var p in products)
                Console.WriteLine(p);
        }

        public IEnumerable<List<Product>> GetCombos(int m)
        {
            var n = products.Count;
            if (m <= 0 || m > n) yield break;

            int[] idx = Enumerable.Range(0, m).ToArray();
            while (true)
            {
                yield return idx.Select(x => products[x]).ToList();
                int i;
                for (i = m - 1; i >= 0 && idx[i] == i + n - m; i--) ;
                if (i < 0) yield break;
                idx[i]++;
                for (int j = i + 1; j < m; j++) idx[j] = idx[j - 1] + 1;
            }
        }

        public void SampleData()
        {
            for (int i = 1; i <= 10; i++)
                Add(new Product
                {
                    Id = $"P{i}",
                    Name = $"Sản phẩm {i}",
                    Type = i % 2 == 0 ? "Áo" : "Quần",
                    Price = 100 + i * 10,
                    Quantity = i + 5
                });
        }
    }

    class Program
    {
        static void Main()
        {
            var store = new ProductStore();
            store.SampleData();

            while (true)
            {
                Console.WriteLine("\n===== QUẢN LÝ CỬA HÀNG THỜI TRANG =====");
                Console.WriteLine("1. Xem danh sách sản phẩm");
                Console.WriteLine("2. Thêm sản phẩm mới");
                Console.WriteLine("3. Tìm sản phẩm theo mã");
                Console.WriteLine("4. Cập nhật sản phẩm");
                Console.WriteLine("5. Xóa sản phẩm");
                Console.WriteLine("6. Xem combo sản phẩm");
                Console.WriteLine("0. Thoát");
                Console.Write("Chọn: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        store.ShowAll();
                        break;

                    case "2":
                        Console.Write("Nhập mã: ");
                        string id = Console.ReadLine();
                        Console.Write("Tên: ");
                        string name = Console.ReadLine();
                        Console.Write("Loại (Áo/Quần): ");
                        string type = Console.ReadLine();
                        Console.Write("Giá: ");
                        double price = double.Parse(Console.ReadLine());
                        Console.Write("Số lượng: ");
                        int quantity = int.Parse(Console.ReadLine());

                        store.Add(new Product { Id = id, Name = name, Type = type, Price = price, Quantity = quantity });
                        Console.WriteLine("✅ Đã thêm sản phẩm!");
                        break;

                    case "3":
                        Console.Write("Nhập mã cần tìm: ");
                        var f = store.Find(Console.ReadLine());
                        Console.WriteLine(f != null ? f.ToString() : "❌ Không tìm thấy sản phẩm!");
                        break;

                    case "4":
                        Console.Write("Nhập mã cần cập nhật: ");
                        string uid = Console.ReadLine();
                        Console.Write("Tên mới: ");
                        string uname = Console.ReadLine();
                        Console.Write("Loại mới: ");
                        string utype = Console.ReadLine();
                        Console.Write("Giá mới: ");
                        double uprice = double.Parse(Console.ReadLine());
                        Console.Write("Số lượng mới: ");
                        int uqty = int.Parse(Console.ReadLine());
                        store.Update(uid, new Product { Name = uname, Type = utype, Price = uprice, Quantity = uqty });
                        break;

                    case "5":
                        Console.Write("Nhập mã cần xóa: ");
                        store.Remove(Console.ReadLine());
                        break;

                    case "6":
                        Console.Write("Số sản phẩm trong combo: ");
                        int m = int.Parse(Console.ReadLine());
                        int count = 0;
                        foreach (var combo in store.GetCombos(m).Take(5))
                        {
                            Console.WriteLine($"Combo {++count}: {string.Join(", ", combo.Select(x => x.Id))}");
                        }
                        if (count == 0) Console.WriteLine("❌ Không đủ sản phẩm để tạo combo!");
                        break;

                    case "0":
                        Console.WriteLine("👋 Thoát chương trình...");
                        return;

                    default:
                        Console.WriteLine("⚠️ Lựa chọn không hợp lệ!");
                        break;
                }
            }
        }
    }
}

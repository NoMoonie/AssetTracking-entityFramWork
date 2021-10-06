using System;
using System.Linq;
using AssetTrackingEF.data;
using AssetTrackingEF.domain;


namespace AssetTrackingEF.UI
{
    class Program
    {

        private static AssetContext _context = new AssetContext();
        static void Main(string[] args)
        {
            _context.Database.EnsureCreated();
            Console.WriteLine("test");
            GetAssets();
            Addphone();
        }

        private static void Addphone(){
            var phone = new phone {Brand = "iphone"};
            _context.Assets.Add(phone);
            _context.SaveChanges();
        }

        private static void Addlaptop(){
            var phone = new phone {Brand = "iphone"};
            _context.Assets.Add(phone);
            _context.SaveChanges();
        }

        private static void GetAssets(){
            var Assets = _context.Assets.ToList();
            foreach(var Asset in Assets){
                Console.WriteLine(Asset.Brand);
            }
        }
    }
}

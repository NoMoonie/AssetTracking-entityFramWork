using System;
using System.Collections.Generic;
using System.Linq;
using AssetTrackingEF.data;
using AssetTrackingEF.domain;
using AssetTrackingEF.input;


namespace AssetTrackingEF.UI
{
    class Program
    {

        private static AssetContext _context = new AssetContext();
        static void Main(string[] args)
        {
            //_context.Database.EnsureCreated();
            HandelInput MainInput = new HandelInput();
            
            MainInput.ShowCommands();
            while(true){
                Console.ResetColor();
				string Input = MainInput.GetUserInput();
                if(Input == null){
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Command not found");
					Console.WriteLine("Type \'help\' to get list of commands");
					continue;
				}
                string[] InputArr = Input.Split(" ");
                if(InputArr[0] == "q"){break;}
                if(InputArr[0] == "create"){
                    if(InputArr[1] == "phone"){
                        string[] TempArr = MainInput.GetAssetFromUser();
                        if(TempArr != null){
                            bool[] IsTemp = new bool[4];
                            IsTemp[0] = int.TryParse(TempArr[3], out int Year);
                            IsTemp[1] = int.TryParse(TempArr[4], out int Month);
                            IsTemp[2] = int.TryParse(TempArr[5], out int Day);
                            IsTemp[3] = double.TryParse(TempArr[6], out double PriceUSD);
                            foreach(bool i in IsTemp){
                                if(!i){
                                    continue;
                                }
                            }
                            double Rate = GetRate(TempArr[2]);
                            double PriceInLocal = GetExchangeRate(PriceUSD, Rate);
                            //convert price to local price
                            try{
                                phone NewPhone = new phone(){
                                    Brand = TempArr[0],
                                    Model = TempArr[1],
                                    OfficeLocation = TempArr[2],
                                    PurchaseDate = new DateTime(Year, Month, Day),
                                    PriceInUSD = PriceUSD,
                                    LocalPrice = PriceInLocal,
                                };
                                Addphone(NewPhone);
                                //MainList.AddAsset(NewPhone);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Phone Added");
                            }catch(Exception){
                                if(ShowErr(Month, Day)){continue;}
                            }
                        }
                    }
                    if(InputArr[1] == "laptop"){
                        string[] TempArr = MainInput.GetAssetFromUser();
                        if(TempArr != null){
                            bool[] IsTemp = new bool[4];
                            IsTemp[0] = int.TryParse(TempArr[3], out int Year);
                            IsTemp[1] = int.TryParse(TempArr[4], out int Month);
                            IsTemp[2] = int.TryParse(TempArr[5], out int Day);
                            IsTemp[3] = double.TryParse(TempArr[6], out double PriceUSD);
                            foreach(bool i in IsTemp){
                                if(!i){
                                    continue;
                                }
                            }
                            double Rate = GetRate(TempArr[2]);
                            double PriceInLocal = GetExchangeRate(PriceUSD, Rate);
                            //convert price to local price
                            try{
                                laptop NewPhone = new laptop(){
                                    Brand = TempArr[0],
                                    Model = TempArr[1],
                                    OfficeLocation = TempArr[2],
                                    PurchaseDate = new DateTime(Year, Month, Day),
                                    PriceInUSD = PriceUSD,
                                    LocalPrice = PriceInLocal,
                                };
                                Addlaptop(NewPhone);
                                //MainList.AddAsset(NewPhone);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Laptop Added");
                            }catch(Exception){
                                if(ShowErr(Month, Day)){continue;}
                            }
                        }
                    }
                }
                if(InputArr[0] == "read"){
                    GetAssets();
                }
                if(InputArr[0] == "update"){
                    bool IsInt = int.TryParse(InputArr[1], out int Id);
                    if(!IsInt){Console.WriteLine(InputArr[1] + "is not a number"); continue;}
                    var Asset = _context.Assets.Where(a => a.Id == Id).FirstOrDefault();
                    string[] Temp = new string[]{
                        "brand", "model","office","date","price"
                    };
                    string Res = MainInput.CheckInput(InputArr[2], Temp);
                    if(Res == null){
                        Console.WriteLine(InputArr[2]+": unknown prop");
                    }
                    if(InputArr[2].ToLower() == "brand"){
                        Console.Write("new Brand: ");
                        string Value = Console.ReadLine();
                        string TempStr = Asset.Brand; 
                        Asset.Brand = Value;
                        _context.SaveChanges();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(TempStr + " -> " + Value);
                    }
                    if(InputArr[2].ToLower() == "model"){
                        Console.Write("new Model: ");
                        string Value = Console.ReadLine();
                        string TempStr = Asset.Model; 
                        Asset.Model = Value;
                        _context.SaveChanges();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(TempStr + " -> " + Value);
                    }
                    if(InputArr[2].ToLower() == "office"){
                        Console.Write("new Office: ");
                        string Value = Console.ReadLine();
                        string TempStr = Asset.OfficeLocation; 

                        //change the local curency
                        double Price = Asset.PriceInUSD;
                        
                        double Rate = GetRate(Value);
                        double PriceInLocal = GetExchangeRate(Price, Rate);
                        
                        Asset.OfficeLocation = Value;
                        Asset.LocalPrice = PriceInLocal;
                        
                        _context.SaveChanges();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(TempStr + " -> " + Value);
                    }
                    if(InputArr[2].ToLower() == "date"){
                        Console.WriteLine("Format: yyyy mm dd");
                        Console.Write("new Date: ");
                        string Brand = Console.ReadLine();
                        string[] TempSplit = Brand.Split(" ");
                        bool IsInt1 = int.TryParse(TempSplit[0], out int Year);
                        bool IsInt2 = int.TryParse(TempSplit[1], out int Month);
                        bool IsInt3 = int.TryParse(TempSplit[2], out int Day);
                        try{
                            DateTime newDate = new DateTime(Year, Month, Day);
                            DateTime TempStr = Asset.PurchaseDate;
                            Asset.PurchaseDate = newDate;
                            _context.SaveChanges();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(TempStr.ToString("yyyy-MM-dd") + " -> " + newDate.ToString("yyyy-MM-dd"));
                        }catch(Exception){
                            if(ShowErr(Month, Day)){continue;}
                        }
                    }
                    if(InputArr[2].ToLower() == "price"){
                        Console.Write("new Price: ");
                        string Brand = Console.ReadLine();
                        bool isDouble = double.TryParse(Brand, out double Value);
                        
                        double Rate = GetRate(Asset.OfficeLocation);
                        double PriceInLocal = GetExchangeRate(Value, Rate);
                        
                        double TempStr = Asset.PriceInUSD;
                        double TempStr2 = Asset.LocalPrice;
                        Asset.PriceInUSD = Value;
                        Asset.LocalPrice = PriceInLocal;
                        _context.SaveChanges();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(TempStr.ToString("0.00") + " -> " + Value.ToString("0.00"));
                        Console.WriteLine(TempStr2.ToString("0.00") + " -> " + PriceInLocal.ToString("0.00"));
                    }
                }
                if(InputArr[0] == "delete"){
                    bool IsInt = int.TryParse(InputArr[1], out int Id);
                    if(!IsInt){Console.WriteLine(InputArr[1] + "is not a number"); continue;}
                    var Asset = _context.Assets.Where(a => a.Id == Id).FirstOrDefault();
                    removeAsset(Asset);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Asset with id " + Id.ToString() + " removed");
                }
				if(InputArr[0] == "help"){
					MainInput.ShowCommands();
				}
                if(InputArr[0] == "report"){
                    ShowReport();
                }

            }
        }

        static void ShowReport(){
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Statistics");
            Console.WriteLine("----------");
            var Assets = _context.Assets.ToList();
            double TotalPirce = 0;
            int count = 0;
            foreach(var Asset in Assets){
                TotalPirce += Asset.PriceInUSD;
                count++;
            }
            
            double Average = TotalPirce/count;
            Console.WriteLine("Total assets: " + count.ToString());
            Console.WriteLine("Total price in USD: " + TotalPirce.ToString("0.00"));
            Console.WriteLine("Average price in USD: " + Average.ToString("0.00"));
            Console.WriteLine("");
        }
        static bool ShowErr(double Month, double Day){
			Console.ForegroundColor = ConsoleColor.Red;
			if(Month < 0 || Day < 0){Console.WriteLine("Can't be negitive");return true;}
			if(Month > 12){Console.WriteLine("There is only 12 months in a year");return true;}
			if(Day > 31){Console.WriteLine("There is max 30-31 days in a month");return true;}
			Console.WriteLine("Error");
			return false;
		}
        private static void Addphone(phone Phone){
            _context.Assets.Add(Phone);
            _context.SaveChanges();
        }
        private static void Addlaptop(laptop Laptop){
            _context.Assets.Add(Laptop);
            _context.SaveChanges();
        }
        private static void removeAsset(Asset Asset){
            _context.Assets.Remove(Asset);
            _context.SaveChanges();
        }
        private static void GetAssets(){
            string[] Temp = new string[]{
				"Id" ,"Brand", "Model","Office","Date","Price in USD","Local price"
			};
			foreach(string i in Temp){
				Console.Write("| "+i.PadRight(15));
			}
			Console.WriteLine("\n|----------------|----------------|----------------|----------------|----------------|----------------|----------------");
            var Assets = _context.Assets.OrderByDescending(Asset => Asset.OfficeLocation).ThenBy(Asset => Asset.PriceInUSD).ToList();
            foreach(var Asset in Assets){
                int Check = CheckDate(Asset.PurchaseDate);
				if(Check == 1){
					Console.ForegroundColor = ConsoleColor.Red;
				}
				else if(Check == -1){
					Console.ForegroundColor = ConsoleColor.Yellow;
				}else{
					Console.ResetColor();
				}
                Console.WriteLine($"| {Asset.Id.ToString().PadRight(15)}| {Asset.Brand.PadRight(15)}| {Asset.Model.PadRight(15)}| {Asset.OfficeLocation.PadRight(15)}| {Asset.PurchaseDate.ToString("yyyy-MM-dd").PadRight(15)}| {Asset.PriceInUSD.ToString("0.00").PadRight(15)}| {Asset.LocalPrice.ToString("0.00").PadRight(15)}");
				Console.ResetColor();
                //Console.WriteLine(Asset.Brand);
            }
            Console.WriteLine("|----------------|----------------|----------------|----------------|----------------|----------------|----------------");
			Console.WriteLine("");
        }
        static int CheckDate(DateTime date){
			//get current date 
			DateTime CurrentDate = DateTime.Today;
			int m1 = (CurrentDate.Month - date.Month);
			int m2 = (CurrentDate.Year - date.Year) * 12;
			int months = m1 + m2;
			if(33 <= months){
				return 1;
			}
			else if(30 <= months){
				return -1;
			}
			else{
				return 0;
			}
		}
        static double GetRate(string currency){

			if(currency.ToLower().Trim() == "sweden"){
				return 8.6562;
			}
			if(currency.ToLower().Trim() == "spain"){
				return 0.85376;
			}
			return 1;
		}
		static double GetExchangeRate(double Amount, double Rate){
			return Amount * Rate;
		}

    }
}

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
            _context.Database.EnsureCreated();
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
                                Console.WriteLine("Asset Added");
                            }catch(Exception){
                                //if(ShowErr(Month, Day)){continue;}
                            }
                        }
                        //Addphone();
                    }
                    if(InputArr[1] == "laptop"){
                        Console.WriteLine("laptop");
                        //Addlaptop();
                    }
                }
                if(InputArr[0] == "read"){
                    GetAssets();
                }
                if(InputArr[0] == "update"){
                    
                }
                if(InputArr[0] == "delete"){
                    var Assets = _context.Assets.ToList();
                    bool IsInt = int.TryParse(InputArr[1], out int Id);
                    if(!IsInt){Console.WriteLine(InputArr[1] + "is not a number"); continue;}
                    IEnumerable<Asset> List = from Asset in Assets where Asset.Id == Id select Asset;
                    foreach(var a in List){
                        removeAsset(a);
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Asset with id " + Id.ToString() + " removed");
                }
				if(InputArr[0] == "help"){
					MainInput.ShowCommands();
				}

            }
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
            var Assets = _context.Assets.ToList();
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

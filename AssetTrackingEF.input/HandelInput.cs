using System;
using System.Collections.Generic;

namespace AssetTrackingEF.input{

    public class HandelInput{
		private int index = 0;
		private string[] CommandArr = new string[7];
		private string[] PropListArr = new string[7];
		public HandelInput(){
			CommandArr[index++] = "q";
			CommandArr[index++] = "create";
			CommandArr[index++] = "read";
			CommandArr[index++] = "update";
			CommandArr[index++] = "delete";
			CommandArr[index++] = "help";
			CommandArr[index++] = "report";
			index = 0; 
			PropListArr[index++] = "brand";
			PropListArr[index++] = "model";
			PropListArr[index++] = "office";
			PropListArr[index++] = "year";
			PropListArr[index++] = "month";
			PropListArr[index++] = "day";
			PropListArr[index++] = "price in USD";
		}
		public void ShowCommands(){
			Console.WriteLine("Offices available:");
			Console.WriteLine("Sweden, Spain, USA\n");
            
            Console.WriteLine("Assets available:");
			Console.WriteLine("phone, laptop\n");

			Console.WriteLine("Commands:");
			Console.WriteLine("Create: create new asset | <type:string> | ex: create phone");
			Console.WriteLine("Read: List out dataBase");
			Console.WriteLine("Update: update asset | <id:int> <prop:string> | ex: update 2 brand");
			Console.WriteLine("Delete: delete asset | <id:int> | ex: delete 3");
			Console.WriteLine("Report: show stats of the data: like total price, avrage price, total offices etc...");
			Console.WriteLine("Help: shows this list");
			
			Console.WriteLine("");
		}
		public string GetUserInput(){
			//ShowCommands();
			Console.Write("$> ");
			string Input = Console.ReadLine();
			string TempInput = CheckInput(Input, CommandArr);
			return TempInput;
		}

		public string[] GetAssetFromUser(){
			string[] ReturnArr = new string[7];
			for (int i = 0; i < ReturnArr.Length; i++){
				Console.Write("Enter "+PropListArr[i]+": ");
				ReturnArr[i] = Console.ReadLine();
				if(ReturnArr[i] == ""){
					return null;
				}
			}
			return ReturnArr;
		}
		
		public string CheckInput(String Input, string[] arr){
            string[] InputArr = Input.Split(" ");
			foreach(string i in arr){
				if(i == InputArr[0].ToLower().Trim()){
					return Input;
				}
			}
			return null;
		}


    }

}
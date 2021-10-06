using System;
using System.Collections.Generic;

namespace AssetTrackerEF.input{

    public class HandelInput{
		private int index = 0;
		private string[] CommandArr = new string[5];
		private string[] PropListArr = new string[7];
		public HandelInput(){
			CommandArr[index++] = "q";
			CommandArr[index++] = "addphone";
			CommandArr[index++] = "addlaptop";
			CommandArr[index++] = "list";
			CommandArr[index++] = "help";
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
			Console.WriteLine("offices available:");
			Console.WriteLine("Sweden, Spain, USA\n");

			Console.WriteLine("Commands:");
			Console.WriteLine("AddPhone: adds Phone asset");
			Console.WriteLine("AddLaptop: adds Laptop asset");
			Console.WriteLine("Q: Quits the program");
			Console.WriteLine("List: lists out assets enterd");
			Console.WriteLine("Help: list out commands");
			
			Console.WriteLine("");
		}
		public string GetUserInput(){
			//ShowCommands();
			Console.Write(">> ");
			string Input = Console.ReadLine();
			string TempInput = CheckInput(Input);
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
		
		private string CheckInput(String Input){
			foreach(string i in CommandArr){
				if(i == Input.ToLower().Trim()){
					return i;
				}
			}
			return null;
		}


    }

}
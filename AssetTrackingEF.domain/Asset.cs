using System;

namespace AssetTrackingEF.domain{
    public class Asset{
        public int Id { get; set; }
        public string Brand { get; set; }
		public string Model { get; set; }
		public string OfficeLocation { get; set; }
		public DateTime PurchaseDate { get; set; }
		public double PriceInUSD { get; set; }
		public double LocalPrice { get; set; }
		public string Currency { get; set; }

    }
    
}
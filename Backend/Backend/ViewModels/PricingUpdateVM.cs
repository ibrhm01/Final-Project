using System;
namespace Backend.ViewModels
{
	public class PricingUpdateVM
	{
        public string PricingType { get; set; }
        public decimal Price { get; set; }
        public string VideoQuality { get; set; }
        public string Resolution { get; set; }
        public int AmountOfScreens { get; set; }
        public bool Cancel { get; set; }
    }
}


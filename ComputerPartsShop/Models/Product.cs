﻿namespace ComputerPartsShop.Models
{
	public class Product
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public int Stock { get; set; }
		public Category Category { get; set; }
	}
}

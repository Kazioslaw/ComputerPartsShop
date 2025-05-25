using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class TempData
	{
		internal List<Address> AddressList = new List<Address>()
		{
			new Address {ID = Guid.Parse("04d79c37-84d2-4942-9cfe-c685053e1064"),
				Street = "Kasprowicza Jana 68", City = "Olsztyn", Region = "Warmińsko-Mazurskie", ZipCode = "10-220", CountryID = 36},
			new Address {ID = Guid.Parse("0be7e499-958b-40e6-9350-19b04b3134fd"),
				Street = "Młynowa 50", City = "Mrągowo", Region = "Warmińsko-Mazurskie", ZipCode = "11-700", CountryID = 36},
			new Address {ID = Guid.Parse("6c24f13d-4d7e-43f6-ab5a-06129781a103"),
				Street = "Juliusza Słowackiego 4", City = "Tomaszow Mazowiecki", Region = "Łódzkie", ZipCode = "97-200", CountryID = 36},
			new Address {ID = Guid.Parse("5ed34104-b523-40c4-918d-c89865fc5eab"),
				Street = "1 Maja 67", City = "Zbytków", Region = "Śląskie", ZipCode = "43-246", CountryID = 36},
			new Address {ID = Guid.Parse("32ef774e-0e01-4cde-a74e-26e56343d280"),
				Street = "Złota 11", City = "Warszawa", Region = "Mazowieckie", ZipCode = "00-019", CountryID = 36},
			new Address {ID = Guid.Parse("2e0f3d85-706d-44d7-8754-33f85d2366de"),
				Street = "Cegłowska 38", City = "Warszawa", Region = "Mazowieckie", ZipCode = "01-803", CountryID = 36},
			new Address {ID = Guid.Parse("df73a4b3-31d2-457c-9f43-5b0f1d083987"),
				Street = "Mikołaja Kopernika 13", City = "Ostrowiec Świętokrzyski", Region = "Świętokrzyskie", ZipCode = "27-400", CountryID = 36},
			new Address {ID = Guid.Parse("6ef0c249-8736-4bfd-b7ca-8cc9e07e4cb5"),
				Street = "Równa 11", City = "Polańczyk", Region = "Podkarpackie", ZipCode = "38-610", CountryID = 36},
			new Address {ID = Guid.Parse("2aa1d3aa-cd03-494a-b740-11ad89d5bd87"),
				Street = "Szwoleżerów 46", City = "Lublin", Region = "Lubelskie", ZipCode = "20-550", CountryID = 36},
			new Address {ID = Guid.Parse("2527db26-9fe0-47ab-80ed-a4faa5322a6f"),
				Street = "Traugutta Romualda 14", City = "Kielce", Region = "Świętokrzyskie", ZipCode = "25-657", CountryID = 36},
			new Address {ID = Guid.Parse("ceaee620-2522-441b-a9ec-6f2d1d418f2e"),
				Street = "274 Lederambachtstraat", City = "Amsterdam", Region = "North Holland", ZipCode = "1069 HM", CountryID = 33},
			new Address {ID = Guid.Parse("74bd7005-0e50-472a-a383-569bb28620f3"),
				Street = "7 Tammepärja", City = "Tallinn", Region = "Harju", ZipCode = "10922", CountryID = 14},
			new Address {ID = Guid.Parse("f6a5c0da-c6a7-4ac1-b695-3a0c8b3b3940"),
				Street = "1 Camino de la Era", City = "Alhendín", Region = "Andalusia", ZipCode = "18620", CountryID = 44},
			new Address {ID = Guid.Parse("e93bc06b-09c1-41c7-a638-6c83b3b5306e"),
				Street = "6 Mykoly Lavrukhina Street", City = "Kyiv",  Region = "Kyiv", ZipCode = "02034", CountryID = 48},
			new Address {ID = Guid.Parse("d97b4d44-6f61-47b2-8e78-682964495bdb"),
				Street = "120 Paldiski mnt", City = "Tallinn", Region = "Harjumaa", ZipCode = "13518", CountryID = 14},
			new Address {ID = Guid.Parse("6377f69a-5702-4e85-83f5-c1a0bd305ade"),
				Street = "Maison relais Babbeltiermchen, 1 Rue Jean-Georges Willmar", City = "Luxembourg", Region = "Luxembourg", ZipCode = "2731", CountryID = 28 },
			new Address {ID = Guid.Parse("2f26633c-7009-40d2-abb3-9195c0461383"),
				Street = "1-5 Gutenberggasse", City = "Vienna", Region = "Vienna", ZipCode = "1070", CountryID = 4},
			new Address {ID = Guid.Parse("f278204d-f3ff-4cfe-9834-98c9f7ef0165"),
				Street = "120 Claus van Amsbergstraat", City = "Amsterdam", Region = "North Holland", ZipCode = "1102 AZ", CountryID = 33},
			new Address {ID = Guid.Parse("b7bee85d-1f37-4180-a5ef-2fdae70fe07f"),
				Street="35 Klosterweg", City="Wiesmoor", Region = "Lower Saxony", ZipCode = "26639", CountryID = 18},
			new Address {ID = Guid.Parse("f0665daf-3648-4b1e-b264-66875d1a036e"),
				Street = "43 Boulevard de Champigny", City = "Saint-Maur-des-Fossés", Region = "Île-de-France", ZipCode = "94210", CountryID = 16},
			new Address {ID = Guid.Parse("732bf33c-614f-47d5-97f2-184a72df6ac3"),
				Street = "24B Ostpreußendamm", City = "Berlin", Region = "Berlin", ZipCode = "12207", CountryID = 18},
			new Address {ID = Guid.Parse("a12602e6-679f-4c8b-9339-5aa88d723f15"),
				Street = "13 Kopli", City = "Tallinn", Region = "Harju", ZipCode = "10412", CountryID = 14},
			new Address {ID = Guid.Parse("02711efb-21ce-4ed7-89a1-61b7e4592625"),
				Street = "2bis Avenue de Vintimille", City = "Saint-Maur-des-Fossés", Region = "Île-de-France", ZipCode = "94100", CountryID = 16},
			new Address {ID = Guid.Parse("0cdff993-bef1-423a-8779-705ed0dc6516"),
				Street = "25 Holton Road", City = "East Lindsey", Region = "Lincolnshire", ZipCode = "DN36 5LS", CountryID = 49},
			new Address {ID = Guid.Parse("316c4302-947b-42a4-8995-5acc33f7e480"),
				Street = "25 Saint Ronan's Terrace", City = "City of Edinburgh", Region = "Scotland", ZipCode = "EH10 5NY", CountryID = 49},
			new Address {ID = Guid.Parse("3f8b2207-b73a-4ef1-bc85-33f9184171b6"),
				Street = "Rectory, N52", City = "Borrisokane", Region = "County Tipperary", ZipCode = "E45 P861", CountryID = 22},
			new Address {ID = Guid.Parse("a189ffb7-30d5-43e8-93a6-8cc7196368d0"),
				Street = "1632/5 Kanadská", City = "Prague", Region = "Prague", ZipCode = "160 00", CountryID = 12},
			new Address {ID = Guid.Parse("959f882b-ce8e-4a64-8e7b-cbd70b2a2be6"),
				Street = "Pańska 14", City = "Pruszków", Region = "Mazowieckie", ZipCode = "05-800", CountryID = 36},
			new Address {ID = Guid.Parse("7b6951c7-7e84-4adc-af82-745d64c3485a"),
				Street = "11 Hotwielstrasse", City = "Bezrik Meilen", Region = "Zurich", ZipCode = "8634", CountryID = 46},
			new Address {ID = Guid.Parse("cf877ff0-977f-4230-8a51-c4ef3922b1e3"),
				Street = "81 Via Sampiero di Bastelica", City = "Rome", Region = "Lazio", ZipCode = "00176", CountryID = 23}
		};

		internal List<Category> CategoryList = new List<Category>()
		{
			new Category { ID = 1, Name = "CPU", Description = "Central Processing Unit – the brain of the computer, executes instructions." },
			new Category { ID = 2, Name = "GPU", Description = "Graphics Processing Unit – handles rendering images, videos, and games." },
			new Category { ID = 3, Name = "Motherboard", Description = "The main circuit board connecting all components of a computer." },
			new Category { ID = 4, Name = "RAM", Description = "Random Access Memory – temporary memory used for active processes." },
			new Category { ID = 5, Name = "Storage", Description = "Devices like SSDs and HDDs for storing data and system files." },
			new Category { ID = 6, Name = "PSU", Description = "Power Supply Unit – provides electrical power to all PC components." },
			new Category { ID = 7, Name = "Computer Case", Description = "The enclosure that houses and protects all internal hardware." },
			new Category { ID = 8, Name = "Cooling System", Description = "Keeps components at optimal temperatures (air or liquid cooling)." },
			new Category { ID = 9, Name = "Sound Card", Description = "Enhances audio quality and processing." },
			new Category { ID = 10, Name = "Network Card", Description = "Enables wired or wireless network connectivity." },
			new Category { ID = 11, Name = "Optical Drive", Description = "Reads and writes CDs, DVDs, or Blu-ray discs." },
			new Category { ID = 12, Name = "Accessories & Mounting Components", Description = "Includes cables, adapters, screws, and other installation parts." },
			new Category { ID = 13, Name = "Monitors", Description = "Output displays for visual content." },
			new Category { ID = 14, Name = "Keyboards", Description = "Input device for typing and system control." },
			new Category { ID = 15, Name = "Mouses", Description = "Pointing device used to interact with the user interface." },
			new Category { ID = 16, Name = "Mouse Pads", Description = "Provides surface for smooth mouse movement and control." },
			new Category { ID = 17, Name = "Speakers", Description = "Audio output devices for playing sound." },
			new Category { ID = 18, Name = "Headphones", Description = "Personal audio output device worn on or in the ears." },
			new Category { ID = 19, Name = "Webcams", Description = "Captures video for conferencing or streaming." },
			new Category { ID = 20, Name = "PC Kits", Description = "Pre-packaged sets of components or barebones systems." },
			new Category { ID = 21, Name = "Servers & Workstations", Description = "High-performance systems for business or technical workloads." },
			new Category { ID = 22, Name = "Software", Description = "Operating systems, utilities, and productivity applications." },
			new Category { ID = 23, Name = "Tools & Service Accessories", Description = "Tools for assembling or maintaining PC hardware." }
		};

		internal List<Country> CountryList = new List<Country>()
		{
			new Country { ID = 1, Alpha2 = "AL", Alpha3 = "ALB", Name = "Albania" },
			new Country { ID = 2, Alpha2 = "AD", Alpha3 = "AND", Name = "Andorra" },
			new Country { ID = 3, Alpha2 = "AM", Alpha3 = "ARM", Name = "Armenia" },
			new Country { ID = 4, Alpha2 = "AT", Alpha3 = "AUT", Name = "Austria" },
			new Country { ID = 5, Alpha2 = "AZ", Alpha3 = "AZE", Name = "Azerbaijan" },
			new Country { ID = 6, Alpha2 = "BY", Alpha3 = "BLR", Name = "Belarus" },
			new Country { ID = 7, Alpha2 = "BE", Alpha3 = "BEL", Name = "Belgium" },
			new Country { ID = 8, Alpha2 = "BA", Alpha3 = "BIH", Name = "Bosnia and Herzegovina" },
			new Country { ID = 9, Alpha2 = "BG", Alpha3 = "BGR", Name = "Bulagria" },
			new Country { ID = 10, Alpha2 = "HR", Alpha3 = "HRV", Name = "Croatia" },
			new Country { ID = 11, Alpha2 = "CY", Alpha3 = "CYP", Name = "Cyprus" },
			new Country { ID = 12, Alpha2 = "CZ", Alpha3 = "CZE", Name = "Czechia" },
			new Country { ID = 13, Alpha2 = "DK", Alpha3 = "DNK", Name = "Denmark" },
			new Country { ID = 14, Alpha2 = "EE", Alpha3 = "EST", Name = "Estonia" },
			new Country { ID = 15, Alpha2 = "FI", Alpha3 = "FIN", Name = "Finland" },
			new Country { ID = 16, Alpha2 = "FR", Alpha3 = "FRA", Name = "France" },
			new Country { ID = 17, Alpha2 = "GE", Alpha3 = "GEO", Name = "Georgia" },
			new Country { ID = 18, Alpha2 = "DE", Alpha3 = "DEU", Name = "Germany" },
			new Country { ID = 19, Alpha2 = "GR", Alpha3 = "GRC", Name = "Greece" },
			new Country { ID = 20, Alpha2 = "HU", Alpha3 = "HUN", Name = "Hungary" },
			new Country { ID = 21, Alpha2 = "IS", Alpha3 = "ISL", Name = "Iceland" },
			new Country { ID = 22, Alpha2 = "IE", Alpha3 = "IRL", Name = "Irland" },
			new Country { ID = 23, Alpha2 = "IT", Alpha3 = "ITA", Name = "Italy" },
			new Country { ID = 24, Alpha2 = "KZ", Alpha3 = "KAZ", Name = "Kazakhstan" },
			new Country { ID = 25, Alpha2 = "LV", Alpha3 = "LVA", Name = "Latvia" },
			new Country { ID = 26, Alpha2 = "LI", Alpha3 = "LIE", Name = "Liechtenstein" },
			new Country { ID = 27, Alpha2 = "LT", Alpha3 = "LTU", Name = "Lithuania" },
			new Country { ID = 28, Alpha2 = "LU", Alpha3 = "LUX", Name = "Luxembourg" },
			new Country { ID = 29, Alpha2 = "MT", Alpha3 = "MLT", Name = "Malta" },
			new Country { ID = 30, Alpha2 = "MD", Alpha3 = "MDA", Name = "Moldovia" },
			new Country { ID = 31, Alpha2 = "MC", Alpha3 = "MCO", Name = "Monaco" },
			new Country { ID = 32, Alpha2 = "ME", Alpha3 = "MNE", Name = "Montenegro" },
			new Country { ID = 33, Alpha2 = "NL", Alpha3 = "NLD", Name = "Netherlands" },
			new Country { ID = 34, Alpha2 = "MK", Alpha3 = "MKD", Name = "North Macedonia" },
			new Country { ID = 35, Alpha2 = "NO", Alpha3 = "NOR", Name = "Norway" },
			new Country { ID = 36, Alpha2 = "PL", Alpha3 = "POL", Name = "Poland" },
			new Country { ID = 37, Alpha2 = "PT", Alpha3 = "PRT", Name = "Portugal" },
			new Country { ID = 38, Alpha2 = "RO", Alpha3 = "ROU", Name = "Romania" },
			new Country { ID = 39, Alpha2 = "RU", Alpha3 = "RUS", Name = "Russia" },
			new Country { ID = 40, Alpha2 = "SM", Alpha3 = "SMR", Name = "San Marino" },
			new Country { ID = 41, Alpha2 = "RS", Alpha3 = "SRB", Name = "Serbia" },
			new Country { ID = 42, Alpha2 = "SK", Alpha3 = "SVK", Name = "Slovakia" },
			new Country { ID = 43, Alpha2 = "SI", Alpha3 = "SVN", Name = "Slovenia" },
			new Country { ID = 44, Alpha2 = "ES", Alpha3 = "ESP", Name = "Spain" },
			new Country { ID = 45, Alpha2 = "SE", Alpha3 = "SWE", Name = "Sweden" },
			new Country { ID = 46, Alpha2 = "CH", Alpha3 = "CHE", Name = "Switzerland" },
			new Country { ID = 47, Alpha2 = "TR", Alpha3 = "TUR", Name = "Turkey" },
			new Country { ID = 48, Alpha2 = "UA", Alpha3 = "UKR", Name = "Ukraine" },
			new Country { ID = 49, Alpha2 = "GB", Alpha3 = "GBR", Name = "United Kingdom" },
			new Country { ID = 50, Alpha2 = "VA", Alpha3 = "VAT", Name = "Vatican" }
		};

		internal List<Customer> CustomerList = new List<Customer>();

		internal List<CustomerAddress> CustomerAddressList = new List<CustomerAddress>();

		internal List<CustomerPaymentSystem> CustomerPaymentSystemList = new List<CustomerPaymentSystem>();

		internal List<Order> OrderList = new List<Order>();

		internal List<OrderProduct> OrderProductList = new List<OrderProduct>();

		internal List<Payment> PaymentList = new List<Payment>();

		internal List<PaymentProvider> PaymentProviderList = new List<PaymentProvider>();

		internal List<Product> ProductList = new List<Product>();

		internal List<Review> ReviewList = new List<Review>();
	}
}

using AutoMapper;
using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.API.Mappings
{
	public class Profiles : Profile
	{
		public Profiles()
		{
			CreateMap<Address, AddressResponse>().ConstructUsing(a => new AddressResponse(a.Id, a.Street, a.City, a.Region, a.ZipCode, a.Country.Alpha3));
			CreateMap<Address, DetailedAddressResponse>().ConstructUsing(a =>
			new DetailedAddressResponse(a.Id, a.Street, a.City, a.Region, a.ZipCode, a.Country.Alpha3,
			a.Users.Where(ca => ca.User != null)
			.Select(c => new UserInAddressResponse(c.User.FirstName, c.User.LastName, c.User.Username, c.User.Email)).ToList()));
			CreateMap<AddressRequest, Address>();
			CreateMap<UpdateAddressRequest, Address>()
				.ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.newStreet))
				.ForMember(dest => dest.City, opt => opt.MapFrom(src => src.newCity))
				.ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.newRegion))
				.ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.newZipCode));
			CreateMap<UserAddress, UserInAddressResponse>().ConstructUsing(ca =>
			new UserInAddressResponse(ca.User.FirstName, ca.User.LastName, ca.User.Username, ca.User.Email));

			CreateMap<Category, CategoryResponse>().ConvertUsing(c => new CategoryResponse(c.Id, c.Name, c.Description));
			CreateMap<CategoryRequest, Category>();

			CreateMap<Country, CountryResponse>().ConstructUsing(c => new CountryResponse(c.Id, c.Alpha2, c.Alpha3, c.Name));
			CreateMap<Country, DetailedCountryResponse>().ConstructUsing(c => new DetailedCountryResponse(c.Id, c.Alpha2, c.Alpha3, c.Name,
				c.Addresses.Select(a => new AddressInCountryResponse(a.Id, a.Street, a.City, a.Region, a.ZipCode)).ToList()));
			CreateMap<CountryRequest, Country>();
			CreateMap<Address, AddressInCountryResponse>().ConstructUsing(a => new AddressInCountryResponse(a.Id, a.Street, a.City, a.Region, a.ZipCode));

			CreateMap<UserPaymentSystem, UserPaymentSystemResponse>()
				.ConstructUsing(cps => new UserPaymentSystemResponse(cps.Id, cps.User.Username, cps.User.Email, cps.Provider.Name, cps.PaymentReference));
			CreateMap<UserPaymentSystem, DetailedUserPaymentSystemResponse>()
				.ConstructUsing(cps => new DetailedUserPaymentSystemResponse(cps.Id, cps.User.Username, cps.User.Email, cps.Provider.Name, cps.PaymentReference,
				cps.Payments.Select(p => new PaymentInUserPaymentSystemResponse(p.Id, p.OrderId, p.Total, p.Method, p.Status, p.PaymentStartAt, p.PaidAt)).ToList()));
			CreateMap<UserPaymentSystemRequest, UserPaymentSystem>();
			CreateMap<Payment, PaymentInUserPaymentSystemResponse>().ConstructUsing(p => new PaymentInUserPaymentSystemResponse(p.Id, p.OrderId, p.Total, p.Method, p.Status, p.PaymentStartAt, p.PaidAt));

			CreateMap<ShopUser, ShopUserResponse>().ConstructUsing(c => new ShopUserResponse(c.Id, c.FirstName, c.LastName, c.Username, c.Email, c.PhoneNumber));
			CreateMap<ShopUser, DetailedShopUserResponse>().ConstructUsing(c => new DetailedShopUserResponse(c.Id, c.FirstName, c.LastName, c.Username, c.Email, c.PhoneNumber,
				c.UserAddresses.Where(x => x.Address != null).Select(ca => new AddressResponse(ca.Address.Id, ca.Address.Street, ca.Address.City, ca.Address.Region, ca.Address.ZipCode, ca.Address.Country.Alpha3)).ToList(),
				c.PaymentInfoList.Where(x => x.Payments != null).Select(cps => new PaymentInfoInShopUserResponse(cps.Id, cps.Provider.Name, cps.PaymentReference)).ToList(),
				c.Reviews!.Where(x => x.Product != null).Select(r => new ReviewInShopUserResponse(r.Id, r.Product.Name, r.Rating, r.Description)).ToList()));
			CreateMap<ShopUser, ShopUserWithAddressResponse>().ConstructUsing(c => new ShopUserWithAddressResponse(c.Id, c.FirstName, c.LastName, c.Username, c.Email, c.PhoneNumber,
				c.UserAddresses.Where(x => x.Address != null).Select(ca => new AddressResponse(ca.Address.Id, ca.Address.Street, ca.Address.City, ca.Address.Region, ca.Address.ZipCode, ca.Address.Country.Alpha3)).ToList()));
			CreateMap<ShopUserRequest, ShopUser>();
			CreateMap<UserPaymentSystem, PaymentInfoInShopUserResponse>().ConstructUsing(cps => new PaymentInfoInShopUserResponse(cps.Id, cps.Provider.Name, cps.PaymentReference));
			CreateMap<Review, ReviewInShopUserResponse>().ConstructUsing(r => new ReviewInShopUserResponse(r.Id, r.Product.Name, r.Rating, r.Description));
			CreateMap<UserAddress, AddressResponse>().ConstructUsing(ca => new AddressResponse(ca.Address.Id, ca.Address.Street, ca.Address.City, ca.Address.Region, ca.Address.ZipCode, ca.Address.Country.Alpha3));

			CreateMap<Order, OrderResponse>().ConstructUsing(o => new OrderResponse(o.Id, o.UserId,
				o.OrdersProducts.Where(op => op.Product != null).Select(p => new ProductInOrderResponse(p.ProductId, p.Product.Name, p.Product.UnitPrice, p.Quantity)).ToList(),
				o.Total, o.DeliveryAddressId, o.Status, o.OrderedAt, o.SendAt, o.Payments.Select(p => p.Id).ToList()));
			CreateMap<OrderRequest, Order>();
			CreateMap<UpdateOrderRequest, Order>();
			CreateMap<OrderProduct, ProductInOrderResponse>().ConstructUsing(p => new ProductInOrderResponse(p.ProductId, p.Product.Name, p.Product.UnitPrice, p.Quantity));

			CreateMap<PaymentProvider, PaymentProviderResponse>().ConstructUsing(pp => new PaymentProviderResponse(pp.Id, pp.Name, pp.UserPayments.Select(cps => cps.Id).ToList()));
			CreateMap<PaymentProvider, DetailedPaymentProviderResponse>().ConstructUsing(pp => new DetailedPaymentProviderResponse(pp.Id, pp.Name,
				pp.UserPayments.Select(cps => new UserPaymentSystemResponse(cps.Id, cps.User.Username, cps.User.Email, cps.Provider.Name, cps.PaymentReference)).ToList()));
			CreateMap<PaymentProviderRequest, PaymentProvider>();

			CreateMap<Payment, PaymentResponse>().ConstructUsing(p => new PaymentResponse(p.Id, p.UserPaymentSystemId, p.OrderId, p.Total, p.Method, p.Status, p.PaymentStartAt, p.PaidAt));
			CreateMap<PaymentRequest, Payment>();
			CreateMap<UpdatePaymentRequest, Payment>();

			CreateMap<Product, ProductResponse>().ConstructUsing(p => new ProductResponse(p.Id, p.Name, p.Description, p.UnitPrice, p.Stock, p.Category.Name, p.InternalCode));
			CreateMap<ProductRequest, Product>();

			CreateMap<Review, ReviewResponse>().ConstructUsing(r => new ReviewResponse(r.Id, r.User != null ? r.User.Username : null, r.Product.Name, r.Rating, r.Description == null ? r.Description : null));
			CreateMap<ReviewRequest, Review>();

		}
	}
}

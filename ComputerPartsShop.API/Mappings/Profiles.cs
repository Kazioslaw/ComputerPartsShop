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
			a.Customers.Where(ca => ca.Customer != null)
			.Select(c => new CustomerInAddressResponse(c.Customer.FirstName, c.Customer.LastName, c.Customer.Username, c.Customer.Email)).ToList()));
			CreateMap<AddressRequest, Address>();
			CreateMap<UpdateAddressRequest, Address>();
			CreateMap<CustomerAddress, CustomerInAddressResponse>().ConstructUsing(ca =>
			new CustomerInAddressResponse(ca.Customer.FirstName, ca.Customer.LastName, ca.Customer.Username, ca.Customer.Email));

			CreateMap<Category, CategoryResponse>().ConvertUsing(c => new CategoryResponse(c.Id, c.Name, c.Description));
			CreateMap<CategoryRequest, Category>();

			CreateMap<Country, CountryResponse>().ConstructUsing(c => new CountryResponse(c.Id, c.Alpha2, c.Alpha3, c.Name));
			CreateMap<Country, DetailedCountryResponse>().ConstructUsing(c => new DetailedCountryResponse(c.Id, c.Alpha2, c.Alpha3, c.Name,
				c.Addresses.Select(a => new AddressInCountryResponse(a.Id, a.Street, a.City, a.Region, a.ZipCode)).ToList()));
			CreateMap<CountryRequest, Country>();
			CreateMap<Address, AddressInCountryResponse>().ConstructUsing(a => new AddressInCountryResponse(a.Id, a.Street, a.City, a.Region, a.ZipCode));

			CreateMap<CustomerPaymentSystem, CustomerPaymentSystemResponse>()
				.ConstructUsing(cps => new CustomerPaymentSystemResponse(cps.Id, cps.Customer.Username, cps.Customer.Email, cps.Provider.Name, cps.PaymentReference));
			CreateMap<CustomerPaymentSystem, DetailedCustomerPaymentSystemResponse>()
				.ConstructUsing(cps => new DetailedCustomerPaymentSystemResponse(cps.Id, cps.Customer.Username, cps.Customer.Email, cps.Provider.Name, cps.PaymentReference,
				cps.Payments.Select(p => new PaymentInCustomerPaymentSystemResponse(p.Id, p.OrderId, p.Total, p.Method, p.Status, p.PaymentStartAt, p.PaidAt)).ToList()));
			CreateMap<CustomerPaymentSystemRequest, CustomerPaymentSystem>();
			CreateMap<Payment, PaymentInCustomerPaymentSystemResponse>().ConstructUsing(p => new PaymentInCustomerPaymentSystemResponse(p.Id, p.OrderId, p.Total, p.Method, p.Status, p.PaymentStartAt, p.PaidAt));

			CreateMap<Customer, CustomerResponse>().ConstructUsing(c => new CustomerResponse(c.Id, c.FirstName, c.LastName, c.Username, c.Email, c.PhoneNumber));
			CreateMap<Customer, DetailedCustomerResponse>().ConstructUsing(c => new DetailedCustomerResponse(c.Id, c.FirstName, c.LastName, c.Username, c.Email, c.PhoneNumber,
				c.CustomersAddresses.Where(x => x.Address != null).Select(ca => new AddressResponse(ca.Address.Id, ca.Address.Street, ca.Address.City, ca.Address.Region, ca.Address.ZipCode, ca.Address.Country.Alpha3)).ToList(),
				c.PaymentInfoList.Where(x => x.Payments != null).Select(cps => new PaymentInfoInCustomerResponse(cps.Id, cps.Provider.Name, cps.PaymentReference)).ToList(),
				c.Reviews!.Where(x => x.Product != null).Select(r => new ReviewInCustomerResponse(r.Id, r.Product.Name, r.Rating, r.Description)).ToList()));
			CreateMap<Customer, CustomerWithAddressResponse>().ConstructUsing(c => new CustomerWithAddressResponse(c.Id, c.FirstName, c.LastName, c.Username, c.Email, c.PhoneNumber,
				c.CustomersAddresses.Where(x => x.Address != null).Select(ca => new AddressResponse(ca.Address.Id, ca.Address.Street, ca.Address.City, ca.Address.Region, ca.Address.ZipCode, ca.Address.Country.Alpha3)).ToList()));
			CreateMap<CustomerRequest, Customer>();
			CreateMap<CustomerPaymentSystem, PaymentInfoInCustomerResponse>().ConstructUsing(cps => new PaymentInfoInCustomerResponse(cps.Id, cps.Provider.Name, cps.PaymentReference));
			CreateMap<Review, ReviewInCustomerResponse>().ConstructUsing(r => new ReviewInCustomerResponse(r.Id, r.Product.Name, r.Rating, r.Description));
			CreateMap<CustomerAddress, AddressResponse>().ConstructUsing(ca => new AddressResponse(ca.Address.Id, ca.Address.Street, ca.Address.City, ca.Address.Region, ca.Address.ZipCode, ca.Address.Country.Alpha3));

			CreateMap<Order, OrderResponse>().ConstructUsing(o => new OrderResponse(o.Id, o.CustomerId,
				o.OrdersProducts.Where(op => op.Product != null).Select(p => new ProductInOrderResponse(p.ProductId, p.Product.Name, p.Product.UnitPrice, p.Quantity)).ToList(),
				o.Total, o.DeliveryAddressId, o.Status, o.OrderedAt, o.SendAt, o.Payments.Select(p => p.Id).ToList()));
			CreateMap<OrderRequest, Order>();
			CreateMap<UpdateOrderRequest, Order>();
			CreateMap<OrderProduct, ProductInOrderResponse>().ConstructUsing(p => new ProductInOrderResponse(p.ProductId, p.Product.Name, p.Product.UnitPrice, p.Quantity));

			CreateMap<PaymentProvider, PaymentProviderResponse>().ConstructUsing(pp => new PaymentProviderResponse(pp.Id, pp.Name, pp.CustomerPayments.Select(cps => cps.Id).ToList()));
			CreateMap<PaymentProvider, DetailedPaymentProviderResponse>().ConstructUsing(pp => new DetailedPaymentProviderResponse(pp.Id, pp.Name,
				pp.CustomerPayments.Select(cps => new CustomerPaymentSystemResponse(cps.Id, cps.Customer.Username, cps.Customer.Email, cps.Provider.Name, cps.PaymentReference)).ToList()));
			CreateMap<PaymentProviderRequest, PaymentProvider>();

			CreateMap<Payment, PaymentResponse>().ConstructUsing(p => new PaymentResponse(p.Id, p.CustomerPaymentSystemId, p.OrderId, p.Total, p.Method, p.Status, p.PaymentStartAt, p.PaidAt));
			CreateMap<PaymentRequest, Payment>();
			CreateMap<UpdatePaymentRequest, Payment>();

			CreateMap<Product, ProductResponse>().ConstructUsing(p => new ProductResponse(p.Id, p.Name, p.Description, p.UnitPrice, p.Stock, p.Category.Name, p.InternalCode));
			CreateMap<ProductRequest, Product>();

			CreateMap<Review, ReviewResponse>().ConstructUsing(r => new ReviewResponse(r.Id, r.Customer != null ? r.Customer.Username : null, r.Product.Name, r.Rating, r.Description == null ? r.Description : null));
			CreateMap<ReviewRequest, Review>();

		}
	}
}

using AutoMapper;

namespace ShopApi.Helpers.Interfaces
{
	public interface IPageData<T>
	{
		public uint PageSize { get; }
		public uint TotalAmount { get; }
		public uint PagesAmount { get; }
		public uint CurrentPage { get; }
		public IEnumerable<T> Data { get; }
		public bool HasPrevious { get; }
		public bool HasNext { get; }

		public IPageData<U?> Map<U>(IMapper mapper);
	}
}

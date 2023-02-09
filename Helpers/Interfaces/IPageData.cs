using AutoMapper;

namespace ShopApi.Helpers.Interfaces
{
	public interface IPageData<T>
	{
		public int PageSize { get; }
		public int TotalAmount { get; }
		public int PagesAmount { get; }
		public int CurrentPage { get; }
		public IEnumerable<T> Data { get; set; }
		public bool HasPrevious { get; }
		public bool HasNext { get; }

		public IPageData<U?> Map<U>(IMapper mapper);
	}
}

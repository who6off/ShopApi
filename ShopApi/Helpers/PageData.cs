using AutoMapper;
using ShopApi.Helpers.Interfaces;

namespace ShopApi.Helpers
{
	public class PageData<T> : IPageData<T>
	{
		public int PageSize { get; private set; }
		public int TotalAmount { get; private set; }
		public int PagesAmount { get; private set; }
		public int CurrentPage { get; private set; }
		public IEnumerable<T> Data { get; set; }

		public bool HasPrevious => (CurrentPage != 0) && (CurrentPage < PagesAmount);
		public bool HasNext => (CurrentPage != (PagesAmount - 1)) && (CurrentPage < PagesAmount);


		public PageData(IEnumerable<T> source, int currentPage, int pageSize, int totalAmount)
		{
			CurrentPage = currentPage;
			PageSize = pageSize;
			TotalAmount = totalAmount;
			Data = source;
			PagesAmount = (int)Math.Ceiling(totalAmount / (double)pageSize);
		}


		public IPageData<U?> Map<U>(IMapper mapper)
		{
			var source = mapper.Map<IEnumerable<U>>(Data);
			return new PageData<U?>(source, CurrentPage, PageSize, TotalAmount);
		}
	}
}

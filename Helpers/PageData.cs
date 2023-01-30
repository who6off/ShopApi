using AutoMapper;
using ShopApi.Helpers.Interfaces;

namespace ShopApi.Helpers
{
	public class PageData<T> : IPageData<T>
	{
		public uint PageSize { get; private set; }
		public uint TotalAmount { get; private set; }
		public uint PagesAmount { get; private set; }
		public uint CurrentPage { get; private set; }
		public IEnumerable<T> Data { get; private set; }

		public bool HasPrevious => (CurrentPage != 0) && (CurrentPage < PagesAmount);
		public bool HasNext => (CurrentPage != (PagesAmount - 1)) && (CurrentPage < PagesAmount);


		public PageData(IEnumerable<T> source, uint currentPage, uint pageSize, uint totalAmount)
		{
			CurrentPage = currentPage;
			PageSize = pageSize;
			TotalAmount = totalAmount;
			Data = source;
			PagesAmount = (uint)Math.Ceiling(totalAmount / (double)pageSize);
		}


		public IPageData<U?> Map<U>(IMapper mapper)
		{
			var source = mapper.Map<IEnumerable<U>>(Data);
			return new PageData<U?>(source, CurrentPage, PageSize, TotalAmount);
		}
	}
}

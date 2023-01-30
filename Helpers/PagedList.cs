using ShopApi.Helpers.Interfaces;

namespace ShopApi.Helpers
{
	public class PagedList<T> : List<T>, IPagedList
	{
		public uint PageSize { get; private set; }
		public uint TotalAmount { get; private set; }
		public uint PagesAmount { get; private set; }
		public uint CurrentPage { get; private set; }


		public bool HasPrevious => CurrentPage != 0;
		public bool HasNext => (CurrentPage != (PagesAmount - 1)) && (CurrentPage != PagesAmount);


		public PagedList(IEnumerable<T> source, uint currentPage, uint pageSize, uint totalAmount)
		{
			CurrentPage = currentPage;
			PageSize = pageSize;
			TotalAmount = totalAmount;
			PagesAmount = (uint)Math.Ceiling(totalAmount / (double)pageSize);

			AddRange(source);
		}
	}
}

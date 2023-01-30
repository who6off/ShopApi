namespace ShopApi.Helpers.Interfaces
{
	public interface IPagedList
	{
		public uint PageSize { get; }
		public uint TotalAmount { get; }
		public uint PagesAmount { get; }
		public uint CurrentPage { get; }
		public bool HasPrevious { get; }
		public bool HasNext { get; }
	}
}

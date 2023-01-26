namespace ShopApi.Data.Models.SearchParameters
{
	public abstract class ASearchParameters
	{
		private const uint MAX_PAGE_SIZE = 3;

		private uint _pageSize = MAX_PAGE_SIZE;


		public uint Page { get; set; } = 0;


		public uint PageSize
		{
			get => _pageSize;
			set
			{
				if (value > MAX_PAGE_SIZE)
				{
					_pageSize = MAX_PAGE_SIZE;
					return;
				}

				_pageSize = value;
			}
		}

		public int GetSkip()
		{
			return (int)(Page * PageSize);
		}
	}
}

namespace ShopApi.Data.Models.SearchParameters
{
	public abstract class ASearchParameters
	{
		private const int MAX_PAGE_SIZE = 50;
		private const int DEFAULT_PAGE_SIZE = 3;

		private int _pageSize = DEFAULT_PAGE_SIZE;
		private int _page = 0;


		public int Page
		{
			get => _page;
			set => _page = (value < 0) ? 0 : value;
		}


		public int PageSize
		{
			get => _pageSize;
			set
			{
				if (value > MAX_PAGE_SIZE)
				{
					_pageSize = MAX_PAGE_SIZE;
					return;
				}

				if (value < 1)
				{
					_pageSize = DEFAULT_PAGE_SIZE;
					return;
				}

				_pageSize = value;
			}
		}

		public int GetSkip()
		{
			return Page * PageSize;
		}
	}
}

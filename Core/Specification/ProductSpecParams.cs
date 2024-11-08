namespace Core.Specification
{
    public class ProductSpecParams
    {
        private const int MaxPageSize = 50;
        public int PageIndex { get; set; } = 1;
        private int _pageSize = 6;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }


        private List<string> _brands = [];
        public List<string> Brands
        {
            get => _brands; //types=borads,gloves
            set {
                _brands = value.SelectMany(x => x.Split(",", 
                StringSplitOptions.RemoveEmptyEntries)).ToList();
            }             
        }
        
        private List<string> _types = [];
        public List<string> Types
        {
            get => _types; //types=borads,gloves
            set {
                _types = value.SelectMany(x => x.Split(",", 
                StringSplitOptions.RemoveEmptyEntries)).ToList();
            }             
        }

        public string? Sorts { get; set; }

        //Search products
        private string?  _search;

        public string  Search
        {
            get => _search ?? ""; //we search or it going to be empty string
            set => _search = value.ToLower();
                 
            
        }

    }
}
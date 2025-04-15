

namespace SalesForceFunctionApp.Models
{
	public class Account
	{
		public string Name { get; set; }
		public string Id { get; set; }
		public Attributes Attributes { get; set; }
	}

    public class  Attributes
    {
        public string Type { get; set; }
		public string Url { get; set; }
    }
}

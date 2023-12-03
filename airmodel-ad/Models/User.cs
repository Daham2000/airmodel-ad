using System.ComponentModel.DataAnnotations;

namespace airmodel_ad.Models
{
    public class User
    {
		[Key]
		public Guid userId { get; set; }
        public string userName { get; set; }
        public string userEmail { get; set; }
        public string userPassword { get; set; }
        public string userRole { get; set; }
	}
}
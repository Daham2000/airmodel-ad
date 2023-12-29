using System.ComponentModel.DataAnnotations;

namespace airmodel_ad.Models
{
    public class User
    {
        [Key]
        private Guid _userId;

        public Guid userId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        private string _userName;

        public string userName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        private string _userEmail;

        public string userEmail
        {
            get { return _userEmail; }
            set { _userEmail = value; }
        }

        private string _userPassword;

        public string userPassword
        {
            get { return _userPassword; }
            set { _userPassword = value; }
        }

        private string _userRole;

        public string userRole
        {
            get { return _userRole; }
            set { _userRole = value; }
        }
	}
}
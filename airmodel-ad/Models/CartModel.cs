using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace airmodel_ad.Models
{
    public class CartModel
    {
        private Guid _cartId;
        private User _users;
        private Guid _userId;

        [Key]
        public Guid cartId
        {
            get { return _cartId; }
            set { _cartId = value; }
        }

        public User users
        {
            get { return _users; }
            set { _users = value; }
        }

        [ForeignKey("users")]
        public Guid userId
        {
            get { return _userId; }
            set { _userId = value; }
        }
    }
}

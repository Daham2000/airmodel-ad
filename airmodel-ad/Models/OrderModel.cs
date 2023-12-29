using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace airmodel_ad.Models
{
    public class OrderModel
    {
        private Guid _oId;
        private User _users;
        private Guid _userId;
        private int _total;
        private string _fName;
        private string _lName;
        private string _county;
        private string _address;
        private string _city;
        private string _postCode;
        private string _phoneNumber;
        private string _orderStatus;
        private string _orderNote;
        private List<OrderItem>? _orderItems;
        private DateTime _orderTime;

        [Key]
        public Guid oId
        {
            get { return _oId; }
            set { _oId = value; }
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

        public int total
        {
            get { return _total; }
            set { _total = value; }
        }

        public string fName
        {
            get { return _fName; }
            set { _fName = value; }
        }

        public string lName
        {
            get { return _lName; }
            set { _lName = value; }
        }

        public string county
        {
            get { return _county; }
            set { _county = value; }
        }

        public string address
        {
            get { return _address; }
            set { _address = value; }
        }

        public string city
        {
            get { return _city; }
            set { _city = value; }
        }

        public string postCode
        {
            get { return _postCode; }
            set { _postCode = value; }
        }

        public string phoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; }
        }

        public string orderStatus
        {
            get { return _orderStatus; }
            set { _orderStatus = value; }
        }

        public string orderNote
        {
            get { return _orderNote; }
            set { _orderNote = value; }
        }

        [NotMapped]
        public List<OrderItem>? orderItems
        {
            get { return _orderItems; }
            set { _orderItems = value; }
        }

        public DateTime orderTime
        {
            get { return _orderTime; }
            set { _orderTime = value; }
        }
    }
}

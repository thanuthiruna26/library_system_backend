
namespace library_sesterm.Models
{
    public class ReturnedBook
    {
        public int Id { get; set; }
        public int BookId { get; set; } // Foreign key to Book
        public int Nic { get; set; } // NIC of the User (Member)
        public DateTime ReturnDate { get; set; }
        public string Status { get; set; } // E.g., Returned
    }
}

namespace library_sesterm.Models
{
    public class BorrowRequest
    {
        public int Id { get; set; }
        public int BookId { get; set; } // Foreign key to Book
        public int Nic { get; set; } // NIC of the User (Member)
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; } // Nullable, since the book might not be returned yet
        public string Status { get; set; } // E.g., Pending, Approved, Returned, Rejected
    }
}

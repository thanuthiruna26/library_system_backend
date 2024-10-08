namespace library_sesterm.DTOs
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public int Nic { get; set; } // NIC as int
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Role { get; set; } // Role: Librarian or Member
    }
}

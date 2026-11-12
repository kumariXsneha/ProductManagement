namespace ProductManagement.API.Dtos
{
    // OUTPUT: what we send back to the client
    public class SupplierDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ContactPerson { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string City { get; set; } = string.Empty;
        public int Rating { get; set; }
        public bool IsActive { get; set; }
        // ❌ No CreatedDate: the client doesn't need it
    }
}
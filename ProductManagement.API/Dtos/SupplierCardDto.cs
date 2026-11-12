namespace ProductManagement.API.Dtos
{
    public class SupplierCardDto
    {
        public int Id { get; set; }                                // same name → automatic
        public string SupplierName { get; set; } = string.Empty;  // from Name
        public string Location { get; set; } = string.Empty;      // from City
        public string Stars { get; set; } = string.Empty;         // from Rating → "★★★★"
        public string Contact { get; set; } = string.Empty;       // from ContactPerson (or "Not available")
    }
}
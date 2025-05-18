namespace AuthenticationAuthorization.Entities
{
    public class User : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string FistName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHashed { get; set; } = string.Empty;
        public string RefreshToken {  get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryTime {  get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set;}
    }
}

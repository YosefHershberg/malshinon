
namespace Models
{
    public class Person
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? SecretCode { get; set; }
        public DateTime CreatedAt { get; set; }

        public Person(int id, string? fullName, string? secretCode, DateTime createdAt)
        {
            Id = id;
            FullName = fullName;
            SecretCode = secretCode;
            CreatedAt = createdAt;
        }
    }
}
namespace UcabGo.App.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string CompleteName { get => $"{Name} {LastName}"; }
        public string Phone { get; set; }
        public float WalkingDistance { get; set; }
        public string ProfilePicture { get; set; }
        public string Initial { get => Name[0].ToString().ToUpper(); }
    }
}

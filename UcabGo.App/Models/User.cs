namespace UcabGo.App.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public string CompleteName { get => $"{Name} {LastName}"; }
        public string Phone { get; set; }
        public float WalkingDistance { get; set; }
    }
}

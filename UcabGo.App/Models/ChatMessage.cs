namespace UcabGo.App.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public int Ride { get; set; }
        public int User { get; set; }
        public string Content { get; set; }
        public DateTime TimeSent { get; set; }
        public string Username { get; set; }
        public bool IsMine { get; set; }
        public bool IsNotMine { get => !IsMine; }
    }
}

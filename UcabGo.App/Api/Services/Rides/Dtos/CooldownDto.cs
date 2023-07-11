namespace UcabGo.App.Api.Services.Rides.Dtos
{
    public class CooldownDto
    {
        public bool IsInCooldown { get; set; }
        public TimeSpan Cooldown { get; set; }
    }
}

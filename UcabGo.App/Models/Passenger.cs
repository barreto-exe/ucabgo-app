using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UcabGo.App.Api.Models;

namespace UcabGo.App.Models
{
    public class Passenger
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Initial { get => User?.Name[0].ToString().ToUpper(); }
        public DateTime TimeSolicited { get; set; }
        public DateTime? TimeAccepted { get; set; }
        public DateTime? TimeIgnored { get; set; }
        public DateTime? TimeCancelled { get; set; }
        public DateTime? TimeFinished { get; set; }
    }
}

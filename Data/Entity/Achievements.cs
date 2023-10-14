using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Callories_Tracker.Data.Entity
{
    public class Achievements
    {
        public int Id { get; set; }
        public string? AccountId { get; set; }
        public string? StartOfALongJourney { get; set; }
        public string? FirstTarget { get; set; }
        public string? StreakOfThree { get; set; }
        public string? StreakOfFive { get; set; }
        public string? StreakOfTen { get; set; }
        public string? CompleteYourProfile { get; set; }
        public string? LoverOfMotivation { get; set; }
        public string? DoublePortion { get; set; }
        public string? TriplePortion { get; set; }
        public string? AccuracyToTheMillimeter { get; set; }
        public string? LeaveMeAlone { get; set; }
        public string? Epilepsy { get; set; }
    }
}

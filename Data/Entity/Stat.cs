using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Callories_Tracker.Data.Entity
{
    public class Stat
    {
        public Guid Id { get; set; }
        public string Account_Id { get; set; }
        public string? Picture { get; set; }
        public string? Weight { get; set; }
        public string? Age { get; set; }
        public string? Height { get; set; }
        public string? Max_Target {  get; set; }
        public string? Target_Points {  get; set; }
        //public string? Fats { get; set; }
        //public string? Carbohydrates { get; set; }
        //public string? Proteins {  get; set; }
    }
}

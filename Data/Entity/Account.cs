using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Callories_Tracker.Data.Entity
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Mail { get; set; } = null!;
        public string Password {  get; set; } = null!;
        public string? Name { get; set; }

    }
}

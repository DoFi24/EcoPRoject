using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationClinik.Models
{
    public class DBKartrigList
    {
        public int Id { get; set; }
        public DBKartrij Kartrij { get; set; }
        public DateTime EndDate { get; set; }

    }
}

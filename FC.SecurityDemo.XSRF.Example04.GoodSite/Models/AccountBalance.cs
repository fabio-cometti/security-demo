using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FC.SecurityDemo.XSRF.Example04.GoodSite.Models
{
    public class AccountBalance
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public decimal Credit { get; set; }
    }
}

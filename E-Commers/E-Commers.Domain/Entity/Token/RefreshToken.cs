using E_Commers.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commers.Domain.Entity.Token
{
    public class RefreshToken:BaseEntity
    {
        public string  Username { get; set; }
        public string RefreshTokenValue { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}

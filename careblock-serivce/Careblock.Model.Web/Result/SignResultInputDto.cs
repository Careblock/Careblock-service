using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Careblock.Model.Web.Result
{
    public class SignResultInputDto
    {
        public Guid ResultId { get; set; }
        public string SignHash { get; set; }
        public string SignerAddress { get; set; }
    }
}

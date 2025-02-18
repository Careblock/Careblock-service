using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Careblock.Model.Web.Result
{
    public class ResultMulSignDto
    {
        public string SignerName { get; set; }
        public string SignerAddress { get; set; }
        public DateTime? SignedDate { get; set; }
        public bool IsSigned { get; set; }
    }
}

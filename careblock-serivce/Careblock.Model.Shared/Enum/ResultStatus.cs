using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Careblock.Model.Shared.Enum
{
    public enum ResultStatus : byte
    {
        Draft = 1,
        Signed = 2,
        Sent = 3,
        Banned = 4,
        Pending = 5,
    }
}

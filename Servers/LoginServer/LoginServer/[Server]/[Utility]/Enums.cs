using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPS.LoginServer.Utility
{
    public enum PacketType
    {
        Verification = 0,
        LoginRequest,
        DataRequest,
        LoginResponse,
        DataResponse,
    }
}

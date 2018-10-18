﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dreambound.Networking.DataHandling
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
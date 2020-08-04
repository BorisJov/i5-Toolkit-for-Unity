﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    /// <summary>
    /// Contract specifying how user information that can be accessed from an OIDC provider 
    /// </summary>
    public interface IUserInfo
    {
        string Username { get; }

        string FullName { get; }

        string Email { get; }
    }
}
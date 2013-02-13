// Based on AllowAnonymousAttribute by Copyright (c) Microsoft Open Technologies
// Modified for EasyAuth by Adam K Dean, 12/02/2013

using System;

namespace EasyAuth
{    
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class EzAllowAnonymousAttribute : Attribute
    {
    }
}
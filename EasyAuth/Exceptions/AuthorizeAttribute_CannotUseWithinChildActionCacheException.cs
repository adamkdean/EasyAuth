using System;

namespace EasyAuth
{
    [Serializable]
    public class EzAuthorizeAttribute_CannotUseWithinChildActionCache : Exception
    {
        public EzAuthorizeAttribute_CannotUseWithinChildActionCache() { }
    }
}
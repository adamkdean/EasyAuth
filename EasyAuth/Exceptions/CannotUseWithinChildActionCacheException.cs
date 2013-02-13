using System;

namespace EasyAuth
{
    [Serializable]
    public class CannotUseWithinChildActionCache : Exception
    {
        public CannotUseWithinChildActionCache() { }
    }
}
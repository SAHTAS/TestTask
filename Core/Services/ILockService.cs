using System;

namespace Core.Services
{
    public interface ILockService
    {
        public bool TryLock(string key, out IDisposable locker);
    }
}
using System;
using System.Collections.Concurrent;

namespace Core.Services
{
    public class LockService : ILockService
    {
        private readonly ConcurrentDictionary<string, Lock> locks = new ConcurrentDictionary<string, Lock>();

        public bool TryLock(string key, out IDisposable locker)
        {
            var @lock = new Lock(this, key);
            if (locks.TryAdd(key, @lock))
            {
                locker = @lock;
                return true;
            }

            locker = null;
            return false;
        }

        private void TryRemoveLock(string key)
        {
            locks.TryRemove(key, out _);
        }

        private class Lock : IDisposable
        {
            private readonly LockService lockService;
            private readonly string key;

            public Lock(LockService lockService, string key)
            {
                this.lockService = lockService;
                this.key = key;
            }

            public void Dispose()
            {
                lockService.TryRemoveLock(key);
            }
        }
    }
}
using BlazorTemplate.Server.Infrastructures.Repositories;
using System;

namespace BlazorTemplate.Server.SharedServices
{
    public class SpamBlockSharedService
    {
        private InMemoryRepository<string, int> Repository { get; set; }
        public int AllowableCount { get; protected set; } = 10;
        public int MonitorTime { get; protected set; }

        public bool IsRegistered(string ip) => Repository.Contains(ip);
        public bool IsRejected(string ip)
        {
            if (IsRegistered(ip))
            {
                if (GetBlockCount(ip) >= AllowableCount)   return true;
            }
            return false;
        }

        public SpamBlockSharedService(int monitorTime = 3600)
        {
            MonitorTime = monitorTime;
            Repository = new InMemoryRepository<string, int>(MonitorTime);
        }

        public int GetBlockCount(string ip)
        {
            if (IsRegistered(ip))   return Repository.Find(ip);
            return 0;
        }

        public void AddOrUpdate(string ip)
        {
            if (IsRegistered(ip))
            {
                Repository.Update(ip, GetBlockCount(ip) + 1);
            }
            else
            {
                Repository.Add(ip, 1);
            }
        }

        public void Remove(string ip)
        {
            if (!IsRegistered(ip)) throw new Exception();
            Repository.Remove(ip);
        }
    }
}

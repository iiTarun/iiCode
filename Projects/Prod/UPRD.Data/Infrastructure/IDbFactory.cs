using System;
using UPRD.Data;

namespace UPRD.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        UPRDEntities Init();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.UnitOfWork
{
    public interface IUnitOfWork  // db ya productnan stocku elave eliyende 2 sin elave eliyib sora savechanges eliyirik
    {
        Task CommitAsync();
        void Commit();
    }
}

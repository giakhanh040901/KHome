using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Interfaces
{
    public interface IBondJobsService
    {
        void ScanOrder(int tradingProviderId);
    }
}

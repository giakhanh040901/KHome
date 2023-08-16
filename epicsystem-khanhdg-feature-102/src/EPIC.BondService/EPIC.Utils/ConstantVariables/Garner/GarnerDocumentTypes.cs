using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Garner
{
    public static class GarnerDocumentTypes
    {
        public const int TAI_SAN_DAM_BAO = 1;
        public const int HO_SO_PHAP_LY = 2;

        public static string DocumentTypes(int documentTypes)
        {
            return documentTypes switch
            {
                TAI_SAN_DAM_BAO => GarnerHistoryUpdateSummary.SUMMARY_COLLATERAL,
                HO_SO_PHAP_LY => GarnerHistoryUpdateSummary.SUMMARY_LEGAL_RECORDS,
                _ => string.Empty
            };
        }
    }
}

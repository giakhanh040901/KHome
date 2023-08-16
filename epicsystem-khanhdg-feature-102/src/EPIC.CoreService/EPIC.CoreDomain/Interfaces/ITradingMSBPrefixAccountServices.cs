using EPIC.CoreEntities.DataEntities;
using EPIC.CoreEntities.Dto.TradingMSBPrefixAccount;
using EPIC.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Interfaces
{
    public interface ITradingMsbPrefixAccountServices
    {
        void Add(CreateTradingMsbPrefixAccountDto input);
        void Update(UpdateTradingMsbPrefixAccountDto input);
        
        /// <summary>
        /// Xoá theo id
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        /// Xem danh sách có phân trang
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<TradingMsbPrefixAccountDto> FindAll(FilterTradingMsbPrefixAccountDto input);
        
        /// <summary>
        /// Xem theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TradingMsbPrefixAccount FindById(int id);
        TradingMsbPrefixAccount FindByTradingBankId(int tradingBankId);
    }
}

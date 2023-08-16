using EPIC.CoreEntities.Dto.TradingProviderConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Interfaces
{
    public interface ITradingProviderConfigServices
    {
        List<TradingProviderConfigDto> GetAll(string keyword);
        void Add(CreateTradingProviderConfigDto input);
        void Update(CreateTradingProviderConfigDto input);
        void Delete(string key);

        /// <summary>
        /// Xóa tự động các hợp đồng không có thanh toán sau 1 khoản thời gian do đại lý cài
        /// </summary>
        void DeletedOrderTimeUpByTradingProvider();
        void SendNotiHappyBirthDayByTradingProvider();

        /// <summary>
        /// Update nhiều config cho đại lý
        /// </summary>
        /// <param name="input"></param>
        void AddMutipleConfig(List<CreateTradingProviderConfigDto> input);


        /// <summary>
        /// Update nhiều config cho đại lý
        /// </summary>
        /// <param name="input"></param>
        void UpdateMutipleConfig(List<CreateTradingProviderConfigDto> input);
    }
}

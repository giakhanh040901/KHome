using EPIC.DataAccess.Models;
using EPIC.LoyaltyEntities.Dto.LoyLuckyProgram;
using EPIC.LoyaltyEntities.Dto.LoyLuckyProgramInvestor;
using EPIC.LoyaltyEntities.Dto.LoyLuckyScenario;
using System.Collections.Generic;

namespace EPIC.LoyaltyDomain.Interfaces
{
    public interface ILoyLuckyProgramServices
    {
        LoyLuckyProgramDto Add(CreateLuckyProgramDto input);
        void Update(UpdateLoyLuckyProgramDto input);

        /// <summary>
        /// Cập nhật cài đặt thời gian tham gia chương trình
        /// </summary>
        /// <param name="input"></param>
        void UpdateSetting(UpdateLuckyLoyProgramSettingDto input);

        void ChangeStatus(int luckyProgramId);
        PagingResult<ViewLoyLuckyProgramDto> FindAllLuckyProgram(FilterLoyLuckyProgramDto input);

        /// <summary>
        /// Xem danh sách lịch sử tham gia
        /// </summary>
        PagingResult<LoyLuckyProgramHistoryDto> LuckyProgramHistory(FilterLoyLuckyProgramHistoryDto input);

        /// <summary>
        /// Danh sách lịch sử trúng thưởng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<LoyLuckyProgramPrizeHistoryDto> LuckyProgramPrizeHistory(FilterLoyLuckyProgramPrizeHistoryDto input);

        LoyLuckyProgramDto FindById(int luckyProgramId);
        void Delete(int luckyProgramId);

        #region App
        AppLoyRandomPrize AppRandomPrizeByInvestor(int luckyScenarioId);

        /// <summary>
        /// APP - Danh sách chương trình được tham gia của nhà đầu tư theo Đại lý
        /// </summary>
        IEnumerable<AppLoyLuckyProgramByInvestorDto> AppGetAllLuckyProgram(int tradingProviderId);

        AppLoyLuckyScenarioDto AppLoyLuckyScenarioRotationProgram(int luckyProgramId);
        #endregion

        #region Danh sách khách hàng tham gia
        void AddLuckyProgramInvestor(CreateLuckyProgramInvestorDto input);

        /// <summary>
        /// Danh sách nhà đầu tư của đại lý
        /// </summary>
        PagingResult<LoyInvestorOfTradingDto> GetAllInvestorByTrading(FilterLoyInvestorOfTradingDto input);
        PagingResult<LoyLuckyProgramInvestorDto> FindAllLuckyProgramInvestor(FilterLoyLuckyProgramInvestorDto input);

        /// <summary>
        /// Xóa khách hàng tham gia/ Khi khách hàng chưa tham gia 
        /// </summary>
        void DeleteLuckyProgramInvestor(int luckyProgramInvestorId);
        #endregion
    }
}

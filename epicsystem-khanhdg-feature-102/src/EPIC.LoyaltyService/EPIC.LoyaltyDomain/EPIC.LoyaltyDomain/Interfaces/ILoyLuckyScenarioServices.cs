using EPIC.LoyaltyEntities.Dto.LoyLuckyRotationInterface;
using EPIC.LoyaltyEntities.Dto.LoyLuckyScenario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyDomain.Interfaces
{
    public interface ILoyLuckyScenarioServices
    {
        void AddLuckyScenario(CreateLoyLuckyScenarioDto input);

        /// <summary>
        /// Cập nhật trạng thái kịch bản
        /// </summary>
        void ChangeStatusLuckyScenario(int luckyScenarioId);

        LoyLuckyScenarioDto FindById(int luckyScenarioId);

        /// <summary>
        /// Cập nhật vòng quay
        /// </summary>
        /// <param name="input"></param>
        void UpdateLuckyScenario(UpdateLoyLuckyScenarioDto input);

        /// <summary>
        /// Xóa kịch bản
        /// </summary>
        /// <param name="luckyScenarioId"></param>
        void DeleteLuckyScenario(int luckyScenarioId);

        /// <summary>
        /// Danh sách kịch bản
        /// </summary>
        /// <param name="luckyProgramId"></param>
        /// <returns></returns>
        List<ViewLoyLuckyScenarioDto> GetAllLuckyScenario(int luckyProgramId);
        void UpdateLuckyRotationInterface(CreateLoyLuckyRotationInterfaceDto input, int luckyScenarioId);
    }
}

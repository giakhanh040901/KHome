using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.ContractData;
using EPIC.EntitiesBase.Dto;
using EPIC.MSB.Dto.Notification;
using EPIC.PaymentEntities.Dto.Msb;
using EPIC.PaymentEntities.Dto.MsbRequestPayment;
using EPIC.PaymentSharedEntities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.PaymentDomain.Interfaces
{
    public interface IMsbPaymentServices
    {
        PagingResult<MsbNotificationDto> FindAll(MsbFilterPaymentDto input);
        Task<string> InquiryBankAccount(int bankId, string bankAccount);
        Task ReceiveNotificationAsync(ReceiveNotificationDto input);
        Task ReceiveNotificationPayment(ReceiveNotificationPaymentDto input);
        PagingResult<ViewMsbCollectionPaymentDto> FindAllCollectionPayment(MsbCollectionPaymentFilterDto input);
        Task<ExportResultDto> GetAllInvestorBankAccountList(FilterInvestorBankAccountDto input);
    }
}

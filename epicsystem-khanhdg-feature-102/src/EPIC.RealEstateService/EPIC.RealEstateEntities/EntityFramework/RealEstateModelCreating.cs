using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils;
using EPIC.RealEstateEntities.DataEntities;
using Microsoft.EntityFrameworkCore;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Entities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProductItemExtend;

namespace EPIC.RealEstateEntities.EntityFramework
{
    public interface IRealEstateModel
    {
        DbSet<RstOwner> RstOwners { get; set; }
        DbSet<RstProject> RstProjects { get; set; }
        DbSet<RstApprove> RstApproves { get; set; }
        DbSet<RstProjectType> RstProjectTypes { get; set; }
        DbSet<RstDistributionPolicyTemp> RstDistributionPolicyTemps { get; set; }
        DbSet<RstProjectGuaranteeBank> RstProjectGuaranteeBanks { get; set; }
        DbSet<RstConfigContractCode> RstConfigContractCodes { get; set; }
        DbSet<RstConfigContractCodeDetail> RstConfigContractCodeDetails { get; set; }
        DbSet<RstProductItem> RstProductItems { get; set; }
        DbSet<RstDistribution> RstDistributions { get; set; }
        DbSet<RstDistributionProductItem> RstDistributionProductItems { get; set; }
        DbSet<RstOpenSell> RstOpenSells { get; set; }
        DbSet<RstOpenSellDetail> RstOpenSellDetails { get; set; }
        DbSet<RstOpenSellFile> RstOpenSellFiles { get; set; }
        DbSet<RstProjectMedia> RstProjectMedias { get; set; }
        DbSet<RstProductItemMedia> RstProductItemMedias { get; set; }
        DbSet<RstOpenSellContractTemplate> RstOpenSellContractTemplates { get; set; }
        DbSet<RstProductItemProjectPolicy> RstProductItemProjectPolicys { get; set; }
        DbSet<RstProjectPolicy> RstProjectPolicys { get; set; }
        DbSet<RstOrder> RstOrders { get; set; }
        DbSet<RstOrderPayment> RstOrderPayments { get; set; }
        DbSet<RstOrderSellingPolicy> RstOrderSellingPolicies { get; set; }
        DbSet<RstOpenSellBank> RstOpenSellBanks { get; set; }
        DbSet<RstOpenSellInterest> RstOpenSellInterests { get; set; }
        DbSet<RstProjectInformationShareDetail> RstProjectInformationShareDetails { get; set; }
        DbSet<RstProjectInformationShare> RstProjectInformationShares { get; set; }
        DbSet<RstProductItemMaterialFile> RstProductItemMaterialFiles { get; set; }
        DbSet<RstProductItemDesignDiagramFile> RstProductItemDesignDiagramFiles { get; set; }
    }

    public class RealEstateModelCreating
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region thông tin dự án
            modelBuilder.Entity<RstProject>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstProject.SEQ, DbSchemas.EPIC_REAL_ESTATE);

            modelBuilder.Entity<RstProjectType>();
            modelBuilder.HasSequence(RstProjectType.SEQ, DbSchemas.EPIC_REAL_ESTATE);

            modelBuilder.Entity<RstProjectGuaranteeBank>();
            modelBuilder.HasSequence(RstProjectGuaranteeBank.SEQ, DbSchemas.EPIC_REAL_ESTATE);

            // Thông tin khác của dự án
            modelBuilder.Entity<RstProjectExtend>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });

            modelBuilder.HasSequence(RstProjectExtend.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstProjectExtend>()
                .HasOne<RstProject>().WithMany().HasForeignKey(p => p.ProjectId);

            modelBuilder.Entity<RstProjectWithGroupAttr>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstProjectWithGroupAttr.SEQ, DbSchemas.EPIC_REAL_ESTATE);

            modelBuilder.Entity<RstProjectAttribute>(entity =>
            {
                entity.Property(e => e.IsShowApp).HasDefaultValue(YesNo.YES);
            });
            modelBuilder.HasSequence(RstProjectAttribute.SEQ, DbSchemas.EPIC_REAL_ESTATE);

            modelBuilder.Entity<RstProjectValue>();
            modelBuilder.HasSequence(RstProjectValue.SEQ, DbSchemas.EPIC_REAL_ESTATE);

            modelBuilder.Entity<RstProjectValueMultiple>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Utils.Status.ACTIVE);
            });
            modelBuilder.HasSequence(RstProjectValueMultiple.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            #endregion

            #region chính sách ưu đãi chủ đầu tư
            //chính sách ưu đãi chủ đầu tư
            modelBuilder.Entity<RstProjectPolicy>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Utils.Status.ACTIVE);
                entity.Property(e => e.Source).HasDefaultValue(SourceOrder.ALL);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstProjectPolicy.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstProjectPolicy>()
                .HasOne<RstProject>().WithMany().HasForeignKey(p => p.ProjectId);
            #endregion

            #region chính sách sản phẩm dự án
            //chính sách sản phẩm dự án
            modelBuilder.Entity<RstDistributionPolicyTemp>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstDistributionPolicyTemp.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            #endregion

            #region Cấu trúc dự án
            //cấu trúc dự án
            modelBuilder.Entity<RstProjectStructure>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.Level).HasDefaultValue(RstProjectStructureLevel.One);
            });
            modelBuilder.HasSequence(RstProjectStructure.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstProjectStructure>()
                .HasOne<RstProject>().WithMany().HasForeignKey(p => p.ProjectId);
            #endregion

            #region Hợp đồng CĐT
            //Hợp đồng CĐT
            modelBuilder.Entity<RstContractTemplate>(entity =>
            {
                entity.Property(e => e.DisplayType).HasDefaultValue(DisplayType.TRUOC_KHI_DUYET);
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.ContractSource).HasDefaultValue(ContractSources.ONLINE);
            });
            modelBuilder.HasSequence(RstContractTemplate.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstContractTemplate>()
                .HasOne<RstProjectPolicy>().WithMany().HasForeignKey(p => p.PolicyId);
            modelBuilder.Entity<RstContractTemplate>()
               .HasOne<RstContractTemplateTemp>().WithMany().HasForeignKey(p => p.ContractTemplateTempId);
            modelBuilder.Entity<RstContractTemplate>()
               .HasOne<RstConfigContractCode>().WithMany().HasForeignKey(p => p.ConfigContractId);

            modelBuilder.Entity<RstContractTemplateTemp>(entity =>
            {
                entity.Property(e => e.ContractType).HasDefaultValue(ContractTypes.DAT_LENH);
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.ContractSource).HasDefaultValue(ContractSources.ONLINE);
            });
            modelBuilder.HasSequence(RstContractTemplateTemp.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            #endregion

            #region Cấu hình mã hợp đồng
            //Cấu hình mã hợp đồng
            modelBuilder.Entity<RstConfigContractCode>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });

            modelBuilder.HasSequence(RstConfigContractCode.SEQ, DbSchemas.EPIC_REAL_ESTATE);

            modelBuilder.Entity<RstConfigContractCodeDetail>();
            modelBuilder.HasSequence(RstConfigContractCodeDetail.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstConfigContractCodeDetail>()
              .HasOne<RstConfigContractCode>().WithMany().HasForeignKey(p => p.ConfigContractCodeId);
            #endregion

            #region Tiện ích
            //Tiện ích
            modelBuilder.Entity<RstProjectUtility>(entity =>
            {
                entity.Property(e => e.IsHighlight).HasDefaultValueSql(YesNo.NO);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });

            modelBuilder.HasSequence(RstProjectUtility.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstProjectUtility>();
            modelBuilder.HasSequence(RstProjectUtility.SEQ, DbSchemas.EPIC_REAL_ESTATE);

            #endregion

            #region Ảnh minh họa tiện ích
            //Ảnh minh họa tiện ích
            modelBuilder.Entity<RstProjectUtilityMedia>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });

            modelBuilder.HasSequence(RstProjectUtilityMedia.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstProjectUtilityMedia>()
                .HasOne<RstProject>().WithMany().HasForeignKey(p => p.ProjectId);
            #endregion

            #region Tiện ích khác
            //Tiện ích mở rộng
            modelBuilder.Entity<RstProjectUtilityExtend>(entity =>
            {
                entity.Property(e => e.IsHighlight).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });

            modelBuilder.HasSequence(RstProjectUtilityExtend.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstProjectUtilityExtend>()
                .HasOne<RstProject>().WithMany().HasForeignKey(p => p.ProjectId);
            #endregion

            #region Tiện ích căn hộ
            modelBuilder.Entity<RstProductItemUtility>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });

            modelBuilder.HasSequence(RstProductItemUtility.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstProductItemUtility>()
             .HasOne<RstProductItem>().WithMany().HasForeignKey(p => p.ProductItemId);
            #endregion

            #region định nghĩa thuộc tính dự án
            modelBuilder.Entity<RstProjectDefineAttr>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstProjectDefineAttr.SEQ, DbSchemas.EPIC_REAL_ESTATE);

            modelBuilder.Entity<RstProjectDefineAttrValue>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Utils.Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstProjectDefineAttrValue.SEQ, DbSchemas.EPIC_REAL_ESTATE);

            modelBuilder.Entity<RstProjectDefineGroupAttr>();
            modelBuilder.HasSequence(RstProjectDefineGroupAttr.SEQ, DbSchemas.EPIC_REAL_ESTATE);

            modelBuilder.Entity<RstProjectDefineAttrInGroup>();
            modelBuilder.HasSequence(RstProjectDefineAttrInGroup.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            #endregion

            #region thông tin duyệt
            modelBuilder.Entity<RstApprove>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstApprove.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            #endregion

            #region Chủ đầu tư
            modelBuilder.Entity<RstOwner>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstOwner.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            #endregion

            #region bảng hàng dự án - sản phẩm
            modelBuilder.Entity<RstProductItem>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstProductItem.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstProductItem>()
                .HasOne<RstProject>().WithMany().HasForeignKey(p => p.ProjectId);

            modelBuilder.Entity<RstProductItemProjectPolicy>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Utils.Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstProductItemProjectPolicy.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstProductItemProjectPolicy>()
                .HasOne<RstProductItem>().WithMany().HasForeignKey(p => p.ProductItemId);
            modelBuilder.Entity<RstProductItemProjectPolicy>()
                .HasOne<RstProjectPolicy>().WithMany().HasForeignKey(p => p.ProjectPolicyId);

            // Thông tin khác của sản phẩm
            modelBuilder.Entity<RstProductItemExtend>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });

            modelBuilder.HasSequence(RstProductItemExtend.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstProductItemExtend>()
                .HasOne<RstProductItem>().WithMany().HasForeignKey(p => p.ProductItemId);

            //File vật liều và file sơ đồ thiết kế
            modelBuilder.HasSequence(RstProductItemMaterialFile.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstProductItemMaterialFile>()
                .HasOne<RstProductItem>().WithMany().HasForeignKey(p => p.ProductItemId);

            modelBuilder.HasSequence(RstProductItemDesignDiagramFile.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstProductItemDesignDiagramFile>()
                .HasOne<RstProductItem>().WithMany().HasForeignKey(p => p.ProductItemId);
            #endregion

            #region Phân phối sản phẩm
            modelBuilder.Entity<RstDistribution>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(RstDistributionStatus.KHOI_TAO);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstDistribution.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstDistribution>()
                .HasOne(d => d.Project).WithMany().HasForeignKey(p => p.ProjectId);

            modelBuilder.Entity<RstDistributionProductItem>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValueSql(Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstDistributionProductItem.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstDistributionProductItem>()
                .HasOne<RstProductItem>().WithMany().HasForeignKey(p => p.ProductItemId);
            modelBuilder.Entity<RstDistributionProductItem>()
                .HasOne<RstDistribution>().WithMany().HasForeignKey(p => p.DistributionId);

            modelBuilder.Entity<RstDistributionBank>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstDistributionBank.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstDistributionBank>()
                .HasOne<RstDistribution>().WithMany().HasForeignKey(p => p.DistributionId);
            #endregion

            #region Đại lý mở bán sản phẩm
            modelBuilder.Entity<RstOpenSell>(entity =>
            {
                entity.Property(e => e.IsRegisterSale).HasDefaultValue(false);
                entity.Property(e => e.Status).HasDefaultValue(RstDistributionStatus.KHOI_TAO);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.IsOutstanding).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.FromType).HasDefaultValue(RstOpenSellBankTypes.All);
            });
            modelBuilder.HasSequence(RstOpenSell.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstOpenSell>()
                .HasOne<RstProject>().WithMany().HasForeignKey(p => p.ProjectId);
            modelBuilder.Entity<RstOpenSell>()
                .HasOne<RstDistribution>().WithMany().HasForeignKey(p => p.DistributionId);

            modelBuilder.Entity<RstOpenSellDetail>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(RstProductItemStatus.KHOI_TAO);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstOpenSellDetail.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstOpenSellDetail>()
                .HasOne<RstDistributionProductItem>().WithMany().HasForeignKey(p => p.DistributionProductItemId);
            modelBuilder.Entity<RstOpenSellDetail>()
                .HasOne<RstOpenSell>().WithMany().HasForeignKey(p => p.OpenSellId);

            modelBuilder.Entity<RstOpenSellFile>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstOpenSellFile.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstOpenSellFile>()
                .HasOne<RstOpenSell>().WithMany().HasForeignKey(p => p.OpenSellId);

            modelBuilder.Entity<RstOpenSellContractTemplate>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstOpenSellContractTemplate.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstOpenSellContractTemplate>()
                .HasOne<RstContractTemplateTemp>().WithMany().HasForeignKey(p => p.ContractTemplateTempId);
            modelBuilder.Entity<RstOpenSellContractTemplate>()
                .HasOne<RstDistributionPolicy>().WithMany().HasForeignKey(p => p.DistributionPolicyId);
            modelBuilder.Entity<RstOpenSellContractTemplate>()
                .HasOne<RstConfigContractCode>().WithMany().HasForeignKey(p => p.ConfigContractCodeId);

            modelBuilder.Entity<RstOpenSellBank>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstOpenSellBank.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstOpenSellBank>()
                .HasOne<RstOpenSell>().WithMany().HasForeignKey(p => p.OpenSellId);
            #endregion

            #region Chính sách bán hàng của đại lý
            modelBuilder.Entity<RstSellingPolicyTemp>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Utils.Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstSellingPolicyTemp.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            #endregion

            #region Hồ sơ pháp lý
            modelBuilder.Entity<RstProjectFile>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.ProjectFileType).HasDefaultValue(RstProjectFileTypes.HoSoPhapLy);
            });
            modelBuilder.HasSequence(RstProjectFile.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstProjectFile>()
                .HasOne<RstProject>().WithMany().HasForeignKey(p => p.ProjectId);
            #endregion

            #region Hình ảnh dự án
            modelBuilder.Entity<RstProjectMedia>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstProjectMedia.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstProjectMedia>()
                .HasOne<RstProject>().WithMany().HasForeignKey(p => p.ProjectId);

            modelBuilder.Entity<RstProjectMediaDetail>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstProjectMediaDetail.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstProjectMediaDetail>()
                .HasOne<RstProjectMedia>().WithMany().HasForeignKey(p => p.ProjectMediaId);
            #endregion

            #region Phân phối sản phẩm, áp dụng chính sách
            modelBuilder.Entity<RstDistributionPolicy>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstDistributionPolicy.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstDistributionPolicy>()
                .HasOne<RstDistribution>().WithMany().HasForeignKey(p => p.DistributionId);
            #endregion

            #region Hình ảnh sản phẩm
            modelBuilder.Entity<RstProductItemMedia>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstProductItemMedia.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstProductItemMedia>()
                .HasOne<RstProductItem>().WithMany().HasForeignKey(p => p.ProductItemId);

            modelBuilder.Entity<RstProductItemMediaDetail>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstProductItemMediaDetail.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstProductItemMediaDetail>()
                .HasOne<RstProductItemMedia>().WithMany().HasForeignKey(p => p.ProductItemMediaId);
            #endregion

            #region Phân phối sản phẩm, biểu mẫu hợp đồng
            modelBuilder.Entity<RstDistributionContractTemplate>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstDistributionContractTemplate.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstDistributionContractTemplate>()
                .HasOne<RstDistributionPolicy>().WithMany().HasForeignKey(p => p.DistributionPolicyId);
            modelBuilder.Entity<RstDistributionContractTemplate>()
                .HasOne<RstConfigContractCode>().WithMany().HasForeignKey(p => p.ConfigContractCodeId);
            modelBuilder.Entity<RstDistributionContractTemplate>()
                .HasOne<RstDistribution>().WithMany().HasForeignKey(p => p.DistributionId);
            modelBuilder.Entity<RstDistributionContractTemplate>()
                .HasOne<RstContractTemplateTemp>().WithMany().HasForeignKey(p => p.ContractTemplateTempId);
            #endregion

            #region Chính sách mở bán (SellingPolicy)
            modelBuilder.Entity<RstSellingPolicy>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstSellingPolicy.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstSellingPolicy>()
                .HasOne<RstSellingPolicyTemp>().WithMany().HasForeignKey(p => p.SellingPolicyTempId);
            modelBuilder.Entity<RstSellingPolicy>()
               .HasOne<RstOpenSell>().WithMany().HasForeignKey(p => p.OpenSellId);
            #endregion

            #region Sổ lệnh
            modelBuilder.Entity<RstOrder>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(RstOrderStatus.KHOI_TAO);
                entity.Property(e => e.PaymentType).HasDefaultValue(RstOrderPaymentypes.THANH_TOAN_THUONG);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstOrder.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstOrder>()
                .HasOne<RstProductItem>().WithMany().HasForeignKey(p => p.ProductItemId);
            modelBuilder.Entity<RstOrder>()
                .HasOne<RstDistributionPolicy>().WithMany().HasForeignKey(p => p.DistributionPolicyId);
            modelBuilder.Entity<RstOrder>()
                .HasOne<RstOpenSellDetail>().WithMany().HasForeignKey(p => p.OpenSellDetailId);
            #endregion

            #region Sổ lệnh đồng sở hữu
            modelBuilder.Entity<RstOrderCoOwner>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstOrderCoOwner.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstOrderCoOwner>()
                .HasOne<RstOrder>().WithMany().HasForeignKey(p => p.OrderId);
            #endregion

            #region Thanh toán sổ lệnh
            modelBuilder.Entity<RstOrderPayment>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(OrderPaymentStatus.NHAP);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstOrderPayment.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstOrderPayment>()
                .HasOne<RstOrder>().WithMany().HasForeignKey(p => p.OrderId);
            #endregion

            #region Giỏ hàng
            modelBuilder.Entity<RstCart>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(RstCartStatus.KhoiTao);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstCart.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstCart>()
                .HasOne<RstOpenSellDetail>().WithMany().HasForeignKey(p => p.OpenSellDetailId);
            #endregion

            #region Lịch sử thay đổi
            modelBuilder.Entity<RstHistoryUpdate>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
            });
            modelBuilder.HasSequence(RstHistoryUpdate.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            #endregion

            #region Sổ lệnh - Chính sách ưu đãi 
            modelBuilder.Entity<RstOrderSellingPolicy>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstOrderSellingPolicy.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstOrderSellingPolicy>()
                .HasOne<RstOrder>().WithMany().HasForeignKey(p => p.OrderId);
            modelBuilder.Entity<RstOrderSellingPolicy>()
                .HasOne<RstProductItemProjectPolicy>().WithMany().HasForeignKey(p => p.ProductItemProjectPolicyId);
            modelBuilder.Entity<RstOrderSellingPolicy>()
               .HasOne<RstSellingPolicy>().WithMany().HasForeignKey(p => p.SellingPolicyId);
            #endregion

            #region Hợp đồng sổ lệnh
            modelBuilder.Entity<RstOrderContractFile>(entity =>
            {
                entity.Property(e => e.IsSign).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstOrderContractFile.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstOrderContractFile>()
                .HasOne<RstOrder>().WithMany().HasForeignKey(p => p.OrderId);
            modelBuilder.Entity<RstOrderContractFile>()
                .HasOne<RstContractTemplateTemp>().WithMany().HasForeignKey(p => p.ContractTempId);
            #endregion

            #region Đánh giá
            modelBuilder.Entity<RstRating>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstRating.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstRating>()
                .HasOne<RstOrder>().WithMany().HasForeignKey(p => p.OrderId);
            #endregion

            #region Danh sách dự án yêu thích
            modelBuilder.Entity<RstProjectFavourite>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
            });
            modelBuilder.HasSequence(RstProjectFavourite.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstProjectFavourite>()
                .HasOne<RstOpenSell>().WithMany().HasForeignKey(p => p.OpenSellId);
            #endregion

            #region bảng quan tâm của mở bán
            modelBuilder.HasSequence(RstProjectFavourite.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstOpenSellInterest>()
                .HasOne<RstOpenSell>().WithMany().HasForeignKey(p => p.OpenSellDetailId);
            #endregion

            #region ProjectInformationShare, ProjectInformationShareDetail
            modelBuilder.HasSequence(RstProjectInformationShare.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstProjectInformationShare>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Status.TAM);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(RstProjectInformationShareDetail.SEQ, DbSchemas.EPIC_REAL_ESTATE);
            modelBuilder.Entity<RstProjectInformationShareDetail>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });

            modelBuilder.Entity<RstProjectInformationShare>()
                        .HasOne(project => project.RstProject)
                        .WithMany(projectshare => projectshare.RstProjectInformationShares)
                        .HasForeignKey(e => e.ProjectId)
                        .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<RstProjectInformationShareDetail>()
                        .HasOne(projectshareDetail => projectshareDetail.RstProjectInformationShare)
                        .WithMany(projectshare => projectshare.RstProjectInformationShareDetails)
                        .HasForeignKey(projectshare => projectshare.ProjectShareId)
                        .OnDelete(DeleteBehavior.SetNull);
            #endregion

        }
    }
}

// FileInformation: nyanya/Cqrs.Domain.Product/ProjectItem.cs
// CreatedTime: 2014/07/15   3:35 PM
// LastUpdatedTime: 2014/07/16   11:03 AM

namespace Cqrs.Domain.Product.Models
{
    public partial class ProjectItem
    {
        public ProjectItem()
        {
            
        }
        public ProjectItem(string projectIdentifier)
        {
            this.ProjectIdentifier = projectIdentifier;
        }

        public ProjectItem AddProjectAgreement(Agreement agreement)
        {
            agreement.ProjectIdentifier = this.ProjectIdentifier;
            this.ProjectAgreement = agreement;
            return this;
        }

        public ProjectItem AddProjectSalePeriod(ProjectSalePeriod period)
        {
            period.ProjectIdentifier = this.ProjectIdentifier;
            this.ProjectSalePeriod = period;
            return this;
        }

        public ProjectItem AddSaleInfo(SaleInfo saleInfo)
        {
            saleInfo.ProjectIdentifier = this.ProjectIdentifier;
            this.SaleInfo = saleInfo;
            return this;
        }

        public ProjectItem AddValueInfo(ValueInfo valueInfo)
        {
            valueInfo.ProjectIdentifier = this.ProjectIdentifier;
            this.ValueInfo = valueInfo;
            return this;
        }
    }
}
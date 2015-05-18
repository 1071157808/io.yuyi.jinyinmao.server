// FileInformation: nyanya/Cqrs.Domain.Product/BankAcceptanceProject.cs
// CreatedTime: 2014/07/16   10:21 AM
// LastUpdatedTime: 2014/07/16   11:04 AM

using System;
using System.Threading.Tasks;

namespace Cqrs.Domain.Product.Models
{
    public partial class BankAcceptanceProject : ProjectItem
    {
        public BankAcceptanceProject(string projectIdentifier)
            : base(projectIdentifier)
        {
        }

        public BankAcceptanceProject()
        {
            
        }

        /// <summary>
        /// event
        /// </summary>
        /// <returns></returns>
        internal Task HitShelvedAsync()
        {
            return Task.Run(() => { throw new NotImplementedException(); });
        }
    }
}
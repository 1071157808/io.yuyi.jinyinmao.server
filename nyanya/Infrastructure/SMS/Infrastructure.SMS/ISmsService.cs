// FileInformation: nyanya/Infrastructure.SMS/ISmsService.cs
// CreatedTime: 2014/07/15   3:35 PM
// LastUpdatedTime: 2014/07/19   3:15 PM

using System.Threading.Tasks;

namespace Infrastructure.SMS
{
    public interface ISmsAlertsService
    {
        #region Public Methods

        Task<bool> AlertAsync(string content);

        #endregion Public Methods
    }

    /// <summary>
    ///     短信服务接口
    /// </summary>
    public interface ISmsService
    {
        #region Public Methods

        Task<bool> SendAsync(string cellphone, string content, int type = 0);

        #endregion Public Methods
    }
}
// FileInformation: nyanya/Domian/CommandHandlerBase.cs
// CreatedTime: 2014/07/30   5:41 PM
// LastUpdatedTime: 2014/08/01   1:50 AM

using Domian.Config;
using Infrastructure.Lib;
using Infrastructure.Lib.Exceptions;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Logs;
using Infrastructure.SMS;
using ServiceStack;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace Domian.Commands
{
    public abstract class CommandHandlerBase
    {
        #region Private Fields

        private readonly CqrsConfiguration config;

        #endregion Private Fields

        #region Protected Constructors

        protected CommandHandlerBase(CqrsConfiguration config)
        {
            this.config = config;
        }

        #endregion Protected Constructors

        #region Protected Properties

        protected ISmsAlertsService AlertService
        {
            get { return this.config.SmsAlertsService; }
        }

        protected ILogger Logger
        {
            get { return this.config.CommandHandlerLogger; }
        }

        #endregion Protected Properties

        #region Protected Methods

        protected virtual async Task DoAsync<T>(Func<T, Task> handlerAction, T command, string errorCode = "0000", string friendlyMessage = "请稍后再试") where T : Command
        {
            Exception exception;
            try
            {
                await handlerAction.Invoke(command);
                return;
            }
            catch (DbUpdateConcurrencyException e)
            {
                this.Logger.Error(e, NyanyaResources.CommandHandlerBase_DbUpdateConcurrencyException.FmtWith(GetExceptionInfo(command, e)));
                if (e.InnerException != null)
                {
                    this.Logger.Error(e.InnerException, e.InnerException.Message);
                }
                exception = e;
            }
            catch (DbUpdateException e)
            {
                this.Logger.Error(e, NyanyaResources.CommandHandlerBase_DbUpdateException.FmtWith(GetExceptionInfo(command, e)));
                if (e.InnerException != null)
                {
                    this.Logger.Error(e.InnerException, e.InnerException.Message);
                }
                exception = e;
            }
            catch (BusinessConcurrenceException e)
            {
                string message = GetExceptionInfo(command, e);
                this.Logger.Error(e, NyanyaResources.CommandHandlerBase_BusinessConcurrenceException.FmtWith(message));
                exception = e;
            }
            catch (BusinessValidationFailedException e)
            {
                string message = GetExceptionInfo(command, e);
                this.Logger.Error(e, NyanyaResources.CommandHandlerBase_BusinessValidationFailedException.FmtWith(message));
                // ReSharper disable once CSharpWarnings::CS4014
                this.AlertService.AlertAsync(NyanyaResources.Alert_CommandHandlerBase_BusinessValidationFailedException.FmtWith(message));
                exception = e;
            }
            catch (ApplicationBusinessException e)
            {
                string message = GetExceptionInfo(command, e);
                this.Logger.Error(e, NyanyaResources.CommandHandlerBase_ApplicationBusinessException.FmtWith(message));
                // ReSharper disable once CSharpWarnings::CS4014
                this.AlertService.AlertAsync(NyanyaResources.Alert_CommandHandlerBase_ApplicationBusinessException.FmtWith(message));
                exception = e;
            }
            catch (Exception e)
            {
                string message = GetExceptionInfo(command, e);
                this.Logger.Error(e, NyanyaResources.CommandHandlerBase_UnexpectedException.FmtWith(e.GetType(), message));
                // ReSharper disable once CSharpWarnings::CS4014
                this.AlertService.AlertAsync(NyanyaResources.Alert_CommandHandlerBase_UnexpectedException.FmtWith(e.GetType(), message));
                if (e is CommandExcuteFaildException) throw;
                exception = e;
            }

            throw new CommandExcuteFaildException(command.CommandId, exception.Message, errorCode, friendlyMessage);
        }

        protected virtual async Task<U> DoAsyncWithResult<T, U>(Func<T, Task<U>> handlerAction, T command,
            string errorCode = "0000", string friendlyMessage = "请稍后再试")
            where U : CommandResult
            where T : Command
        {
            Exception exception;
            try
            {
                return await handlerAction.Invoke(command);
            }
            catch (DbUpdateConcurrencyException e)
            {
                this.Logger.Error(e, NyanyaResources.CommandHandlerBase_DbUpdateConcurrencyException.FmtWith(GetExceptionInfo(command, e)));
                if (e.InnerException != null)
                {
                    this.Logger.Error(e.InnerException, e.InnerException.Message);
                }
                exception = e;
            }
            catch (DbUpdateException e)
            {
                this.Logger.Error(e, NyanyaResources.CommandHandlerBase_DbUpdateException.FmtWith(GetExceptionInfo(command, e)));
                if (e.InnerException != null)
                {
                    this.Logger.Error(e.InnerException, e.InnerException.Message);
                }
                exception = e;
            }
            catch (BusinessConcurrenceException e)
            {
                string message = GetExceptionInfo(command, e);
                this.Logger.Error(e, NyanyaResources.CommandHandlerBase_BusinessConcurrenceException.FmtWith(message));
                exception = e;
            }
            catch (BusinessValidationFailedException e)
            {
                string message = GetExceptionInfo(command, e);
                this.Logger.Error(e, NyanyaResources.CommandHandlerBase_BusinessValidationFailedException.FmtWith(message));
                // ReSharper disable once CSharpWarnings::CS4014
                this.AlertService.AlertAsync(NyanyaResources.Alert_CommandHandlerBase_BusinessValidationFailedException.FmtWith(message));
                exception = e;
            }
            catch (ApplicationBusinessException e)
            {
                string message = GetExceptionInfo(command, e);
                this.Logger.Error(e, NyanyaResources.CommandHandlerBase_ApplicationBusinessException.FmtWith(message));
                // ReSharper disable once CSharpWarnings::CS4014
                this.AlertService.AlertAsync(NyanyaResources.Alert_CommandHandlerBase_ApplicationBusinessException.FmtWith(message));
                exception = e;
            }
            catch (Exception e)
            {
                string message = GetExceptionInfo(command, e);
                this.Logger.Error(e, NyanyaResources.CommandHandlerBase_UnexpectedException.FmtWith(e.GetType(), message));
                // ReSharper disable once CSharpWarnings::CS4014
                this.AlertService.AlertAsync(NyanyaResources.Alert_CommandHandlerBase_UnexpectedException.FmtWith(e.GetType(), message));
                if (e is CommandExcuteFaildException) throw;
                exception = e;
            }

            throw new CommandExcuteFaildException(command.CommandId, exception.Message, errorCode, friendlyMessage);
        }

        #endregion Protected Methods

        #region Private Methods

        private static string GetExceptionInfo(Command command, DbUpdateException e)
        {
            string extraMessage;
            try
            {
                extraMessage = "{0} {1}".FmtWith(e.Source, e.Entries.Select(entity => entity.Entity).ToJson());
            }
            catch (Exception exception)
            {
                extraMessage = exception.Message;
            }
            return "{0} {1} {2} {3} {4}".FmtWith(command.CommandId, command.Source, command.ToJson(), e.Message, extraMessage);
        }

        private static string GetExceptionInfo(Command command, Exception e)
        {
            return "{0} {1} {2} {3}".FmtWith(command.CommandId, command.Source, command.ToJson(), e.Message);
        }

        #endregion Private Methods
    }
}
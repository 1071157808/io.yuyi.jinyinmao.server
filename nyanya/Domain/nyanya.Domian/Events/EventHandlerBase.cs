// FileInformation: nyanya/Domian/EventHandlerBase.cs
// CreatedTime: 2014/08/06   2:40 PM
// LastUpdatedTime: 2014/08/16   7:32 PM

using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Domian.Config;
using Infrastructure.Lib;
using Infrastructure.Lib.Exceptions;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Logs;
using Infrastructure.SMS;
using ServiceStack;

namespace Domian.Events
{
    public abstract class EventHandlerBase
    {
        #region Private Fields

        private readonly CqrsConfiguration config;

        #endregion Private Fields

        #region Protected Constructors

        protected EventHandlerBase(CqrsConfiguration config)
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
            get { return this.config.EventHandlerLogger; }
        }

        #endregion Protected Properties

        #region Protected Methods

        protected virtual async Task DoAsync<T>(Func<T, Task> handlerAction, T @event) where T : Event
        {
            try
            {
                await handlerAction.Invoke(@event);
            }
            catch (DbUpdateConcurrencyException e)
            {
                this.Logger.Error(e, NyanyaResources.EventHandlerBase_DbUpdateConcurrencyException.FmtWith(GetExceptionInfo(@event, e)));
                if (e.InnerException != null)
                {
                    this.Logger.Error(e.InnerException, e.InnerException.Message);
                }
            }
            catch (DbUpdateException e)
            {
                this.Logger.Error(e, NyanyaResources.EventHandlerBase_DbUpdateException.FmtWith(GetExceptionInfo(@event, e)));
                if (e.InnerException != null)
                {
                    this.Logger.Error(e.InnerException, e.InnerException.Message);
                }
            }
            catch (BusinessConcurrenceException e)
            {
                string message = GetExceptionInfo(@event, e);
                this.Logger.Error(e, NyanyaResources.EventHandlerBase_BusinessConcurrenceException.FmtWith(message));
            }
            catch (BusinessValidationFailedException e)
            {
                string message = GetExceptionInfo(@event, e);
                this.Logger.Error(e, NyanyaResources.EventHandlerBase_BusinessValidationFailedException.FmtWith(message));
                // ReSharper disable once CSharpWarnings::CS4014
                this.AlertService.AlertAsync(NyanyaResources.Alert_EventHandlerBase_BusinessValidationFailedException.FmtWith(message));
            }
            catch (ApplicationBusinessException e)
            {
                string message = GetExceptionInfo(@event, e);
                this.Logger.Error(e, NyanyaResources.EventHandlerBase_ApplicationBusinessException.FmtWith(message));
                // ReSharper disable once CSharpWarnings::CS4014
                this.AlertService.AlertAsync(NyanyaResources.Alert_EventHandlerBase_ApplicationBusinessException.FmtWith(message));
            }
            catch (Exception e)
            {
                string message = GetExceptionInfo(@event, e);
                this.Logger.Error(e, NyanyaResources.EventHandlerBase_UnexpectedException.FmtWith(e.GetType(), message));
                // ReSharper disable once CSharpWarnings::CS4014
                this.AlertService.AlertAsync(NyanyaResources.Alert_EventHandlerBase_UnexpectedException.FmtWith(e.GetType(), message));
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private static string GetExceptionInfo(Event @event, DbUpdateException e)
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
            return "{0} {1} {2} {3} {4} {5}".FmtWith(@event.EventId, @event.SourceId, @event.SourceType, @event.ToJson(), e.Message, extraMessage);
        }

        private static string GetExceptionInfo(Event @event, Exception e)
        {
            return "{0} {1} {2} {3} {4}".FmtWith(@event.EventId, @event.SourceId, @event.SourceType, @event.ToJson(), e.Message);
        }

        #endregion Private Methods
    }
}
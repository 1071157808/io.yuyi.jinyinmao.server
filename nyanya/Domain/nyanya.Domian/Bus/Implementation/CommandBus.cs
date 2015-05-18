// FileInformation: nyanya/nyanya.Domian/CommandBus.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/18   6:20 PM

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Domian.Commands;
using Domian.Config;
using Infrastructure.Lib;
using Infrastructure.Lib.Disposal;
using Infrastructure.Lib.Exceptions;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Logs;
using Infrastructure.SMS;
using ServiceStack;

namespace Domian.Bus.Implementation
{
    public class CommandBus : DisposableObject, ICommandBus
    {
        #region Private Fields

        // 使用Logger必须采用这种写法，这样可以重新配置Logger
        private readonly CqrsConfiguration config;

        #endregion Private Fields

        #region Public Constructors

        public CommandBus(CqrsConfiguration config)
        {
            this.config = config;
        }

        #endregion Public Constructors

        #region Private Properties

        private ISmsAlertsService AlertService
        {
            get { return this.config.SmsAlertsService; }
        }

        // 使用Logger必须采用这种写法，这样可以重新配置Logger
        private ILogger Logger
        {
            get { return this.config.CommandBusLogger; }
        }

        #endregion Private Properties

        #region ICommandBus Members

        public void Excute<T>(T command) where T : ICommand
        {
            try
            {
                this.Logger.Info("Begin => {0}|{1}".FmtWith(command.GetType(), command.CommandId));
                CommandResult result;
                using (ServiceClientBase client = this.InitJsonServiceClient())
                {
                    result = client.Send(command);
                }
                if (!result.Result && result.Message.IsNotNullOrEmpty())
                {
                    throw new CommandExcuteFaildException(command.CommandId, result.Message, result.Message);
                }
                this.Logger.Info("End => {0}|{1}".FmtWith(command.GetType(), command.CommandId));
            }
            catch (CommandExcuteFaildException e)
            {
                this.Logger.Error(e,
                    NyanyaResources.CommandBus_CommandExcuteFaildException.FmtWith(command.CommandId, e.FriendlyMessage,
                        e.Message));
                this.Logger.Info("Error => {0}|{1}".FmtWith(command.GetType(), command.CommandId));
                throw;
            }
            catch (WebServiceException e)
            {
                this.Logger.Error(e,
                    NyanyaResources.CommandBus_WebServiceException.FmtWith(command.CommandId, e.Message, e.ErrorMessage,
                        e.ResponseBody));
                this.Logger.Info("Error => {0}|{1}".FmtWith(command.GetType(), command.CommandId));
                throw new CommandExcuteFaildException(command.CommandId, e.Message, e);
            }
            catch (Exception e)
            {
                this.Logger.Error(e,
                    NyanyaResources.CommandBus_UnexpectedException.FmtWith(e.GetType(), command.CommandId, e.Message));
                // ReSharper disable once CSharpWarnings::CS4014
                this.AlertService.AlertAsync(NyanyaResources.Alert_CommandBus_UnexpectedException.FmtWith(e.GetType(),
                    command.CommandId, e.Message));
                this.Logger.Info("Error => {0}|{1}".FmtWith(command.GetType(), command.CommandId));
                throw new CommandExcuteFaildException(command.CommandId, e.Message, e);
            }
        }

        public void Excute<T>(IEnumerable<T> commands) where T : ICommand
        {
            throw new NotImplementedException();
        }

        public ObjectCommandResult ObjectExcute<T>(T command) where T : IObjectCommand
        {
            try
            {
                this.Logger.Info("Begin => {0}|{1}".FmtWith(command.GetType(), command.CommandId));
                ObjectCommandResult result;
                using (ServiceClientBase client = InitJsonServiceClientforLuckhub())
                {
                    result = client.Send(command);
                }
                if (result.Result && result.Message.IsNotNullOrEmpty())
                {
                    throw new CommandExcuteFaildException(command.CommandId, result.Message, result.Message);
                }
                this.Logger.Info("End => {0}|{1}".FmtWith(command.GetType(), command.CommandId));
                return result;
            }
            catch (CommandExcuteFaildException e)
            {
                this.Logger.Error(e, NyanyaResources.CommandBus_CommandExcuteFaildException.FmtWith(command.CommandId, e.FriendlyMessage, e.Message));
                this.Logger.Info("Error => {0}|{1}".FmtWith(command.GetType(), command.CommandId));
                throw;
            }
            catch (WebServiceException e)
            {
                this.Logger.Error(e, NyanyaResources.CommandBus_WebServiceException.FmtWith(command.CommandId, e.Message, e.ErrorMessage, e.ResponseBody));
                this.Logger.Info("Error => {0}|{1}".FmtWith(command.GetType(), command.CommandId));
                throw new CommandExcuteFaildException(command.CommandId, e.Message, e);
            }
            catch (Exception e)
            {
                this.Logger.Error(e, NyanyaResources.CommandBus_UnexpectedException.FmtWith(e.GetType(), command.CommandId, e.Message));
                // ReSharper disable once CSharpWarnings::CS4014
                this.AlertService.AlertAsync(NyanyaResources.Alert_CommandBus_UnexpectedException.FmtWith(e.GetType(), command.CommandId, e.Message));
                this.Logger.Info("Error => {0}|{1}".FmtWith(command.GetType(), command.CommandId));
                throw new CommandExcuteFaildException(command.CommandId, e.Message, e);
            }
        }

        public ObjectCommandResult ResultExcute<T>(T command) where T : ICommand
        {
            return ExcuteCommand<T, ObjectCommandResult>(command);
        }

        private U ExcuteCommand<T, U>(T command)
            where T : ICommand
            where U : CommandResult
        {
            try
            {
                this.Logger.Info("Begin => {0}|{1}".FmtWith(command.GetType(), command.CommandId));
                U result;
                using (ServiceClientBase client = this.InitJsonServiceClient())
                {
                    result = client.Send<U>(command);
                }
                if (!result.Result && result.Message.IsNotNullOrEmpty())
                {
                    throw new CommandExcuteFaildException(command.CommandId, result.Message, result.Message);
                }
                this.Logger.Info("End => {0}|{1}".FmtWith(command.GetType(), command.CommandId));
                return result;
            }
            catch (CommandExcuteFaildException e)
            {
                this.Logger.Error(e,
                    NyanyaResources.CommandBus_CommandExcuteFaildException.FmtWith(command.CommandId, e.FriendlyMessage,
                        e.Message));
                this.Logger.Info("Error => {0}|{1}".FmtWith(command.GetType(), command.CommandId));
                throw;
            }
            catch (WebServiceException e)
            {
                this.Logger.Error(e,
                    NyanyaResources.CommandBus_WebServiceException.FmtWith(command.CommandId, e.Message, e.ErrorMessage,
                        e.ResponseBody));
                this.Logger.Info("Error => {0}|{1}".FmtWith(command.GetType(), command.CommandId));
                throw new CommandExcuteFaildException(command.CommandId, e.Message, e);
            }
            catch (Exception e)
            {
                this.Logger.Error(e,
                    NyanyaResources.CommandBus_UnexpectedException.FmtWith(e.GetType(), command.CommandId, e.Message));
                // ReSharper disable once CSharpWarnings::CS4014
                this.AlertService.AlertAsync(NyanyaResources.Alert_CommandBus_UnexpectedException.FmtWith(e.GetType(),
                    command.CommandId, e.Message));
                this.Logger.Info("Error => {0}|{1}".FmtWith(command.GetType(), command.CommandId));
                throw new CommandExcuteFaildException(command.CommandId, e.Message, e);
            }
        }

        #endregion ICommandBus Members

        #region Private Methods

        private JsonServiceClient InitJsonServiceClient()
        {
            string address;
            try
            {
                address = ConfigurationManager.AppSettings.Get("CommandProcessorAddress");
            }
            catch (Exception e)
            {
                throw new CqrsConfigException(NyanyaResources.Config_CommandBus_001, e);
            }
            JsonServiceClient serviceClient = new JsonServiceClient(address);
            serviceClient.Timeout = new TimeSpan(0, 0, 0, 60);
            serviceClient.ReadWriteTimeout = new TimeSpan(0, 0, 0, 10);
            return serviceClient;
        }

        private JsonServiceClient InitJsonServiceClient(string addressName)
        {
            string address;
            try
            {
                address = ConfigurationManager.AppSettings.Get(addressName);
            }
            catch (Exception e)
            {
                throw new CqrsConfigException(NyanyaResources.Config_CommandBus_001, e);
            }
            JsonServiceClient serviceClient = new JsonServiceClient(address);
            serviceClient.Timeout = new TimeSpan(0, 0, 0, 10);
            serviceClient.ReadWriteTimeout = new TimeSpan(0, 0, 0, 10);
            return serviceClient;
        }

        private JsonServiceClient InitJsonServiceClientforLuckhub()
        {
            string address;
            try
            {
                address = ConfigurationManager.AppSettings.Get("LuckhubCommandAddress");
            }
            catch (Exception e)
            {
                throw new CqrsConfigException(NyanyaResources.Config_CommandBus_001, e);
            }
            JsonServiceClient serviceClient = new JsonServiceClient(address);
            serviceClient.Timeout = new TimeSpan(0, 0, 0, 10);
            serviceClient.ReadWriteTimeout = new TimeSpan(0, 0, 0, 10);
            return serviceClient;
        }

        #endregion Private Methods
    }
}
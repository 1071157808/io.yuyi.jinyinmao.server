// ***********************************************************************
// Project          : ServiceBusExplorer
// Author           : Siqi Lu
// Created          : 2015-04-21  11:29 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-25  11:44 PM
// ***********************************************************************
// <copyright file="HandleSubscriptionControl.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.CAT.ServiceBusExplorer.Properties;

#endregion Using Directives

namespace Microsoft.WindowsAzure.CAT.ServiceBusExplorer
{
    public partial class HandleSubscriptionControl : UserControl
    {
        #region Public Constructors

        public HandleSubscriptionControl(WriteToLogDelegate writeToLog, ServiceBusHelper serviceBusHelper, SubscriptionWrapper subscriptionWrapper)
        {
            this.writeToLog = writeToLog;
            this.serviceBusHelper = serviceBusHelper;
            this.subscriptionWrapper = subscriptionWrapper;
            this.dataPointBindingList = new BindingList<MetricDataPoint>
            {
                AllowNew = true,
                AllowEdit = true,
                AllowRemove = true
            };
            this.InitializeComponent();
            this.InitializeControls();
        }

        #endregion Public Constructors

        #region DllImports

        [DllImport("user32.dll")]
        private static extern bool HideCaret(IntPtr hWnd);

        #endregion DllImports

        #region Private Constants

        private const string AccessedAt = "Accessed At";

        private const string ActionExpression = "Action Expression";

        private const string ActiveMessageCount = "Active Message Count";

        private const string AutoDeleteOnIdleDaysMustBeANumber = "The Days value of the AutoDeleteOnIdle field must be a number.";

        private const string AutoDeleteOnIdleHoursMustBeANumber = "The Hours value of the AutoDeleteOnIdle field must be a number.";

        private const string AutoDeleteOnIdleMillisecondsMustBeANumber = "The Milliseconds value of the AutoDeleteOnIdle field must be a number.";

        private const string AutoDeleteOnIdleMinutesMustBeANumber = "The Minutes value of the AutoDeleteOnIdle field must be a number.";

        private const string AutoDeleteOnIdleSecondsMustBeANumber = "The Seconds value of the AutoDeleteOnIdle field must be a number.";

        private const string AutoDeleteOnIdleTooltip = "Gets or sets the maximum period of idleness after which the queue is auto deleted.";

        private const string BatchFlushInterval = "BatchFlushInterval";

        private const string CancelText = "Cancel";

        private const string CreatedAt = "Created At";

        private const string CreateText = "Create";

        private const string DeadletterMessageCount = "DeadLetter Message Count";

        private const string DeadletterTabPage = "tabPageDeadletter";

        private const string DefaultMessageTimeToLiveDaysMustBeANumber = "The Days value of the DefaultMessageTimeToLive field must be a number.";

        private const string DefaultMessageTimeToLiveHoursMustBeANumber = "The Hours value of the DefaultMessageTimeToLive field must be a number.";

        private const string DefaultMessageTimeToLiveMillisecondsMustBeANumber = "The Milliseconds value of the DefaultMessageTimeToLive field must be a number.";

        private const string DefaultMessageTimeToLiveMinutesMustBeANumber = "The Minutes value of the DefaultMessageTimeToLive field must be a number.";

        private const string DefaultMessageTimeToLiveSecondsMustBeANumber = "The Seconds value of the DefaultMessageTimeToLive field must be a number.";

        private const string DefaultMessageTimeToLiveTooltip = "Gets or sets the default message time to live of a queue.";

        private const string DeleteName = "Delete";

        //***************************
        // Texts
        //***************************
        private const string DeleteText = "Delete";

        private const string DeleteTooltip = "Delete the row.";

        private const string DisableText = "Disable";

        private const string DoubleClickMessage = "Double-click a row to repair and resubmit the corresponding message.";

        //***************************
        // Indexes
        //***************************
        private const int EnableBatchedOperationsIndex = 0;

        private const int EnableDeadLetteringOnFilterEvaluationExceptionsIndex = 1;

        private const int EnableDeadLetteringOnMessageExpirationIndex = 2;

        private const string EnableText = "Enable";

        private const string EnqueuedTimeUtc = "EnqueuedTimeUtc";

        //***************************
        // Formats
        //***************************
        private const string ExceptionFormat = "Exception: {0}";

        private const string ExpiresAtUtc = "ExpiresAtUtc";
        private const string FilterActionTooltip = "Gets or sets the filter action of the default rule.";
        private const string FilterExpression = "Filter Expression";
        private const string FilterExpressionAppliedMessage = "The filter expression [{0}] has been successfully applied. [{1}] messages retrieved.";
        private const string FilterExpressionLabel = "Filter Expression";
        private const string FilterExpressionNotValidMessage = "The filter expression [{0}] is not valid: {1}";
        private const string FilterExpressionRemovedMessage = "The filter expression has been removed.";
        private const string FilterExpressionTitle = "Define Filter Expression";
        private const string FilterExpressionTooltip = "Gets or sets the filter expression of the default rule.";
        private const string ForwardDeadLetteredMessagesToTooltip = "Gets or sets the path to the recipient to which the dead lettered message is forwarded.";
        private const string ForwardToTooltip = "Gets or sets the path to the recipient to which the message is forwarded.";
        private const string FriendlyNameProperty = "DisplayName";
        private const string GranularityProperty = "Granularity";
        private const string GrouperFormat = "Metric: [{0}] Unit: [{1}]";

        //***************************
        // Constants
        //***************************
        private const int GrouperMessagePropertiesWith = 312;

        private const string InnerExceptionFormat = "InnerException: {0}";
        private const string IsReadOnly = "Is ReadOnly";
        private const string JsonExtension = "json";
        private const string JsonFilter = "JSON Files|*.json|Text Documents|*.txt";
        private const string Label = "Label";
        private const string LockDurationDaysMustBeANumber = "The Days value of the LockDuration field must be a number.";
        private const string LockDurationHoursMustBeANumber = "The Hours value of the LockDuration field must be a number.";
        private const string LockDurationMillisecondsMustBeANumber = "The Milliseconds value of the LockDuration field must be a number.";
        private const string LockDurationMinutesMustBeANumber = "The Minutes value of the LockDuration field must be a number.";
        private const string LockDurationSecondsMustBeANumber = "The Seconds value of the LockDuration field must be a number.";
        private const string LockDurationTooltip = "Gets or sets the lock duration timespan associated with this queue.";
        private const string MaxDeliveryCountMustBeANumber = "The MaxDeliveryCount field must be a number.";
        private const string MaxDeliveryCountTooltip = "Gets or sets the maximum delivery count. A message is automatically deadlettered after this number of deliveries.";
        private const string MessageCount = "Message Count";
        private const string MessageFileFormat = "BrokeredMessage_{0}_{1}.json";
        private const string MessageId = "MessageId";
        private const string MessageSentMessage = "[{0}] messages where sent to [{1}]";
        private const string MessageSize = "Size";
        private const string MessagesPeekedFromTheDeadletterQueue = "[{0}] messages peeked from the deadletter queue of the subscription [{1}].";
        private const string MessagesPeekedFromTheSubscription = "[{0}] messages peeked from the subscription [{1}].";
        private const string MessagesReceivedFromTheDeadletterQueue = "[{0}] messages received from the deadletter queue of the subscription [{1}].";
        private const string MessagesReceivedFromTheSubscription = "[{0}] messages received from the subscription [{1}].";

        //***************************
        // Pages
        //***************************
        private const string MessagesTabPage = "tabPageMessages";

        //***************************
        // Metrics Constants
        //***************************
        private const string MetricProperty = "Metric";

        private const string MetricsTabPage = "tabPageMetrics";

        //***************************
        // Metrics Formats
        //***************************
        private const string MetricTabPageKeyFormat = "MetricTabPage{0}";

        private const string Mode = "Mode";

        //***************************
        // Messages
        //***************************
        private const string NameCannotBeNull = "The Name field cannot be null.";

        private const string NameProperty = "Name";

        //***************************
        // Tooltips
        //***************************
        private const string NameTooltip = "Gets or sets the subscription name.";

        private const string NoMessageReceivedFromTheDeadletterQueue = "The timeout  of [{0}] seconds has expired and no message was retrieved from the deadletter queue of the subscription [{1}].";
        private const string NoMessageReceivedFromTheSubscription = "The timeout  of [{0}] seconds has expired and no message was retrieved from the subscription [{1}].";
        private const string Path = "Path";
        private const int RequiresSessionIndex = 3;
        private const string RetrieveMessagesFromDeadletterQueue = "Retrieve messages from deadletter queue";
        private const string RetrieveMessagesFromSubscription = "Retrieve messages from subscription";
        private const string SaveAsTitle = "Save File As";
        private const string ScheduledMessageCount = "Scheduled Message Count";
        private const string SelectEntityDialogTitle = "Select a target Queue or Topic";
        private const string SelectEntityGrouperTitle = "Forward To";
        private const string SelectEntityLabelText = "Target Queue or Topic:";
        private const string SequenceNumberName = "Seq";
        private const string SequenceNumberValue = "SequenceNumber";
        private const string SessionId = "SessionId";
        private const string SessionsGotFromTheSubscription = "[{0}] sessions retrieved for the subscription [{1}].";
        private const string SessionsTabPage = "tabPageSessions";

        //***************************
        // Property Labels
        //***************************
        private const string Status = "Status";

        private const string SubscriptionEntity = "Subscription";
        private const string SubscriptionPathFormat = "{0}/Subscriptions/{1}";
        private const string TimeFilterOperator = "Operator";
        private const string TimeFilterOperator1Name = "FilterOperator1";
        private const string TimeFilterOperator2Name = "FilterOperator2";
        private const string TimeFilterValue = "Value";
        private const string TimeFilterValue1Name = "FilterValue1";
        private const string TimeFilterValue2Name = "FilterValue2";
        private const string TransferDeadLetterMessageCount = "Transfer DL Message Count";
        private const string TransferMessageCount = "Transfer Message Count";
        private const string Unknown = "Unkown";
        private const string UpdatedAt = "Updated At";
        private const string UpdateText = "Update";
        private const string UserMetadata = "User Metadata";
        private const string UserMetadataTooltip = "Gets or sets the user metadata.";

        #endregion Private Constants

        #region Private Fields

        private readonly BindingList<MetricDataPoint> dataPointBindingList;
        private readonly BindingSource dataPointBindingSource = new BindingSource();
        private readonly List<TabPage> hiddenPages = new List<TabPage>();
        private readonly ManualResetEvent metricsManualResetEvent = new ManualResetEvent(false);
        private readonly List<string> metricTabPageIndexList = new List<string>();
        private readonly ServiceBusHelper serviceBusHelper;
        private readonly WriteToLogDelegate writeToLog;
        private BrokeredMessage brokeredMessage;
        private int currentDeadletterMessageRowIndex;
        private int currentMessageRowIndex;
        private SortableBindingList<BrokeredMessage> deadletterBindingList;
        private string deadletterFilterExpression;
        private BrokeredMessage deadletterMessage;
        private SortableBindingList<BrokeredMessage> messageBindingList;
        private string messagesFilterExpression;
        private SortableBindingList<MessageSession> sessionBindingList;
        private bool sorting;
        private SubscriptionWrapper subscriptionWrapper;

        #endregion Private Fields

        #region Private Static Fields

        private static readonly List<string> operators = new List<string> { "ge", "gt", "le", "lt", "eq", "ne" };
        private static readonly List<string> timeGranularityList = new List<string> { "PT5M", "PT1H", "P1D", "P7D" };

        #endregion Private Static Fields

        #region Public Events

        public event Action OnCancel;

        public event Action OnChangeStatus;

        public event Action OnRefresh;

        #endregion Public Events

        #region Public Methods

        public void GetDeadletterMessages()
        {
            using (var receiveModeForm = new ReceiveModeForm(RetrieveMessagesFromDeadletterQueue, MainForm.SingletonMainForm.TopCount, this.serviceBusHelper.BrokeredMessageInspectors.Keys))
            {
                if (receiveModeForm.ShowDialog() == DialogResult.OK)
                {
                    this.txtDeadletterText.Text = string.Empty;
                    this.deadletterPropertyListView.Items.Clear();
                    this.deadletterPropertyGrid.SelectedObject = null;
                    var messageInspector = !string.IsNullOrEmpty(receiveModeForm.Inspector) && this.serviceBusHelper.BrokeredMessageInspectors.ContainsKey(receiveModeForm.Inspector) ?
                        Activator.CreateInstance(this.serviceBusHelper.BrokeredMessageInspectors[receiveModeForm.Inspector]) as IBrokeredMessageInspector :
                        null;
                    if (this.subscriptionWrapper.TopicDescription.EnablePartitioning)
                    {
                        this.ReadDeadletterMessagesOneAtTheTime(receiveModeForm.Peek, receiveModeForm.All, receiveModeForm.Count, messageInspector);
                    }
                    else
                    {
                        this.GetDeadletterMessages(receiveModeForm.Peek, receiveModeForm.All, receiveModeForm.Count, messageInspector);
                    }
                }
            }
        }

        public void GetMessages()
        {
            using (var receiveModeForm = new ReceiveModeForm(RetrieveMessagesFromSubscription, MainForm.SingletonMainForm.TopCount, this.serviceBusHelper.BrokeredMessageInspectors.Keys))
            {
                if (receiveModeForm.ShowDialog() == DialogResult.OK)
                {
                    this.txtMessageText.Text = string.Empty;
                    this.messagePropertyListView.Items.Clear();
                    this.messagePropertyGrid.SelectedObject = null;
                    var messageInspector = !string.IsNullOrEmpty(receiveModeForm.Inspector) && this.serviceBusHelper.BrokeredMessageInspectors.ContainsKey(receiveModeForm.Inspector) ?
                        Activator.CreateInstance(this.serviceBusHelper.BrokeredMessageInspectors[receiveModeForm.Inspector]) as IBrokeredMessageInspector :
                        null;
                    if (this.subscriptionWrapper.TopicDescription.EnablePartitioning)
                    {
                        this.ReadMessagesOneAtTheTime(receiveModeForm.Peek, receiveModeForm.All, receiveModeForm.Count, messageInspector);
                    }
                    else
                    {
                        this.GetMessages(receiveModeForm.Peek, receiveModeForm.All, receiveModeForm.Count, messageInspector);
                    }
                }
            }
        }

        public void GetMessageSessions()
        {
            try
            {
                this.mainTabControl.SuspendDrawing();
                this.mainTabControl.SuspendLayout();
                this.tabPageSessions.SuspendDrawing();
                this.tabPageSessions.SuspendLayout();

                var subscriptionClient = this.serviceBusHelper.MessagingFactory.CreateSubscriptionClient(this.subscriptionWrapper.SubscriptionDescription.TopicPath, this.subscriptionWrapper.SubscriptionDescription.Name,
                    ReceiveMode.PeekLock);
                var sessionEnumerable = subscriptionClient.GetMessageSessions();
                if (sessionEnumerable == null)
                {
                    return;
                }
                Cursor.Current = Cursors.WaitCursor;
                if (this.mainTabControl.TabPages[SessionsTabPage] == null)
                {
                    this.EnablePage(SessionsTabPage);
                }
                var messageSessions = sessionEnumerable as MessageSession[] ?? sessionEnumerable.ToArray();
                this.sessionBindingList = new SortableBindingList<MessageSession>(messageSessions)
                {
                    AllowEdit = false,
                    AllowNew = false,
                    AllowRemove = false
                };
                this.writeToLog(string.Format(SessionsGotFromTheSubscription, this.sessionBindingList.Count, this.subscriptionWrapper.SubscriptionDescription.Name));
                this.sessionsBindingSource.DataSource = this.sessionBindingList;
                this.sessionsDataGridView.DataSource = this.sessionsBindingSource;

                this.sessionsSplitContainer.SplitterDistance = this.sessionsSplitContainer.Width -
                                                               GrouperMessagePropertiesWith - this.sessionsSplitContainer.SplitterWidth;
                this.sessionListTextPropertiesSplitContainer.SplitterDistance = this.sessionListTextPropertiesSplitContainer.Size.Height / 2 - 8;

                if (this.mainTabControl.TabPages[SessionsTabPage] != null)
                {
                    this.mainTabControl.SelectTab(SessionsTabPage);
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
            finally
            {
                this.mainTabControl.ResumeLayout();
                this.mainTabControl.ResumeDrawing();
                this.tabPageSessions.ResumeLayout();
                this.tabPageSessions.ResumeDrawing();
                Cursor.Current = Cursors.Default;
            }
        }

        public void RefreshData(SubscriptionWrapper wrapper)
        {
            try
            {
                this.subscriptionWrapper = wrapper;
                this.InitializeData();
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        ///     Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && (this.components != null))
                {
                    this.components.Dispose();
                }

                for (var i = 0; i < this.Controls.Count; i++)
                {
                    this.Controls[i].Dispose();
                }

                base.Dispose(disposing);
            }
                // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }

        private static void textBox_GotFocus(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                HideCaret(textBox.Handle);
            }
        }

        private void btnCancelUpdate_Click(object sender, EventArgs e)
        {
            if (this.btnCancelUpdate.Text == CancelText)
            {
                if (this.OnCancel != null)
                {
                    this.OnCancel();
                }
            }
            else
            {
                try
                {
                    this.subscriptionWrapper.SubscriptionDescription.UserMetadata = this.txtUserMetadata.Text;
                    this.subscriptionWrapper.SubscriptionDescription.ForwardTo = string.IsNullOrWhiteSpace(this.txtForwardTo.Text) ? null : this.txtForwardTo.Text;
                    this.subscriptionWrapper.SubscriptionDescription.ForwardDeadLetteredMessagesTo = string.IsNullOrWhiteSpace(this.txtForwardDeadLetteredMessagesTo.Text) ? null : this.txtForwardDeadLetteredMessagesTo.Text;

                    if (!string.IsNullOrWhiteSpace(this.txtMaxDeliveryCount.Text))
                    {
                        int value;
                        if (int.TryParse(this.txtMaxDeliveryCount.Text, out value))
                        {
                            this.subscriptionWrapper.SubscriptionDescription.MaxDeliveryCount = value;
                        }
                        else
                        {
                            this.writeToLog(MaxDeliveryCountMustBeANumber);
                            return;
                        }
                    }

                    var days = 0;
                    var hours = 0;
                    var minutes = 0;
                    var seconds = 0;
                    var milliseconds = 0;

                    if (!string.IsNullOrWhiteSpace(this.txtDefaultMessageTimeToLiveDays.Text) ||
                        !string.IsNullOrWhiteSpace(this.txtDefaultMessageTimeToLiveHours.Text) ||
                        !string.IsNullOrWhiteSpace(this.txtDefaultMessageTimeToLiveMinutes.Text) ||
                        !string.IsNullOrWhiteSpace(this.txtDefaultMessageTimeToLiveSeconds.Text) ||
                        !string.IsNullOrWhiteSpace(this.txtDefaultMessageTimeToLiveMilliseconds.Text))
                    {
                        if (!string.IsNullOrWhiteSpace(this.txtDefaultMessageTimeToLiveDays.Text))
                        {
                            if (!int.TryParse(this.txtDefaultMessageTimeToLiveDays.Text, out days))
                            {
                                this.writeToLog(DefaultMessageTimeToLiveDaysMustBeANumber);
                                return;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(this.txtDefaultMessageTimeToLiveHours.Text))
                        {
                            if (!int.TryParse(this.txtDefaultMessageTimeToLiveHours.Text, out hours))
                            {
                                this.writeToLog(DefaultMessageTimeToLiveHoursMustBeANumber);
                                return;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(this.txtDefaultMessageTimeToLiveMinutes.Text))
                        {
                            if (!int.TryParse(this.txtDefaultMessageTimeToLiveMinutes.Text, out minutes))
                            {
                                this.writeToLog(DefaultMessageTimeToLiveMinutesMustBeANumber);
                                return;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(this.txtDefaultMessageTimeToLiveSeconds.Text))
                        {
                            if (!int.TryParse(this.txtDefaultMessageTimeToLiveSeconds.Text, out seconds))
                            {
                                this.writeToLog(DefaultMessageTimeToLiveSecondsMustBeANumber);
                                return;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(this.txtDefaultMessageTimeToLiveMilliseconds.Text))
                        {
                            if (!int.TryParse(this.txtDefaultMessageTimeToLiveMilliseconds.Text, out milliseconds))
                            {
                                this.writeToLog(DefaultMessageTimeToLiveMillisecondsMustBeANumber);
                                return;
                            }
                        }
                        this.subscriptionWrapper.SubscriptionDescription.DefaultMessageTimeToLive = new TimeSpan(days, hours, minutes, seconds,
                            milliseconds);
                    }

                    days = 0;
                    hours = 0;
                    minutes = 0;
                    seconds = 0;
                    milliseconds = 0;

                    if (!string.IsNullOrWhiteSpace(this.txtLockDurationDays.Text) ||
                        !string.IsNullOrWhiteSpace(this.txtLockDurationHours.Text) ||
                        !string.IsNullOrWhiteSpace(this.txtLockDurationMinutes.Text) ||
                        !string.IsNullOrWhiteSpace(this.txtLockDurationSeconds.Text) ||
                        !string.IsNullOrWhiteSpace(this.txtLockDurationMilliseconds.Text))
                    {
                        if (!string.IsNullOrWhiteSpace(this.txtLockDurationDays.Text))
                        {
                            if (!int.TryParse(this.txtLockDurationDays.Text, out days))
                            {
                                this.writeToLog(LockDurationDaysMustBeANumber);
                                return;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(this.txtLockDurationHours.Text))
                        {
                            if (!int.TryParse(this.txtLockDurationHours.Text, out hours))
                            {
                                this.writeToLog(LockDurationHoursMustBeANumber);
                                return;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(this.txtLockDurationMinutes.Text))
                        {
                            if (!int.TryParse(this.txtLockDurationMinutes.Text, out minutes))
                            {
                                this.writeToLog(LockDurationMinutesMustBeANumber);
                                return;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(this.txtLockDurationSeconds.Text))
                        {
                            if (!int.TryParse(this.txtLockDurationSeconds.Text, out seconds))
                            {
                                this.writeToLog(LockDurationSecondsMustBeANumber);
                                return;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(this.txtLockDurationMilliseconds.Text))
                        {
                            if (!int.TryParse(this.txtLockDurationMilliseconds.Text, out milliseconds))
                            {
                                this.writeToLog(LockDurationMillisecondsMustBeANumber);
                                return;
                            }
                        }
                        var timeSpan = new TimeSpan(days, hours, minutes, seconds, milliseconds);
                        if (!timeSpan.IsMaxValue())
                        {
                            this.subscriptionWrapper.SubscriptionDescription.LockDuration = timeSpan;
                        }
                    }

                    days = 0;
                    hours = 0;
                    minutes = 0;
                    seconds = 0;
                    milliseconds = 0;

                    if (!string.IsNullOrWhiteSpace(this.txtAutoDeleteOnIdleDays.Text) ||
                        !string.IsNullOrWhiteSpace(this.txtAutoDeleteOnIdleHours.Text) ||
                        !string.IsNullOrWhiteSpace(this.txtAutoDeleteOnIdleMinutes.Text) ||
                        !string.IsNullOrWhiteSpace(this.txtAutoDeleteOnIdleSeconds.Text) ||
                        !string.IsNullOrWhiteSpace(this.txtAutoDeleteOnIdleMilliseconds.Text))
                    {
                        if (!string.IsNullOrWhiteSpace(this.txtAutoDeleteOnIdleDays.Text))
                        {
                            if (!int.TryParse(this.txtAutoDeleteOnIdleDays.Text, out days))
                            {
                                this.writeToLog(AutoDeleteOnIdleDaysMustBeANumber);
                                return;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(this.txtAutoDeleteOnIdleHours.Text))
                        {
                            if (!int.TryParse(this.txtAutoDeleteOnIdleHours.Text, out hours))
                            {
                                this.writeToLog(AutoDeleteOnIdleHoursMustBeANumber);
                                return;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(this.txtAutoDeleteOnIdleMinutes.Text))
                        {
                            if (!int.TryParse(this.txtAutoDeleteOnIdleMinutes.Text, out minutes))
                            {
                                this.writeToLog(AutoDeleteOnIdleMinutesMustBeANumber);
                                return;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(this.txtAutoDeleteOnIdleSeconds.Text))
                        {
                            if (!int.TryParse(this.txtAutoDeleteOnIdleSeconds.Text, out seconds))
                            {
                                this.writeToLog(AutoDeleteOnIdleSecondsMustBeANumber);
                                return;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(this.txtAutoDeleteOnIdleMilliseconds.Text))
                        {
                            if (!int.TryParse(this.txtAutoDeleteOnIdleMilliseconds.Text, out milliseconds))
                            {
                                this.writeToLog(AutoDeleteOnIdleMillisecondsMustBeANumber);
                                return;
                            }
                        }
                        var timeSpan = new TimeSpan(days, hours, minutes, seconds, milliseconds);
                        if (!timeSpan.IsMaxValue())
                        {
                            this.subscriptionWrapper.SubscriptionDescription.AutoDeleteOnIdle = timeSpan;
                        }
                    }

                    this.subscriptionWrapper.SubscriptionDescription.EnableBatchedOperations = this.checkedListBox.GetItemChecked(EnableBatchedOperationsIndex);
                    this.subscriptionWrapper.SubscriptionDescription.EnableDeadLetteringOnFilterEvaluationExceptions = this.checkedListBox.GetItemChecked(EnableDeadLetteringOnFilterEvaluationExceptionsIndex);
                    this.subscriptionWrapper.SubscriptionDescription.EnableDeadLetteringOnMessageExpiration = this.checkedListBox.GetItemChecked(EnableDeadLetteringOnMessageExpirationIndex);
                    this.subscriptionWrapper.SubscriptionDescription.Status = EntityStatus.Disabled;
                    this.serviceBusHelper.UpdateSubscription(this.subscriptionWrapper.TopicDescription, this.subscriptionWrapper.SubscriptionDescription);
                }
                catch (Exception ex)
                {
                    this.HandleException(ex);
                    this.subscriptionWrapper.SubscriptionDescription = this.serviceBusHelper.GetSubscription(this.subscriptionWrapper.TopicDescription.Path, this.subscriptionWrapper.SubscriptionDescription.Name);
                }
                finally
                {
                    this.subscriptionWrapper.SubscriptionDescription.Status = EntityStatus.Active;
                    this.subscriptionWrapper.SubscriptionDescription = this.serviceBusHelper.NamespaceManager.UpdateSubscription(this.subscriptionWrapper.SubscriptionDescription);
                    this.InitializeData();
                }
            }
        }

        private void btnChangeStatus_Click(object sender, EventArgs e)
        {
            if (this.OnChangeStatus != null)
            {
                this.OnChangeStatus();
            }
        }

        private void btnCloseTabs_Click(object sender, EventArgs e)
        {
            if (this.metricTabPageIndexList.Count <= 0)
            {
                return;
            }
            for (var i = 0; i < this.metricTabPageIndexList.Count; i++)
            {
                this.mainTabControl.TabPages.RemoveByKey(this.metricTabPageIndexList[i]);
            }
            this.metricTabPageIndexList.Clear();
            this.btnCloseTabs.Enabled = false;
        }

        private void btnCreateDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.serviceBusHelper == null || this.subscriptionWrapper == null || this.subscriptionWrapper.TopicDescription == null)
                {
                    return;
                }
                if (this.btnCreateDelete.Text == DeleteText && this.subscriptionWrapper.SubscriptionDescription != null &&
                    !string.IsNullOrWhiteSpace(this.subscriptionWrapper.SubscriptionDescription.Name))
                {
                    using (var deleteForm = new DeleteForm(this.subscriptionWrapper.SubscriptionDescription.Name, SubscriptionEntity.ToLower()))
                    {
                        if (deleteForm.ShowDialog() == DialogResult.OK)
                        {
                            this.serviceBusHelper.DeleteSubscription(this.subscriptionWrapper.SubscriptionDescription);
                        }
                    }
                    return;
                }
                if (this.btnCreateDelete.Text == CreateText)
                {
                    if (string.IsNullOrWhiteSpace(this.txtName.Text))
                    {
                        this.writeToLog(NameCannotBeNull);
                        return;
                    }
                    var subscriptionDescription = new SubscriptionDescription(this.subscriptionWrapper.TopicDescription.Path, this.txtName.Text);
                    if (!string.IsNullOrWhiteSpace(this.txtUserMetadata.Text))
                    {
                        subscriptionDescription.UserMetadata = this.txtUserMetadata.Text;
                    }
                    if (!string.IsNullOrWhiteSpace(this.txtForwardTo.Text))
                    {
                        subscriptionDescription.ForwardTo = this.txtForwardTo.Text;
                    }
                    if (!string.IsNullOrWhiteSpace(this.txtForwardDeadLetteredMessagesTo.Text))
                    {
                        subscriptionDescription.ForwardDeadLetteredMessagesTo = this.txtForwardDeadLetteredMessagesTo.Text;
                    }
                    if (!string.IsNullOrWhiteSpace(this.txtMaxDeliveryCount.Text))
                    {
                        int value;
                        if (int.TryParse(this.txtMaxDeliveryCount.Text, out value))
                        {
                            subscriptionDescription.MaxDeliveryCount = value;
                        }
                        else
                        {
                            this.writeToLog(MaxDeliveryCountMustBeANumber);
                            return;
                        }
                    }

                    var days = 0;
                    var hours = 0;
                    var minutes = 0;
                    var seconds = 0;
                    var milliseconds = 0;

                    if (!string.IsNullOrWhiteSpace(this.txtDefaultMessageTimeToLiveDays.Text) ||
                        !string.IsNullOrWhiteSpace(this.txtDefaultMessageTimeToLiveHours.Text) ||
                        !string.IsNullOrWhiteSpace(this.txtDefaultMessageTimeToLiveMinutes.Text) ||
                        !string.IsNullOrWhiteSpace(this.txtDefaultMessageTimeToLiveSeconds.Text) ||
                        !string.IsNullOrWhiteSpace(this.txtDefaultMessageTimeToLiveMilliseconds.Text))
                    {
                        if (!string.IsNullOrWhiteSpace(this.txtDefaultMessageTimeToLiveDays.Text))
                        {
                            if (!int.TryParse(this.txtDefaultMessageTimeToLiveDays.Text, out days))
                            {
                                this.writeToLog(DefaultMessageTimeToLiveDaysMustBeANumber);
                                return;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(this.txtDefaultMessageTimeToLiveHours.Text))
                        {
                            if (!int.TryParse(this.txtDefaultMessageTimeToLiveHours.Text, out hours))
                            {
                                this.writeToLog(DefaultMessageTimeToLiveHoursMustBeANumber);
                                return;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(this.txtDefaultMessageTimeToLiveMinutes.Text))
                        {
                            if (!int.TryParse(this.txtDefaultMessageTimeToLiveMinutes.Text, out minutes))
                            {
                                this.writeToLog(DefaultMessageTimeToLiveMinutesMustBeANumber);
                                return;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(this.txtDefaultMessageTimeToLiveSeconds.Text))
                        {
                            if (!int.TryParse(this.txtDefaultMessageTimeToLiveSeconds.Text, out seconds))
                            {
                                this.writeToLog(DefaultMessageTimeToLiveSecondsMustBeANumber);
                                return;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(this.txtDefaultMessageTimeToLiveMilliseconds.Text))
                        {
                            if (!int.TryParse(this.txtDefaultMessageTimeToLiveMilliseconds.Text, out milliseconds))
                            {
                                this.writeToLog(DefaultMessageTimeToLiveMillisecondsMustBeANumber);
                                return;
                            }
                        }
                        subscriptionDescription.DefaultMessageTimeToLive = new TimeSpan(days, hours, minutes, seconds, milliseconds);
                    }

                    days = 0;
                    hours = 0;
                    minutes = 0;
                    seconds = 0;
                    milliseconds = 0;

                    if (!string.IsNullOrWhiteSpace(this.txtLockDurationDays.Text) ||
                        !string.IsNullOrWhiteSpace(this.txtLockDurationHours.Text) ||
                        !string.IsNullOrWhiteSpace(this.txtLockDurationMinutes.Text) ||
                        !string.IsNullOrWhiteSpace(this.txtLockDurationSeconds.Text) ||
                        !string.IsNullOrWhiteSpace(this.txtLockDurationMilliseconds.Text))
                    {
                        if (!string.IsNullOrWhiteSpace(this.txtLockDurationDays.Text))
                        {
                            if (!int.TryParse(this.txtLockDurationDays.Text, out days))
                            {
                                this.writeToLog(LockDurationDaysMustBeANumber);
                                return;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(this.txtLockDurationHours.Text))
                        {
                            if (!int.TryParse(this.txtLockDurationHours.Text, out hours))
                            {
                                this.writeToLog(LockDurationHoursMustBeANumber);
                                return;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(this.txtLockDurationMinutes.Text))
                        {
                            if (!int.TryParse(this.txtLockDurationMinutes.Text, out minutes))
                            {
                                this.writeToLog(LockDurationMinutesMustBeANumber);
                                return;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(this.txtLockDurationSeconds.Text))
                        {
                            if (!int.TryParse(this.txtLockDurationSeconds.Text, out seconds))
                            {
                                this.writeToLog(LockDurationSecondsMustBeANumber);
                                return;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(this.txtLockDurationMilliseconds.Text))
                        {
                            if (!int.TryParse(this.txtLockDurationMilliseconds.Text, out milliseconds))
                            {
                                this.writeToLog(LockDurationMillisecondsMustBeANumber);
                                return;
                            }
                        }
                        subscriptionDescription.LockDuration = new TimeSpan(days, hours, minutes, seconds, milliseconds);
                    }

                    days = 0;
                    hours = 0;
                    minutes = 0;
                    seconds = 0;
                    milliseconds = 0;

                    if (!string.IsNullOrWhiteSpace(this.txtAutoDeleteOnIdleDays.Text) ||
                        !string.IsNullOrWhiteSpace(this.txtAutoDeleteOnIdleHours.Text) ||
                        !string.IsNullOrWhiteSpace(this.txtAutoDeleteOnIdleMinutes.Text) ||
                        !string.IsNullOrWhiteSpace(this.txtAutoDeleteOnIdleSeconds.Text) ||
                        !string.IsNullOrWhiteSpace(this.txtAutoDeleteOnIdleMilliseconds.Text))
                    {
                        if (!string.IsNullOrWhiteSpace(this.txtAutoDeleteOnIdleDays.Text))
                        {
                            if (!int.TryParse(this.txtAutoDeleteOnIdleDays.Text, out days))
                            {
                                this.writeToLog(AutoDeleteOnIdleDaysMustBeANumber);
                                return;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(this.txtAutoDeleteOnIdleHours.Text))
                        {
                            if (!int.TryParse(this.txtAutoDeleteOnIdleHours.Text, out hours))
                            {
                                this.writeToLog(AutoDeleteOnIdleHoursMustBeANumber);
                                return;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(this.txtAutoDeleteOnIdleMinutes.Text))
                        {
                            if (!int.TryParse(this.txtAutoDeleteOnIdleMinutes.Text, out minutes))
                            {
                                this.writeToLog(AutoDeleteOnIdleMinutesMustBeANumber);
                                return;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(this.txtAutoDeleteOnIdleSeconds.Text))
                        {
                            if (!int.TryParse(this.txtAutoDeleteOnIdleSeconds.Text, out seconds))
                            {
                                this.writeToLog(AutoDeleteOnIdleSecondsMustBeANumber);
                                return;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(this.txtAutoDeleteOnIdleMilliseconds.Text))
                        {
                            if (!int.TryParse(this.txtAutoDeleteOnIdleMilliseconds.Text, out milliseconds))
                            {
                                this.writeToLog(AutoDeleteOnIdleMillisecondsMustBeANumber);
                                return;
                            }
                        }
                        subscriptionDescription.AutoDeleteOnIdle = new TimeSpan(days, hours, minutes, seconds, milliseconds);
                    }

                    subscriptionDescription.EnableBatchedOperations = this.checkedListBox.GetItemChecked(EnableBatchedOperationsIndex);
                    subscriptionDescription.EnableDeadLetteringOnFilterEvaluationExceptions = this.checkedListBox.GetItemChecked(EnableDeadLetteringOnFilterEvaluationExceptionsIndex);
                    subscriptionDescription.EnableDeadLetteringOnMessageExpiration = this.checkedListBox.GetItemChecked(EnableDeadLetteringOnMessageExpirationIndex);
                    subscriptionDescription.RequiresSession = this.checkedListBox.GetItemChecked(RequiresSessionIndex);

                    var ruleDescription = new RuleDescription();

                    if (!string.IsNullOrWhiteSpace(this.txtFilter.Text))
                    {
                        ruleDescription.Filter = new SqlFilter(this.txtFilter.Text);
                    }
                    if (!string.IsNullOrWhiteSpace(this.txtAction.Text))
                    {
                        ruleDescription.Action = new SqlRuleAction(this.txtAction.Text);
                    }

                    this.subscriptionWrapper.SubscriptionDescription = this.serviceBusHelper.CreateSubscription(this.subscriptionWrapper.TopicDescription,
                        subscriptionDescription,
                        ruleDescription);
                    this.InitializeData();
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
        }

        private void btnDeadletter_Click(object sender, EventArgs e)
        {
            this.GetDeadletterMessages();
        }

        private void btnMessages_Click(object sender, EventArgs e)
        {
            this.GetMessages();
        }

        private void btnMetrics_Click(object sender, EventArgs e)
        {
            try
            {
                if (!MetricInfo.EntityMetricDictionary.ContainsKey(SubscriptionEntity))
                {
                    return;
                }
                if (this.metricTabPageIndexList.Count > 0)
                {
                    for (var i = 0; i < this.metricTabPageIndexList.Count; i++)
                    {
                        this.mainTabControl.TabPages.RemoveByKey(this.metricTabPageIndexList[i]);
                    }
                    this.metricTabPageIndexList.Clear();
                }
                Cursor.Current = Cursors.WaitCursor;
                if (this.dataPointBindingList.Count == 0)
                {
                    return;
                }
                foreach (var item in this.dataPointBindingList)
                {
                    item.Entity = string.Format(SubscriptionPathFormat, this.subscriptionWrapper.SubscriptionDescription.TopicPath, this.subscriptionWrapper.SubscriptionDescription.Name);
                    item.Type = SubscriptionEntity;
                }
                BindingList<MetricDataPoint> pointBindingList;
                var allDataPoint = this.dataPointBindingList.FirstOrDefault(m => string.Compare(m.Metric, "all", StringComparison.OrdinalIgnoreCase) == 0);
                if (allDataPoint != null)
                {
                    pointBindingList = new BindingList<MetricDataPoint>();
                    foreach (var item in MetricInfo.EntityMetricDictionary[SubscriptionEntity])
                    {
                        if (string.Compare(item.Name, "all", StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            continue;
                        }
                        pointBindingList.Add(new MetricDataPoint
                        {
                            Entity = allDataPoint.Entity,
                            FilterOperator1 = allDataPoint.FilterOperator1,
                            FilterOperator2 = allDataPoint.FilterOperator2,
                            FilterValue1 = allDataPoint.FilterValue1,
                            FilterValue2 = allDataPoint.FilterValue2,
                            Granularity = allDataPoint.Granularity,
                            Graph = allDataPoint.Graph,
                            Metric = item.Name,
                            Type = allDataPoint.Type
                        });
                    }
                }
                else
                {
                    pointBindingList = this.dataPointBindingList;
                }
                var uris = MetricHelper.BuildUriListForDataPointMetricQueries(MainForm.SingletonMainForm.SubscriptionId, this.serviceBusHelper.Namespace,
                    pointBindingList);
                var uriList = uris as IList<Uri> ?? uris.ToList();
                if (uris == null || !uriList.Any())
                {
                    return;
                }
                var metricData = MetricHelper.ReadMetricDataUsingTasks(uriList,
                    MainForm.SingletonMainForm.CertificateThumbprint);
                var metricList = metricData as IList<IEnumerable<MetricValue>> ?? metricData.ToList();
                if (metricData == null && metricList.Count == 0)
                {
                    return;
                }
                for (var i = 0; i < metricList.Count; i++)
                {
                    if (metricList[i] == null || !metricList[i].Any())
                    {
                        continue;
                    }
                    var key = string.Format(MetricTabPageKeyFormat, i);
                    var metricInfo = MetricInfo.EntityMetricDictionary[SubscriptionEntity].FirstOrDefault(m => m.Name == pointBindingList[i].Metric);
                    var friendlyName = metricInfo != null ? metricInfo.DisplayName : pointBindingList[i].Metric;
                    var unit = metricInfo != null ? metricInfo.Unit : Unknown;
                    this.mainTabControl.TabPages.Add(key, friendlyName);
                    this.metricTabPageIndexList.Add(key);
                    var tabPage = this.mainTabControl.TabPages[key];
                    tabPage.BackColor = Color.FromArgb(215, 228, 242);
                    tabPage.ForeColor = SystemColors.ControlText;
                    var control = new MetricValueControl(this.writeToLog,
                        () => this.mainTabControl.TabPages.RemoveByKey(key),
                        metricList[i],
                        pointBindingList[i],
                        metricInfo)
                    {
                        Location = new Point(0, 0),
                        Dock = DockStyle.Fill,
                        Tag = string.Format(GrouperFormat, friendlyName, unit)
                    };
                    this.mainTabControl.TabPages[key].Controls.Add(control);
                    this.btnCloseTabs.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnOpenActionForm_Click(object sender, EventArgs e)
        {
            using (var form = new TextForm(ActionExpression, this.txtAction.Text))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    this.txtAction.Text = form.Content;
                }
            }
        }

        private void btnOpenForwardDeadLetteredMessagesToForm_Click(object sender, EventArgs e)
        {
            using (var form = new SelectEntityForm(SelectEntityDialogTitle, SelectEntityGrouperTitle, SelectEntityLabelText))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    this.txtForwardDeadLetteredMessagesTo.Text = form.Path;
                }
            }
        }

        private void btnOpenForwardToForm_Click(object sender, EventArgs e)
        {
            using (var form = new SelectEntityForm(SelectEntityDialogTitle, SelectEntityGrouperTitle, SelectEntityLabelText))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    this.txtForwardTo.Text = form.Path;
                }
            }
        }

        private void btnOpenUserMetadataForm_Click(object sender, EventArgs e)
        {
            using (var form = new TextForm(UserMetadata, this.txtUserMetadata.Text))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    this.txtUserMetadata.Text = form.Content;
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (this.OnRefresh != null)
            {
                this.OnRefresh();
            }
        }

        private void btnSessions_Click(object sender, EventArgs e)
        {
            this.GetMessageSessions();
        }

        private void button_MouseEnter(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control != null)
            {
                control.ForeColor = Color.White;
            }
        }

        private void button_MouseLeave(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control != null)
            {
                control.ForeColor = SystemColors.ControlText;
            }
        }

        private void CalculateLastColumnWidth(object sender)
        {
            if (this.sorting)
            {
                return;
            }
            var dataGridView = sender as DataGridView;
            if (dataGridView == null)
            {
                return;
            }
            try
            {
                dataGridView.SuspendDrawing();
                dataGridView.SuspendLayout();
                if (dataGridView.Columns.Count == 2)
                {
                    var width = dataGridView.Width - dataGridView.Columns[0].Width - dataGridView.RowHeadersWidth;
                    var verticalScrollbar = dataGridView.Controls.OfType<VScrollBar>().First();
                    if (verticalScrollbar.Visible)
                    {
                        width -= verticalScrollbar.Width;
                    }
                    dataGridView.Columns[1].Width = width;
                }
                if (dataGridView == this.messagesDataGridView ||
                    dataGridView == this.deadletterDataGridView)
                {
                    var width = dataGridView.Width -
                                dataGridView.RowHeadersWidth -
                                dataGridView.Columns[1].Width -
                                dataGridView.Columns[2].Width;
                    var verticalScrollbar = dataGridView.Controls.OfType<VScrollBar>().First();
                    if (verticalScrollbar.Visible)
                    {
                        width -= verticalScrollbar.Width;
                    }
                    var columnWidth = width / 4;
                    dataGridView.Columns[0].Width = columnWidth - 20;
                    dataGridView.Columns[3].Width = columnWidth;
                    dataGridView.Columns[4].Width = columnWidth + (width - (columnWidth * 4)) + 10;
                    dataGridView.Columns[5].Width = columnWidth + 10;
                }
                if (dataGridView == this.sessionsDataGridView)
                {
                    var width = dataGridView.Width - dataGridView.RowHeadersWidth;
                    var verticalScrollbar = dataGridView.Controls.OfType<VScrollBar>().First();
                    if (verticalScrollbar.Visible)
                    {
                        width -= verticalScrollbar.Width;
                    }
                    const int columnNumber = 4;
                    var columnWidth = width / columnNumber;
                    for (var i = 0; i < 3; i++)
                    {
                        dataGridView.Columns[i].Width = columnWidth;
                    }
                    dataGridView.Columns[3].Width = columnWidth + (width - (columnWidth * columnNumber));
                }
            }
            finally
            {
                dataGridView.ResumeLayout();
                dataGridView.ResumeDrawing();
            }
        }

        private void CalculateLastColumnWidth()
        {
            if (this.dataPointDataGridView.Columns.Count < 5)
            {
                return;
            }
            var otherColumnsWidth = 0;
            for (var i = 1; i < this.dataPointDataGridView.Columns.Count; i++)
            {
                otherColumnsWidth += this.dataPointDataGridView.Columns[i].Width;
            }
            var width = this.dataPointDataGridView.Width - this.dataPointDataGridView.RowHeadersWidth - otherColumnsWidth;
            var verticalScrollbar = this.dataPointDataGridView.Controls.OfType<VScrollBar>().First();
            if (verticalScrollbar.Visible)
            {
                width -= verticalScrollbar.Width;
            }
            this.dataPointDataGridView.Columns[0].Width = width;
        }

        private void checkedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (this.subscriptionWrapper == null || this.subscriptionWrapper.SubscriptionDescription == null)
            {
                return;
            }
            if (e.Index == RequiresSessionIndex)
            {
                e.NewValue = this.subscriptionWrapper.SubscriptionDescription.RequiresSession ? CheckState.Checked : CheckState.Unchecked;
            }
        }

        private string CreateFileName()
        {
            return string.Format(MessageFileFormat,
                CultureInfo.CurrentCulture.TextInfo.ToTitleCase(this.serviceBusHelper.Namespace),
                DateTime.Now.ToString(CultureInfo.InvariantCulture).Replace('/', '-').Replace(':', '-'));
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                this.sorting = true;
            }
        }

        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //var textBox = (TextBox)e.Control;
            //textBox.Multiline = true;
            //textBox.ScrollBars = ScrollBars.Both;
        }

        private void dataGridView_Resize(object sender, EventArgs e)
        {
            this.CalculateLastColumnWidth(sender);
        }

        private void dataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            this.CalculateLastColumnWidth(sender);
        }

        private void dataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            this.CalculateLastColumnWidth(sender);
        }

        private void dataGridView_Sorted(object sender, EventArgs e)
        {
            this.sorting = false;
        }

        private void dataPointDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var dataGridViewColumn = this.dataPointDataGridView.Columns[DeleteName];
            if (dataGridViewColumn != null &&
                e.ColumnIndex == dataGridViewColumn.Index &&
                e.RowIndex > -1 &&
                !this.dataPointDataGridView.Rows[e.RowIndex].IsNewRow)
            {
                this.dataPointDataGridView.Rows.RemoveAt(e.RowIndex);
                return;
            }
            this.dataPointDataGridView.NotifyCurrentCellDirty(true);
        }

        private void dataPointDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void dataPointDataGridView_Resize(object sender, EventArgs e)
        {
            this.CalculateLastColumnWidth();
            this.btnMetrics.Enabled = this.dataPointDataGridView.Rows.Count > 1;
        }

        private void dataPointDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            this.CalculateLastColumnWidth();
            this.btnMetrics.Enabled = this.dataPointDataGridView.Rows.Count > 1;
        }

        private void dataPointDataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            this.CalculateLastColumnWidth();
            this.btnMetrics.Enabled = this.dataPointDataGridView.Rows.Count > 1;
        }

        private void deadletterDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            var bindingList = this.deadletterBindingSource.DataSource as BindingList<BrokeredMessage>;
            if (bindingList == null)
            {
                return;
            }
            using (var messageForm = new MessageForm(bindingList[e.RowIndex], this.serviceBusHelper, this.writeToLog))
            {
                messageForm.ShowDialog();
            }
        }

        private void deadletterDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var cell = this.deadletterDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
            cell.ToolTipText = DoubleClickMessage;
        }

        private void deadletterDataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || e.RowIndex == -1)
            {
                return;
            }
            this.deadletterDataGridView.Rows[e.RowIndex].Selected = true;
            var multipleSelectedRows = this.deadletterDataGridView.SelectedRows.Count > 1;
            this.repairAndResubmitDeadletterToolStripMenuItem.Visible = !multipleSelectedRows;
            this.saveSelectedDeadletteredMessageToolStripMenuItem.Visible = !multipleSelectedRows;
            this.resubmitSelectedDeadletterInBatchModeToolStripMenuItem.Visible = multipleSelectedRows;
            this.saveSelectedDeadletteredMessagesToolStripMenuItem.Visible = multipleSelectedRows;
            this.deadletterContextMenuStrip.Show(Cursor.Position);
        }

        private void deadletterDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void deadletterDataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            var bindingList = this.deadletterBindingSource.DataSource as BindingList<BrokeredMessage>;
            this.currentDeadletterMessageRowIndex = e.RowIndex;
            if (bindingList == null)
            {
                return;
            }
            if (this.deadletterMessage != bindingList[e.RowIndex])
            {
                this.deadletterMessage = bindingList[e.RowIndex];
                this.deadletterPropertyGrid.SelectedObject = this.deadletterMessage;
                BodyType bodyType;
                this.txtDeadletterText.Text = XmlHelper.Indent(this.serviceBusHelper.GetMessageText(this.deadletterMessage, out bodyType));
                var listViewItems = this.deadletterMessage.Properties.Select(p => new ListViewItem(new[] { p.Key, Convert.ToString(p.Value) })).ToArray();
                this.deadletterPropertyListView.Items.Clear();
                this.deadletterPropertyListView.Items.AddRange(listViewItems);
            }
        }

        private void deadletterTabPage_Resize(object sender, EventArgs e)
        {
        }

        private void DisablePage(string pageName)
        {
            var page = this.mainTabControl.TabPages[pageName];
            if (page == null)
            {
                return;
            }
            this.mainTabControl.TabPages.Remove(page);
            this.hiddenPages.Add(page);
        }

        private void DrawTabControlTabs(TabControl tabControl, DrawItemEventArgs e, ImageList images)
        {
            // Get the bounding end of tab strip rectangles.
            var tabstripEndRect = tabControl.GetTabRect(tabControl.TabPages.Count - 1);
            var tabstripEndRectF = new RectangleF(tabstripEndRect.X + tabstripEndRect.Width, tabstripEndRect.Y - 5,
                tabControl.Width - (tabstripEndRect.X + tabstripEndRect.Width), tabstripEndRect.Height + 5);
            var leftVerticalLineRect = new RectangleF(2, tabstripEndRect.Y + tabstripEndRect.Height + 2, 2, tabControl.TabPages[tabControl.SelectedIndex].Height + 2);
            var rightVerticalLineRect = new RectangleF(tabControl.TabPages[tabControl.SelectedIndex].Width + 4, tabstripEndRect.Y + tabstripEndRect.Height + 2, 2, tabControl.TabPages[tabControl.SelectedIndex].Height + 2);
            var bottomHorizontalLineRect = new RectangleF(2, tabstripEndRect.Y + tabstripEndRect.Height + tabControl.TabPages[tabControl.SelectedIndex].Height + 2, tabControl.TabPages[tabControl.SelectedIndex].Width + 4, 2);
            RectangleF leftVerticalBarNearFirstTab = new Rectangle(0, 0, 2, tabstripEndRect.Height + 2);

            // First, do the end of the tab strip.
            // If we have an image use it.
            if (tabControl.Parent.BackgroundImage != null)
            {
                var src = new RectangleF(tabstripEndRectF.X + tabControl.Left, tabstripEndRectF.Y + tabControl.Top, tabstripEndRectF.Width, tabstripEndRectF.Height);
                e.Graphics.DrawImage(tabControl.Parent.BackgroundImage, tabstripEndRectF, src, GraphicsUnit.Pixel);
            }
            // If we have no image, use the background color.
            else
            {
                using (Brush backBrush = new SolidBrush(tabControl.Parent.BackColor))
                {
                    e.Graphics.FillRectangle(backBrush, tabstripEndRectF);
                    e.Graphics.FillRectangle(backBrush, leftVerticalLineRect);
                    e.Graphics.FillRectangle(backBrush, rightVerticalLineRect);
                    e.Graphics.FillRectangle(backBrush, bottomHorizontalLineRect);
                    if (this.mainTabControl.SelectedIndex != 0)
                    {
                        e.Graphics.FillRectangle(backBrush, leftVerticalBarNearFirstTab);
                    }
                }
            }

            // Set up the page and the various pieces.
            var page = tabControl.TabPages[e.Index];
            using (var backBrush = new SolidBrush(page.BackColor))
            {
                using (var foreBrush = new SolidBrush(page.ForeColor))
                {
                    var tabName = page.Text;

                    // Set up the offset for an icon, the bounding rectangle and image size and then fill the background.
                    var iconOffset = 0;
                    Rectangle tabBackgroundRect;

                    if (e.Index == this.mainTabControl.SelectedIndex)
                    {
                        tabBackgroundRect = e.Bounds;
                        e.Graphics.FillRectangle(backBrush, tabBackgroundRect);
                    }
                    else
                    {
                        tabBackgroundRect = new Rectangle(e.Bounds.X, e.Bounds.Y - 2, e.Bounds.Width,
                            e.Bounds.Height + 4);
                        e.Graphics.FillRectangle(backBrush, tabBackgroundRect);
                        var rect = new Rectangle(e.Bounds.X - 2, e.Bounds.Y - 2, 1, 2);
                        e.Graphics.FillRectangle(backBrush, rect);
                        rect = new Rectangle(e.Bounds.X - 1, e.Bounds.Y - 2, 1, 2);
                        e.Graphics.FillRectangle(backBrush, rect);
                        rect = new Rectangle(e.Bounds.X + e.Bounds.Width, e.Bounds.Y - 2, 1, 2);
                        e.Graphics.FillRectangle(backBrush, rect);
                        rect = new Rectangle(e.Bounds.X + e.Bounds.Width + 1, e.Bounds.Y - 2, 1, 2);
                        e.Graphics.FillRectangle(backBrush, rect);
                    }

                    // If we have images, process them.
                    if (images != null)
                    {
                        // Get sice and image.
                        var size = images.ImageSize;
                        Image icon = null;
                        if (page.ImageIndex > -1)
                            icon = images.Images[page.ImageIndex];
                        else if (page.ImageKey != "")
                            icon = images.Images[page.ImageKey];

                        // If there is an image, use it.
                        if (icon != null)
                        {
                            var startPoint =
                                new Point(tabBackgroundRect.X + 2 + ((tabBackgroundRect.Height - size.Height) / 2),
                                    tabBackgroundRect.Y + 2 + ((tabBackgroundRect.Height - size.Height) / 2));
                            e.Graphics.DrawImage(icon, new Rectangle(startPoint, size));
                            iconOffset = size.Width + 4;
                        }
                    }

                    // Draw out the label.
                    var labelRect = new Rectangle(tabBackgroundRect.X + iconOffset, tabBackgroundRect.Y + 5,
                        tabBackgroundRect.Width - iconOffset, tabBackgroundRect.Height - 3);
                    using (var sf = new StringFormat { Alignment = StringAlignment.Center })
                    {
                        e.Graphics.DrawString(tabName, new Font(e.Font.FontFamily, 8.25F, e.Font.Style), foreBrush, labelRect, sf);
                    }
                }
            }
        }

        private void EnablePage(string pageName)
        {
            var page = this.hiddenPages.FirstOrDefault(p => string.Compare(p.Name, pageName, StringComparison.InvariantCultureIgnoreCase) == 0);
            if (page == null)
            {
                return;
            }
            this.mainTabControl.TabPages.Add(page);
            this.hiddenPages.Remove(page);
        }

        private void GetDeadletterMessages(bool peek, bool all, int count, IBrokeredMessageInspector messageInspector)
        {
            try
            {
                this.mainTabControl.SuspendDrawing();
                this.mainTabControl.SuspendLayout();
                this.tabPageDeadletter.SuspendDrawing();
                this.tabPageDeadletter.SuspendLayout();

                Cursor.Current = Cursors.WaitCursor;
                var brokeredMessages = new List<BrokeredMessage>();

                if (peek)
                {
                    var messageReceiver = this.serviceBusHelper.MessagingFactory.CreateMessageReceiver(SubscriptionClient.FormatDeadLetterPath(this.subscriptionWrapper.SubscriptionDescription.TopicPath, this.subscriptionWrapper.SubscriptionDescription.Name),
                        ReceiveMode.PeekLock);
                    var messageEnumerable = messageReceiver.PeekBatch(count);
                    if (messageEnumerable == null)
                    {
                        return;
                    }
                    var messageArray = messageEnumerable as BrokeredMessage[] ?? messageEnumerable.ToArray();
                    brokeredMessages = messageInspector != null ?
                        messageArray.Select(b => messageInspector.AfterReceiveMessage(b, this.writeToLog)).ToList() :
                        new List<BrokeredMessage>(messageArray);
                    this.writeToLog(string.Format(MessagesPeekedFromTheDeadletterQueue, brokeredMessages.Count, this.subscriptionWrapper.SubscriptionDescription.Name));
                }
                else
                {
                    var messageReceiver = this.serviceBusHelper.MessagingFactory.CreateMessageReceiver(SubscriptionClient.FormatDeadLetterPath(this.subscriptionWrapper.SubscriptionDescription.TopicPath, this.subscriptionWrapper.SubscriptionDescription.Name),
                        ReceiveMode.ReceiveAndDelete);
                    var totalRetrieved = 0;
                    int retrieved;
                    do
                    {
                        var messages = messageReceiver.ReceiveBatch(all ?
                            MainForm.SingletonMainForm.TopCount :
                            count - totalRetrieved,
                            TimeSpan.FromSeconds(MainForm.SingletonMainForm.ReceiveTimeout));
                        var enumerable = messages as BrokeredMessage[] ?? messages.ToArray();
                        retrieved = enumerable.Count();
                        if (retrieved == 0)
                        {
                            continue;
                        }
                        totalRetrieved += retrieved;
                        brokeredMessages.AddRange(messageInspector != null ? enumerable.Select(b => messageInspector.AfterReceiveMessage(b, this.writeToLog)) : enumerable);
                    } while (retrieved > 0 && (all || count > totalRetrieved));
                    this.writeToLog(string.Format(MessagesReceivedFromTheDeadletterQueue, brokeredMessages.Count, this.subscriptionWrapper.SubscriptionDescription.Name));
                }

                this.deadletterBindingList = new SortableBindingList<BrokeredMessage>(brokeredMessages)
                {
                    AllowEdit = false,
                    AllowNew = false,
                    AllowRemove = false
                };

                this.deadletterBindingSource.DataSource = this.deadletterBindingList;
                this.deadletterDataGridView.DataSource = this.deadletterBindingSource;

                this.deadletterSplitContainer.SplitterDistance = this.deadletterSplitContainer.Width -
                                                                 GrouperMessagePropertiesWith - this.deadletterSplitContainer.SplitterWidth;
                this.deadletterListTextPropertiesSplitContainer.SplitterDistance = this.deadletterListTextPropertiesSplitContainer.Size.Height / 2 - 8;
                this.deadletterCustomPropertiesSplitContainer.SplitterDistance = this.deadletterCustomPropertiesSplitContainer.Size.Width / 2 - 8;

                if (!peek)
                {
                    if (this.OnRefresh != null)
                    {
                        this.OnRefresh();
                    }
                }
                if (this.mainTabControl.TabPages[DeadletterTabPage] == null)
                {
                    this.EnablePage(DeadletterTabPage);
                }
                if (this.mainTabControl.TabPages[DeadletterTabPage] != null)
                {
                    this.mainTabControl.SelectTab(DeadletterTabPage);
                }
            }
            catch (TimeoutException)
            {
                this.writeToLog(string.Format(NoMessageReceivedFromTheDeadletterQueue,
                    MainForm.SingletonMainForm.ReceiveTimeout, this.subscriptionWrapper.SubscriptionDescription.Name));
            }
            catch (NotSupportedException)
            {
                this.ReadDeadletterMessagesOneAtTheTime(peek, all, count, messageInspector);
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
            finally
            {
                this.mainTabControl.ResumeLayout();
                this.mainTabControl.ResumeDrawing();
                this.tabPageDeadletter.ResumeLayout();
                this.tabPageDeadletter.ResumeDrawing();
                Cursor.Current = Cursors.Default;
            }
        }

        private void GetMessages(bool peek, bool all, int count, IBrokeredMessageInspector messageInspector)
        {
            try
            {
                this.mainTabControl.SuspendDrawing();
                this.mainTabControl.SuspendLayout();
                this.tabPageMessages.SuspendDrawing();
                this.tabPageMessages.SuspendLayout();

                Cursor.Current = Cursors.WaitCursor;
                var brokeredMessages = new List<BrokeredMessage>();
                if (peek)
                {
                    var subscriptionClient = this.serviceBusHelper.MessagingFactory.CreateSubscriptionClient(this.subscriptionWrapper.SubscriptionDescription.TopicPath, this.subscriptionWrapper.SubscriptionDescription.Name,
                        ReceiveMode.PeekLock);
                    var totalRetrieved = 0;
                    while (totalRetrieved < count)
                    {
                        var messageEnumerable = subscriptionClient.PeekBatch(count);
                        if (messageEnumerable == null)
                        {
                            break;
                        }
                        var messageArray = messageEnumerable as BrokeredMessage[] ?? messageEnumerable.ToArray();
                        var partialList = messageInspector != null ?
                            messageArray.Select(b => messageInspector.AfterReceiveMessage(b, this.writeToLog)).ToList() :
                            new List<BrokeredMessage>(messageArray);
                        brokeredMessages.AddRange(partialList);
                        totalRetrieved += partialList.Count;
                        if (partialList.Count == 0)
                        {
                            break;
                        }
                    }
                    this.writeToLog(string.Format(MessagesPeekedFromTheSubscription, brokeredMessages.Count, this.subscriptionWrapper.SubscriptionDescription.Name));
                }
                else
                {
                    MessageReceiver messageReceiver;
                    if (this.subscriptionWrapper.SubscriptionDescription.RequiresSession)
                    {
                        var subscriptionClient = this.serviceBusHelper.MessagingFactory.CreateSubscriptionClient(this.subscriptionWrapper.SubscriptionDescription.TopicPath, this.subscriptionWrapper.SubscriptionDescription.Name,
                            ReceiveMode.ReceiveAndDelete);
                        messageReceiver = subscriptionClient.AcceptMessageSession(TimeSpan.FromSeconds(MainForm.SingletonMainForm.ReceiveTimeout));
                    }
                    else
                    {
                        messageReceiver = this.serviceBusHelper.MessagingFactory.CreateMessageReceiver(SubscriptionClient.FormatSubscriptionPath(this.subscriptionWrapper.SubscriptionDescription.TopicPath, this.subscriptionWrapper.SubscriptionDescription.Name),
                            ReceiveMode.ReceiveAndDelete);
                    }
                    var totalRetrieved = 0;
                    int retrieved;
                    do
                    {
                        var messages = messageReceiver.ReceiveBatch(all
                            ? MainForm.SingletonMainForm.TopCount
                            : count - totalRetrieved,
                            TimeSpan.FromSeconds(MainForm.SingletonMainForm.ReceiveTimeout));
                        var enumerable = messages as BrokeredMessage[] ?? messages.ToArray();
                        retrieved = enumerable.Count();
                        if (retrieved == 0)
                        {
                            continue;
                        }
                        totalRetrieved += retrieved;
                        brokeredMessages.AddRange(messageInspector != null ? enumerable.Select(b => messageInspector.AfterReceiveMessage(b, this.writeToLog)) : enumerable);
                    } while (retrieved > 0 && (all || count > totalRetrieved));
                    this.writeToLog(string.Format(MessagesReceivedFromTheSubscription, brokeredMessages.Count, this.subscriptionWrapper.SubscriptionDescription.Name));
                }
                this.messageBindingList = new SortableBindingList<BrokeredMessage>(brokeredMessages)
                {
                    AllowEdit = false,
                    AllowNew = false,
                    AllowRemove = false
                };
                this.messagesBindingSource.DataSource = this.messageBindingList;
                this.messagesDataGridView.DataSource = this.messagesBindingSource;

                this.messagesSplitContainer.SplitterDistance = this.messagesSplitContainer.Width -
                                                               GrouperMessagePropertiesWith - this.messagesSplitContainer.SplitterWidth;
                this.messageListTextPropertiesSplitContainer.SplitterDistance = this.messageListTextPropertiesSplitContainer.Size.Height / 2 - 8;
                this.messagesCustomPropertiesSplitContainer.SplitterDistance = this.messagesCustomPropertiesSplitContainer.Size.Width / 2 - 8;

                if (!peek)
                {
                    if (this.OnRefresh != null)
                    {
                        this.OnRefresh();
                    }
                }
                if (this.mainTabControl.TabPages[MessagesTabPage] == null)
                {
                    this.EnablePage(MessagesTabPage);
                }
                if (this.mainTabControl.TabPages[MessagesTabPage] != null)
                {
                    this.mainTabControl.SelectTab(MessagesTabPage);
                }
            }
            catch (TimeoutException)
            {
                this.writeToLog(string.Format(NoMessageReceivedFromTheSubscription,
                    MainForm.SingletonMainForm.ReceiveTimeout, this.subscriptionWrapper.SubscriptionDescription.Name));
            }
            catch (NotSupportedException)
            {
                this.ReadMessagesOneAtTheTime(peek, all, count, messageInspector);
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
            finally
            {
                this.mainTabControl.ResumeLayout();
                this.mainTabControl.ResumeDrawing();
                this.tabPageMessages.ResumeLayout();
                this.tabPageMessages.ResumeDrawing();
                Cursor.Current = Cursors.Default;
            }
        }

        private void grouperDatapoints_CustomPaint(PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(SystemColors.ActiveBorder, 1), this.dataPointDataGridView.Location.X - 1, this.dataPointDataGridView.Location.Y - 1, this.dataPointDataGridView.Size.Width + 1, this.dataPointDataGridView.Size.Height + 1);
        }

        private void grouperDeadletterCustomProperties_CustomPaint(PaintEventArgs obj)
        {
            this.deadletterPropertyListView.Size = new Size(this.grouperDeadletterCustomProperties.Size.Width - (this.deadletterPropertyListView.Location.X * 2), this.grouperDeadletterCustomProperties.Size.Height - this.deadletterPropertyListView.Location.Y - this.deadletterPropertyListView.Location.X);
        }

        private void grouperDeadletterList_CustomPaint(PaintEventArgs e)
        {
            this.deadletterDataGridView.Size = new Size(this.grouperDeadletterList.Size.Width - (this.messagesDataGridView.Location.X * 2 + 2), this.grouperDeadletterList.Size.Height - this.messagesDataGridView.Location.Y - this.messagesDataGridView.Location.X - 2);
            e.Graphics.DrawRectangle(new Pen(SystemColors.ActiveBorder, 1), this.deadletterDataGridView.Location.X - 1, this.deadletterDataGridView.Location.Y - 1, this.deadletterDataGridView.Size.Width + 1, this.deadletterDataGridView.Size.Height + 1);
        }

        private void grouperDeadletterProperties_CustomPaint(PaintEventArgs obj)
        {
            this.deadletterPropertyGrid.Size = new Size(this.grouperDeadletterProperties.Size.Width - (this.deadletterPropertyGrid.Location.X * 2), this.grouperDeadletterProperties.Size.Height - this.deadletterPropertyGrid.Location.Y - this.deadletterPropertyGrid.Location.X);
        }

        private void grouperDeadletterText_CustomPaint(PaintEventArgs obj)
        {
            this.txtDeadletterText.Size = new Size(this.grouperDeadletterText.Size.Width - (this.txtDeadletterText.Location.X * 2), this.grouperDeadletterText.Size.Height - this.txtDeadletterText.Location.Y - this.txtDeadletterText.Location.X);
        }

        private void grouperMessageCustomProperties_CustomPaint(PaintEventArgs obj)
        {
            this.messagePropertyListView.Size = new Size(this.grouperMessageCustomProperties.Size.Width - (this.messagePropertyListView.Location.X * 2), this.grouperMessageCustomProperties.Size.Height - this.messagePropertyListView.Location.Y - this.messagePropertyListView.Location.X);
        }

        private void grouperMessageList_CustomPaint(PaintEventArgs e)
        {
            this.messagesDataGridView.Size = new Size(this.grouperMessageList.Size.Width - (this.messagesDataGridView.Location.X * 2 + 2), this.grouperMessageList.Size.Height - this.messagesDataGridView.Location.Y - this.messagesDataGridView.Location.X - 2);
            e.Graphics.DrawRectangle(new Pen(SystemColors.ActiveBorder, 1), this.messagesDataGridView.Location.X - 1, this.messagesDataGridView.Location.Y - 1, this.messagesDataGridView.Size.Width + 1, this.messagesDataGridView.Size.Height + 1);
        }

        private void grouperMessageProperties_CustomPaint(PaintEventArgs obj)
        {
            this.messagePropertyGrid.Size = new Size(this.grouperMessageProperties.Size.Width - (this.messagePropertyGrid.Location.X * 2), this.grouperMessageProperties.Size.Height - this.messagePropertyGrid.Location.Y - this.messagePropertyGrid.Location.X);
        }

        private void grouperMessageText_CustomPaint(PaintEventArgs obj)
        {
            this.txtMessageText.Size = new Size(this.grouperMessageText.Size.Width - (this.txtMessageText.Location.X * 2), this.grouperMessageText.Size.Height - this.txtMessageText.Location.Y - this.txtMessageText.Location.X);
        }

        private void grouperSessionList_CustomPaint(PaintEventArgs e)
        {
            this.sessionsDataGridView.Size = new Size(this.grouperSessionList.Size.Width - (this.sessionsDataGridView.Location.X * 2 + 2), this.grouperSessionList.Size.Height - this.sessionsDataGridView.Location.Y - this.sessionsDataGridView.Location.X - 2);
            e.Graphics.DrawRectangle(new Pen(SystemColors.ActiveBorder, 1), this.sessionsDataGridView.Location.X - 1, this.sessionsDataGridView.Location.Y - 1, this.sessionsDataGridView.Size.Width + 1, this.sessionsDataGridView.Size.Height + 1);
        }

        private void grouperSessionProperties_CustomPaint(PaintEventArgs obj)
        {
            this.sessionPropertyGrid.Size = new Size(this.grouperSessionProperties.Size.Width - (this.sessionPropertyGrid.Location.X * 2), this.grouperSessionProperties.Size.Height - this.sessionPropertyGrid.Location.Y - this.sessionPropertyGrid.Location.X);
        }

        private void grouperSessionState_CustomPaint(PaintEventArgs obj)
        {
            this.txtSessionState.Size = new Size(this.grouperSessionState.Size.Width - (this.txtSessionState.Location.X * 2), this.grouperSessionState.Size.Height - this.txtSessionState.Location.Y - this.txtSessionState.Location.X);
        }

        private void HandleException(Exception ex)
        {
            if (ex == null || string.IsNullOrWhiteSpace(ex.Message))
            {
                return;
            }
            this.writeToLog(string.Format(CultureInfo.CurrentCulture, ExceptionFormat, ex.Message));
            if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
            {
                this.writeToLog(string.Format(CultureInfo.CurrentCulture, InnerExceptionFormat, ex.InnerException.Message));
            }
        }

        private void InitializeControls()
        {
            // Splitter controls
            this.messagesSplitContainer.SplitterWidth = 16;
            this.sessionsSplitContainer.SplitterWidth = 16;
            this.deadletterSplitContainer.SplitterWidth = 16;
            this.messagesCustomPropertiesSplitContainer.SplitterWidth = 16;
            this.deadletterCustomPropertiesSplitContainer.SplitterWidth = 16;
            this.messageListTextPropertiesSplitContainer.SplitterWidth = 8;
            this.deadletterListTextPropertiesSplitContainer.SplitterWidth = 8;

            // Tabe pages
            this.DisablePage(MessagesTabPage);
            this.DisablePage(SessionsTabPage);
            this.DisablePage(DeadletterTabPage);

            // Set Grid style
            this.dataPointDataGridView.EnableHeadersVisualStyles = false;

            // Set the selection background color for all the cells.
            this.dataPointDataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(92, 125, 150);
            this.dataPointDataGridView.DefaultCellStyle.SelectionForeColor = SystemColors.Window;

            // Set RowHeadersDefaultCellStyle.SelectionBackColor so that its default
            // value won't override DataGridView.DefaultCellStyle.SelectionBackColor.
            this.dataPointDataGridView.RowHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(153, 180, 209);

            // Set the background color for all rows and for alternating rows.
            // The value for alternating rows overrides the value for all rows.
            this.dataPointDataGridView.RowsDefaultCellStyle.BackColor = SystemColors.Window;
            this.dataPointDataGridView.RowsDefaultCellStyle.ForeColor = SystemColors.ControlText;
            //filtersDataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            //filtersDataGridView.AlternatingRowsDefaultCellStyle.ForeColor = SystemColors.ControlText;

            // Set the row and column header styles.
            this.dataPointDataGridView.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(215, 228, 242);
            this.dataPointDataGridView.RowHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;
            this.dataPointDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(215, 228, 242);
            this.dataPointDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;

            // Initialize the DataGridView.
            this.dataPointBindingSource.DataSource = this.dataPointBindingList;
            this.dataPointDataGridView.AutoGenerateColumns = false;
            this.dataPointDataGridView.AutoSize = true;
            this.dataPointDataGridView.DataSource = this.dataPointBindingSource;
            this.dataPointDataGridView.ForeColor = SystemColors.WindowText;

            if (this.subscriptionWrapper != null && this.subscriptionWrapper.SubscriptionDescription != null)
            {
                MetricInfo.GetMetricInfoListAsync(this.serviceBusHelper.Namespace,
                    SubscriptionEntity,
                    string.Format(SubscriptionPathFormat, this.subscriptionWrapper.SubscriptionDescription.TopicPath, this.subscriptionWrapper.SubscriptionDescription.Name)).ContinueWith(t => this.metricsManualResetEvent.Set());
            }

            if (this.dataPointDataGridView.Columns.Count == 0)
            {
                // Create the Metric column
                var metricColumn = new DataGridViewComboBoxColumn
                {
                    DataSource = MetricInfo.EntityMetricDictionary.ContainsKey(SubscriptionEntity) ?
                        MetricInfo.EntityMetricDictionary[SubscriptionEntity] :
                        null,
                    DataPropertyName = MetricProperty,
                    DisplayMember = FriendlyNameProperty,
                    ValueMember = NameProperty,
                    Name = MetricProperty,
                    Width = 144,
                    DropDownWidth = 250,
                    FlatStyle = FlatStyle.Flat,
                    DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
                };
                this.dataPointDataGridView.Columns.Add(metricColumn);

                // Create the Time Granularity column
                var timeGranularityColumn = new DataGridViewComboBoxColumn
                {
                    DataSource = timeGranularityList,
                    DataPropertyName = GranularityProperty,
                    Name = GranularityProperty,
                    Width = 72,
                    FlatStyle = FlatStyle.Flat
                };
                this.dataPointDataGridView.Columns.Add(timeGranularityColumn);

                // Create the Time Operator 1 column
                var operator1Column = new DataGridViewComboBoxColumn
                {
                    DataSource = operators,
                    DataPropertyName = TimeFilterOperator1Name,
                    HeaderText = TimeFilterOperator,
                    Name = TimeFilterOperator1Name,
                    Width = 72,
                    FlatStyle = FlatStyle.Flat
                };
                this.dataPointDataGridView.Columns.Add(operator1Column);

                // Create the Time Value 1 column
                var value1Column = new DataGridViewDateTimePickerColumn
                {
                    DataPropertyName = TimeFilterValue1Name,
                    HeaderText = TimeFilterValue,
                    Name = TimeFilterValue1Name,
                    Width = 136
                };
                this.dataPointDataGridView.Columns.Add(value1Column);

                // Create the Time Operator 1 column
                var operator2Column = new DataGridViewComboBoxColumn
                {
                    DataSource = operators,
                    DataPropertyName = TimeFilterOperator2Name,
                    HeaderText = TimeFilterOperator,
                    Name = TimeFilterOperator2Name,
                    Width = 72,
                    FlatStyle = FlatStyle.Flat
                };
                this.dataPointDataGridView.Columns.Add(operator2Column);

                // Create the Time Value 1 column
                var value2Column = new DataGridViewDateTimePickerColumn
                {
                    DataPropertyName = TimeFilterValue2Name,
                    HeaderText = TimeFilterValue,
                    Name = TimeFilterValue2Name,
                    Width = 136
                };
                this.dataPointDataGridView.Columns.Add(value2Column);

                // Create delete column
                var deleteButtonColumn = new DataGridViewButtonColumn
                {
                    Name = DeleteName,
                    CellTemplate = new DataGridViewDeleteButtonCell(),
                    HeaderText = string.Empty,
                    Width = 22
                };
                deleteButtonColumn.CellTemplate.ToolTipText = DeleteTooltip;
                deleteButtonColumn.UseColumnTextForButtonValue = true;
                this.dataPointDataGridView.Columns.Add(deleteButtonColumn);
            }

            if (this.subscriptionWrapper != null && this.subscriptionWrapper.TopicDescription != null && this.subscriptionWrapper.SubscriptionDescription != null)
            {
                // Tab pages
                if (this.serviceBusHelper.IsCloudNamespace)
                {
                    this.EnablePage(MetricsTabPage);
                }
                else
                {
                    this.DisablePage(MetricsTabPage);
                }

                // Initialize textboxes
                this.txtName.ReadOnly = true;
                this.txtName.BackColor = SystemColors.Window;
                this.txtName.GotFocus += textBox_GotFocus;

                this.txtFilter.ReadOnly = true;
                this.txtFilter.BackColor = SystemColors.Window;
                this.txtFilter.GotFocus += textBox_GotFocus;

                this.txtAction.ReadOnly = true;
                this.txtAction.BackColor = SystemColors.Window;
                this.txtAction.GotFocus += textBox_GotFocus;

                this.txtMessageText.ReadOnly = true;
                this.txtMessageText.BackColor = SystemColors.Window;
                this.txtMessageText.GotFocus += textBox_GotFocus;

                this.txtDeadletterText.ReadOnly = true;
                this.txtDeadletterText.BackColor = SystemColors.Window;
                this.txtDeadletterText.GotFocus += textBox_GotFocus;

                this.txtSessionState.ReadOnly = true;
                this.txtSessionState.BackColor = SystemColors.Window;
                this.txtSessionState.GotFocus += textBox_GotFocus;

                // Initialize groupers
                this.grouperDefaultRule.Visible = false;
                this.grouperSubscriptionSettings.Location = this.grouperDefaultRule.Location;
                this.grouperSubscriptionSettings.Size = new Size(this.grouperSubscriptionSettings.Size.Width, this.grouperSubscriptionSettings.Size.Height + this.grouperDefaultRule.Size.Height + 8);

                // Initialize Data
                this.InitializeData();

                // Set Grid style
                this.messagesDataGridView.EnableHeadersVisualStyles = false;
                this.messagesDataGridView.AutoGenerateColumns = false;
                this.messagesDataGridView.AutoSize = true;

                // Create the MessageId column
                var textBoxColumn = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = MessageId,
                    Name = MessageId,
                    Width = 120
                };
                this.messagesDataGridView.Columns.Add(textBoxColumn);

                // Create the SequenceNumber column
                textBoxColumn = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = SequenceNumberValue,
                    Name = SequenceNumberName,
                    Width = 52
                };
                this.messagesDataGridView.Columns.Add(textBoxColumn);

                // Create the Size column
                textBoxColumn = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = MessageSize,
                    Name = MessageSize,
                    Width = 52
                };
                this.messagesDataGridView.Columns.Add(textBoxColumn);

                // Create the Label column
                textBoxColumn = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = Label,
                    Name = Label,
                    Width = 120
                };
                this.messagesDataGridView.Columns.Add(textBoxColumn);

                // Create the EnqueuedTimeUtc column
                textBoxColumn = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = EnqueuedTimeUtc,
                    Name = EnqueuedTimeUtc,
                    Width = 120
                };
                this.messagesDataGridView.Columns.Add(textBoxColumn);

                // Create the ExpiresAtUtc column
                textBoxColumn = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = ExpiresAtUtc,
                    Name = ExpiresAtUtc,
                    Width = 120
                };
                this.messagesDataGridView.Columns.Add(textBoxColumn);

                // Set the selection background color for all the cells.
                this.messagesDataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(92, 125, 150);
                this.messagesDataGridView.DefaultCellStyle.SelectionForeColor = SystemColors.Window;

                // Set RowHeadersDefaultCellStyle.SelectionBackColor so that its default
                // value won't override DataGridView.DefaultCellStyle.SelectionBackColor.
                this.messagesDataGridView.RowHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(153, 180, 209);

                // Set the background color for all rows and for alternating rows.
                // The value for alternating rows overrides the value for all rows.
                this.messagesDataGridView.RowsDefaultCellStyle.BackColor = SystemColors.Window;
                this.messagesDataGridView.RowsDefaultCellStyle.ForeColor = SystemColors.ControlText;
                //messagesDataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                //messagesDataGridView.AlternatingRowsDefaultCellStyle.ForeColor = SystemColors.ControlText;

                // Set the row and column header styles.
                this.messagesDataGridView.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(215, 228, 242);
                this.messagesDataGridView.RowHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;
                this.messagesDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(215, 228, 242);
                this.messagesDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;

                // Set Grid style
                this.sessionsDataGridView.EnableHeadersVisualStyles = false;
                this.sessionsDataGridView.AutoGenerateColumns = false;
                this.sessionsDataGridView.AutoSize = true;

                // Create the SessionId column
                textBoxColumn = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = SessionId,
                    Name = SessionId,
                    Width = 120
                };
                this.sessionsDataGridView.Columns.Add(textBoxColumn);

                // Create the Path column
                textBoxColumn = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = Path,
                    Name = Path,
                    Width = 120
                };
                this.sessionsDataGridView.Columns.Add(textBoxColumn);

                // Create the Mode column
                textBoxColumn = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = Mode,
                    Name = Mode,
                    Width = 120
                };
                this.sessionsDataGridView.Columns.Add(textBoxColumn);

                // Create the BatchFlushInterval column
                textBoxColumn = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = BatchFlushInterval,
                    Name = BatchFlushInterval,
                    Width = 120
                };
                this.sessionsDataGridView.Columns.Add(textBoxColumn);

                // Set the selection background color for all the cells.
                this.sessionsDataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(92, 125, 150);
                this.sessionsDataGridView.DefaultCellStyle.SelectionForeColor = SystemColors.Window;

                // Set RowHeadersDefaultCellStyle.SelectionBackColor so that its default
                // value won't override DataGridView.DefaultCellStyle.SelectionBackColor.
                this.sessionsDataGridView.RowHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(153, 180, 209);

                // Set the background color for all rows and for alternating rows.
                // The value for alternating rows overrides the value for all rows.
                this.sessionsDataGridView.RowsDefaultCellStyle.BackColor = SystemColors.Window;
                this.sessionsDataGridView.RowsDefaultCellStyle.ForeColor = SystemColors.ControlText;
                //sessionsDataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                //sessionsDataGridView.AlternatingRowsDefaultCellStyle.ForeColor = SystemColors.ControlText;

                // Set the row and column header styles.
                this.sessionsDataGridView.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(215, 228, 242);
                this.sessionsDataGridView.RowHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;
                this.sessionsDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(215, 228, 242);
                this.sessionsDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;

                // Set Grid style
                this.deadletterDataGridView.EnableHeadersVisualStyles = false;
                this.deadletterDataGridView.AutoGenerateColumns = false;
                this.deadletterDataGridView.AutoSize = true;

                // Create the MessageId column
                textBoxColumn = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = MessageId,
                    Name = MessageId,
                    Width = 120
                };
                this.deadletterDataGridView.Columns.Add(textBoxColumn);

                // Create the SequenceNumber column
                textBoxColumn = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = SequenceNumberValue,
                    Name = SequenceNumberName,
                    Width = 52
                };
                this.deadletterDataGridView.Columns.Add(textBoxColumn);

                // Create the Size column
                textBoxColumn = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = MessageSize,
                    Name = MessageSize,
                    Width = 52
                };
                this.deadletterDataGridView.Columns.Add(textBoxColumn);

                // Create the Label column
                textBoxColumn = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = Label,
                    Name = Label,
                    Width = 120
                };
                this.deadletterDataGridView.Columns.Add(textBoxColumn);

                // Create the EnqueuedTimeUtc column
                textBoxColumn = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = EnqueuedTimeUtc,
                    Name = EnqueuedTimeUtc,
                    Width = 120
                };
                this.deadletterDataGridView.Columns.Add(textBoxColumn);

                // Create the ExpiresAtUtc column
                textBoxColumn = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = ExpiresAtUtc,
                    Name = ExpiresAtUtc,
                    Width = 120
                };
                this.deadletterDataGridView.Columns.Add(textBoxColumn);

                // Set the selection background color for all the cells.
                this.deadletterDataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(92, 125, 150);
                this.deadletterDataGridView.DefaultCellStyle.SelectionForeColor = SystemColors.Window;

                // Set RowHeadersDefaultCellStyle.SelectionBackColor so that its default
                // value won't override DataGridView.DefaultCellStyle.SelectionBackColor.
                this.deadletterDataGridView.RowHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(153, 180, 209);

                // Set the background color for all rows and for alternating rows.
                // The value for alternating rows overrides the value for all rows.
                this.deadletterDataGridView.RowsDefaultCellStyle.BackColor = SystemColors.Window;
                this.deadletterDataGridView.RowsDefaultCellStyle.ForeColor = SystemColors.ControlText;
                //deadletterDataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                //deadletterDataGridView.AlternatingRowsDefaultCellStyle.ForeColor = SystemColors.ControlText;

                // Set the row and column header styles.
                this.deadletterDataGridView.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(215, 228, 242);
                this.deadletterDataGridView.RowHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;
                this.deadletterDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(215, 228, 242);
                this.deadletterDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;

                this.checkedListBox.ItemCheck += this.checkedListBox_ItemCheck;

                this.toolTip.SetToolTip(this.txtName, NameTooltip);
                this.toolTip.SetToolTip(this.txtUserMetadata, UserMetadataTooltip);
                this.toolTip.SetToolTip(this.txtForwardTo, ForwardToTooltip);
                this.toolTip.SetToolTip(this.txtForwardDeadLetteredMessagesTo, ForwardDeadLetteredMessagesToTooltip);
                this.toolTip.SetToolTip(this.txtDefaultMessageTimeToLiveDays, DefaultMessageTimeToLiveTooltip);
                this.toolTip.SetToolTip(this.txtDefaultMessageTimeToLiveHours, DefaultMessageTimeToLiveTooltip);
                this.toolTip.SetToolTip(this.txtDefaultMessageTimeToLiveMinutes, DefaultMessageTimeToLiveTooltip);
                this.toolTip.SetToolTip(this.txtDefaultMessageTimeToLiveSeconds, DefaultMessageTimeToLiveTooltip);
                this.toolTip.SetToolTip(this.txtDefaultMessageTimeToLiveMilliseconds, DefaultMessageTimeToLiveTooltip);
                this.toolTip.SetToolTip(this.txtFilter, FilterExpressionTooltip);
                this.toolTip.SetToolTip(this.txtAction, FilterActionTooltip);
                this.toolTip.SetToolTip(this.txtLockDurationDays, LockDurationTooltip);
                this.toolTip.SetToolTip(this.txtLockDurationHours, LockDurationTooltip);
                this.toolTip.SetToolTip(this.txtLockDurationMinutes, LockDurationTooltip);
                this.toolTip.SetToolTip(this.txtLockDurationSeconds, LockDurationTooltip);
                this.toolTip.SetToolTip(this.txtLockDurationMilliseconds, LockDurationTooltip);
                this.toolTip.SetToolTip(this.txtMaxDeliveryCount, MaxDeliveryCountTooltip);
                this.toolTip.SetToolTip(this.txtAutoDeleteOnIdleDays, AutoDeleteOnIdleTooltip);
                this.toolTip.SetToolTip(this.txtAutoDeleteOnIdleHours, AutoDeleteOnIdleTooltip);
                this.toolTip.SetToolTip(this.txtAutoDeleteOnIdleMinutes, AutoDeleteOnIdleTooltip);
                this.toolTip.SetToolTip(this.txtAutoDeleteOnIdleSeconds, AutoDeleteOnIdleTooltip);
                this.toolTip.SetToolTip(this.txtAutoDeleteOnIdleMilliseconds, AutoDeleteOnIdleTooltip);
            }
            else
            {
                // Tab pages
                this.DisablePage(MetricsTabPage);

                // Initialize buttons
                this.btnCreateDelete.Text = CreateText;
                this.btnCancelUpdate.Text = CancelText;
                this.btnRefresh.Visible = false;
                this.btnChangeStatus.Visible = false;
                this.btnMessages.Visible = false;
                this.btnSessions.Visible = false;
                this.btnDeadletter.Visible = false;
                this.btnMetrics.Visible = false;
                this.btnCloseTabs.Visible = false;
                this.txtName.Focus();
            }
        }

        private void InitializeData()
        {
            // Initialize buttons
            this.btnCreateDelete.Text = DeleteText;
            this.btnCancelUpdate.Text = UpdateText;
            this.btnChangeStatus.Text = this.subscriptionWrapper.SubscriptionDescription.Status == EntityStatus.Active ? DisableText : EnableText;
            this.btnRefresh.Visible = true;
            this.btnChangeStatus.Visible = true;
            this.btnMessages.Visible = true;
            this.btnSessions.Visible = this.subscriptionWrapper.SubscriptionDescription.RequiresSession;
            this.btnMessages.Visible = string.IsNullOrWhiteSpace(this.subscriptionWrapper.SubscriptionDescription.ForwardTo);
            this.btnDeadletter.Visible = string.IsNullOrWhiteSpace(this.subscriptionWrapper.SubscriptionDescription.ForwardDeadLetteredMessagesTo);

            if (!this.btnMessages.Visible && !this.btnSessions.Visible)
            {
                this.btnMetrics.Location = this.btnCloseTabs.Location;
                this.btnCloseTabs.Location = this.btnMessages.Location;
            }
            if (!this.btnMessages.Visible && this.btnSessions.Visible)
            {
                this.btnMetrics.Location = this.btnCloseTabs.Location;
                this.btnCloseTabs.Location = this.btnSessions.Location;
                this.btnSessions.Location = this.btnMessages.Location;
            }
            if (this.btnMessages.Visible && !this.btnSessions.Visible)
            {
                this.btnMetrics.Location = this.btnCloseTabs.Location;
                this.btnCloseTabs.Location = this.btnSessions.Location;
            }

            this.btnMetrics.Visible = this.serviceBusHelper.IsCloudNamespace;
            this.btnDeadletter.Visible = true;
            this.btnOpenFilterForm.Enabled = false;
            this.btnOpenActionForm.Enabled = false;

            // Initialize property grid
            var propertyList = new List<string[]>();

            propertyList.AddRange(new[]
            {
                new[] { Status, this.subscriptionWrapper.SubscriptionDescription.Status.ToString() },
                new[] { IsReadOnly, this.subscriptionWrapper.SubscriptionDescription.IsReadOnly.ToString() },
                new[] { CreatedAt, this.subscriptionWrapper.SubscriptionDescription.CreatedAt.ToString(CultureInfo.CurrentCulture) },
                new[] { AccessedAt, this.subscriptionWrapper.SubscriptionDescription.AccessedAt.ToString(CultureInfo.CurrentCulture) },
                new[] { UpdatedAt, this.subscriptionWrapper.SubscriptionDescription.UpdatedAt.ToString(CultureInfo.CurrentCulture) },
                new[] { ActiveMessageCount, this.subscriptionWrapper.SubscriptionDescription.MessageCountDetails.ActiveMessageCount.ToString("N0") },
                new[] { DeadletterMessageCount, this.subscriptionWrapper.SubscriptionDescription.MessageCountDetails.DeadLetterMessageCount.ToString("N0") },
                new[] { ScheduledMessageCount, this.subscriptionWrapper.SubscriptionDescription.MessageCountDetails.ScheduledMessageCount.ToString("N0") },
                new[] { TransferMessageCount, this.subscriptionWrapper.SubscriptionDescription.MessageCountDetails.TransferMessageCount.ToString("N0") },
                new[] { TransferDeadLetterMessageCount, this.subscriptionWrapper.SubscriptionDescription.MessageCountDetails.TransferDeadLetterMessageCount.ToString("N0") },
                new[] { MessageCount, this.subscriptionWrapper.SubscriptionDescription.MessageCount.ToString("N0") }
            });

            this.propertyListView.Items.Clear();
            foreach (var array in propertyList)
            {
                this.propertyListView.Items.Add(new ListViewItem(array));
            }

            // Name
            if (!string.IsNullOrWhiteSpace(this.subscriptionWrapper.SubscriptionDescription.Name))
            {
                this.txtName.Text = this.subscriptionWrapper.SubscriptionDescription.Name;
            }

            // UserMetadata
            if (!string.IsNullOrWhiteSpace(this.subscriptionWrapper.SubscriptionDescription.UserMetadata))
            {
                this.txtUserMetadata.Text = this.subscriptionWrapper.SubscriptionDescription.UserMetadata;
            }

            // ForwardTo
            if (!string.IsNullOrWhiteSpace(this.subscriptionWrapper.SubscriptionDescription.ForwardTo))
            {
                int i;
                this.txtForwardTo.Text = !string.IsNullOrWhiteSpace(this.subscriptionWrapper.SubscriptionDescription.ForwardTo) &&
                                         (i = this.subscriptionWrapper.SubscriptionDescription.ForwardTo.IndexOf('/')) > 0 &&
                                         i < this.subscriptionWrapper.SubscriptionDescription.ForwardTo.Length - 1 ? this.subscriptionWrapper.SubscriptionDescription.ForwardTo.Substring(this.subscriptionWrapper.SubscriptionDescription.ForwardTo.LastIndexOf('/') + 1) : this.subscriptionWrapper.SubscriptionDescription.ForwardTo;
            }

            // ForwardDeadLetteredMessagesTo
            if (!string.IsNullOrWhiteSpace(this.subscriptionWrapper.SubscriptionDescription.ForwardDeadLetteredMessagesTo))
            {
                int i;
                this.txtForwardDeadLetteredMessagesTo.Text = !string.IsNullOrWhiteSpace(this.subscriptionWrapper.SubscriptionDescription.ForwardDeadLetteredMessagesTo) &&
                                                             (i = this.subscriptionWrapper.SubscriptionDescription.ForwardDeadLetteredMessagesTo.IndexOf('/')) > 0 &&
                                                             i < this.subscriptionWrapper.SubscriptionDescription.ForwardDeadLetteredMessagesTo.Length - 1 ? this.subscriptionWrapper.SubscriptionDescription.ForwardDeadLetteredMessagesTo.Substring(this.subscriptionWrapper.SubscriptionDescription.ForwardDeadLetteredMessagesTo.LastIndexOf('/') + 1) : this.subscriptionWrapper.SubscriptionDescription.ForwardDeadLetteredMessagesTo;
            }

            // MaxDeliveryCount
            this.txtMaxDeliveryCount.Text = this.subscriptionWrapper.SubscriptionDescription.MaxDeliveryCount.ToString(CultureInfo.InvariantCulture);

            // DefaultMessageTimeToLive
            this.txtDefaultMessageTimeToLiveDays.Text = this.subscriptionWrapper.SubscriptionDescription.DefaultMessageTimeToLive.Days.ToString(CultureInfo.InvariantCulture);
            this.txtDefaultMessageTimeToLiveHours.Text = this.subscriptionWrapper.SubscriptionDescription.DefaultMessageTimeToLive.Hours.ToString(CultureInfo.InvariantCulture);
            this.txtDefaultMessageTimeToLiveMinutes.Text = this.subscriptionWrapper.SubscriptionDescription.DefaultMessageTimeToLive.Minutes.ToString(CultureInfo.InvariantCulture);
            this.txtDefaultMessageTimeToLiveSeconds.Text = this.subscriptionWrapper.SubscriptionDescription.DefaultMessageTimeToLive.Seconds.ToString(CultureInfo.InvariantCulture);
            this.txtDefaultMessageTimeToLiveMilliseconds.Text = this.subscriptionWrapper.SubscriptionDescription.DefaultMessageTimeToLive.Milliseconds.ToString(CultureInfo.InvariantCulture);

            // LockDuration
            this.txtLockDurationDays.Text = this.subscriptionWrapper.SubscriptionDescription.LockDuration.Days.ToString(CultureInfo.InvariantCulture);
            this.txtLockDurationHours.Text = this.subscriptionWrapper.SubscriptionDescription.LockDuration.Hours.ToString(CultureInfo.InvariantCulture);
            this.txtLockDurationMinutes.Text = this.subscriptionWrapper.SubscriptionDescription.LockDuration.Minutes.ToString(CultureInfo.InvariantCulture);
            this.txtLockDurationSeconds.Text = this.subscriptionWrapper.SubscriptionDescription.LockDuration.Seconds.ToString(CultureInfo.InvariantCulture);
            this.txtLockDurationMilliseconds.Text = this.subscriptionWrapper.SubscriptionDescription.LockDuration.Milliseconds.ToString(CultureInfo.InvariantCulture);

            // AutoDeleteOnIdle
            this.txtAutoDeleteOnIdleDays.Text = this.subscriptionWrapper.SubscriptionDescription.AutoDeleteOnIdle.Days.ToString(CultureInfo.InvariantCulture);
            this.txtAutoDeleteOnIdleHours.Text = this.subscriptionWrapper.SubscriptionDescription.AutoDeleteOnIdle.Hours.ToString(CultureInfo.InvariantCulture);
            this.txtAutoDeleteOnIdleMinutes.Text = this.subscriptionWrapper.SubscriptionDescription.AutoDeleteOnIdle.Minutes.ToString(CultureInfo.InvariantCulture);
            this.txtAutoDeleteOnIdleSeconds.Text = this.subscriptionWrapper.SubscriptionDescription.AutoDeleteOnIdle.Seconds.ToString(CultureInfo.InvariantCulture);
            this.txtAutoDeleteOnIdleMilliseconds.Text = this.subscriptionWrapper.SubscriptionDescription.AutoDeleteOnIdle.Milliseconds.ToString(CultureInfo.InvariantCulture);

            // EnableDeadLetteringOnFilterEvaluationExceptions
            this.checkedListBox.SetItemChecked(EnableBatchedOperationsIndex, this.subscriptionWrapper.SubscriptionDescription.EnableBatchedOperations);

            // EnableDeadLetteringOnFilterEvaluationExceptions
            this.checkedListBox.SetItemChecked(EnableDeadLetteringOnFilterEvaluationExceptionsIndex, this.subscriptionWrapper.SubscriptionDescription.EnableDeadLetteringOnFilterEvaluationExceptions);

            // EnableDeadLetteringOnMessageExpiration
            this.checkedListBox.SetItemChecked(EnableDeadLetteringOnMessageExpirationIndex, this.subscriptionWrapper.SubscriptionDescription.EnableDeadLetteringOnMessageExpiration);

            // RequiresSession
            this.checkedListBox.SetItemChecked(RequiresSessionIndex, this.subscriptionWrapper.SubscriptionDescription.RequiresSession);
        }

        private void listView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            var startX = e.ColumnIndex == 0 ? -1 : e.Bounds.X;
            var endX = e.Bounds.X + e.Bounds.Width - 1;
            // Background
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(215, 228, 242)), startX, -1, e.Bounds.Width + 1, e.Bounds.Height + 1);
            // Left vertical line
            e.Graphics.DrawLine(new Pen(SystemColors.ControlLightLight), startX, -1, startX, e.Bounds.Y + e.Bounds.Height + 1);
            // TopCount horizontal line
            e.Graphics.DrawLine(new Pen(SystemColors.ControlLightLight), startX, -1, endX, -1);
            // Bottom horizontal line
            e.Graphics.DrawLine(new Pen(SystemColors.ControlDark), startX, e.Bounds.Height - 1, endX, e.Bounds.Height - 1);
            // Right vertical line
            e.Graphics.DrawLine(new Pen(SystemColors.ControlDark), endX, -1, endX, e.Bounds.Height + 1);
            var roundedFontSize = (float)Math.Round(e.Font.SizeInPoints);
            var bounds = new RectangleF(e.Bounds.X + 4, (e.Bounds.Height - 8 - roundedFontSize) / 2, e.Bounds.Width, roundedFontSize + 6);
            e.Graphics.DrawString(e.Header.Text, e.Font, new SolidBrush(SystemColors.ControlText), bounds);
        }

        private void listView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawText();
        }

        private void listView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawText();
        }

        private void listView_Resize(object sender, EventArgs e)
        {
            var listView = sender as ListView;
            if (listView == null)
            {
                return;
            }
            try
            {
                listView.SuspendDrawing();
                listView.SuspendLayout();
                var width = listView.Width - listView.Columns[0].Width - 5;
                var scrollbars = ScrollBarHelper.GetVisibleScrollbars(listView);
                if (scrollbars == ScrollBars.Vertical || scrollbars == ScrollBars.Both)
                {
                    width -= 17;
                }
                listView.Columns[1].Width = width;
            }
            finally
            {
                listView.ResumeLayout();
                listView.ResumeDrawing();
            }
        }

        private void mainTabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            this.DrawTabControlTabs(this.mainTabControl, e, null);
        }

        private void mainTabControl_Selected(object sender, TabControlEventArgs e)
        {
            if (string.Compare(e.TabPage.Name, MetricsTabPage, StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                return;
            }
            Task.Run(() =>
            {
                this.metricsManualResetEvent.WaitOne();
                var dataGridViewComboBoxColumn = (DataGridViewComboBoxColumn)this.dataPointDataGridView.Columns[MetricProperty];
                if (dataGridViewComboBoxColumn != null)
                {
                    dataGridViewComboBoxColumn.DataSource = MetricInfo.EntityMetricDictionary.ContainsKey(SubscriptionEntity)
                        ? MetricInfo.EntityMetricDictionary[SubscriptionEntity]
                        : null;
                }
            });
        }

        private void messagesDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            var bindingList = this.messagesBindingSource.DataSource as BindingList<BrokeredMessage>;
            if (bindingList == null)
            {
                return;
            }
            using (var messageForm = new MessageForm(bindingList[e.RowIndex], this.serviceBusHelper, this.writeToLog))
            {
                messageForm.ShowDialog();
            }
        }

        private void messagesDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var cell = this.messagesDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
            cell.ToolTipText = DoubleClickMessage;
        }

        private void messagesDataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || e.RowIndex == -1)
            {
                return;
            }
            this.messagesDataGridView.Rows[e.RowIndex].Selected = true;
            var multipleSelectedRows = this.messagesDataGridView.SelectedRows.Count > 1;
            this.repairAndResubmitMessageToolStripMenuItem.Visible = !multipleSelectedRows;
            this.saveSelectedMessageToolStripMenuItem.Visible = !multipleSelectedRows;
            this.resubmitSelectedMessagesInBatchModeToolStripMenuItem.Visible = multipleSelectedRows;
            this.saveSelectedMessagesToolStripMenuItem.Visible = multipleSelectedRows;
            this.messagesContextMenuStrip.Show(Cursor.Position);
        }

        private void messagesDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void messagesDataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            var bindingList = this.messagesBindingSource.DataSource as BindingList<BrokeredMessage>;
            this.currentMessageRowIndex = e.RowIndex;
            if (bindingList == null)
            {
                return;
            }
            if (this.brokeredMessage != bindingList[e.RowIndex])
            {
                this.brokeredMessage = bindingList[e.RowIndex];
                this.messagePropertyGrid.SelectedObject = this.brokeredMessage;
                BodyType bodyType;
                this.txtMessageText.Text = XmlHelper.Indent(this.serviceBusHelper.GetMessageText(this.brokeredMessage, out bodyType));
                var listViewItems = this.brokeredMessage.Properties.Select(p => new ListViewItem(new[] { p.Key, Convert.ToString(p.Value) })).ToArray();
                this.messagePropertyListView.Items.Clear();
                this.messagePropertyListView.Items.AddRange(listViewItems);
            }
        }

        private void openOpenFilterForm_Click(object sender, EventArgs e)
        {
            using (var form = new TextForm(FilterExpression, this.txtFilter.Text))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    this.txtFilter.Text = form.Content;
                }
            }
        }

        private void pictFindDeadletter_Click(object sender, EventArgs e)
        {
            try
            {
                this.deadletterDataGridView.SuspendDrawing();
                this.deadletterDataGridView.SuspendLayout();
                if (this.deadletterBindingList == null)
                {
                    return;
                }
                using (var form = new TextForm(FilterExpressionTitle, FilterExpressionLabel, this.deadletterFilterExpression))
                {
                    form.Size = new Size(600, 200);
                    if (form.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    this.deadletterFilterExpression = form.Content;
                    if (string.IsNullOrWhiteSpace(this.deadletterFilterExpression))
                    {
                        this.deadletterBindingSource.DataSource = this.deadletterBindingList;
                        this.deadletterDataGridView.DataSource = this.deadletterBindingSource;
                        this.writeToLog(FilterExpressionRemovedMessage);
                    }
                    else
                    {
                        Filter filter;
                        try
                        {
                            var sqlFilter = new SqlFilter(this.deadletterFilterExpression);
                            sqlFilter.Validate();
                            filter = sqlFilter.Preprocess();
                        }
                        catch (Exception ex)
                        {
                            this.writeToLog(string.Format(FilterExpressionNotValidMessage, this.deadletterFilterExpression, ex.Message));
                            return;
                        }
                        var filteredList = this.deadletterBindingList.Where(filter.Match).ToList();
                        var bindingList = new SortableBindingList<BrokeredMessage>(filteredList)
                        {
                            AllowEdit = false,
                            AllowNew = false,
                            AllowRemove = false
                        };
                        this.deadletterBindingSource.DataSource = bindingList;
                        this.deadletterDataGridView.DataSource = this.deadletterBindingSource;
                        this.writeToLog(string.Format(FilterExpressionAppliedMessage, this.deadletterFilterExpression, bindingList.Count));
                    }
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
            finally
            {
                this.deadletterDataGridView.ResumeLayout();
                this.deadletterDataGridView.ResumeDrawing();
            }
        }

        private void pictFindMessages_Click(object sender, EventArgs e)
        {
            try
            {
                this.messagesDataGridView.SuspendDrawing();
                this.messagesDataGridView.SuspendLayout();
                if (this.messageBindingList == null)
                {
                    return;
                }
                using (var form = new TextForm(FilterExpressionTitle, FilterExpressionLabel, this.messagesFilterExpression))
                {
                    form.Size = new Size(600, 200);
                    if (form.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    this.messagesFilterExpression = form.Content;
                    if (string.IsNullOrWhiteSpace(this.messagesFilterExpression))
                    {
                        this.messagesBindingSource.DataSource = this.messageBindingList;
                        this.messagesDataGridView.DataSource = this.messagesBindingSource;
                        this.writeToLog(FilterExpressionRemovedMessage);
                    }
                    else
                    {
                        Filter filter;
                        try
                        {
                            var sqlFilter = new SqlFilter(this.messagesFilterExpression);
                            sqlFilter.Validate();
                            filter = sqlFilter.Preprocess();
                        }
                        catch (Exception ex)
                        {
                            this.writeToLog(string.Format(FilterExpressionNotValidMessage, this.messagesFilterExpression, ex.Message));
                            return;
                        }
                        var filteredList = this.messageBindingList.Where(filter.Match).ToList();
                        var bindingList = new SortableBindingList<BrokeredMessage>(filteredList)
                        {
                            AllowEdit = false,
                            AllowNew = false,
                            AllowRemove = false
                        };
                        this.messagesBindingSource.DataSource = bindingList;
                        this.messagesDataGridView.DataSource = this.messagesBindingSource;
                        this.writeToLog(string.Format(FilterExpressionAppliedMessage, this.messagesFilterExpression, bindingList.Count));
                    }
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
            finally
            {
                this.messagesDataGridView.ResumeLayout();
                this.messagesDataGridView.ResumeDrawing();
            }
        }

        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            var pictureBox = sender as PictureBox;
            if (pictureBox != null)
            {
                pictureBox.Image = Resources.FindExtensionRaised;
            }
        }

        private void pictureBox_MouseLeave(object sender, EventArgs e)
        {
            var pictureBox = sender as PictureBox;
            if (pictureBox != null)
            {
                pictureBox.Image = Resources.FindExtension;
            }
        }

        private void ReadDeadletterMessagesOneAtTheTime(bool peek, bool all, int count, IBrokeredMessageInspector messageInspector)
        {
            try
            {
                var brokeredMessages = new List<BrokeredMessage>();

                if (peek)
                {
                    var messageReceiver = this.serviceBusHelper.MessagingFactory.CreateMessageReceiver(SubscriptionClient.FormatDeadLetterPath(this.subscriptionWrapper.SubscriptionDescription.TopicPath, this.subscriptionWrapper.SubscriptionDescription.Name),
                        ReceiveMode.PeekLock);
                    for (var i = 0; i < count; i++)
                    {
                        var message = messageReceiver.Peek();
                        if (message != null)
                        {
                            if (messageInspector != null)
                            {
                                message = messageInspector.AfterReceiveMessage(message);
                            }
                            brokeredMessages.Add(message);
                        }
                    }
                    this.writeToLog(string.Format(MessagesPeekedFromTheDeadletterQueue, brokeredMessages.Count, this.subscriptionWrapper.SubscriptionDescription.Name));
                }
                else
                {
                    var messageReceiver = this.serviceBusHelper.MessagingFactory.CreateMessageReceiver(SubscriptionClient.FormatDeadLetterPath(this.subscriptionWrapper.SubscriptionDescription.TopicPath, this.subscriptionWrapper.SubscriptionDescription.Name),
                        ReceiveMode.ReceiveAndDelete);
                    var totalRetrieved = 0;
                    int retrieved;
                    do
                    {
                        var message = messageReceiver.Receive(TimeSpan.FromSeconds(MainForm.SingletonMainForm.ReceiveTimeout));
                        retrieved = message != null ? 1 : 0;
                        if (retrieved == 0)
                        {
                            continue;
                        }
                        totalRetrieved += retrieved;
                        brokeredMessages.Add(messageInspector != null ? messageInspector.AfterReceiveMessage(message) : message);
                    } while (retrieved > 0 && (all || count > totalRetrieved));
                    this.writeToLog(string.Format(MessagesReceivedFromTheDeadletterQueue, brokeredMessages.Count, this.subscriptionWrapper.SubscriptionDescription.Name));
                }
                this.deadletterBindingList = new SortableBindingList<BrokeredMessage>(brokeredMessages)
                {
                    AllowEdit = false,
                    AllowNew = false,
                    AllowRemove = false
                };
                this.deadletterBindingSource.DataSource = this.deadletterBindingList;
                this.deadletterDataGridView.DataSource = this.deadletterBindingSource;

                this.deadletterSplitContainer.SplitterDistance = this.deadletterSplitContainer.Width -
                                                                 GrouperMessagePropertiesWith - this.deadletterSplitContainer.SplitterWidth;
                this.deadletterListTextPropertiesSplitContainer.SplitterDistance = this.deadletterListTextPropertiesSplitContainer.Size.Height / 2 - 8;
                this.deadletterCustomPropertiesSplitContainer.SplitterDistance = this.deadletterCustomPropertiesSplitContainer.Size.Width / 2 - 8;

                if (!peek)
                {
                    if (this.OnRefresh != null)
                    {
                        this.OnRefresh();
                    }
                }
                if (this.mainTabControl.TabPages[DeadletterTabPage] == null)
                {
                    this.EnablePage(DeadletterTabPage);
                }
                if (this.mainTabControl.TabPages[DeadletterTabPage] != null)
                {
                    this.mainTabControl.SelectTab(DeadletterTabPage);
                }
            }
            catch (TimeoutException)
            {
                this.writeToLog(string.Format(NoMessageReceivedFromTheDeadletterQueue,
                    MainForm.SingletonMainForm.ReceiveTimeout, this.subscriptionWrapper.SubscriptionDescription.Name));
            }
            catch (Exception e)
            {
                this.HandleException(e);
            }
        }

        private void ReadMessagesOneAtTheTime(bool peek, bool all, int count, IBrokeredMessageInspector messageInspector)
        {
            try
            {
                var brokeredMessages = new List<BrokeredMessage>();
                if (peek)
                {
                    var subscriptionClient = this.serviceBusHelper.MessagingFactory.CreateSubscriptionClient(this.subscriptionWrapper.SubscriptionDescription.TopicPath, this.subscriptionWrapper.SubscriptionDescription.Name,
                        ReceiveMode.PeekLock);
                    for (var i = 0; i < count; i++)
                    {
                        var message = subscriptionClient.Peek();
                        if (message != null)
                        {
                            if (messageInspector != null)
                            {
                                message = messageInspector.AfterReceiveMessage(message);
                            }
                            brokeredMessages.Add(message);
                        }
                    }
                    this.writeToLog(string.Format(MessagesPeekedFromTheSubscription, brokeredMessages.Count, this.subscriptionWrapper.SubscriptionDescription.Name));
                }
                else
                {
                    MessageReceiver messageReceiver;
                    if (this.subscriptionWrapper.SubscriptionDescription.RequiresSession)
                    {
                        var subscriptionClient = this.serviceBusHelper.MessagingFactory.CreateSubscriptionClient(this.subscriptionWrapper.SubscriptionDescription.TopicPath, this.subscriptionWrapper.SubscriptionDescription.Name,
                            ReceiveMode.ReceiveAndDelete);
                        messageReceiver = subscriptionClient.AcceptMessageSession(TimeSpan.FromSeconds(MainForm.SingletonMainForm.ReceiveTimeout));
                    }
                    else
                    {
                        messageReceiver = this.serviceBusHelper.MessagingFactory.CreateMessageReceiver(SubscriptionClient.FormatSubscriptionPath(this.subscriptionWrapper.SubscriptionDescription.TopicPath, this.subscriptionWrapper.SubscriptionDescription.Name),
                            ReceiveMode.ReceiveAndDelete);
                    }

                    var totalRetrieved = 0;
                    int retrieved;
                    do
                    {
                        var message = messageReceiver.Receive(TimeSpan.FromSeconds(MainForm.SingletonMainForm.ReceiveTimeout));
                        retrieved = message != null ? 1 : 0;
                        if (retrieved == 0)
                        {
                            continue;
                        }
                        totalRetrieved += retrieved;
                        brokeredMessages.Add(messageInspector != null ? messageInspector.AfterReceiveMessage(message) : message);
                    } while (retrieved > 0 && (all || count > totalRetrieved));
                    this.writeToLog(string.Format(MessagesReceivedFromTheSubscription, brokeredMessages.Count, this.subscriptionWrapper.SubscriptionDescription.Name));
                }
                this.messageBindingList = new SortableBindingList<BrokeredMessage>(brokeredMessages)
                {
                    AllowEdit = false,
                    AllowNew = false,
                    AllowRemove = false
                };
                this.messagesBindingSource.DataSource = this.messageBindingList;
                this.messagesDataGridView.DataSource = this.messagesBindingSource;

                this.messagesSplitContainer.SplitterDistance = this.messagesSplitContainer.Width -
                                                               GrouperMessagePropertiesWith - this.messagesSplitContainer.SplitterWidth;
                this.messageListTextPropertiesSplitContainer.SplitterDistance = this.messageListTextPropertiesSplitContainer.Size.Height / 2 - 8;
                this.messagesCustomPropertiesSplitContainer.SplitterDistance = this.messagesCustomPropertiesSplitContainer.Size.Width / 2 - 8;

                if (!peek)
                {
                    if (this.OnRefresh != null)
                    {
                        this.OnRefresh();
                    }
                }
                if (this.mainTabControl.TabPages[MessagesTabPage] == null)
                {
                    this.EnablePage(MessagesTabPage);
                }
                if (this.mainTabControl.TabPages[MessagesTabPage] != null)
                {
                    this.mainTabControl.SelectTab(MessagesTabPage);
                }
            }
            catch (TimeoutException)
            {
                this.writeToLog(string.Format(NoMessageReceivedFromTheSubscription,
                    MainForm.SingletonMainForm.ReceiveTimeout, this.subscriptionWrapper.SubscriptionDescription.Name));
            }
            catch (Exception e)
            {
                this.HandleException(e);
            }
        }

        private void repairAndResubmitDeadletterMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.deadletterDataGridView_CellDoubleClick(this.deadletterDataGridView, new DataGridViewCellEventArgs(0, this.currentDeadletterMessageRowIndex));
        }

        private void repairAndResubmitMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.messagesDataGridView_CellDoubleClick(this.messagesDataGridView, new DataGridViewCellEventArgs(0, this.currentMessageRowIndex));
        }

        private async void resubmitSelectedDeadletterMessagesInBatchModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.deadletterDataGridView.SelectedRows.Count <= 0)
                {
                    return;
                }
                string entityPath;
                using (var form = new SelectEntityForm(SelectEntityDialogTitle, SelectEntityGrouperTitle, SelectEntityLabelText))
                {
                    if (form.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(form.Path))
                    {
                        return;
                    }
                    entityPath = form.Path;
                }
                var sent = 0;
                var messageSender = await this.serviceBusHelper.MessagingFactory.CreateMessageSenderAsync(entityPath);
                var messages = this.deadletterDataGridView.SelectedRows.Cast<DataGridViewRow>().Select(r =>
                {
                    BodyType bodyType;
                    var message = r.DataBoundItem as BrokeredMessage;
                    this.serviceBusHelper.GetMessageText(message, out bodyType);
                    if (bodyType == BodyType.Wcf)
                    {
                        var wcfUri = this.serviceBusHelper.IsCloudNamespace ?
                            new Uri(this.serviceBusHelper.NamespaceUri, messageSender.Path) :
                            new UriBuilder
                            {
                                Host = this.serviceBusHelper.NamespaceUri.Host,
                                Path = string.Format("{0}/{1}", this.serviceBusHelper.NamespaceUri.AbsolutePath, messageSender.Path),
                                Scheme = "sb"
                            }.Uri;
                        return this.serviceBusHelper.CreateMessageForWcfReceiver(message,
                            0,
                            false,
                            false,
                            wcfUri);
                    }
                    return this.serviceBusHelper.CreateMessageForApiReceiver(message,
                        0,
                        false,
                        false,
                        false,
                        bodyType,
                        null);
                });
                IEnumerable<BrokeredMessage> brokeredMessages = messages as IList<BrokeredMessage> ?? messages.ToList();
                if (brokeredMessages.Any())
                {
                    sent = brokeredMessages.Count();
                    await messageSender.SendBatchAsync(brokeredMessages);
                }
                this.writeToLog(string.Format(MessageSentMessage, sent, entityPath));
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
        }

        private async void resubmitSelectedMessagesInBatchModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.messagesDataGridView.SelectedRows.Count <= 0)
                {
                    return;
                }
                string entityPath;
                using (var form = new SelectEntityForm(SelectEntityDialogTitle, SelectEntityGrouperTitle, SelectEntityLabelText))
                {
                    if (form.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(form.Path))
                    {
                        return;
                    }
                    entityPath = form.Path;
                }
                var sent = 0;
                var messageSender = await this.serviceBusHelper.MessagingFactory.CreateMessageSenderAsync(entityPath);
                var messages = this.messagesDataGridView.SelectedRows.Cast<DataGridViewRow>().Select(r =>
                {
                    BodyType bodyType;
                    var message = r.DataBoundItem as BrokeredMessage;
                    this.serviceBusHelper.GetMessageText(message, out bodyType);
                    if (bodyType == BodyType.Wcf)
                    {
                        var wcfUri = this.serviceBusHelper.IsCloudNamespace ?
                            new Uri(this.serviceBusHelper.NamespaceUri, messageSender.Path) :
                            new UriBuilder
                            {
                                Host = this.serviceBusHelper.NamespaceUri.Host,
                                Path = string.Format("{0}/{1}", this.serviceBusHelper.NamespaceUri.AbsolutePath, messageSender.Path),
                                Scheme = "sb"
                            }.Uri;
                        return this.serviceBusHelper.CreateMessageForWcfReceiver(message,
                            0,
                            false,
                            false,
                            wcfUri);
                    }
                    return this.serviceBusHelper.CreateMessageForApiReceiver(message,
                        0,
                        false,
                        false,
                        false,
                        bodyType,
                        null);
                });
                IEnumerable<BrokeredMessage> brokeredMessages = messages as IList<BrokeredMessage> ?? messages.ToList();
                if (brokeredMessages.Any())
                {
                    sent = brokeredMessages.Count();
                    await messageSender.SendBatchAsync(brokeredMessages);
                }
                this.writeToLog(string.Format(MessageSentMessage, sent, entityPath));
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
        }

        private void saveSelectedDeadletteredMessagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.deadletterDataGridView.SelectedRows.Count <= 0)
                {
                    return;
                }
                var messages = this.deadletterDataGridView.SelectedRows.Cast<DataGridViewRow>().Select(r => r.DataBoundItem as BrokeredMessage);
                IEnumerable<BrokeredMessage> brokeredMessages = messages as BrokeredMessage[] ?? messages.ToArray();
                if (!brokeredMessages.Any())
                {
                    return;
                }
                this.saveFileDialog.Title = SaveAsTitle;
                this.saveFileDialog.DefaultExt = JsonExtension;
                this.saveFileDialog.Filter = JsonFilter;
                this.saveFileDialog.FileName = this.CreateFileName();
                if (this.saveFileDialog.ShowDialog() != DialogResult.OK ||
                    string.IsNullOrWhiteSpace(this.saveFileDialog.FileName))
                {
                    return;
                }
                if (File.Exists(this.saveFileDialog.FileName))
                {
                    File.Delete(this.saveFileDialog.FileName);
                }
                using (var writer = new StreamWriter(this.saveFileDialog.FileName))
                {
                    BodyType bodyType;
                    var bodies = brokeredMessages.Select(bm => this.serviceBusHelper.GetMessageText(bm, out bodyType));
                    writer.Write(MessageSerializationHelper.Serialize(brokeredMessages, bodies));
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
        }

        private void saveSelectedDeadletteredMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.currentDeadletterMessageRowIndex < 0)
                {
                    return;
                }
                var bindingList = this.deadletterBindingSource.DataSource as BindingList<BrokeredMessage>;
                if (bindingList == null)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(this.txtDeadletterText.Text))
                {
                    return;
                }
                this.saveFileDialog.Title = SaveAsTitle;
                this.saveFileDialog.DefaultExt = JsonExtension;
                this.saveFileDialog.Filter = JsonFilter;
                this.saveFileDialog.FileName = this.CreateFileName();
                if (this.saveFileDialog.ShowDialog() != DialogResult.OK ||
                    string.IsNullOrWhiteSpace(this.saveFileDialog.FileName))
                {
                    return;
                }
                if (File.Exists(this.saveFileDialog.FileName))
                {
                    File.Delete(this.saveFileDialog.FileName);
                }
                using (var writer = new StreamWriter(this.saveFileDialog.FileName))
                {
                    writer.Write(MessageSerializationHelper.Serialize(bindingList[this.currentDeadletterMessageRowIndex], this.txtDeadletterText.Text));
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
        }

        private void saveSelectedMessagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.messagesDataGridView.SelectedRows.Count <= 0)
                {
                    return;
                }
                var messages = this.messagesDataGridView.SelectedRows.Cast<DataGridViewRow>().Select(r => r.DataBoundItem as BrokeredMessage);
                IEnumerable<BrokeredMessage> brokeredMessages = messages as BrokeredMessage[] ?? messages.ToArray();
                if (!brokeredMessages.Any())
                {
                    return;
                }
                this.saveFileDialog.Title = SaveAsTitle;
                this.saveFileDialog.DefaultExt = JsonExtension;
                this.saveFileDialog.Filter = JsonFilter;
                this.saveFileDialog.FileName = this.CreateFileName();
                if (this.saveFileDialog.ShowDialog() != DialogResult.OK ||
                    string.IsNullOrWhiteSpace(this.saveFileDialog.FileName))
                {
                    return;
                }
                if (File.Exists(this.saveFileDialog.FileName))
                {
                    File.Delete(this.saveFileDialog.FileName);
                }
                using (var writer = new StreamWriter(this.saveFileDialog.FileName))
                {
                    BodyType bodyType;
                    var bodies = brokeredMessages.Select(bm => this.serviceBusHelper.GetMessageText(bm, out bodyType));
                    writer.Write(MessageSerializationHelper.Serialize(brokeredMessages, bodies));
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
        }

        private void saveSelectedMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.currentMessageRowIndex < 0)
                {
                    return;
                }
                var bindingList = this.messagesBindingSource.DataSource as BindingList<BrokeredMessage>;
                if (bindingList == null)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(this.txtMessageText.Text))
                {
                    return;
                }
                this.saveFileDialog.Title = SaveAsTitle;
                this.saveFileDialog.DefaultExt = JsonExtension;
                this.saveFileDialog.Filter = JsonFilter;
                this.saveFileDialog.FileName = this.CreateFileName();
                if (this.saveFileDialog.ShowDialog() != DialogResult.OK ||
                    string.IsNullOrWhiteSpace(this.saveFileDialog.FileName))
                {
                    return;
                }
                if (File.Exists(this.saveFileDialog.FileName))
                {
                    File.Delete(this.saveFileDialog.FileName);
                }
                using (var writer = new StreamWriter(this.saveFileDialog.FileName))
                {
                    writer.Write(MessageSerializationHelper.Serialize(bindingList[this.currentMessageRowIndex], this.txtMessageText.Text));
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
        }

        private void sessionsDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void sessionsDataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            var bindingList = this.sessionsBindingSource.DataSource as BindingList<MessageSession>;
            if (bindingList == null)
            {
                return;
            }
            var messageSession = bindingList[e.RowIndex];
            this.sessionPropertyGrid.SelectedObject = messageSession;
            var stream = messageSession.GetState();
            if (stream == null)
            {
                return;
            }
            using (stream)
            {
                using (var reader = new StreamReader(stream))
                {
                    this.txtSessionState.Text = reader.ReadToEnd();
                }
            }
        }

        private void tabPageMessages_Resize(object sender, EventArgs e)
        {
        }

        private void tabPageSessions_Resize(object sender, EventArgs e)
        {
            this.sessionPropertyGrid.Size = new Size(this.grouperSessionProperties.Size.Width - 32, this.sessionPropertyGrid.Size.Height);
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.OnKeyPress(e);

            var numberFormatInfo = CultureInfo.CurrentCulture.NumberFormat;
            var decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
            var groupSeparator = numberFormatInfo.NumberGroupSeparator;
            var negativeSign = numberFormatInfo.NegativeSign;

            var keyInput = e.KeyChar.ToString(CultureInfo.InvariantCulture);

            if (Char.IsDigit(e.KeyChar))
            {
                // Digits are OK
            }
            else if (keyInput.Equals(decimalSeparator) || keyInput.Equals(groupSeparator) ||
                     keyInput.Equals(negativeSign))
            {
                // Decimal separator is OK
            }
            else if (e.KeyChar == '\b')
            {
                // Backspace key is OK
            }
            else if (e.KeyChar == ' ')
            {
            }
            else
            {
                // Swallow this invalid key and beep
                e.Handled = true;
            }
        }

        #endregion Private Methods
    }
}

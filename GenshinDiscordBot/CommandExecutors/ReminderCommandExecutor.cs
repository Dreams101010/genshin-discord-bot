using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DomainModels.HelperModels;
using GenshinDiscordBotDomainLayer.Interfaces.Services;
using GenshinDiscordBotDomainLayer.Helpers;
using GenshinDiscordBotUI.ResponseGenerators;
using GenshinDiscordBotDomainLayer.Helpers.Time;
using Serilog;
using GenshinDiscordBotDomainLayer.DomainModels;
using GenshinDiscordBotDomainLayer.ValidationLogic;
using System.Globalization;
using System.Threading;
using GenshinDiscordBotDomainLayer.Contexts;
using GenshinDiscordBotDomainLayer.BusinessLogic;

namespace GenshinDiscordBotUI.CommandExecutors
{
    public class ReminderCommandExecutor : CommandExecutorBase
    {
        private IReminderService ReminderService { get; set; }
        private ILogger Logger { get; }
        private GeneralResponseGenerator GeneralResponseGenerator { get; set; }
        private ReminderResponseGenerator ReminderResponseGenerator { get; set; }
        public ReminderConversionHelper ReminderConversionHelper { get; }
        public DateTimeBusinessLogic DateTimeBusinessLogic { get; set; }
        public DateTimeArgumentValidator DateTimeArgumentValidator { get; set; }
        private IUserService UserService { get; set; }
        public RequestContext Context { get; }

        public ReminderCommandExecutor(IReminderService reminderService, ILogger logger, 
            GeneralResponseGenerator generalResponseGenerator,
            ReminderResponseGenerator reminderResponseGenerator,
            ReminderConversionHelper reminderConversionHelper,
            DateTimeArgumentValidator dateTimeArgumentValidator,
            DateTimeBusinessLogic dateTimeBusinessLogic,
            IUserService userService,
            RequestContext requestContext) : base(userService, logger, requestContext)
        {
            ReminderService = reminderService ?? throw new ArgumentNullException(nameof(reminderService));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            GeneralResponseGenerator = generalResponseGenerator ?? throw new ArgumentNullException(nameof(generalResponseGenerator));
            ReminderResponseGenerator = reminderResponseGenerator ?? throw new ArgumentNullException(nameof(reminderResponseGenerator));
            ReminderConversionHelper = reminderConversionHelper ?? throw new ArgumentNullException(nameof(reminderConversionHelper));
            DateTimeArgumentValidator = dateTimeArgumentValidator ?? throw new ArgumentNullException(nameof(dateTimeArgumentValidator));
            DateTimeBusinessLogic = dateTimeBusinessLogic ?? throw new ArgumentNullException(nameof(dateTimeBusinessLogic));
            UserService = userService ?? throw new ArgumentNullException(nameof(userService));
            Context = requestContext ?? throw new ArgumentNullException(nameof(requestContext));
        }

        public async Task<string> UpdateOrCreateReminderAsync(string messageText, string timeSpanStr)
        {
            try
            {
                var id = Context.DiscordContext.UserId;
                var userLocale = Context.UserContext.User.Locale;
                var userName = Context.DiscordContext.UserName;
                var messageContext = new DiscordMessageContext
                {
                    UserDiscordId = id,
                    GuildId = Context.DiscordContext.GuildId,
                    ChannelId = Context.DiscordContext.ChannelId,
                };
                if (!DateTimeBusinessLogic.ParseTimeSpan(timeSpanStr, out TimeSpan timeSpan))
                {
                    return ReminderResponseGenerator
                        .GetReminderTimeSpanInvalid(userLocale, userName);
                }
                await ReminderService.UpdateOrCreateReminderAsync(messageContext, messageText, timeSpan);
                return ReminderResponseGenerator.GetReminderSetupSuccessMessage(userLocale, userName);
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        // TODO: put parsing logic into separate class
        public async Task<string> UpdateOrCreateReminderByDateAsync(string messageText, string dateTimeStr)
        {
            try
            {
                var id = Context.DiscordContext.UserId;
                var userLocale = Context.UserContext.User.Locale;
                var userName = Context.DiscordContext.UserName;
                var messageContext = new DiscordMessageContext
                {
                    UserDiscordId = id,
                    GuildId = Context.DiscordContext.GuildId,
                    ChannelId = Context.DiscordContext.ChannelId,
                };
                CultureInfo culture = Context.GetUserCulture();
                if (!DateTimeBusinessLogic.ParseLocalDateTime(dateTimeStr, culture, out DateTime dateTime))
                {
                    return ReminderResponseGenerator
                        .GetReminderDateTimeInvalid(userLocale, userName);
                }
                if (!DateTimeArgumentValidator.IsInFuture(dateTime))
                {
                    return ReminderResponseGenerator
                        .GetReminderDateTimeNotInFuture(userLocale, userName);
                }
                await ReminderService.UpdateOrCreateReminderAsync(messageContext, messageText, dateTime);
                return ReminderResponseGenerator.GetReminderSetupSuccessMessage(userLocale, userName);
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> UpdateOrCreateRecurrentReminderAsync(
            string messageText, string timeSpanStr)
        {
            try
            {
                var id = Context.DiscordContext.UserId;
                var userLocale = Context.UserContext.User.Locale;
                var userName = Context.DiscordContext.UserName;
                var messageContext = new DiscordMessageContext
                {
                    UserDiscordId = id,
                    GuildId = Context.DiscordContext.GuildId,
                    ChannelId = Context.DiscordContext.ChannelId,
                };
                if (!DateTimeBusinessLogic.ParseTimeSpan(timeSpanStr, out TimeSpan timeSpan))
                {
                    return ReminderResponseGenerator
                        .GetReminderTimeSpanInvalid(userLocale, userName);
                }
                await ReminderService.UpdateOrCreateRecurrentReminderAsync(messageContext, messageText, timeSpan);
                return ReminderResponseGenerator.GetReminderSetupSuccessMessage(userLocale, userName);
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> UpdateOrCreateRecurrentReminderAsync(
            string messageText, string startDateTimeStr, string timeSpanStr)
        {
            try
            {
                var id = Context.DiscordContext.UserId;
                var userLocale = Context.UserContext.User.Locale;
                var userName = Context.DiscordContext.UserName;
                var messageContext = new DiscordMessageContext
                {
                    UserDiscordId = id,
                    GuildId = Context.DiscordContext.GuildId,
                    ChannelId = Context.DiscordContext.ChannelId,
                };
                CultureInfo culture = Context.GetUserCulture();
                if (!DateTimeBusinessLogic.ParseLocalDateTime(
                    startDateTimeStr, culture, out DateTime dateTime))
                {
                    return ReminderResponseGenerator
                        .GetReminderDateTimeInvalid(userLocale, userName);
                }
                if (!DateTimeArgumentValidator.IsInFuture(dateTime))
                {
                    return ReminderResponseGenerator
                        .GetReminderDateTimeNotInFuture(userLocale, userName);
                }
                if (!DateTimeBusinessLogic.ParseTimeSpan(timeSpanStr, out TimeSpan timeSpan))
                {
                    return ReminderResponseGenerator
                        .GetReminderTimeSpanInvalid(userLocale, userName);
                }
                await ReminderService.UpdateOrCreateRecurrentReminderAsync(messageContext, messageText, dateTime, timeSpan);
                return ReminderResponseGenerator.GetReminderSetupSuccessMessage(userLocale, userName);
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> UpdateOrCreateArtifactReminderAsync()
        {
			try
			{
                var id = Context.DiscordContext.UserId;
                var userLocale = Context.UserContext.User.Locale;
                var userName = Context.DiscordContext.UserName;
                var messageContext = new DiscordMessageContext
                {
                    UserDiscordId = id,
                    GuildId = Context.DiscordContext.GuildId,
                    ChannelId = Context.DiscordContext.ChannelId,
                };
                await ReminderService.UpdateOrCreateArtifactReminderAsync(messageContext);
                return ReminderResponseGenerator.GetArtifactReminderSetupSuccessMessage(userLocale, userName);
			}
			catch (Exception e)
			{
				Logger.Error($"An error has occured while handling a command: {e}");
				string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
				return errorMessage;
			}
		}

        public async Task<string> UpdateOrCreateArtifactReminderWithCustomTimeAsync(string time)
        {
            try
            {
                var id = Context.DiscordContext.UserId;
                var userLocale = Context.UserContext.User.Locale;
                var userName = Context.DiscordContext.UserName;
                var messageContext = new DiscordMessageContext
                {
                    UserDiscordId = id,
                    GuildId = Context.DiscordContext.GuildId,
                    ChannelId = Context.DiscordContext.ChannelId,
                };
                var parseSuccess = TimeParser.TryParseTime(time, out Time timeObject);
                if (!parseSuccess)
                {
                    return ReminderResponseGenerator
                        .GetReminderTimeInvalid(userLocale, userName);
                }
                var timeOnly = new TimeOnly(timeObject.Hours, timeObject.Minutes);
                await ReminderService.UpdateOrCreateArtifactReminderWithCustomTimeAsync(messageContext, timeOnly);
                return ReminderResponseGenerator
                    .GetArtifactReminderSetupSuccessMessageWithCustomTime(userLocale, timeOnly, userName);
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> RemoveArtifactRemindersForUserAsync()
        {
            try
            {
                var id = Context.DiscordContext.UserId;
                var userLocale = Context.UserContext.User.Locale;
                var userName = Context.DiscordContext.UserName;
                var messageContext = new DiscordMessageContext
                {
                    UserDiscordId = id,
                    GuildId = Context.DiscordContext.GuildId,
                    ChannelId = Context.DiscordContext.ChannelId,
                };
                var result = await ReminderService.RemoveArtifactRemindersForUserAsync(messageContext);
                if (result)
                {
                    return ReminderResponseGenerator
                        .GetArtifactReminderCancelSuccessMessage(userLocale, userName);
                }
                else
                {
                    return ReminderResponseGenerator
                        .GetArtifactReminderCancelNotFoundMessage(userLocale, userName);
                }
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> UpdateOrCreateCheckInReminderAsync()
        {
            try
            {
                var id = Context.DiscordContext.UserId;
                var userLocale = Context.UserContext.User.Locale;
                var userName = Context.DiscordContext.UserName;
                var messageContext = new DiscordMessageContext
                {
                    UserDiscordId = id,
                    GuildId = Context.DiscordContext.GuildId,
                    ChannelId = Context.DiscordContext.ChannelId,
                };
                await ReminderService.UpdateOrCreateCheckInReminderAsync(messageContext);
                return ReminderResponseGenerator.GetCheckInReminderSetupSuccessMessage(userLocale, userName);
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> UpdateOrCreateCheckInReminderWithCustomTimeAsync(string time)
        {
            try
            {
                var id = Context.DiscordContext.UserId;
                var userLocale = Context.UserContext.User.Locale;
                var userName = Context.DiscordContext.UserName;
                var messageContext = new DiscordMessageContext
                {
                    UserDiscordId = id,
                    GuildId = Context.DiscordContext.GuildId,
                    ChannelId = Context.DiscordContext.ChannelId,
                };
                var parseSuccess = TimeParser.TryParseTime(time, out Time timeObject);
                if (!parseSuccess)
                {
                    return ReminderResponseGenerator.GetReminderTimeInvalid(userLocale, userName);
                }
                var timeOnly = new TimeOnly(timeObject.Hours, timeObject.Minutes);
                await ReminderService.UpdateOrCreateCheckInReminderWithCustomTimeAsync(messageContext, timeOnly);
                return ReminderResponseGenerator
                    .GetCheckInReminderSetupSuccessMessageWithCustomTime(
                        userLocale, timeOnly, userName);
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> RemoveCheckInRemindersForUserAsync()
        {
            try
            {
                var id = Context.DiscordContext.UserId;
                var userLocale = Context.UserContext.User.Locale;
                var userName = Context.DiscordContext.UserName;
                var messageContext = new DiscordMessageContext
                {
                    UserDiscordId = id,
                    GuildId = Context.DiscordContext.GuildId,
                    ChannelId = Context.DiscordContext.ChannelId,
                };
                var result = await ReminderService.RemoveCheckInRemindersForUserAsync(messageContext);
                if (result)
                {
                    return ReminderResponseGenerator
                        .GetCheckInReminderCancelSuccessMessage(userLocale, userName);
                }
                else
                {
                    return ReminderResponseGenerator
                        .GetCheckInReminderCancelNotFoundMessage(userLocale, userName);
                }
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> UpdateOrCreateSereniteaPotPlantHarvestReminderAsync()
        {
            try
            {
                var id = Context.DiscordContext.UserId;
                var userLocale = Context.UserContext.User.Locale;
                var userName = Context.DiscordContext.UserName;
                var messageContext = new DiscordMessageContext
                {
                    UserDiscordId = id,
                    GuildId = Context.DiscordContext.GuildId,
                    ChannelId = Context.DiscordContext.ChannelId,
                };
                await ReminderService.UpdateOrCreateSereniteaPotPlantHarvestReminderAsync(messageContext);
                return ReminderResponseGenerator
                    .GetSereniteaPotPlantHarvestSetupSuccessMessage(userLocale, userName);
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> UpdateOrCreateSereniteaPotPlantHarvestReminderAsync(int days, int hours)
        {
            try
            {
                var id = Context.DiscordContext.UserId;
                var userLocale = Context.UserContext.User.Locale;
                var userName = Context.DiscordContext.UserName;
                var messageContext = new DiscordMessageContext
                {
                    UserDiscordId = id,
                    GuildId = Context.DiscordContext.GuildId,
                    ChannelId = Context.DiscordContext.ChannelId,
                };
                string validationErrorMessage = ReminderResponseGenerator
                    .GetUpdateOrCreateSereniteaPotPlantHarvestReminderValidationErrorMessage(
                    userLocale, days, hours, userName);
                if (validationErrorMessage != string.Empty)
                {
                    return validationErrorMessage;
                }
                await ReminderService.UpdateOrCreateSereniteaPotPlantHarvestReminderAsync(messageContext, days, hours);
                return ReminderResponseGenerator
                    .GetSereniteaPotPlantHarvestSetupSuccessMessageWithCustomTime(
                        userLocale, days, hours, userName);
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> RemoveSereniteaPotPlantHarvestRemindersForUserAsync()
        {
            try
            {
                var id = Context.DiscordContext.UserId;
                var userLocale = Context.UserContext.User.Locale;
                var userName = Context.DiscordContext.UserName;
                var messageContext = new DiscordMessageContext
                {
                    UserDiscordId = id,
                    GuildId = Context.DiscordContext.GuildId,
                    ChannelId = Context.DiscordContext.ChannelId,
                };
                var result = await ReminderService.RemoveSereniteaPotPlantHarvestRemindersForUserAsync(messageContext);
                if (result)
                {
                    return ReminderResponseGenerator
                        .GetSereniteaPotPlantHarvestCancelSuccessMessage(userLocale, userName);
                }
                else
                {
                    return ReminderResponseGenerator
                        .GetSereniteaPotPlantHarvestCheckInReminderCancelNotFoundMessage(userLocale, userName);
                }
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> UpdateOrCreateParametricTransformerReminderAsync()
        {
            try
            {
                var id = Context.DiscordContext.UserId;
                var userLocale = Context.UserContext.User.Locale;
                var userName = Context.DiscordContext.UserName;
                var messageContext = new DiscordMessageContext
                {
                    UserDiscordId = id,
                    GuildId = Context.DiscordContext.GuildId,
                    ChannelId = Context.DiscordContext.ChannelId,
                };
                await ReminderService.UpdateOrCreateParametricTransformerReminderAsync(messageContext);
                return ReminderResponseGenerator
                    .GetParametricTransformerReminderSetupSuccessMessage(userLocale, userName);
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> UpdateOrCreateParametricTransformerReminderAsync(int days, int hours)
        {
            try
            {
                var id = Context.DiscordContext.UserId;
                var userLocale = Context.UserContext.User.Locale;
                var userName = Context.DiscordContext.UserName;
                var messageContext = new DiscordMessageContext
                {
                    UserDiscordId = id,
                    GuildId = Context.DiscordContext.GuildId,
                    ChannelId = Context.DiscordContext.ChannelId,
                };
                string validationErrorMessage = ReminderResponseGenerator
                    .GetUpdateOrCreateParametricTransformerReminderCustomTimeValidationErrorMessage(
                    userLocale, days, hours, userName);
                if (validationErrorMessage != string.Empty)
                {
                    return validationErrorMessage;
                }
                await ReminderService.UpdateOrCreateParametricTransformerReminderAsync(messageContext, days, hours);
                return ReminderResponseGenerator
                    .GetParametricTransformerReminderSetupSuccessMessageWithCustomTime(
                        userLocale, days, hours, userName);
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> RemoveParametricTransformerRemindersForUserAsync()
        {
            try
            {
                var id = Context.DiscordContext.UserId;
                var userLocale = Context.UserContext.User.Locale;
                var userName = Context.DiscordContext.UserName;
                var messageContext = new DiscordMessageContext
                {
                    UserDiscordId = id,
                    GuildId = Context.DiscordContext.GuildId,
                    ChannelId = Context.DiscordContext.ChannelId,
                };
                var result = await ReminderService.RemoveParametricTransformerRemindersForUserAsync(messageContext);
                if (result)
                {
                    return ReminderResponseGenerator
                        .GetParametricTransformerReminderCancelSuccessMessage(userLocale, userName);
                }
                else
                {
                    return ReminderResponseGenerator
                        .GetParametricTransformerReminderCancelNotFoundMessage(userLocale, userName);
                }
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> GetRemindersForUserAsync()
        {
            try
            {
                var id = Context.DiscordContext.UserId;
                var userLocale = Context.UserContext.User.Locale;
                var userName = Context.DiscordContext.UserName;
                var messageContext = new DiscordMessageContext
                {
                    UserDiscordId = id,
                    GuildId = Context.DiscordContext.GuildId,
                    ChannelId = Context.DiscordContext.ChannelId,
                };
                var reminderList = await ReminderService.GetRemindersForUserAsync(messageContext);
                var reminderResultModelList = ReminderConversionHelper.GetReminderResultModelList(reminderList);
                var result = ReminderResponseGenerator
                    .GetReminderListString(userLocale, reminderResultModelList, userName);
                return result;
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }

        public async Task<string> RemoveReminderById(ulong reminderId)
        {
            try
            {
                var userId = Context.DiscordContext.UserId;
                var userLocale = Context.UserContext.User.Locale;
                var userName = Context.DiscordContext.UserName;
                bool successFlag = await ReminderService.RemoveReminderByIdAsync(userId, reminderId);
                if (successFlag)
                {
                    return ReminderResponseGenerator.GetReminderRemoveByIdSuccessMessage(userLocale);
                }
                else
                {
                    return ReminderResponseGenerator.GetReminderRemoveByIdNotFoundMessage(userLocale);
                }
            }
            catch (Exception e)
            {
                Logger.Error($"An error has occured while handling a command: {e}");
                string errorMessage = GeneralResponseGenerator.GetGeneralErrorMessage();
                return errorMessage;
            }
        }
    }
}

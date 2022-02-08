using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotSQLiteDataAccessLayer.Repositories;
using GenshinDiscordBotSQLiteDataAccessLayer.DataModels;
using GenshinDiscordBotDomainLayer.Parameters.Command;
using Serilog;

namespace GenshinDiscordBotSQLiteDataAccessLayer.Commands
{
    public class AddOrUpdateResinInfoCommand 
        : SQLiteAbstractCommand<AddOrUpdateResinInfoCommandParam, bool>
    {
        private ResinTrackingInfoRepository ResinRepository { get; }
        public AddOrUpdateResinInfoCommand(
            SQLiteConnectionProvider connectionProvider,
            ILogger logger,
            ResinTrackingInfoRepository resinRepository
            ) : base(connectionProvider, logger)
        {
            ResinRepository = resinRepository
                ?? throw new ArgumentNullException(nameof(resinRepository));
        }

        protected override async Task<bool> PayloadAsync(AddOrUpdateResinInfoCommandParam param)
        {
            var resinInfoDataModel = new ResinTrackingInfoDataModel(param.ResinInfo);
            await ResinRepository.AddOrUpdateResinCountAsync(resinInfoDataModel);
            return true;
        }
    }
}

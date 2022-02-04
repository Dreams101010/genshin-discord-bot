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
    public class AddOrUpdateUserSQLiteCommand : SQLiteAbstractCommand<AddOrUpdateUserCommandParam, bool>
    {
        private UserRepository UserRepository { get; }
        public AddOrUpdateUserSQLiteCommand(
            SQLiteConnectionProvider connectionProvider,
            ILogger logger,
            UserRepository userRepository
            ) : base(connectionProvider, logger)
        {
            UserRepository = userRepository 
                ?? throw new ArgumentNullException(nameof(userRepository));
        }
        protected override async Task<bool> PayloadAsync(AddOrUpdateUserCommandParam param)
        {
            var userDataModel = new UserDataModel(param.User);
            await UserRepository.InsertOrUpdateUserAsync(userDataModel);
            return true;
        }
    }
}

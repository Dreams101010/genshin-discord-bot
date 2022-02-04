using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotSQLiteDataAccessLayer.Repositories;
using GenshinDiscordBotSQLiteDataAccessLayer.DataModels;
using GenshinDiscordBotDomainLayer.Parameters.Query;
using GenshinDiscordBotDomainLayer.DomainModels;
using Serilog;

namespace GenshinDiscordBotSQLiteDataAccessLayer.Queries
{
    public class GetUserByDiscordIdQuery
        : SQLiteAbstractQuery<GetUserByDiscordIdQueryParam, User>
    {
        public UserRepository UserRepository { get; }
        public GetUserByDiscordIdQuery(
            SQLiteConnectionProvider connectionProvider,
            ILogger logger,
            UserRepository userRepository)
            : base(connectionProvider, logger)
        {
            UserRepository = userRepository 
                ?? throw new ArgumentNullException(nameof(userRepository));
        }

        protected override Task<User> PayloadAsync(GetUserByDiscordIdQueryParam param)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinDiscordBotDomainLayer.DataProviders;

namespace GenshinDiscordBotDomainLayer.ValidationLogic
{
    public class ResinCommandArgumentValidator
    {
        private ResinDataProvider DataProvider { get; }

        public ResinCommandArgumentValidator(ResinDataProvider dataProvider)
        {
            DataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        }

        public bool SetResinCount_ResinCountValid(int count)
        {
            if (count < DataProvider.MinResin || count > DataProvider.MaxResin)
            {
                return false;
            }
            return true;
        }

        public void SetResinCount_Validate(int resinCount)
        {
            var validationResult = SetResinCount_ResinCountValid(resinCount);
            if (!validationResult)
            {
                throw new ArgumentException("Invalid value of a parameter", nameof(resinCount));
            }
        }
    }
}

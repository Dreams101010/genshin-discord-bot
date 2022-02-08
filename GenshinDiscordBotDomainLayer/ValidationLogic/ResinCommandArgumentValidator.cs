using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.ValidationLogic
{
    public class ResinCommandArgumentValidator
    {
        const int MIN_RESIN = 0;
        const int MAX_RESIN = 160;
        public bool SetResinCount_ResinCountValid(int count)
        {
            if (count < MIN_RESIN || count > MAX_RESIN)
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

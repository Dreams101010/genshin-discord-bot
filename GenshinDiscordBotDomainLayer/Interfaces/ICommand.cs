﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinDiscordBotDomainLayer.Interfaces
{
    public interface ICommand<TParam, TResult>
    {
        public TResult Execute(TParam param, bool useTransaction = true);
    }
}

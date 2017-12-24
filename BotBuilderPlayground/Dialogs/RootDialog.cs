using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace BotBuilderPlayground.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        /// <inheritdoc />
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Hello, world!");
            await context.PostAsync("Type something to continue.");
            context.Wait(async (ctx, awaitable) =>
            {
                var message = await awaitable;
                await ctx.PostAsync("You said " + message.Text);
                ctx.Done<object>(null);
            });
        }
    }
}

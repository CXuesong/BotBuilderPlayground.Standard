using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

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
                await NoneIntent(ctx, null);
            });
        }

        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            PromptDialog.Confirm(context, AfterAskInvestigate, "Would you like us to investigate and follow-up?");
        }

        private async Task AfterAskInvestigate(IDialogContext context, IAwaitable<bool> result)
        {
            var investigate = await result;

            if (investigate)
                await context.PostAsync("OK, we're on the case.");
            else
                await context.PostAsync("No problem, we'll leave it.");

            context.Wait(MessageReceived);
        }

        private async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            await context.PostAsync("TODO MessageReceived");
            context.Done<object>(null);
        }

    }
}

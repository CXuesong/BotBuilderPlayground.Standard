using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotBuilderPlayground.Dialogs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Extensions.Logging;

namespace BotBuilderPlayground.Controllers
{

    [Route("api/[controller]")]
    [BotAuthentication]
    public class MessagesController : Controller
    {
        private ILogger logger;

        public MessagesController(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
            this.logger = loggerFactory.CreateLogger<MessagesController>();
        }

        public virtual async Task<IActionResult> Post([FromBody] Activity activity)
        {
            if (activity != null)
            {
                var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                // one of these will have an interface and process it
                switch (activity.GetActivityType())
                {
                    case ActivityTypes.Message:
                        await Conversation.SendAsync(activity, () => new RootDialog());
                        break;

                    case ActivityTypes.ConversationUpdate:
                    case ActivityTypes.ContactRelationUpdate:
                    case ActivityTypes.Typing:
                    case ActivityTypes.DeleteUserData:
                    default:
                        logger.LogWarning("Unknown activity type ignored: {0}", activity.GetActivityType());
                        break;
                }
            }
            return Ok();
        }
    }

}

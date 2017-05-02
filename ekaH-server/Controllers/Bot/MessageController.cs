using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Bot.Connector;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using ekaH_server.Bot.Dialogs;

namespace ekaH_server.Controllers.Bot
{
    [BotAuthentication]
    public class MessageController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new RootDialog());
            }
            
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}

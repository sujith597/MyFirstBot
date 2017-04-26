using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace MyFirstBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity incomingMessage)
        {
            if (incomingMessage.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(incomingMessage.ServiceUrl));
                
                // calculate something for us to return
                int length = (incomingMessage.Text ?? string.Empty).Length;
             
                // return our reply to the user
                Activity reply = incomingMessage.CreateReply($"You sent {incomingMessage.Text} which was {length} characters");
                
                await connector.Conversations.ReplyToActivityAsync(reply);
                await connector.Conversations.ReplyToActivityAsync(incomingMessage.CreateReply("Previous message length : " ));
                await connector.Conversations.ReplyToActivityAsync(incomingMessage.CreateReply("This Bot is crazy"));
            }
            else
            {
                HandleSystemMessage(incomingMessage);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}
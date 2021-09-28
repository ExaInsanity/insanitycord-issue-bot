using DSharpPlus.EventArgs;

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace IssueBot
{
    public static class Parser
    {
        public static void Parse(MessageCreateEventArgs message)
        {
            _ = Task.Run(async () =>
            {
                if(message.Author.IsBot || !message.Message.Content.Contains("##"))
                    return;

                String id = message.Message.Content.Split(' ')
                    .First(xm => xm.StartsWith("##"))
                    .AsSpan()[2..]
                    .ToString();

                ChannelData channel = (from v in IssueBot.Configuration.AssociatedChannels
                                       where v.Channels.Contains(message.Channel.Id)
                                       select v).First();

                if(channel == null)
				{
                    return;
				}

                await message.Channel.SendMessageAsync($"{IssueBot.Configuration.RepositoryIds[channel.Id]}/{id}");
            });
        }
    }
}

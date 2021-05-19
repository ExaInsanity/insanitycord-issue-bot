using DSharpPlus;
using DSharpPlus.EventArgs;

using Microsoft.Extensions.Logging;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IssueBot
{
    public class IssueBot
    {
        private static UInt64 IBDevChannel, StarnightDevChannel;
        private static UInt64 IBSupportChannel, StarnightSupportChannel;

        public async static Task Main()
        {
            StreamReader reader = new("./config.txt");

            String Token = reader.ReadLine();
            IBDevChannel = Convert.ToUInt64(reader.ReadLine());
            StarnightDevChannel = Convert.ToUInt64(reader.ReadLine());
            IBSupportChannel = Convert.ToUInt64(reader.ReadLine());
            StarnightSupportChannel = Convert.ToUInt64(reader.ReadLine());

            DiscordClient client = new(new DiscordConfiguration
            {
                Token = Token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged,
                AlwaysCacheMembers = false,
                MessageCacheSize = 0,
                MinimumLogLevel = LogLevel.Debug
            });

            await client.ConnectAsync();

            client.MessageCreated += Client_MessageCreated;

            await Task.Delay(-1);
        }

        private static async Task Client_MessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {
            if (!e.Message.Content.Contains("##"))
                return;

            // get the issue ID
            Int32 index = e.Message.Content.IndexOf("##");
            String issueRef = e.Message.Content[(index + 2)..].Split(' ')[0];

            if (e.Channel.Id == IBDevChannel || e.Channel.Id == IBSupportChannel)
                await e.Message.RespondAsync($"https://github.com/InsanityBot/insanitybot/issues/{issueRef}");

            else if (e.Channel.Id == StarnightDevChannel || e.Channel.Id == StarnightSupportChannel)
                await e.Message.RespondAsync($"https://github.com/InsanityBot/starnight/issues/{issueRef}");
        }
    }
}

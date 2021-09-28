using DSharpPlus;
using DSharpPlus.EventArgs;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IssueBot
{
    public class IssueBot
    {
        public static Config Configuration { get; set; }

        public async static Task Main()
        {
            if(!File.Exists("./config.json"))
            {
                StreamWriter writer = new(File.Create("./config.json"));
                writer.Write(JsonConvert.SerializeObject(new Config()));
                writer.Flush();
                return;
            }

            StreamReader reader = new("./config.json");
            Configuration = JsonConvert.DeserializeObject<Config>(reader.ReadToEnd());
            reader.Close();

            DiscordClient client = new(new DiscordConfiguration
            {
                Token = Configuration.Token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged,
                AlwaysCacheMembers = false,
                MessageCacheSize = 0,
                MinimumLogLevel = LogLevel.Information
            });

            await client.ConnectAsync();

            client.MessageCreated += Client_MessageCreated;

            await Task.Delay(-1);
        }

        private static Task Client_MessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {
            _ = Task.Run(() =>
            {
                Parser.Parse(e);
            });
            return Task.CompletedTask;
        }
    }
}

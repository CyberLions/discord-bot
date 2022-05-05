using System;
using Discord;
using Discord.WebSocket;

namespace CCSODiscordBot
{
	public class Logging
	{
		/// <summary>
        /// Relay log messages
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
		public static Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}
		/// <summary>
        /// Log shards created
        /// </summary>
        /// <param name="shard"></param>
        /// <returns></returns>
		public static Task ReadyAsync(DiscordSocketClient shard)
		{
			Console.WriteLine($"Shard Number {shard.ShardId} is connected and ready!");
			return Task.CompletedTask;
		}
	}
}


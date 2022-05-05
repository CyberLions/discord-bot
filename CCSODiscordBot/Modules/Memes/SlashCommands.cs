using Discord;
using Discord.Interactions;

namespace CCSODiscordBot.Modules.Memes
{
	public class SlashCommands : InteractionModuleBase<ShardedInteractionContext>
	{
		[SlashCommand("pop", "Pop Smoke", runMode: RunMode.Async)]
		[EnabledInDm(false)]
		[DefaultMemberPermissions(GuildPermission.SendMessages)]
		public async Task Pop()
        {
			// Ack the command:
			await DeferAsync(false);

            // pop smoke songs:
            string[] popSmokeSongs = new string[] {
                "https://www.youtube.com/watch?v=AzQJO6AyfaQ", // Coupe
                "https://www.youtube.com/watch?v=Q9pjm4cNsfc", // For the night
                "https://www.youtube.com/watch?v=uuodbSVO3z0", // What u know bout love <3
                "https://www.youtube.com/watch?v=usu0XY4QNB0", // Welcome to the party
                "https://www.youtube.com/watch?v=kx7P_ENnDPE", // Gatti
                "https://www.youtube.com/watch?v=wP1PpQT4oC8", // Get Back grrrt
                "https://www.youtube.com/watch?v=oorVWW9ywG0", // Dior
                "https://www.youtube.com/watch?v=EZkNUmVXg6U", // Element
                "https://www.youtube.com/watch?v=Yr2Nq-7mQoY", // AP SPICY I BUST A CHECK ON MY NIKES
                "https://www.youtube.com/watch?v=XLQ3O_SXw1I", // Ordinary
                "https://www.youtube.com/watch?v=OIf2V402q-0", // Beethoven
                "https://www.youtube.com/watch?v=cO7kNxEmygs", // Dr. Dre
                "https://www.youtube.com/watch?v=Mb5ez0iIVKI", // Smokepurp
                "https://www.youtube.com/watch?v=Wuod-IanlkA"  // Candyshop
            };

            // Create a Random object  
            Random rand = new Random();
            // Generate a random index less than the size of the array.  
            int index = rand.Next(popSmokeSongs.Length);

            // Reply to the command:
            await FollowupAsync("Woo back baby 💫💫" + "\n" + popSmokeSongs[index]);
        }

	}
}


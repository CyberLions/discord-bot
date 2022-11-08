using Discord;
using Discord.Interactions;

namespace CCSODiscordBot.Modules.Memes
{
	public class SlashCommands : InteractionModuleBase<ShardedInteractionContext>
	{
		[SlashCommand("pop", "Pop Smoke")]
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
        [SlashCommand("capy", "A random capybara.")]
        [EnabledInDm(false)]
        [DefaultMemberPermissions(GuildPermission.SendMessages)]
        public async Task Capy()
        {
            // Ack the command:
            await DeferAsync(false);

            // List capys:
            string[] capybaraImageLinks = new string[] {
                "https://i.imgur.com/lEy6WEe.jpeg",
                "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fexternal-preview.redd.it%2FduAmQ7534wbKWtdnnCkD4YeQ7qWHDsXRo5T3alpz2L0.jpg%3Fauto%3Dwebp%26s%3Dd69e944d602cf28009fab232b62e3213b260629c&f=1&nofb=1",
                "https://external-content.duckduckgo.com/iu/?u=http%3A%2F%2Fi.imgur.com%2FYHPplRv.png&f=1&nofb=1",
                "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2F4.bp.blogspot.com%2F-mn0-VShv4ik%2FUob35QzS6kI%2FAAAAAAAAFM0%2FUuLbtTX-tlk%2Fs1600%2FFunny-Capybara.jpg&f=1&nofb=1",
                "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fi.pinimg.com%2Foriginals%2F1d%2Ff1%2Fcd%2F1df1cd1a94efee7fc6204af50129bc9d.png&f=1&nofb=1",
                "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fi.pinimg.com%2Foriginals%2F4c%2F74%2F60%2F4c7460393cddcd8a468a07f6576d00fd.jpg&f=1&nofb=1",
                "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fpbs.twimg.com%2Fmedia%2FEcfOAmYX0AAhrEd.jpg&f=1&nofb=1",
                "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fwww.barnorama.com%2Fwp-content%2Fuploads%2F2019%2F04%2Fcapybara_10.gif&f=1&nofb=1",
                "https://external-content.duckduckgo.com/iu/?u=http%3A%2F%2Fwww.freakingnews.com%2Fpictures%2F12500%2FCapybara-Dean--12811.jpg&f=1&nofb=1",
                "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fcdn.vox-cdn.com%2Fthumbor%2FynK1OlLXH592bSEWHBdLxJ1uHXE%3D%2F0x435%3A4047x3133%2F1200x800%2Ffilters%3Afocal(0x435%3A4047x3133)%2Fcdn.vox-cdn.com%2Fuploads%2Fchorus_image%2Fimage%2F47657465%2FGettyImages-460739676.0.0.jpg&f=1&nofb=1",
                "https://www.youtube.com/watch?v=tRChhg_dpJ0",
                "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fpreview.redd.it%2Fvqfbuca6s6o71.jpg%3Fauto%3Dwebp%26s%3D806b06dd6360874e3aca1252bab49619895834fe&f=1&nofb=1",
                "https://www.youtube.com/watch?v=VEaNiE1cgvw",
                "https://external-content.duckduckgo.com/iu/?u=http%3A%2F%2F2.bp.blogspot.com%2F-_n_g5OOfgu8%2FUob3ec2KIfI%2FAAAAAAAAFMk%2FarPw13KvUho%2Fs1600%2FFunny-Capybara-Animals-Pic.jpg&f=1&nofb=1",
                "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fpbs.twimg.com%2Fmedia%2FBIpwDAcCAAAhVN5.jpg&f=1&nofb=1",
                "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fi.pinimg.com%2Foriginals%2Fc7%2Ff5%2Fc8%2Fc7f5c8873f54b7f3ea5830ffaaa4739c.jpg&f=1&nofb=1",
                "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fi.pinimg.com%2Foriginals%2F6a%2Fa9%2Fd9%2F6aa9d94dc9115280a3ff7efaa98f40cc.jpg&f=1&nofb=1",
                "https://external-content.duckduckgo.com/iu/?u=http%3A%2F%2Fcdn.vidible.tv%2Fprod%2F2015-07%2F21%2F552d1c3de4b0026284f3b130_cv1.jpg&f=1&nofb=1",
                "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fi.pinimg.com%2F736x%2Fb8%2F45%2Ff9%2Fb845f90961a9cd1f65b0b61b7e306c6d--crowns.jpg&f=1&nofb=1",
                "https://i.imgur.com/YwkIOAj.jpg"
            };

            // Create a Random object  
            Random rand = new Random();
            // Generate a random index less than the size of the array.  
            int index = rand.Next(capybaraImageLinks.Length);

            // Send the capy:
            await FollowupAsync(capybaraImageLinks[index]);
        }
        [SlashCommand("vim", "Everyone's favorite text editor on a cube.")]
        [EnabledInDm(false)]
        [DefaultMemberPermissions(GuildPermission.SendMessages)]
        public async Task VimCube()
        {
            // Create embed:
            var embed = new EmbedBuilder();
            embed.WithTitle("vim on a cube!");
            embed.WithUrl("https://github.com/oakes/vim_cubed");
            embed.WithDescription("It's on a cube!\n" +
                "[GitHub Repo](https://github.com/oakes/vim_cubed)");
            embed.WithImageUrl("https://github.com/oakes/vim_cubed/raw/master/vim3.gif");
            embed.WithColor(Color.Blue);

            // Reply:
            await RespondAsync(embed: embed.Build());
        }
        [SlashCommand("simp", "Petr simp")]
        [EnabledInDm(false)]
        [DefaultMemberPermissions(GuildPermission.SendMessages)]
        public async Task Simp()
        {
            await DeferAsync(false);
            string imgPath = Path.Join(Directory.GetCurrentDirectory(), "Modules/Memes/Media/simpy.png");
            await FollowupWithFileAsync(new FileAttachment(imgPath));
        }

        [SlashCommand("cbat", "CBAT is all you need")]
        [EnabledInDm(false)]
        [DefaultMemberPermissions(GuildPermission.SendMessages)]
        public async Task Cbat()
        {
            // Ack the command:
            await DeferAsync(false);

            // Reply to the command:
            await FollowupAsync("https://www.youtube.com/watch?v=KAwyWkksXuo");
        }
    }
}


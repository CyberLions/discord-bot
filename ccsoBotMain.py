import discord
import time
from discord import webhook
import ccsoBotReactions
import ccsoBotCreds
import ccsoBotScheduler
import requests
import random
from discord.ext import tasks, commands

# Main: Handles main bot code and links everything together. Will use commands to run, start, and stop features.

# ------------------------------------------------------------------------------------------------------------


# Intents library required for the on_raw_reaction_add function
intents = discord.Intents.default()
intents.reactions = True
client = discord.Client(intents=intents)

# ------------------------------------- Bot commands / events -------------------------------------
# ------------------------------------- 1) Reaction Role Message -------------------------------------
# Takes an emoji and returns a role based on the input, default case is None, this return is handled in on_raw__reaction_add()


@client.event
async def on_ready():
    # What do you want the bot to do at login?
    print('We have logged in as {0.user}'.format(client))

# ------------------------------------- 2) Youtube Parsing/Messaging -------------------------------------

# Checks for new content and posts
# https://discordpy.readthedocs.io/en/latest/ext/tasks/
# change the interval using this
''' Move to Content and Time file

@tasks.loop(hours=8)
async def grabSomeContent():
    print("fetching videos...")

    mustHaveChannels = ["UC0ArlFuFYMpEewyRBzdLHiw", "UCByOX6pW9k1OYKpDA2UHvJw", "UCLDnEn-TxejaDB8qm2AUhHQ",
                        "UCKGe7fZ_S788Jaspxg-_5Sg", "UCVeW9qkBjo3zosnqUbG7CFw", "UC0ZTPkdxlAKf-V33tqXwi3Q"]

    print("fetching new articles...")
    # ccsoBotScheduler.checkForArticles()

    contentEmbed = discord.Embed(
        title="New Content for You!", description="**The hottest content, straight to your inbox.**")

    latestVidEachChanel = []

    for channel in mustHaveChannels:
        print("sending")
        videos = []
        # SAVE API CALLS
        # UGH
        videos = ccsoBotScheduler.checkForUpdates(channel)
        print("mainvids: ", videos)

        latestVidEachChanel.append(videos[0])


    print(latestVidEachChanel)

    # FOR TEST BIG MESSAGE 
    sendWebhookMessage(latestVidEachChanel)

def sendWebhookMessage(videos):

    video = videos[0]

    descriptions = ""
    for video in videos:
        descriptions = descriptions + "\n" + "\n" + video['title']

    webhookUrl = ccsoBotCreds.getWebhookUrl()
    # for testing

    # TODO: branch for articles VS videos

    params = {'username': 'webhook-test',
              'avatar_url': "", 'content': "New video from {}!".format(video['channelTitle']), }
    params["embeds"] = [
        {
            'image': {
                "url": video['thumbnail'],
            },
            # "description": video['description'],
            "description": descriptions,
            "title": video['title'],
            "url": video['url'],
            "color": 4,  # colors: https://gist.github.com/thomasbnt/b6f455e2c7d743b796917fa3c205f812
            "author": {
                "name": video['channelTitle'],
                "url": video['channelId'],
            }
        }
    ]

    try:
        requests.post(webhookUrl, json=params)

    except:
        print("request error")
'''

# ------------------------------------- 3) Role Reactions -------------------------------------

@client.event
async def on_raw_reaction_add(payload):
    # Defines the message, reaction, user, and role variables

    # Waits for a specific message to be reacted to, then adds a user to a specific role
    await ccsoBotReactions.addRoleReaction(payload, client)
    # passing the client is probably doodoo; do this @ bootstrapping


# ------------------------------------- 4) Bot Commands -------------------------------------

@client.event
async def on_message(message):
    if message.author == client.user:
        return

    # if the message !rules is sent, it will send the rules message into #rules ADD: check for server admin role
    elif message.content.startswith("!rules"):
        print(message.author.roles)
        for role in message.author.roles:
            if role.name == "server-manager":
                rulesChannel = discord.utils.get(
                    client.get_all_channels(), name="rules")
                await rulesChannel.purge()
                rulesVar = discord.Embed(
                    title="CCSO Server Rules", description="")
                rulesVar.add_field(
                    name="Rules", value="Feature still in testing", inline=False)
                await rulesChannel.send(embed=rulesVar)

    # this command purges the roles channel and sends the message to react to
    elif message.content == "!embedRoles":
        # TODO: CHECK IF THE SENDER IS ADMIN SO THAT RANDOMS CAN'T JUST...
        await ccsoBotReactions.embedrolemessage()

    # pop smoke command
    elif message.content == "!pop":

        popSmokeSongs = ["https://www.youtube.com/watch?v=AzQJO6AyfaQ", "https://www.youtube.com/watch?v=Q9pjm4cNsfc",
                         "https://www.youtube.com/watch?v=uuodbSVO3z0", "https://www.youtube.com/watch?v=usu0XY4QNB0"]
        aSong = random.choice(popSmokeSongs)
        await message.channel.send("RIP the goat" + "\n" + aSong)
    

# ------------------------------------------------------------------------------------------------------------


def bootstrapping():

    # Initial log in and check from the bot
    # grab Discord API token from creds.py file
    token = ccsoBotCreds.getDiscordKey()
    gcpKey = ccsoBotCreds.getGCPKey()

    ccsoBotReactions.sendClientToken(client)

    # starts the content scheduler
    #grabSomeContent.start()
    print("bootstrapping done")
    client.run(token)


bootstrapping()

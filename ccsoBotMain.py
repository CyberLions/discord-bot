import discord
import time
import asyncio
from discord import webhook
import ccsoBotReactions
import ccsoBotCreds
import ccsoBotScheduler
import requests
import random
from discord.ext import tasks, commands

# Main: Handles main bot code and links everything together. Will use commands to run, start, and stop features.

# ------------------------------------------------------------------------------------------------------------

# Emojis for roles
emojis = {
    '👍',
    '👀',
    '🍉',
    '<:coolspot:854115885013663774>',
    '🏳️',
    '⚔️',
    '🛡️',
    '🎮',
    '⚪',
    '🔵',
    '🟤',
    '⚫',
    
}

staticRoles = {
    '1️⃣',
    '2️⃣',
    '3️⃣',
    '4️⃣',
    '⭐' 
}

dynamicRoles = {
    '🏳️',
    '⚔️',
    '🛡️',
    '🎮',
}

# Intents library required for the on_raw_reaction_add function
intents = discord.Intents.default()
intents.reactions = True

client = discord.Client(intents=intents)

# ------------------------------------- Bot commands / events -------------------------------------
# ------------------------------------- 1) Reaction Role Message -------------------------------------
# Takes an emoji and returns a role based on the input, default case is None, this return is handled in on_raw__reaction_add()


@client.event
async def on_ready():
    print('We have logged in as {0.user}'.format(client))

    # channel and message id of msg to react to for roles
    # TEST SERVER
    # channel_id = 852925086238900225
    # message_id = 876803239385923584

    # static_id = 876806469939519489
    # dynamic_id = 876806477745127494

    # FOR LIVE CCSO SERVER
    channel_id = 876897589889495070
    static_id = 876897950914187284
    dynamic_id = 876898145185964063
    
    
    # message = await client.get_channel(channel_id).fetch_message(message_id)

    staticMessage = await client.get_channel(channel_id).fetch_message(static_id)
    dynamicMessage = await client.get_channel(channel_id).fetch_message(dynamic_id)


    # Adds the emoji reactions to the message initially
    for emoji in emojis:
        # await message.add_reaction(emoji)
        print("")

    for role in staticRoles:
        await staticMessage.add_reaction(role)

    for role in dynamicRoles:
        await dynamicMessage.add_reaction(role)

# ------------------------------------- 2) Youtube Parsing/Messaging -------------------------------------


def startContentScheduler(gcpKey):
    print('...starting scheduler')
    ccsoBotScheduler.startScheduler()

# content test
# https://discordpy.readthedocs.io/en/latest/ext/tasks/
@tasks.loop(seconds=10.0)
async def grabSomeContent():
    print("ddddddddddddd")

    # SAVE API CALLS 
    # videos = ccsoBotScheduler.checkForUpdates()
    # print("mainvids: ", videos)

    # sendWebhookMessage(videos)


def sendWebhookMessage(videos):

    video = videos[0]

    webhookUrl = ccsoBotCreds.getWebhookUrl()
    # for testing
    webhookUrl = "https://discord.com/api/webhooks/877404142178557963/LduzUUPzaxzVCJV2u5uTPzkH4-aapzWHI_1GEuZj4KKMJRxkxOXXrnM5YGt2UyFnvbx3"

    # TODO: branch for articles VS videos 

    params = {'username': 'webhook-test',
              'avatar_url': "", 'content': "New video from {}!".format(video['channelTitle']), }
    params["embeds"] = [
        {
            'image': {
                "url": video['thumbnail'],
            },
            "description": video['description'],
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


# ------------------------------------- Talk to josh abt. modularizing these -------------------------------------

def rolePicker(string, message):
    role1 = discord.utils.get(message.guild.roles, name="test role 1")
    role2 = discord.utils.get(message.guild.roles, name="balls")
    role3 = discord.utils.get(message.guild.roles, name="super")
    role4 = discord.utils.get(message.guild.roles, name="server-manager"),
    
    ctfRole = discord.utils.get(message.guild.roles, name="CTF")
    cptcRole = discord.utils.get(message.guild.roles, name="Offense (CPTC)")
    ccdcRole = discord.utils.get(message.guild.roles, name="Defense (CCDC)")
    gamingRole = discord.utils.get(message.guild.roles, name="Gaming")

    firstYear = discord.utils.get(message.guild.roles, name="Freshman")
    secondYear = discord.utils.get(message.guild.roles, name="Sophomore")
    thirdYear = discord.utils.get(message.guild.roles, name="Junior")
    fourthYear = discord.utils.get(message.guild.roles, name="Senior")


    switcher = {
        '👍': role1,
        '👀': role2,
        '🍉': role3,
        '<:coolspot:854115885013663774>': role4,
        '🏳️': ctfRole,
        '⚔️': cptcRole,
        '🛡️': ccdcRole,
        '🎮': gamingRole,

        # '⚪': firstYear,
        # '🔵': secondYear,
        # '🟤': thirdYear,
        # '⚫': fourthYear, 
        '1️⃣': firstYear,
        '2️⃣': secondYear,
        '3️⃣': thirdYear,
        '4️⃣': fourthYear,
        '⭐': ""
    }
    return switcher.get(string, None)

# Waits for a specific message to be reacted to, then adds a user to a specific role


@client.event
async def on_raw_reaction_add(payload):
    # Defines the message, reaction, user, and role variables
    
    await ccsoBotReactions.addRoleReaction(payload, client)
    # passing the client is probably doodoo; do this @ bootstrapping





@client.event
async def on_message(message):
    if message.author == client.user:
        return
    elif message.content == "test":
        print("recognize command")
        embedVar = discord.Embed(
            title="balls in my face title", description="awesome description")
        embedVar.add_field(
            name="Field1", value="balls in my face 1", inline=False)
        embedVar.add_field(
            name="Field2", value="balls in my face 2", inline=True)
        await message.channel.send(embed=embedVar)

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
                    name="Rules", value="I have a ginormous colkc and \nalso my balls are ginorumes", inline=False)
                await rulesChannel.send(embed=rulesVar)

    # best cmmand
    elif message.content == "balls in my face":
        counter = 0
        while (True):
            await message.channel.send(message.content)
            time.sleep(1)
            counter += 1
            if counter > 10:
                return False

    # pop smoke command 
    elif message.content == "!pop":
        
        popSmokeSongs = ["https://www.youtube.com/watch?v=AzQJO6AyfaQ", "https://www.youtube.com/watch?v=Q9pjm4cNsfc", "https://www.youtube.com/watch?v=uuodbSVO3z0", "https://www.youtube.com/watch?v=usu0XY4QNB0"]
        aSong = random.choice(popSmokeSongs)
        await message.channel.send("RIP the goat" + "\n" + aSong)



# ------------------------------------------------------------------------------------------------------------


def bootstrapping():

    # Initial log in and check from the bot
    # grab Discord API token from creds.py file
    token = ccsoBotCreds.getDiscordKey()
    gcpKey = ccsoBotCreds.getGCPKey()

    print("bootstrapping done")
    # startContentScheduler(gcpKey)

    grabSomeContent.start()
    # starts the content scheduler
    # getting it done...
    client.run(token)
    



bootstrapping()

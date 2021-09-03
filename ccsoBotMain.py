import discord
from discord import webhook

import ccsoBotReactions
import botCreds
import ccsoBotCool
import ccsoBotScheduler

# Main: Handles main bot code and links everything together. Will use commands to run, start, and stop features.
# FLAG FOR TEST BED VS LIVE 

# TODO
'''
FLAG IN MAIN FOR WHETHER WE ARE ON TESTING OR LIVE 
FORMATTING FOR CONTENT MESSAGE 
WHAT IS THE INTERVAL WE WANT TO LOOK BACK TO FOR CONTENT? 
'''

# ------------------------------------------------------------------------------------------------------------
def setLiveOrTesting():

    areWeTesting = True
    # what needs to change? 
    # does each file need a method like this? (yes) 

    rules_channel_id = 123
    reaction_channel_id = 123
    content_channel_id = 123


    if(areWeTesting == True):
        #TESTING THINGS 
        rules_channel_id = 854122888771403807
        reaction_channel_id = 852925086238900225
        content_channel_id = 853734878034395167
        # TESTING CHANNEL NAMES
        # rules - "rules"
        # content - "links-test"
        # reactions - "reactions-channel"

    else:
        #LIVE THINGS 
        # REAL CHANNELS 
        rules_channel_id = 567055850292641792
        reaction_channel_id = 879214780764798996
        content_channel_id = 566484653331185675
        # rules
        # "role-selection"
        # "links

        #LIVE "BOT DEV"
        rules_channel_id = 883101230614859836
        reaction_channel_id = 878313648618098709
        content_channel_id = 877403989619142666
        # "react-test"
        # "link-test-live"
        # "bot-react-post-test"


    testingIds = [rules_channel_id, reaction_channel_id, content_channel_id]

    # CHANNEL NAMES 
    ccsoBotReactions.setLiveOrTesting(testingIds)
    ccsoBotCool.setLiveOrTesting(testingIds)
    ccsoBotScheduler.setLiveOrTesting(testingIds)
    

# Intents library required for the on_raw_reaction_add function
intents = discord.Intents.default()
intents.reactions = True
client =  discord.Client(intents=intents)

# ------------------------------------- Bot commands / events -------------------------------------
# ------------------------------------- 1) Reaction Role Message -------------------------------------
# Takes an emoji and returns a role based on the input, default case is None, this return is handled in on_raw__reaction_add()

# moved to ccsoreactions! 


# ------------------------------------- 2) Youtube Parsing/Messaging -------------------------------------

#MOVED TO ccsoBotScheduler!

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
        await ccsoBotReactions.embedRoleMessage()

    # pop smoke command
    elif message.content == "!pop":
        await ccsoBotCool.youCannotSayPopAndForgetTheSmoke(message)
    
    elif message.content == "!stop":
        await message.channel.send("stopping content scheduler...")
        # code that stops the content scheduler 

    # change the name of this command later 
    elif message.content == "!testformat":
        await ccsoBotScheduler.testFormmatedMessage()

# ------------------------------------------------------------------------------------------------------------

@client.event
async def on_ready():
    # What do you want the bot to do at login?
    print('We have logged in as {0.user}'.format(client))
    # start the contentscheduler 
    ccsoBotScheduler.grabSomeContent.start()

# ------------------------------------------------------------------------------------------------------------

def bootstrapping():

    # Initial log in and check from the bot
    # grab Discord API token from creds.py file
    token = botCreds.getDiscordKey()
    gcpKey = botCreds.getGCPKey()

    # ARE WE TESTING? 
    setLiveOrTesting()

    ccsoBotReactions.setClientToken(client)
    # setting global client var for scheduler 
    ccsoBotScheduler.setClientToken(client)

    # starts the content scheduler
    # will this still work if...
    
    print("bootstrapping done")
    client.run(token)
    
bootstrapping()

from discord.ext import tasks, commands
import os
import time
import sched
from datetime import datetime

import ccsoBotYtParse
import botCreds
import ccsoBotArticleParse
import discord

# Content and time: Links the content features and has the main timing logic for posting articles.
# Timing logic will have a boolean feature within so that it can be stopped if it isn’t working from a command.

# ------------------------------------------------------------------------------------------------------------

def setClientToken(token):
    print("client set!")
    global client
    client = token
    # sets global var pog


def getTime():
    # datetime object containing current date and time
    now = datetime.now()

    # replaces with an hour before
    currHour = now.hour
    now = now.replace(hour=currHour-4)

    dt_string = now.strftime("%Y/%m/%d %H:%M:%S")
    print("date and time =", dt_string)

    # good code
    dtSplit = dt_string.split(".")[0]
    dtRFC = dtSplit.replace(" ", "T")
    dtRFC = dtRFC.replace("/", "-")
    dtRFC = dtRFC + "Z"
    print(dtRFC)

    return dtRFC


videos = []

def checkForArticles():
    link = ""
    ccsoBotArticleParse.grabAnArticle(link)


def checkForUpdates(channel_id):
    print("checking for updates...")
    gcpKey = botCreds.getGCPKey()

    date = getTime()
    # RFC formatted time
    # date = '2021-03-01T00:00:00Z'
    videos = ccsoBotYtParse.search_videos(channel_id, date, gcpKey)
    return videos


async def testFormmatedMessage():
    # get # of written content sources
    # get # of videos
    # separate messages for each

    # TODO: command that lists video  and article sources 
    print("SEE COMMENT")


# Checks for new content and posts
# https://discordpy.readthedocs.io/en/latest/ext/tasks/
# change the interval using this
@tasks.loop(hours=8)
async def grabSomeContent():
    # VIDEOS
    print("fetching videos...")

    mustHaveChannels = ["UC0ArlFuFYMpEewyRBzdLHiw", "UCByOX6pW9k1OYKpDA2UHvJw", "UCLDnEn-TxejaDB8qm2AUhHQ",
                        "UCKGe7fZ_S788Jaspxg-_5Sg", "UCVeW9qkBjo3zosnqUbG7CFw", "UC0ZTPkdxlAKf-V33tqXwi3Q"]

    latestVidEachChanel = []

    for channel in mustHaveChannels:
        print("sending")
        videos = []
        # SAVE API CALLS
        videos = checkForUpdates(channel)

        if(videos == []):
            print("no new videos!")
        else:
            latestVidEachChanel.append(videos[0])


    # change this based on testing...
    contentChannel = client.get_channel(853734878034395167)
        
    contentEmbed = discord.Embed(
        title="New Content for You!", description="**The hottest content, straight to your inbox.**")

    for video in latestVidEachChanel:

        contentEmbed.add_field(
            name="{} - {}".format(video['title'], video['channelTitle']), value=video['url'] + "\n" + video['description'] + "\n", inline=False)
    
    await contentChannel.send(embed=contentEmbed)

    # ARTICLES 
    print("fetching new articles...")
    # ccsoBotScheduler.checkForArticles()



def setLiveOrTesting(liveOrTest):
    print("changing from live to testing...")
    print(liveOrTest)

    

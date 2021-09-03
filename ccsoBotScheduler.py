import os
import time
import sched
from datetime import datetime

import ccsoBotYtParse
import ccsoBotCreds
import ccsoBotArticleParse

# Content and time: Links the content features and has the main timing logic for posting articles.
# Timing logic will have a boolean feature within so that it can be stopped if it isn’t working from a command.

# ------------------------------------------------------------------------------------------------------------


def getTime():
    # datetime object containing current date and time
    now = datetime.now()

    # replaces with an hour before
    currHour = now.hour
    now = now.replace(hour=currHour-2)

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

def testMessage():
    print("testing!")

def checkForArticles():
    link = ""
    ccsoBotArticleParse.grabAnArticle(link)

def checkForUpdates(channel_id):
    print("checking for updates...")

    # youtube parse channels 
    # what a fucking joke that you cant uuse the channel name
    # channel_id = "MissLiliTrifilio"
    # channel_id = 'UCvyz2XB3Seq9MCbtORt1WIw'

    # must-have channels
    # *https://www.youtube.com/channel/UC0ArlFuFYMpEewyRBzdLHiw*
    # https://www.youtube.com/channel/UCByOX6pW9k1OYKpDA2UHvJw
    # https://www.youtube.com/channel/UCLDnEn-TxejaDB8qm2AUhHQ
    # *https://www.youtube.com/channel/UCKGe7fZ_S788Jaspxg-_5Sg*
    # *https://www.youtube.com/channel/UCVeW9qkBjo3zosnqUbG7CFw*
    # *https://www.youtube.com/channel/UC0ZTPkdxlAKf-V33tqXwi3Q*

    # mustHaveChannels = ["UC0ArlFuFYMpEewyRBzdLHiw", "UCByOX6pW9k1OYKpDA2UHvJw", "UCLDnEn-TxejaDB8qm2AUhHQ", "UCKGe7fZ_S788Jaspxg-_5Sg", "UCVeW9qkBjo3zosnqUbG7CFw", "UC0ZTPkdxlAKf-V33tqXwi3Q"]

    gcpKey = ccsoBotCreds.getGCPKey()

    # date = getTime()
    # RFC formatted time
    date = '2021-03-01T00:00:00Z'

    # for channel in mustHaveChannels:
    #     channel_id = channel
    #     videos = ccsoBotYtParse.search_videos(channel_id, date, gcpKey)
        
    videos = ccsoBotYtParse.search_videos(channel_id, date, gcpKey)

    # print(videos)
    return videos

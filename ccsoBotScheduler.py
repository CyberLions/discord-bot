import os
import time
import sched
from datetime import datetime

import ccsoBotReactions
import ccsoBotCreds

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

def startScheduler():
    # https://stackoverflow.com/questions/2398661/schedule-a-repeating-event-in-python-3
    # The normal pattern (in any language) to transform a one-off scheduler into a periodic scheduler 
    # is to have each event re-schedule itself at the specified interval
    s = sched.scheduler(time.time, time.sleep)
    s.enter(10, 1, startScheduler)
    
    #do stuff 
    checkForUpdates()

    s.run()


def checkForUpdates():
    print("checking for updates...")

    # youtube parse
    # what a fucking joke that you cant uuse the channel name
    channel_id = "MissLiliTrifilio"
    channel_id = 'UCvyz2XB3Seq9MCbtORt1WIw'

    gcpKey = ccsoBotCreds.getGCPKey()

    # date = getTime()
    # RFC formatted time
    date = '2021-03-01T00:00:00Z'

    videos = ccsoBotReactions.search_videos(channel_id, date, gcpKey)

    # print(videos)


def getVideos():
    return videos


def setVideos(input):
    videos = input

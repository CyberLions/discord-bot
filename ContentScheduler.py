import os
import time
import sched
import ytparse
from datetime import datetime

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

def do_something(gcpKey):
    # DO THE THING  
    #youtube parse
    channel_id = "MissLiliTrifilio"
    channel_id = 'UCvyz2XB3Seq9MCbtORt1WIw'
    # what a fucking joke that you cant uuse the channel name 
    
    # channel_id = "UC4eYXhJI4-7wSWc8UNRwD4A"

    # RFC formatted time
    # 1970-01-01T00:00:00Z
    # time = '2021-06-16T00:00:00Z'
    # time = 

    # date = getTime()
    date = '2021-03-01T00:00:00Z'

    videos = ytparse.search_videos(channel_id, date, gcpKey)

    return videos

def runTheThing():
    s = sched.scheduler(time.time, time.sleep)
    s.enter(5, 1, do_something)
    s.run()



import os
import time
import sched
import urllib
import json
from datetime import datetime
import requests

# npr music
# https://www.youtube.com/channel/UC4eYXhJI4-7wSWc8UNRwD4A

# Youtube Parse: Parses all the channels

# ------------------------------------------------------------------------------------------------------------

def testMessage():
    print("you have imported ytparse!")

#searches for vids in a given single channel 
def search_videos(channel_id, time, gcpKey):

    base_video_url = 'https://www.youtube.com/watch?v='
    base_search_url = 'https://www.googleapis.com/youtube/v3/search?'
    channel_base_url = 'https://www.youtube.com/channel/'

    # RFC formatted time
    # 1970-01-01T00:00:00Z
    # time = '2021-06-16T00:00:00Z'
    # time = '2021-08-01T00:00:00Z'
    first_url = base_search_url + \
        'key={}&channelId={}&publishedAfter={}&order=date&maxResults=10&part=snippet'.format(
            gcpKey, channel_id, time)

    # adds videos (if returned) to a list
    videos = []
    url = first_url
    while True:
        inp = urllib.request.urlopen(url, timeout=1)
        resp = json.load(inp)

        for i in resp['items']:
            if i['id']['kind'] == "youtube#video":

                # print(i['snippet'])

                video = {
                    'title': i['snippet']['title'],
                    'url': base_video_url + i['id']['videoId'],
                    'description': i['snippet']['description'],
                    'thumbnail': i['snippet']['thumbnails']['high']['url'],
                    'channelId': channel_base_url + i['snippet']['channelId'],
                    'channelTitle': i['snippet']['channelTitle'],
                }

                videos.append(video)
                # print(i['snippet']['thumbnails']['high']['url'])

        try:
            next_page_token = resp['nextPageToken']
            url = first_url + '&pageToken={}'.format(next_page_token)
        except:
            break

    return videos


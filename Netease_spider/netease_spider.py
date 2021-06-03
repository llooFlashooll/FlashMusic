import time
import random
import threading
import json
import os

import requests
from fake_useragent import UserAgent
from bs4 import BeautifulSoup
from PIL import Image

import urllib3

urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

ua = UserAgent()

def spider():

    url = "https://music.163.com/store/product/column?id=55001&title=" + "热销爆品"
    headers = {
        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.212 Safari/537.36"
    }

    response = requests.get(url, headers=headers, timeout = 10)
    html = response.text
    fileName = "neteasecloud.html"
    with open(fileName, 'w', encoding='utf-8') as f:
        f.write(html)

    # print(html)
    soup = BeautifulSoup(html, 'html.parser')
    images = soup.find('p', attrs={'class': 'txt f-thide'})

    print(images)

    # print(soup)



if __name__=='__main__':
    spider()
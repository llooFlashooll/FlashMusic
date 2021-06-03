import json
import requests
import sys, os
import uuid

# 将图片下载到本地
# json_one = []

# with open("数码影音.json", 'r', encoding='utf-8') as f:
#     json_one = json.load(f)

# print(len(json_one))

# for i in range(0, len(json_one)):
#     picurl = json_one[i]["PicUrl"]
#     print(picurl)
#     try:
#         pic = requests.get(picurl, timeout = 10)
#     except requests.exceptions.ConnectionError:
#         print("Download fails.")
#         continue

#     dir = "./pics/" + str(i + 61) + '.jpg'
#     fp = open(dir, 'wb')
#     fp.write(pic.content)
#     fp.close

#     print(i)

# 转换内容

file_name = "IP周边.json"
json_data = []

with open(file_name, 'r', encoding='utf-8') as f:
    json_data = json.load(f)

    for i in range(0, len(json_data)):
        # json_data[i]["PicUrl"] = "https://flash-1302948545.cos.ap-shanghai.myqcloud.com/flashmusic/pics/" + str(i + 61) + ".jpg"
        json_data[i]["ProductId"] = str(uuid.uuid1())
        print(i+1)

os.remove(file_name)
with open(file_name, 'w', encoding='utf-8') as f:
    json.dump(json_data, f, indent=4, ensure_ascii=False)
import json
import pymysql

coon=pymysql.connect(host="47.103.56.113",port=3306,user="root",password="123456",db="flashmusic_db");
cursor=coon.cursor();

product = []

with open("productMockData.json", "r", encoding="utf-8") as file:
    product = json.load(file)

    for i in range(0, len(product)):
        effect = cursor.execute(
            "insert into product(productid, categoryid, price, name, picurl) values (%s, %s, %s, %s, %s)"
            ,(product[i]["ProductId"], product[i]["CategoryId"], product[i]["Price"], product[i]["Name"],
                product[i]["PicUrl"]));

        print(effect);
        coon.commit();

cursor.close();
coon.close();


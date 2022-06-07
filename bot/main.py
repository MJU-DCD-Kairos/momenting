import threading
from datetime import datetime

import firebase_admin
from firebase_admin import credentials
from firebase_admin import firestore

db = None

def checkDB():
    gameChatRoom = db.collection(u'gameChatRoom')
    docs = gameChatRoom.stream()

    # 파이어스토어에서 사용되는 날짜포맷
    dt_format = '%Y-%m-%d %p %I:%M:%S'

    print("[봇체크] " + str(datetime.now()))

    for doc in docs:
        #print('문서정보: ' + doc.id)
        roomInfo = doc.to_dict()

        # 오픈되지 않았고, 생성시간이 1분을 넘어섰다면...
        if roomInfo['isOpen'] is False:
            print('[생성방] ' + doc.id)

            # 생성시간 체크
            createTime = datetime.strptime(roomInfo['createTime'], dt_format)
            now = datetime.now()
            date_diff = now - createTime
            print('[생성방] 생성후 지난 시간(초): ' + str(date_diff.seconds))

            # 1분을 넘어섰다면...
            if date_diff.seconds > 60:
                db.collection(u'gameChatRoom').document(doc.id).delete()
                print('[매칭실패] ' + doc.id)

        # 오픈되고 활성화되고 오픈시간이 20분을 넘어섰다면...
        else:
            if roomInfo['isActive'] is True:
                print('[오픈방] ' + doc.id)

                # 오픈시간 체크
                openTime = datetime.strptime(roomInfo['openTime'], dt_format)
                now = datetime.now()
                date_diff = now - openTime
                print('[오픈방] 오픈후 지난 시간(초): ' + str(date_diff.seconds))

                # 20분을 넘어섰다면...
                if date_diff.seconds > (60 * 20):
                    db.collection(u'gameChatRoom').document(doc.id).update({
                        u'isActive': False
                    })
                    print('[오픈방 비활성 처리] ' + doc.id)

    # 10초마다 체크
    threading.Timer(10, checkDB).start()

if __name__ == '__main__':
    # cred = credentials.Certificate('./momenting-a1670-e40bfa4dbe68.json')
    cred = credentials.Certificate('d:/momenting/bot/momenting-a1670-e40bfa4dbe68.json')
    firebase_admin.initialize_app(cred)

    db = firestore.client()

    checkDB()


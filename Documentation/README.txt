https://www.qappdesign.com/using-mongodb-with-net-core-webapi/

"C:\Program Files\MongoDB\Server\4.0\bin\mongo.exe" --help

"C:\Program Files\MongoDB\Server\4.0\bin\mongo.exe" --dbpath "C:\Project\Mongo\Data\ProductStock"

MONGOD.CFG
systemLog:
  destination: file
  path: "C:\\Project\\Mongo\\Logs\\mongo.log"
  logAppend: true
storage:
  dbPath: "C:\\Project\\Mongo\\Data"
  

"C:\Program Files\MongoDB\Server\4.0\bin\mongo.exe" --config "C:\Project\ProductStock\Documentation\mongod.cfg"

use admin
db.createUser(
  {
    user: "admin",
    pwd: "abc123!",
    roles: [ { role: "root", db: "admin" } ]
  }
);
exit;

LPARAMETERS lBugid as String
STORE '' TO currPath
currpath = CURDIR()
BUILD EXE currpath+"\UpdtExe\vu10Updt_"+ALLTRIM(lbugid) FROM currpath+"\UpdtExe\vu10Updt.PJX"
#ifndef SQLITE_HAS_CODEC
#define SQLITE_HAS_CODEC
#endif

#include "include\mysql.h"
#include "network.h"
#include "sqlite3.h"
#include <fstream>
#include <windows.h>
#include <iostream>
#include <filesystem>
#include <string>
#include <errno.h>
#include <tchar.h>
#include <ctime>
#include "SHA1.h"
#include "unicode2utf8.h"
#include <Shlwapi.h>
#include <locale>
#include <thread>
#include <Winnls.h>
#include <string>

#ifndef _exception_
#define _exception_
#pragma comment(lib,"Shlwapi.lib")
#pragma comment(lib,"sqlite3.lib")
#pragma comment(lib,"libMysql.lib")
#pragma comment(lib, "wsock32.lib")  
#pragma comment(lib, "Advapi32.lib")

#if defined _DEBUG
#pragma comment(lib,"mysqlclient_debug.lib")
#else
#pragma comment(lib,"mysqlclient.lib")
#endif


class MysqlServer :protected  network
{
public:
	MysqlServer();
	~MysqlServer();
	void sync();//执行同步函数
	
private:
	bool createDB();  //create DB directory and data.db database file
	TCHAR Name[100];
	sqlite3 * conn,* conn1 = NULL;
	char * err_msg;
	char sql[200];
	int bit;     //the bit of key
	char * key;  //key of database
	TCHAR * sha1;
	bool connectdata();
	void makekey();   //produce key
	int ramdom(int, int);  //produce ramdom number
	bool getFilesha1(char *);
	//sqlite3回调函数
	bool detect_file(char *);
	char* ConvertLPWSTRToLPSTR(LPWSTR lpwszStrIn);
	void ConvertCharToTCHAR(char *);
	void query(sqlite3 * ,char *);
	MYSQL* mysql = NULL, *mysql1 = NULL;
	MYSQL_RES* res = NULL, *res1 = NULL;
	MYSQL_ROW record, record1;
	bool conncetsql(); //连接数据库
	bool conncetsql1();
	bool queryDatabase();//查询数据库
	void printtest();//测试函数
	bool sqlconnect();
	void uploadToMysql();//线程1
	bool Mysqlupdatequery(MYSQL *, char *);
	bool SqliteNoCallbackQuery(sqlite3 * ,char *);
	void downloadToSqlite();//更新本地sqlite数据库
	friend int sqlite3_exec_callback(void *data, int nColumn, char **colValues, char **colNames);//callback function must be static or overall function
};
#endif









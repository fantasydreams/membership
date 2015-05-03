#ifndef SQLITE_HAS_CODEC   
#define SQLITE_HAS_CODEC //define HAS_CODE
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

#ifndef _exception_ //pragma once
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
	void sync();//执行同步并创建多线程函数
private:
	bool createDB();
	TCHAR Name[100];
	sqlite3 * conn,* conn1 = NULL;
	char * err_msg;
	char sql[200];
	int bit;     //the bit of key
	char * key;  //key of database
	TCHAR * sha1; //sha1 地址
	bool connectdata(); //连接本地sql_database
	void makekey();   //produce key
	int ramdom(int, int);  //produce ramdom number
	bool getFilesha1(char *); //get 某文件的sha1值
	//sqlite3回调函数
	bool detect_file(char *); //检测文件是否存在
	char* ConvertLPWSTRToLPSTR(LPWSTR lpwszStrIn); //LPWSTR 转换至 LPSTR
	void ConvertCharToTCHAR(char *); //char convert to TCHAR
	void query(sqlite3 * ,char *); //本地sql查询函数
	MYSQL* mysql = NULL, *mysql1 = NULL;
	MYSQL_RES* res = NULL, *res1 = NULL;
	MYSQL_ROW record, record1;
	bool conncetsql(); //连接database
	bool conncetsql1();
	bool queryDatabase();//查询database
	void printtest(MYSQL *mysql);//测试函数
	bool sqlconnect();
	void uploadToMysql();//thread 1
	bool Mysqlupdatequery(MYSQL *, char *);//用于执行没有返回值得sql语句
	bool SqliteNoCallbackQuery(sqlite3 * ,char *);
	void downloadToSqlite();//更新本地sqlite数据库
	bool DownloadMysqlQuery(MYSQL *, char *);//用于执行有返回值的sql语句
	friend int sqlite3_exec_callback(void *data, int nColumn, char **colValues, char **colNames);//callback function must be static or overall function
};
#endif









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


//#pragma comment( linker, "/subsystem:\"windows\" /entry:\"mainCRTStartup\"")  //不显示

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

class exception
{
public:
	exception();  //constructor
	~exception(); //destrucor
	bool createDB();  //create DB directory and data.db database file
protected:
	sqlite3 * conn;
	char * err_msg;
	char sql[200];
	int bit;     //the bit of key
	char * key;  //key of database
	TCHAR * sha1;
	bool createData();
	bool connectdata();
	bool CreateTable(sqlite3 *);  //create new database tables
	void makekey();   //produce key
	int ramdom(int, int);  //produce ramdom number
	void getFilesha1();
	//sqlite3回调函数
	static int sqlite3_exec_callback_print(void *data, int nColumn, char **colValues, char **colNames);//callback function must be static
	void query();
};

class MysqlServer :protected exception,protected network
{
public:
	MysqlServer();
	~MysqlServer();
private:
	MYSQL* mysql = NULL;
	MYSQL_RES* res = NULL;
	MYSQL_ROW record;
	bool conncetsql(); //连接数据库

};

#endif







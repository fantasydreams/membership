#ifndef SQLITE_HAS_CODEC   
#define SQLITE_HAS_CODEC //define HAS_CODE
#endif

#include "include\mysql.h"
#include "network.h"
#include "sqlite3.h"
#include "unicode2utf8.h"
#include "SHA1.h"
#include "chain_template.h"
#include <fstream>
#include <windows.h>
#include <iostream>
#include <filesystem>
#include <string>
#include <errno.h>
#include <tchar.h>
#include <ctime>
#include <Shlwapi.h>
#include <locale>
#include <thread>
#include <Winnls.h>
#include <string>
#include <vector>

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

int sqlite3_exec_callback_utos(void *data, int nColumn, char **colValues, char **colNames);
int sqlite3_exec_callback(void *data, int nColumn, char **colValues, char **colNames);//callback function must be static or overall function

class MysqlServer :protected  network
{
public:
	MysqlServer();
	~MysqlServer();
	void sync(const char *);//ִ��ͬ�����������̺߳���
private:
	char value = 1,value1 = 1;
	const char * shop_id = NULL;
	bool createDB();
	TCHAR Name[100];
	sqlite3 * conn;
	char * err_msg;
	char sql[200];
	int bit;     //the bit of key
	char * key;  //key of database
	TCHAR * sha1; //sha1 ��ַ
	bool connectdata(); //���ӱ���sql_database
	void makekey();   //produce key
	int ramdom(int, int);  //produce ramdom number
	bool getFilesha1(char *); //get ĳ�ļ���sha1ֵ
	//sqlite3�ص�����
	bool detect_file(const char *); //����ļ��Ƿ����
	char* ConvertLPWSTRToLPSTR(LPWSTR lpwszStrIn); //LPWSTR ת���� LPSTR
	void ConvertCharToTCHAR(char *); //char convert to TCHAR
	void query(sqlite3 * ,const char *); //����sql��ѯ����
	void query_utos_insert(sqlite3 *, const char *);//utos_insert
	MYSQL* mysql = NULL, *mysql1 = NULL;
	MYSQL_RES* res = NULL, *res1 = NULL;
	MYSQL_ROW record, record1;
	bool conncetsql(); //����database
	bool conncetsql1();
	bool queryDatabase();//��ѯdatabase
	void printtest(MYSQL *mysql);//���Ժ���
	bool sqlconnect();
	void uploadToMysql();//thread 1
	bool Mysqlupdatequery(MYSQL *, const char *);//����ִ��û�з���ֵ��sql���
	bool SqliteNoCallbackQuery(sqlite3 * ,const char *);
	void downloadToSqlite();//���±���sqlite���ݿ�  //thread 2
	bool DownloadMysqlQuery(MYSQL *, const char *);//����ִ���з���ֵ��sql���
	bool DownloadMysqlQuery1(MYSQL *, const char *);
	bool NetworkIsAvliable(); //�ж������Ƿ�ͨ
	bool casherupdate(MYSQL *mysql,const MYSQL_ROW); //casher������
	bool goodsupdate(MYSQL *mysql, const MYSQL_ROW); //goods������
	bool utosupdate(MYSQL *mysql, const MYSQL_ROW);  //utos������
	//bool shopupdate(MYSQL *mysql, const MYSQL_ROW);//shop������
	bool userupdate(MYSQL *mysql, const MYSQL_ROW);  //user������
	bool userinfoupdate(MYSQL *mysql, const MYSQL_ROW);//userinfo ������
	friend int sqlite3_exec_callback(void *data, int nColumn, char **colValues, char **colNames);//callback function must be static or overall function
	friend int sqlite3_exec_callback_utos(void *data, int nColumn, char **colValues, char **colNames);
};
#endif









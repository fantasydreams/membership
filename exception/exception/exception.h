#include "sqlite3.h"
#include <fstream>
#include <windows.h>
#include <iostream>
#include <filesystem>
#include <string>
#include <errno.h>
#include <tchar.h>


//#pragma comment( linker, "/subsystem:\"windows\" /entry:\"mainCRTStartup\"")  //����ʾ
#pragma comment(lib,"Shlwapi.lib")
#pragma comment(lib,"sqlite3.lib")

class exception
{
public:
	exception();
	~exception(){};
	bool createDB();
private:
	sqlite3 * conn;
	char * err_msg;
	char sql[200];
	bool createData();
	bool connectdata();
	bool CreateTable();

};

exception::exception()
{
	conn = NULL;
	err_msg = NULL;
	memset(sql, 0, sizeof(char) * 200);
}

//not stop create DB directory and data.db file untill seccussfully
bool exception::createDB()
{
	WIN32_FIND_DATA wfd;
	HANDLE fhandle = FindFirstFile(_T("./DB"), &wfd);
	if (fhandle == INVALID_HANDLE_VALUE)
	{
		//std::cout << "directory not exsit!";
		while (!CreateDirectory(_T("./DB"), NULL));
		while (!createData());
	}
	else
	{
		//std::cout << "directory exsit!";
		while (!createData());
	}
	
	return true;
}

//create data.db file
bool exception::createData()
{
	std::ofstream out;
	out.open("./DB/data.db", std::ios::out);
	if (out)
	{
		//std::cout << "1";
		out.close();
		return true;
	}
	else
	{
		//std::cout << "2";
		out.close();
		return false;
	}
	
}

bool exception::connectdata()
{
	if (sqlite3_open("./RGS.db", &conn) != SQLITE_OK)
	{
		std::cout << "�޷������ݿ⣬�볢�������������ɾ����˳���ͬһĿ¼�µ�RSG.db�ļ���" << std::endl;
		exit(-1);
	}
	else
	{
		CreateTable(conn, sql, err_msg);
		std::cout << "���ݿտ���Ҳ�������ֶ�������ݰɣ�" << std::endl;
	}
	return true;
}
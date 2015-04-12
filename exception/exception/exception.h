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

//#pragma comment( linker, "/subsystem:\"windows\" /entry:\"mainCRTStartup\"")  //不显示
#pragma comment(lib,"Shlwapi.lib")
#pragma comment(lib,"sqlite3.lib")

class exception
{
public:
	exception();  //constructor
	~exception(); //destrucor
	bool createDB();  //create DB directory and data.db database file
private:
	sqlite3 * conn;
	char * err_msg;
	char sql[200];
	int bit;     //the bit of key
	char * key;  //key of database
	TCHAR * sha1;
	bool createData();
	bool connectdata();
	bool CreateTable(sqlite3 * ,char *);  //create new database tables
	void makekey();   //produce key
	int ramdom(int, int);  //produce ramdom number
	void getFilesha1();
};

exception::exception()
{
	conn = NULL;
	err_msg = NULL;
	memset(sql, 0, sizeof(char) * 200);
	bit = 0;
	key = NULL;
	sha1 = NULL;
}

exception::~exception()
{
	if (key)
		delete[] key;
	if (sha1)
		delete[]sha1;
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
		connectdata();
		makekey();
		getFilesha1();
		return true;
	}
	else
	{
		//std::cout << "2";
		out.close();
		connectdata();
		return false;
	}
}

bool exception::connectdata()
{
	if (sqlite3_open("./RGS.db", &conn) != SQLITE_OK)
	{
		//std::cout << "无法打开数据库，请尝试重启程序或者删除与此程序同一目录下的RSG.db文件！" << std::endl;
		//exit(-1);
	}
	else
	{
		//CreateTable(conn, sql);
		/*sqlite3_key(conn, "sqlite3", 7);
		sqlite3_rekey(conn, "sqlite4", 7);*/
	}
	return true;
}

void exception::makekey()
{
	bit = ramdom(16, 32);
	while (!(key = new char[bit + 1])); //apply key memory
	memset(key, 0, sizeof(char)*(bit + 1));
	srand(time(NULL));
	for (int i = 0; i < bit; i++)
		key[i] = char(ramdom(65, 112));
	
	std::cout << key << std::endl;
}

//produce ramdom number between x and y
inline int exception::ramdom(int x,int y)
{
//	srand(time(NULL));
	return rand() % abs(y - x) + ((x > y) ? y : x);
}

void exception::getFilesha1()
{
	CSHA1 sha;
	const bool bSuccess = sha.HashFile(_T("./DB/data.db"));
	sha.Final();
	sha1 = new TCHAR[41];
	sha.ReportHash(sha1, CSHA1::REPORT_HEX_SHORT);
	if (bSuccess)
		std::cout << sha1 << std::endl;
	else
		getFilesha1();

}
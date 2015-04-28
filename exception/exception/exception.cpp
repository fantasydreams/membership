
#include "exception.h"

bool exception::connectdata()
{
	if (sqlite3_open("./DB/data1.db", &conn) != SQLITE_OK)
	{
		//std::cout << "无法打开数据库，请尝试重启程序或者删除与此程序同一目录下的RSG.db文件！" << std::endl;
		//exit(-1);
	}
	else
	{

		//while (sqlite3_key(conn, "sqlite3", 7) !=SQLITE_OK);
		/*if (sqlite3_key(conn, "sql", 3) != SQLITE_OK)
			std::cout << "error msg:密码错误";*/
		//CreateTable(conn);
		if (sqlite3_rekey(conn, "sql", 3) != SQLITE_OK)
		std::cout << "error msg:修改密码错误";
	}

	return true;
}

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
		query();
		makekey();
		getFilesha1();
		return true;
	}
	else
	{
		//std::cout << "2";
		out.close();
		connectdata();
		query();
		return false;
	}
}



void exception::makekey()
{
	bit = ramdom(16, 32);
	while (!(key = new char[bit + 1])); //apply key memory
	memset(key, 0, sizeof(char)*(bit + 1));
	srand(time_t(NULL));
	for (int i = 0; i < bit; i++)
		key[i] = char(ramdom(65, 112));

	std::cout << key << std::endl;
}

//produce ramdom number between x and y
inline int exception::ramdom(int x, int y)
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

//create a new data table
bool exception::CreateTable(sqlite3 *)
{
	if (sqlite3_exec(conn, "create table test(name TEXT(50));", 0, 0, &err_msg) == SQLITE_OK)
		return true;
	else
	{
		std::cout << "error msg:" << err_msg;
		return false;
	}
}

//sqlite3回调函数
int exception::sqlite3_exec_callback_print(void *data, int nColumn, char **colValues, char **colNames)
{
	//std::cout << "in";
	std::wstring wstr;
	std::wcout.imbue(std::locale("chs"));
	wstr = UTF8ToUnicode(colValues[0]);
	std::wcout << wstr << "\t";
	return 0;
}

void exception::query()
{
	if (sqlite3_exec(conn, "select * from test;", &sqlite3_exec_callback_print, NULL, &err_msg) != SQLITE_OK)
		std::cout << "err_msg:" << err_msg << std::endl;
}
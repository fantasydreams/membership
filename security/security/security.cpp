#include "security.h"


bool security:: getFilesha1(char * name)
{
	TCHAR * sha1;
	CSHA1 sha;
	const bool bSuccess = sha.HashFile(name);
	sha.Final();
	sha1 = new TCHAR[41];
	sha.ReportHash(sha1, CSHA1::REPORT_HEX_SHORT);
	if (bSuccess)
		std::cout << sha1 << std::endl;
	else
		getFilesha1(name);
}

inline security::security()
{
	errmsg = NULL;
	if (detectfile() && connect() && query())
		;
	else
		exit(0);
}


void security::applyMemery(char *name, char *code)
{
	Table * node;
	while (!(node = new Table));
	char * ptr;
	while (!(ptr = new char[strlen(name)+1]));
	strcpy_s(ptr, strlen(name) + 1, name);
	node->name = ptr;
	while (!(ptr = new char[strlen(code) + 1]));
	strcpy_s(ptr, strlen(code) + 1, code);
	node->security_code = ptr;
	node->next = NULL;
	if (head)
	{
		Last->next = node;
		Last = node;
	}
	else
	{
		head = node;
		Last = node;
	}

}

bool security::query()
{
	if (sqlite3_exec(conn, "select exec_name, security from file_security;", &sqlite3_exec_callback, NULL, &errmsg) != SQLITE_OK)
		//std::cout << "err_msg:" << errmsg << std::endl;
		return false;
	return true;
}

bool security::connect()
{
	if (sqlite3_open("./DB/security.db", &conn) != SQLITE_OK)
	{
		//std::cout << "failed" << std::endl;
		return true;
	}
	else
	{
		//std::cout << "yes" << std::endl;
		return false;
	}
	return true;
}

//检测文件和文件夹是否存在
bool security::detectfile()
{
	std::ifstream in("./DB/security.db");
	if (in)
	{
		//std::cout << "exist";
		return false;
	}
	else
	{
		//std::cout << "not exist";
		return true;
	}
}

#include "sqlite3.h"
#include <iostream >
#include <fstream>
#include <windows.h>
#include <tchar.h>
using namespace std;

#pragma comment(lib,"sqlite3.lib")

class dbexec
{
public:
	dbexec();
	~dbexec(){};
	bool execute();
private:
	sqlite3 * conn;
	char * errmsg;
	bool connect();
	bool createTable();
	bool detectfile();
	bool createfile();
};
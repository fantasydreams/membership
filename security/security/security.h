#include"SHA1.h"
#include "sqlite3.h"
#include <iostream>
#include <fstream>
#include <windows.h>
//#define _UNICODE
#pragma comment(lib,"sqlite3.lib")

typedef struct table
{
	char * name;
	char * security_code;
	Table * next;
}Table;

class security
{
public:
	security();
	~security(){};
private:
	Table * head;
	Table * Last;
	char * errmsg;
	sqlite3 * conn;
	bool getFilesha1(char *);
	static int sqlite3_exec_callback(void *data, int nColumn, char **colValues, char **colNames);//callback function must be static
	void applyMemery(char * ,char *);//–Œ≥…¡¥±Ì
	bool query();
	bool security::connect();
	bool detectfile();
};
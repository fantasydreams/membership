#include"SHA1.h"
#include "sqlite3.h"
#include <iostream>
#include <fstream>
#include <windows.h>

#pragma comment(lib,"sqlite3.lib")

#ifndef _member_security
#define _member_security

typedef struct table
{
	char * name;
	char * security_code;
	struct table * next;
}Table;


class security
{
public:

	security();
	~security(){ deleteMemery(); };
	bool exec();
	static Table * head;
	static Table * Last;
	bool getFilesha1(char *);
private:

	char * errmsg;
	sqlite3 * conn;
	TCHAR * sha1;
	
	static int sqlite3_exec_callback(void *, int , char **, char **);//callback function must be static
	static void applyMemery(char * ,char *);//–Œ≥…¡¥±Ì
	bool query();
	bool security::connect();
	bool detectfile(char *);
	bool check();//ºÏ≤‚
	void deleteMemery();
};

#endif 
#include "security.h"


Table * security::head = NULL;
Table * security::Last = NULL;
//�ͷ��ڴ�
void security::deleteMemery()
{
	Table * temp;
	while (head)
	{
		if (head->name)
			delete[] head->name;
		if (head->security_code)
			delete[] head->security_code;
		temp = head;
		head = head->next;
		delete temp;
	}
}

bool security::exec()
{
	if (detectfile("./DB/security.db") && connect() && query())
		if (check())
			return true;
		else
			return false;
	else
		return false;
}

//�õ��ļ���ϣֵ
bool security:: getFilesha1(char * name)
{
	CSHA1 sha;
	const bool bSuccess = sha.HashFile(name);
	sha.Final();
	if (sha1)
		delete[] sha1;
	sha1 = new TCHAR[41];
	sha.ReportHash(sha1, CSHA1::REPORT_HEX_SHORT);
	if (bSuccess)
	{
		std::cout << sha1 << std::endl;
		return true;
	}
	else
		return false;
}
//���캯��
security::security()
{
	sha1 = NULL;
	errmsg = NULL;
}

//�����ڴ壬���γ�����
void security::applyMemery(char *name, char *code)
{
	Table * node;
	while (!(node = new Table));
	node->name = node->security_code = NULL;
	node->next = NULL;
	char * ptr;
	while (!(ptr = new char[strlen(name)+1]));
	strcpy_s(ptr, strlen(name) + 1, name);
	node->name = ptr;
	while (!(ptr = new char[strlen(code) + 1]));
	strcpy_s(ptr, strlen(code) + 1, code);
	node->security_code = ptr;
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
//sqlite3��ѯ����
bool security::query()
{
	if (sqlite3_exec(conn, "select exec_name, security from file_security;", &sqlite3_exec_callback, NULL, &errmsg) != SQLITE_OK)
	{
		std::cout << "err_msg:" << errmsg << std::endl;
		return false;
	}
	return true;
}
//sqlite���Ӻ���
bool security::connect()
{
	if (sqlite3_open("./DB/security.db", &conn) != SQLITE_OK)
	{
		//std::cout << "failed" << std::endl;
		return false;
	}
	else
	{
		//std::cout << "yes" << std::endl;
		return true;
	}
	return true;
}

//����ļ����ļ����Ƿ����
bool security::detectfile(char * path)
{
	std::ifstream in(path);
	if (in)
	{
		//std::cout << "exist";
		return true;
	}
	else
	{
		//std::cout << "not exist";
		return false;
	}
}

int security::sqlite3_exec_callback(void *data, int nColumn, char **colValues, char **colNames)
{
	if (colValues)
	{
		applyMemery(colValues[0], colValues[1]);
	}
	return 0;
}


bool security::check()
{
	Table * p = head;
	while (p)
	{
		point:if (getFilesha1(p->name))
		{
			if (strcmp(sha1, p->security_code))
				return false;
		}
		else
		{
			if (!detectfile(p->name))
				return false;
			else
				goto point;
		}
		p = p->next;
	}
	return true;
}
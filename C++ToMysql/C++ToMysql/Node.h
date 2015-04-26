#include "include\mysql.h"
#include <iostream>
#include <winsock2.h>
#include <windows.h>
#include <vector>

#pragma comment(lib,"libMysql.lib")
#pragma comment(lib, "wsock32.lib")  
#pragma comment(lib, "Advapi32.lib")



#if defined _DEBUG
#pragma comment(lib,"mysqlclient_debug.lib")
#else
#pragma comment(lib,"mysqlclient.lib")
#endif

struct Node_Coordinate
{
	int Node_id;  //节点ID
	int x;  //x坐标
	int y;   //y坐标
	int z;  //z坐标
};


class Node
{
public:
	Node();
	~Node();
	std::vector<Node_Coordinate> N_Coordinate;  //存储终端节点ID
private:
	MYSQL* mysql = NULL;
	MYSQL_RES* res = NULL;
	MYSQL_ROW record;
	bool conncetsql(); //连接数据库
	void GetNodeToDo();  //给节点下激活或者激活的指令
	void Active();  //激活节点
	void Dormant();  //休眠节点
	bool quary(char, char *); //查询
	void print();  //测试函数
	void CoordinateWriteToSql(); //将终端节点坐标写入数据库
	void CoordinateQuery(int, int, int, int);
};





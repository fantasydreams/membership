#include "Node.h"


Node::Node()
{
	mysql = mysql_init(mysql);
	if (conncetsql())
		std::cout << "yes"<<std::endl;
	else
		std::cout << "no"<<std::endl;
	quary('1', "ff-ff-ff-ff-ff-ff");
		//print();
}

Node::~Node()
{
	if (res)
		mysql_free_result(res);
	if (mysql)
		mysql_close(mysql);
}

inline bool Node::conncetsql()
{
	if (mysql_real_connect(mysql, "localhost", "root", "lsw19940816", "park", 3306, NULL, NULL))  //连接至数据库
		return true;  //成功返回真
	else
		return false;  //否则返回假
}


void Node::GetNodeToDo()
{
	while (true)
	{
		quary(9, NULL);
		if (res)
			Active();  //激活操作
		else
			Sleep(100);  //线程暂时性休眠
		quary(0, NULL);
		if (res)
			Dormant();  //休眠操作
		else
			Sleep(100);  //线程暂时性休眠
	}
}

bool Node::quary(char var,char * Mac)
{
	mysql_query(mysql, "set names utf8");
	if ('0' == var)  //得到需要休眠节点的id
	{
		//select POINT_id from point where todo = 0
		mysql_query(mysql, "select Mac from point where todo = 0");
		if (!(res = mysql_use_result(mysql)))
		{
			std::cout << "some little problem！" << mysql_error(mysql);
			return false;
		}
		return true;
	}
	if (var == '9')//得到需要激活节点的id
	{
		mysql_query(mysql, "select Mac from point where todo = 9");
		if (!(res = mysql_use_result(mysql)))
		{
			std::cout << "some little problem！" << mysql_error(mysql);
			return false;
		}
		return true;
	}
	if (var == '1' && Mac!=NULL) //将节点的状态置为不做任何操作
	{
		std::string s = std::string(Mac);
		std::string qurysql = "update point set todo = 1 where Mac = '" + s + "'";
		mysql_query(mysql, qurysql.c_str());
	}
	return false;  //防止意外情况发生
}


void Node::print()
{
	while (record = mysql_fetch_row(res))
		std::cout << *record << std::endl;
}

void Node::CoordinateWriteToSql()
{
	while (true)
	{
		if (N_Coordinate.begin() != N_Coordinate.end())  //一段队列中有需要写入的信息，线程工作将坐标信息写入数据库服务器
		{
			for (std::vector<Node_Coordinate>::iterator iter = N_Coordinate.begin(); iter != N_Coordinate.end(); iter++)
				CoordinateQuery(iter->Node_id, iter->x, iter->y, iter->z); //坐标信息更新函数调用
		}
		else
			Sleep(1000);  //线程暂时性休眠
	}
}

void Node::CoordinateQuery(int N_id, int posX, int posY, int posZ)
{
	char str[100];
	_itoa_s(posX, str, 10);    //开始初始化需要写入节点的位置信息
	std::string PositionX(str);
	_itoa_s(posY, str, 10);
	std::string PositionY(str);
	_itoa_s(posZ, str, 10);
	std::string PositionZ(str);
	_itoa_s(N_id, str, 10);
	std::string Node_id(str);  
	std::string query = "update point set posX = " + PositionX + " and posY = " + PositionY + " and posZ = " + PositionZ + " where Mac = " + Node_id;
	mysql_query(mysql, "set names utf8");
	mysql_query(mysql, query.c_str());  //更新位置信息命令
}

void Node::Active()
{
	
	while (record = mysql_fetch_row(res)) // 通过Mac地址激活需要激活的终端节点
	{
		
	}
}

void Node::Dormant()
{
	while (record = mysql_fetch_row(res))// 通过Mac地址休眠需要休眠的终端节点
	{

	}
}
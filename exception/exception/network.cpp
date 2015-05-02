#include"network.h"
#include <iostream>

network::~network()
{
	for (int i = 0; i < 8; i++)
	{
		if (*(IpAddr + i))
			delete[] *(IpAddr + i);
		else
			break;
	}
}

network::network()
{
	memset(IpAddr, NULL, 8);
	err = gethostip();
}

bool network::gethostip()
{
	//�����׽��ֿ�  
	WORD wVersionRequested;
	WSADATA wsaData;
	wVersionRequested = MAKEWORD(1, 1);    //��ʼ��Socket��̬���ӿ�,����1.1�汾��winsocket��  

	err = WSAStartup(wVersionRequested, &wsaData);

	if (LOBYTE(wsaData.wVersion) != 1 ||   //�ж������winsocket�ǲ���1.1�İ汾  
		HIBYTE(wsaData.wVersion) != 1)
	{
		WSACleanup();          //����  
		return false;                  //��ֹ��winsocketʹ��  
	}
	//WSADATA ws;  
	//WSAStartup(MAKEWORD(2,2),&ws);//  
	char http[] = "www.myvip6.com";           //���ʷ���������
	SOCKET sock = socket(AF_INET, SOCK_STREAM, 0);//����socket  
	if (sock == INVALID_SOCKET)
	{
		//std::cout << "��������socket�׽���ʧ��!" << std::endl;
		return false;
	}
	hostent* host = gethostbyname(http);//ȡ��������IP��ַ  
	if (host == NULL)
	{
		//std::cout << "��������û������״̬;" << std::endl;
		return false;
	}
	in_addr addr;
	for (int i = 0;; i++)
	{
		char *p = host->h_addr_list[i];
		if (p == NULL)
			break;
		memcpy(&addr.S_un.S_addr, p, host->h_length);
		*(IpAddr + i) = new char[strlen(inet_ntoa(addr)) + 1];
		strcpy(*(IpAddr + i), inet_ntoa(addr));
		//std::cout << *(IpAddr + i);
	}
	return true;
}
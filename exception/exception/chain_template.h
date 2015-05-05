/*=====================================================

����ģ�壬�ɵ�����vector����ʹ�ã�����û����Ƶ�������
powered by Carol(luoshengwen)
date : 2015 - 5 - 4 

=====================================================*/
//#include <iostream>
#ifndef _chain_template //
#define _chain_template
template <typename T>struct element
{
	T ele;//�洢Ԫ��
	element<T> * next;//ָ����һ��Ԫ��
};

template< class type >class chain_table
{
public:
	chain_table(){};
	~chain_table();
	void push_back(type);
	type getelement();
private:
	element<type> * head = NULL, * last = NULL; //record the chain of head and last elementarty
	element<type> * record = NULL;				//��¼��ǰԪ��
};

//��������
template<class type> chain_table<type>::~chain_table()
{
	while (head)
	{
		record = head;
		head = head->next;
		delete record;
	}
}

//������Ϊ���Ԫ��
template <class type> void chain_table<type>::push_back(type ele)
{
	element<type> * temp = new element<type>;
	temp->ele = ele;
	/*std::cout << temp->ele[0] << "	"
		<< temp->ele[1] << "	"
		<< temp->ele[2] << "	"
		<< temp->ele[3] << "	" << std::endl;*/
	temp->next = NULL;
	if (head)
	{
		last->next = temp;
		last = temp;
	}
	else
		last = record = head = temp;
}


//�õ���ǰԪ��ֵ
template<class type>type chain_table<type>::getelement()
{
	if (record)
	{
		type ele = record->ele;
		record = record->next;
		return ele;
	}
	else
		return NULL;
}


#endif
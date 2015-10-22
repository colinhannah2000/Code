#include <iostream>

#include "SimpleDisruptorQueue.hpp"
#include "QueuedThreadPool.hpp"

using namespace std;

class TestSimpleDisruptorQueue
{
public:
    static void Run(void)
    {
        unique_ptr<SimpleDisruptorQueue<int>> queueInt(new SimpleDisruptorQueue<int>(100,3,1));
        queueInt->RegisterNewMessageCreator(&CreateMessageInt);

        unique_ptr<SimpleDisruptorQueue<Element>> queueElement(new SimpleDisruptorQueue<Element>(100,3,1));
        queueElement->RegisterNewMessageCreator(&CreateMessageElement);

        Ex2<int> *e = new Ex2<int>(1);
        Ex1 *e1 = new Ex1(1);
    }
private:
    class Element
    {
        public:
            int One;
            int Two;
            int Three;
    };

    static CoreErrors::ErrorId CreateMessageInt(int *pNewMessage, int *pBufferSlot)
    {
        // Copy the message.
        *pBufferSlot = *pNewMessage;
        return CoreErrors::SUCCESS;
    }

    static CoreErrors::ErrorId CreateMessageElement(Element *pNewMessage, Element *pBufferSlot)
    {
        // Copy the message.
        pBufferSlot->One = pNewMessage->One;
        return CoreErrors::SUCCESS;
    }
};

#include <iostream>
#include "SimpleDisruptorQueue.hpp"
#include "QueuedThreadPool.hpp"

using namespace std;

class TestSimpleDisruptorQueue
{
public:
    static void Run(void)
    {
        SimpleDisruptorQueue<int> *queue = new SimpleDisruptorQueue<int>
        (
            100,
            3,
            1
        );

        Ex2<int> *e = new Ex2<int>(1);
        Ex1 *e1 = new Ex1(1);
    }
private:
    class Element
    {
        public:
            int One;
    };
};

#include <iostream>
#include "TestSimpleDisruptorQueue.cpp"

using namespace std;

int main()
{
    cout << "Running Unit Tests." << endl;

    cout << "   Running Core Tests." << endl;

    TestSimpleDisruptorQueue::Run();

    cout << "   End Core Tests." << endl;

    cout << "End." << endl;

    return 0;
}



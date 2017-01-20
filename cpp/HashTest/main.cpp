#include <iostream>
#include "IHashTable.h"
#include "SimpleHash.h"

using namespace std;

namespace HashTest
{
    static int _count = 1;

    int GenerateHash(int key)
    {
        return _count++;
    }
}


int main()
{
    HashTest::SimpleHash<int, string> hash(&HashTest::GenerateHash);

    cout << "Hello world!" << endl;
    return 0;
}


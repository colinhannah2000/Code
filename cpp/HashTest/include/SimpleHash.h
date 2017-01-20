#pragma once
#include <array>
#include <IHashTable.h>

namespace HashTest
{
    template <typename K, typename T>
    class SimpleHash : IHashTable<K,T>
    {
        public:
            typedef int (*FHashFunction)(K key);

            SimpleHash(FHashFunction hashFunction, int defaultSize = 100)
            :
            _hashFunction(hashFunction),
            _size(defaultSize),
            _ppData(nullptr)
            {
                _ppData = new (T*)[_size];


            }

            virtual ~SimpleHash() {}

            virtual void Add(K key, T* item)
            {
            }

            virtual void Remove(K key)
            {
            }

            virtual int GetCount()
            {
                return 0;
            }

            virtual T* Get(K key)
            {
                return nullptr;
            }

//            virtual std::iterator<std::input_iterator_tag, T> GetIterator()
//            {
//
//            }

        private:
            FHashFunction _hashFunction;
            T **_ppData;
            int _size;
    };
}

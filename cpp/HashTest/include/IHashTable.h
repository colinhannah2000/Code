#pragma once

namespace HashTest
{
    template <typename K, typename T>
    class IHashTable
    {
        public:
            virtual void Add(K key, T *item) = 0;
            virtual void Remove(K key) = 0;
            virtual int GetCount() = 0;
            virtual T* Get(K key) = 0;
            //virtual std::iterator<std::input_iterator_tag, T> GetIterator() = 0;
        protected:
        private:
    };
}

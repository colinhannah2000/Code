#include "CoreErrors.hpp"
#include "SimpleDisruptorQueue.hpp"

template<class TMessage>
SimpleDisruptorQueue<TMessage>::SimpleDisruptorQueue
(
  int ringSize,
  int consumerCount,
  int readerCount
) :  mRingSize (ringSize), mEnricherCount(consumerCount), mReaderCount(readerCount)
{
  mRingBuffer = new TMessage[mRingSize];
}

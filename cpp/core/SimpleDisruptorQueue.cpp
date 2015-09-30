#include "CoreErrors.hpp"
#include "SimpleDisruptorQueue.hpp"

template<class TMessage>
SimpleDisruptorQueue<TMessage>::SimpleDisruptorQueue
  (
    int ringSize,
    int enricherCount,
    int readerCount
  ) :  mRingSize (ringSize), mEnricherCount(enricherCount), mReaderCount(readerCount)
  {
    mRingBuffer = new TMessage[mRingSize];
    mEnricherPosition = new long[mEnricherCount];
    mReaderPosition = new long[mReaderCount];
  }

template<class TMessage>
CoreErrors::ErrorId SimpleDisruptorQueue<TMessage>::RegisterNewMessageCreator(fCreator function)
  {
    CoreErrors::ErrorId status = CoreErrors::DISRUPTER_MESSAGE_CREATOR_ALREADY_REGISTERED;
    
    if(mCreateNewMessageFunction == NULL)
    {
      mCreateNewMessageFunction = new std::unique_ptr<fCreator>(function);
      status = CoreErrors::SUCCESS;
    }
    
    return status;    
  }
  
template<class TMessage>
CoreErrors::ErrorId SimpleDisruptorQueue<TMessage>::Start(void)
  {
    CoreErrors::ErrorId status = CoreErrors::SUCCESS;
    
    // Test all assumptions here so we can skip during message handling.
    if(mCreateNewMessageFunction == NULL) 
    {
      status = CoreErrors::DISRUPTER_NOT_READY_TO_START;
    }
    return status;   
  }
  
template<class TMessage>
CoreErrors::ErrorId SimpleDisruptorQueue<TMessage>::CreateNewMessage
  (
    TMessage *pMessage,
    long &id // out.
  )
  {
    CoreErrors::ErrorId status = CoreErrors::SUCCESS; 
    if (mNextNewPosition > GetLastConsumerPosition())
    {
      status = CoreErrors::DISRUPTER_RING_BUFFER_FULL;
    }
    else
    {
      status = mCreateNewMessageFunction(
        pMessage, 
        &(mRingBuffer[mNextNewPosition]));
        
      mNextNewPosition++;
    }
    return status;  
  }
  
template<class TMessage>
long SimpleDisruptorQueue<TMessage>::GetLastConsumerPosition(void)
  {
    return min(GetLastEnricherPosition(), GetLastReaderPosition);
  }
  
template<class TMessage>
long SimpleDisruptorQueue<TMessage>::GetLastEnricherPosition(void)
  {
    return GetLastPosition(mEnricherPosition, mEnricherCount);
  }
  
template<class TMessage>
long SimpleDisruptorQueue<TMessage>::GetLastReaderPosition(void)
  {
    return GetLastPosition(mReaderPosition, mReaderCount);
  }  
    
template<class TMessage>
long SimpleDisruptorQueue<TMessage>::GetLastPosition(int positions[], int length)
  {
    int lowest = mNextNewPosition;
    for(int i=0;i<length;i++)
    {
      lowest = lowest < positions[i] ? lowest : positions[i];
    }
    return lowest;
  }
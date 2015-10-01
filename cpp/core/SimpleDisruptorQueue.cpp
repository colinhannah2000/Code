#include "CoreErrors.hpp"
#include "SimpleDisruptorQueue.hpp"

template<class TMessage>
SimpleDisruptorQueue<TMessage>::SimpleDisruptorQueue
  (
    RingSizeType ringSize,
    int enricherCount,
    int readerCount
  ) :  mRingSize (ringSize), mEnricherCount(enricherCount), mReaderCount(readerCount)
  {
    mpRingBuffer = new std::unique_ptr<TMessage>(new TMessage[mRingSize]);
    InitialisePositonArray(mpEnricherPosition, mEnricherCount);
    InitialisePositonArray(mpReaderPosition, mReaderCount);
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
    RingSizeType &id // out.
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
        &(mpRingBuffer[mNextNewPosition]));
        
      mNextNewPosition++;
    }
    return status;  
  }
  
template<class TMessage>
RingSizeType SimpleDisruptorQueue<TMessage>::GetLastConsumerPosition(void)
  {
    return min(GetLastEnricherPosition(), GetLastReaderPosition);
  }
  
template<class TMessage>
RingSizeType SimpleDisruptorQueue<TMessage>::GetLastEnricherPosition(void)
  {
    return GetLastPosition(mpEnricherPosition, mEnricherCount);
  }
  
template<class TMessage>
RingSizeType SimpleDisruptorQueue<TMessage>::GetLastReaderPosition(void)
  {
    return GetLastPosition(mpReaderPosition, mReaderCount);
  }  
    
template<class TMessage>
RingSizeType SimpleDisruptorQueue<TMessage>::GetLastPosition(int positions[], int length)
  {
    int lowest = mNextNewPosition;
    for(int i=0;i<length;i++)
    {
      lowest = lowest < positions[i] ? lowest : positions[i];
    }
    return lowest;
  }
  
template<class TMessage>
void SimpleDisruptorQueue<TMessage>::GetRingStatus(
  RingSizeType &ringSize, 
  RingSizeType *pEnricherPosition, 
  RingSizeType *pReaderPosition, 
  RingSizeType &newPosition)
  {
    ringSize = mRingSize; 
    // We don't care about races, these are approximations.
    CopyPositonArray(mpEnricherPosition, pEnricherPosition, sizeof(RingSizeType*)*mEnricherCount);
    CopyPositonArray(mpReaderPosition, pReaderPosition, sizeof(RingSizeType*)*mReaderCount);
    newPosition = mNextNewPosition;
  }
  
template<class TMessage>
void SimpleDisruptorQueue<TMessage>::InitialisePositonArray(
  std::unique_ptr<RingSizeType[]> &pPositions, 
  int size)
  {
    pPositions = std::unique_ptr<RingSizeType[]>(new RingSizeType[size]{});
    for(int i=0;i<size;i++)
    {
      pPositions[i] = mInvalidPosition;
    }
  } 
  
 
template<class TMessage>
void SimpleDisruptorQueue<TMessage>::CopyPositonArray(
  std::unique_ptr<RingSizeType[]> &pSource,
  RingSizeType *pTarget,  
  int size)
  {
    for(int i=0;i<size;i++)
    {
      pTarget[i] = pSource[i];
    }
  }
  
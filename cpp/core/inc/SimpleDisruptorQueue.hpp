#ifndef _SimpleDisruptorQueue_H_
#define _SimpleDisruptorQueue_H_

#include <string>
#include <memory>
#include <iterator>
#include "CoreErrors.hpp"

using namespace std;

typedef long long RingSizeType;

/*
  Ring buffer with a single creator, multiple enrichers and readers.
  Only a single thread/writer can create a new message.
  An enricher may only modify unique elements of TMessage.
  A fully enriched message must be read by all readers before it is destroyed.

  TMessage should be a class of nested structs. One per Enricher.
*/
template<class TMessage>
class SimpleDisruptorQueue
{
public:
  // A function called on the Creator thread that copies non-enriched
  // elements from NewMessage to BufferSlot. Called on CreateNewMessage thread.
  typedef CoreErrors::ErrorId (*FCREATOR) (TMessage *pNewMessage, TMessage *pBufferSlot);

  // A registered function called on the ENRICHER'S THREAD when a message is enriched.
  // Register an instance method to access the enricher's private data.
  typedef CoreErrors::ErrorId (*FENRICHER) (TMessage *pBufferSlot);

  typedef CoreErrors::ErrorId (*FREADER) (const TMessage *pBufferSlot);

  SimpleDisruptorQueue
  (
    RingSizeType ringSize, // Should be very large. Virtual Memory.
    int enricherCount, // Number of writers that can enrich a message. All must enrich before read.
    int readerCount // Number of readers must read a message before the message is destroyed.
  )
  :
  mRingSize(ringSize),
  mEnricherCount(enricherCount),
  mReaderCount(readerCount),
  mpRingBuffer(new TMessage[ringSize]),
  mCreateNewMessageFunction(nullptr)
  {
    //mpRingBuffer = std::make_unique<TMessage[]>(new TMessage[mRingSize]);
    InitialisePositonArray(mpEnricherPosition, mEnricherCount);
    InitialisePositonArray(mpReaderPosition, mReaderCount);
  }

  // Each id must match an index into the set of enrichers.
  CoreErrors::ErrorId RegisterEnricher(int id, FENRICHER fEnricher)
  {
    CoreErrors::ErrorId status = CoreErrors::SUCCESS;
    if (id < mEnricherCount)
    {
        CoreErrors::ErrorId status = CoreErrors::DISRUPTER_ENRICHER_ID_INVALID;
    }

    if (m)
  }

  void RegisterReader(int id) {}

  // Register a message creator that will populate the buffered message on a different thread.
  CoreErrors::ErrorId RegisterNewMessageCreator(FCREATOR createNewMessageFunction)
  {
    CoreErrors::ErrorId status = CoreErrors::DISRUPTER_MESSAGE_CREATOR_ALREADY_REGISTERED;

    if(mCreateNewMessageFunction == nullptr)
    {
      mCreateNewMessageFunction = createNewMessageFunction;
      status = CoreErrors::SUCCESS;
    }

    return status;
  }

  CoreErrors::ErrorId Enrich(int id)

  CoreErrors::ErrorId Start(void)
  {
    CoreErrors::ErrorId status = CoreErrors::SUCCESS;

    // Test all assumptions here so we can skip during message handling.
    if(mCreateNewMessageFunction == NULL)
    {
      status = CoreErrors::DISRUPTER_NOT_READY_TO_START;
    }
    return status;
  }

  CoreErrors::ErrorId CreateNewMessage(TMessage *pMessage, RingSizeType &id)
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

  // Expose state for monitoring.
  void GetRingSizes(
    RingSizeType &ringSize,
    RingSizeType &enrichersSize,
    RingSizeType &readersSize,
    RingSizeType &newPosition)
    {
        ringSize = mRingSize;
        enrichersSize = mEnricherCount;
        readersSize = mReaderCount;
        newPosition = mNextNewPosition;
    }

  void GetRingStatus(
    RingSizeType &ringSize,
    RingSizeType pEnricherPosition[], // Buffer the size of matching GetRingSizes.
    RingSizeType pReaderPosition[], // as above.
    RingSizeType &newPosition)
    {
        ringSize = mRingSize;
        // We don't care about races, these are approximations.
        CopyPositonArray(mpEnricherPosition, pEnricherPosition, sizeof(RingSizeType*)*mEnricherCount);
        CopyPositonArray(mpReaderPosition, pReaderPosition, sizeof(RingSizeType*)*mReaderCount);
        //std::copy(mpReaderPosition, mpReaderPosition+mReaderCount, pReaderPosition);
        newPosition = mNextNewPosition;
    }

private:
  //std::shared_ptr<FCREATOR> mCreateNewMessageFunction;
  FCREATOR mCreateNewMessageFunction;
  std::unique_ptr<TMessage> mpRingBuffer;
  RingSizeType mRingSize;
  int mEnricherCount = 0;
  int mReaderCount = 0;

  std::unique_ptr<FENRICHER[]> mpEnrichers;
  std::unique_ptr<FREADER[]> mpReaders;

  const RingSizeType mInvalidPosition = -1;
  std::unique_ptr<RingSizeType[]> mpEnricherPosition;
  std::unique_ptr<RingSizeType[]> mpReaderPosition;
  RingSizeType mNextNewPosition;

  RingSizeType GetLastConsumerPosition(void)
  {
    return min(GetLastEnricherPosition(), GetLastReaderPosition);
  }

  RingSizeType GetLastEnricherPosition(void)
  {
    return GetLastPosition(mpEnricherPosition, mEnricherCount);
  }

  RingSizeType GetLastReaderPosition(void)
  {
    return GetLastPosition(mpReaderPosition, mReaderCount);
  }

  RingSizeType GetLastPosition(int positions[], int length)
  {
    int lowest = mNextNewPosition;
    for(int i=0;i<length;i++)
    {
      lowest = lowest < positions[i] ? lowest : positions[i];
    }
    return lowest;
  }

  void InitialisePositonArray(std::unique_ptr<RingSizeType[]> &pPositions, int size)
  {
    pPositions = std::unique_ptr<RingSizeType[]>(new RingSizeType[size]{});
    for(int i=0;i<size;i++)
    {
      pPositions[i] = mInvalidPosition;
    }
  }

  void CopyPositonArray(std::unique_ptr<RingSizeType[]> &pSource, RingSizeType *pTarget, int size)
  {
    for(int i=0;i<size;i++)
    {
      pTarget[i] = pSource[i];
    }
  }
};

#endif // _SimpleDisruptorQueue_H_

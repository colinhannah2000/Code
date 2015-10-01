#ifndef _SimpleDisruptorQueue_H_
#define _SimpleDisruptorQueue_H_

#include <memory>
#include "CoreErrors.hpp"

using namespace std;

/*
  Ring buffer with a single creator, multiple enrichers and readers.
  Only a single thread/writer can create a new message.
  An enricher may only modify unique elements of TMessage.
  A fully enriched message must be read by all readers before it is destroyed.


  TMessage should be a class of nested structs. One per constr
*/
template<class TMessage>
class SimpleDisruptorQueue
{
public:
  // A function called on the Creator thread that copies non-enriched
  // elements from NewMessage to BufferSlot.
  typedef CoreErrors::ErrorId (*fCreator) (TMessage *pNewMessage, TMessage *pBufferSlot);
  typedef long long RingSizeType;

  SimpleDisruptorQueue
  (
    RingSizeType ringSize, // Should be very large. Virtual Memory.
    int enricherCount, // Number of writers that can enrich a message. All must enrich before read.
    int readerCount // Number of readers must read a message before the message is destroyed.
  );

  void RegisterEnricher(int id); // Each id must match an index into the set of enrichers.
  void RegisterReader(int id);
  
  CoreErrors::ErrorId RegisterNewMessageCreator(fCreator createNewMessageFunction);

  CoreErrors::ErrorId Start(void); 

  CoreErrors::ErrorId CreateNewMessage(TMessage *pMessage, long &id);
  
  // Expose state for monitoring.
  void GetRingSizes(
    RingSizeType &ringSize, 
    RingSizeType &enrichersSize, 
    RingSizeType &readersSize, 
    RingSizeType &newPosition);
    
  void GetRingStatus(
    RingSizeType &ringSize, 
    RingSizeType pEnricherPosition[], // Buffer the size of matching GetRingSizes.
    RingSizeType pReaderPosition[], // as above.
    RingSizeType &newPosition);

private:
  std::unique_ptr<fCreator> mCreateNewMessageFunction;
  std::unique_ptr<TMessage> mpRingBuffer;
  RingSizeType mRingSize;
  int mEnricherCount;
  int mReaderCount;
  
  const int mInvalidPosition = -1;
  std::unique_ptr<RingSizeType> mpEnricherPosition;
  std::unique_ptr<RingSizeType> mpReaderPosition;
  RingSizeType mNextNewPosition;
  
  RingSizeType GetLastConsumerPosition(void);
  RingSizeType GetLastEnricherPosition(void);
  RingSizeType GetLastReaderPosition(void);  
  RingSizeType GetLastPosition(int positions[], int length);
  void InitialisePositonArray(std::unique_ptr<RingSizeType> &pPositions, int size);
};

#endif // _SimpleDisruptorQueue_H_

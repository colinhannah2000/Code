#ifndef _SimpleDisruptorQueue_H_
#define _SimpleDisruptorQueue_H_

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
  typedef CoreErrors::ErrorId (*fCreator)
    (TMessage *pNewMessage, TMessage *pBufferSlot) CreateNewMessageFunction;

  SimpleDisruptorQueue
  (
    int ringSize,
    int enricherCount, // Number of writers that can enrich a message
    int readerCount
  );

  void RegisterEnricher(int id);
  void RegisterReader(int id);
  ErrorId RegisterNewMessageCreator(CreateNewMessageFunction createNewMessageFunction);

  ErrorId CreateNewMessage
  (
    TMessage *pMessage,
    long &id // out.
  );

private:
  TMessage mRingBuffer;
  long mRingSize;
  int mEnricherCount;
  int mReaderCount;
};



#endif // _SimpleDisruptorQueue_H_

#ifndef _CoreErrors_H_
#define _CoreErrors_H_


class CoreErrors
{
public:
  typedef int ErrorId;

  // Errors.
  const static ErrorId SUCCESS = 0;

  // Distrupter Errors:
  const static ErrorId DISRUPTER_ENRICHER_ALREADY_REGISTERED = 1;
  const static ErrorId DISRUPTER_READER_ALREADY_REGISTERED = 2;
  const static ErrorId DISRUPTER_MESSAGE_CREATOR_ALREADY_REGISTERED = 3;
  
  const static ErrorId DISRUPTER_NOT_READY_TO_START = 4;
  const static ErrorId DISRUPTER_RING_BUFFER_FULL = 5;
};
/*
const CoreErrorMap[int,const string] =
{
  {}
}
*/
#endif // _CoreErrors_H_

#ifndef _CoreErrors_H_
#define _CoreErrors_H_


class CoreErrors
{
public:
  typedef int ErrorId;

  // Errors.
  const ErrorId SUCCESS = 0;

  // Distrupter Errors:
  const ErrorId DISRUPTER_ENRICHER_ALREADY_REGISTERED = 1;
  const ErrorId DISRUPTER_READER_ALREADY_REGISTERED = 2;
};
/*
const CoreErrorMap[int,const string] =
{
  {}
}
*/
#endif // _CoreErrors_H_

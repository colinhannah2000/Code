IDIR=core/inc
CC=g++
CFLAGS=-std=c++0x -pedantic  -I$(IDIR)


ODIR=obj
LDIR =lib

LIBS=-lm

COMPILE=$(CC) $(CFLAGS) -c
LINK=$(CC) $(CFLAGS)

COREDIR=core
COREOBJDIR=$(COREDIR)/obj

UNITTESTSDIR=unitTests
UNITTESTSOBJDIR=$(UNITTESTSDIR)/obj

TARGETDEBUGDIR=bin/debug

#CoreCode = $(COREDIR)/QueuedThreadPool.cpp
#CORECODE = $(COREDIR)/QueuedThreadPool.cpp $(COREDIR)/Other.cpp
#DEPS = $(patsubst %,$(IDIR)/%,$(_DEPS))

#OBJ = $(patsubst %,$(ODIR)/%,$(_OBJ))

# $@ says to put the output of the compilation in the file named on the left side
# of the :, the $< is the first item in the dependencies list

#$(ODIR)/%.o: $(CoreCode)
#	$(CC) -c -o $@ $< $(CFLAGS)



coreobj:
	$(COMPILE) $(COREDIR)/QueuedThreadPool.cpp -o $(COREOBJDIR)/QueuedThreadPool.o
	$(COMPILE) $(COREDIR)/SimpleDisruptorQueue.cpp -o $(COREOBJDIR)/SimpleDisruptorQueue.o

COREOBJ = $(COREOBJDIR)/QueuedThreadPool.o $(COREOBJDIR)/SimpleDisruptorQueue.o

#corelibmake: coreobj
#	ar rvs $(LDIR)/core.ar $(COREOBJ)

unittestsobj: coreobj
	$(COMPILE) $(UNITTESTSDIR)/main.cpp -o $(UNITTESTSOBJDIR)/main.o
	$(COMPILE) $(UNITTESTSDIR)/TestSimpleDisruptorQueue.cpp -o $(UNITTESTSOBJDIR)/TestSimpleDisruptorQueue.o

UNITTESTSOBJ = $(UNITTESTSOBJDIR)/main.o $(UNITTESTSOBJDIR)/TestSimpleDisruptorQueue.o

unittestsmake: unittestsobj
	$(LINK) $(UNITTESTSOBJ) $(COREOBJ) -o $(TARGETDEBUGDIR)/$(UNITTESTSDIR) -lstdc++

all: unittestsmake

	#	$(CC) $(CFLAGS) -c $(COREOBJ)

#executable
#	$(CC) -c $@ $^ $(CFLAGS) $(LIBS)

.PHONY: clean

clean:
	rm -f $(COREOBJDIR)/*.o
	rm -f $(UNITTESTSOBJDIR)/*.o
	rm -f $(LDIR)/*

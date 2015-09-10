IDIR=core/inc
CC=g++
CFLAGS=-I$(IDIR)


ODIR=obj
LDIR =lib

LIBS=-lm

CCC=$(CC) $(CFLAGS) -c

COREDIR=core

COREOBJDIR=$(COREDIR)/obj

#CoreCode = $(COREDIR)/QueuedThreadPool.cpp
#CORECODE = $(COREDIR)/QueuedThreadPool.cpp $(COREDIR)/Other.cpp
#DEPS = $(patsubst %,$(IDIR)/%,$(_DEPS))

COREOBJ = $(COREOBJDIR)/QueuedThreadPool.o $(COREOBJDIR)/SimpleDisruptorQueue.o
#OBJ = $(patsubst %,$(ODIR)/%,$(_OBJ))

# $@ says to put the output of the compilation in the file named on the left side
# of the :, the $< is the first item in the dependencies list

#$(ODIR)/%.o: $(CoreCode)
#	$(CC) -c -o $@ $< $(CFLAGS)



coreobj:
	$(CCC) $(COREDIR)/QueuedThreadPool.cpp -o $(COREOBJDIR)/QueuedThreadPool.o
	$(CCC) $(COREDIR)/SimpleDisruptorQueue.cpp -o $(COREOBJDIR)/SimpleDisruptorQueue.o

corelibmake: coreobj
	ar rvs $(LDIR)/core.ar $(COREOBJ)

all: corelibmake

	#	$(CC) $(CFLAGS) -c $(COREOBJ)

#executable
#	$(CC) -c $@ $^ $(CFLAGS) $(LIBS)

.PHONY: clean

clean:
	rm -f $(COREOBJDIR)/*.o
	rm -f $(LDIR)/*

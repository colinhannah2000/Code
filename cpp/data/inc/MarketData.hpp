#ifndef _MARKETDATA_H_
#define _MARKETDATA_H_

typedef struct 
{
	double Price;
	long Quantity;
	int BrokerId;
} PriceQuantity;

typedef struct 
{
	PriceQuantity Bids[10];
	PriceQuantity Offers[10];
} TenPriceLevels;

#endif //_MARKETDATA_H_

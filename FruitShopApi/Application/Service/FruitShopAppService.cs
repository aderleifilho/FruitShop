﻿using System;
using System.Collections.Generic;
using System.Linq;
using FruitShopApi.Application.Interface;
using FruitShopApi.Domain.Entity;
using FruitShopApi.Domain.Interface;

namespace FruitShopApi.Application.Service
{
    public class FruitShopAppService : IFruitShopAppService
    {
        private readonly IFruitShopService _service;

        public FruitShopAppService(IFruitShopService service)
        {
            this._service = service;
        }

        public Receipt RunFruitShop()
        {
            Receipt r = new Receipt();
            r.ReceiptId = 1;
            r.Total = 0;
            IEnumerable<Purchase> purchases = this._service.GetAllPurchases();

            foreach (Purchase purchase in purchases)
            {
                Product product = this._service.GetProductById(purchase.Product);
                Offer offer = this._service.GetOfferByProduct(purchase.Product);
                r.Purchase.Add(purchase);

                if (offer != null)
                {
                    if (purchase.Quantity >= offer.MinQuantity)
                    {
                        switch (offer.Category)
                        {
                            case "1":
                                r.Total = product.Price * (offer.OfferDiscount / 100) * purchase.Quantity;
                                break;
                            case "2":
                                purchase.Quantity += 1;
                                break;
                            case "3":
                                r.Total = product.Price - offer.OfferDiscount * purchase.Quantity;
                                break;
                            default:
                                break;
                        }

                    }

                }

            }

            return r;
        }
    }
}
﻿using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteNoiThat.Models
{
    [Serializable]
    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public bool Status { get; set; }
        public int Points { get; set; }
        public decimal? TotalPurchase { get; set; }
        public decimal? DiscountedPrice { get; set; }
    }
}
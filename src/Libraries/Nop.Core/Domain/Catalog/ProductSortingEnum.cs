﻿namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// Represents the product sorting
    /// </summary>
    public enum ProductSortingEnum
    {
        /// <summary>
        /// Position (display order)
        /// </summary>
        PositionAsc = 0,
        
        /// <summary>
        /// Position (display order)
        /// </summary>
        PositionDesc = 1,
        
        /// <summary>
        /// Name: A to Z
        /// </summary>
        NameAsc = 5,

        /// <summary>
        /// Name: Z to A
        /// </summary>
        NameDesc = 6,

        /// <summary>
        /// Price: Low to High
        /// </summary>
        PriceAsc = 10,

        /// <summary>
        /// Price: High to Low
        /// </summary>
        PriceDesc = 11,

        /// <summary>
        /// Product creation date
        /// </summary>
        CreatedOn = 15,
        
        /// <summary>
        /// Stock: Low to High
        /// </summary>
        StockAsc = 16,
        
        /// <summary>
        /// Stock: Low to High
        /// </summary>
        StockDesc = 17,
      
        /// <summary>
        /// SKU: A to Z
        /// </summary>
        SkuAsc = 18,

        /// <summary>
        /// SKU: Z to A
        /// </summary>
        SkuDesc = 19,

        /// <summary>
        /// Published: Yes to No
        /// </summary>
        PublishedTrue = 20,

        /// <summary>
        /// Published: No to Yes
        /// </summary>
        PublishedFalse = 21
        
    }
}
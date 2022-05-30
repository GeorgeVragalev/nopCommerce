namespace Nop.Web.Models.Catalog;

public enum ProductOrderByEnum
{
    /// <summary>
    /// Position (display order)
    /// </summary>
    Position = 0,

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
}
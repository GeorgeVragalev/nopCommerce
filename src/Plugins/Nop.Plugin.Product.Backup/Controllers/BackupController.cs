using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nop.Plugin.Product.Backup.Models;
using Nop.Services.Catalog;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Product.Backup.Controllers;

[Area(AreaNames.Admin)]
[AutoValidateAntiforgeryToken]
[ValidateIpAddress]
[AuthorizeAdmin]
public class BackupController: BasePluginController
{
    private readonly IPermissionService _permissionService;
    private readonly IProductService _productService;

    public BackupController(IPermissionService permissionService, IProductService productService)
    {
        _permissionService = permissionService;
        _productService = productService;
    }
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public virtual async Task<IActionResult> Configure()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
            return AccessDeniedView();
        
        //prepare model
        var models = await _productService.GetFiveUnexportedProductsAsync();
        var modelList = new List<ProductModel>();

        foreach (var model in models)
        {
            var mappedModel = new ProductModel
            {
                ProductTypeId = model.ProductTypeId,
                Name = model.Name,
                ShortDescription = model.ShortDescription,
                FullDescription = model.ShortDescription,
                Sku = model.Sku,
                StockQuantity = model.StockQuantity,
                OldPrice = model.OldPrice,
                Price = model.Price,
                Processed = model.Processed,
                CreatedOnUtc = model.CreatedOnUtc,
                UpdatedOnUtc = model.UpdatedOnUtc,
            };
            modelList.Add(mappedModel);
            
            System.IO.File.WriteAllText(@"C:\Users\vraga\Desktop\NopCommerce\Files\product"+model.Id+".json", JsonConvert.SerializeObject(mappedModel));

            await using var file = System.IO.File.CreateText(@"C:\Users\vraga\Desktop\NopCommerce\Files\product"+model.Id+".json");
            var serializer = new JsonSerializer();
            serializer.Serialize(file, mappedModel);
        }

        return View("~/Plugins/Product.Backup/Views/Configure.cshtml", modelList);
    }

    public virtual async Task<IActionResult> CreateJsonFileAsync()
    {

        
        
        return null;
    }
 
    
}
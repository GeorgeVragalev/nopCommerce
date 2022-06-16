using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Plugin.Product.Backup.Models;
using Nop.Plugin.Product.Backup.Services;
using Nop.Services.Configuration;
using Nop.Services.Media;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;

namespace Nop.Plugin.Product.Backup.Factory;

public class ProductBackupFactory : IProductBackupFactory
{
    private readonly ProductBackupSettings _productBackupSettings;
    private readonly IStoreContext _storeContext;
    private readonly ISettingService _settingService;
    private readonly IProductService _productService;
    private readonly IPictureService _pictureService;
    private readonly Nop.Services.Catalog.IProductService _service;

    public ProductBackupFactory(IStoreContext storeContext, ISettingService settingService,
        ProductBackupSettings productBackupSettings, IProductService productService, IPictureService pictureService, Nop.Services.Catalog.IProductService service)
    {
        _storeContext = storeContext;
        _settingService = settingService;
        _productBackupSettings = productBackupSettings;
        _productService = productService;
        _pictureService = pictureService;
        _service = service;
    }

    public async Task<ProductBackupSettingsModel> PrepareProductBackupSettingsModelAsync(
        ProductBackupSettingsModel model = null)
    {
        //load settings for a chosen store scope
        var storeId = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var productBackupSettings = await _settingService.LoadSettingAsync<ProductBackupSettings>(storeId);

        //fill in model values from the entity
        model ??= productBackupSettings.ToSettingsModel<ProductBackupSettingsModel>();

        //fill in additional values (not existing in the entity)
        model.ActiveStoreScopeConfiguration = storeId;

        if (storeId <= 0)
            return model;

        model.BackupConfigurationEnabled_OverrideForStore =
            await _settingService.SettingExistsAsync(productBackupSettings, x => x.BackupConfigurationEnabled, storeId);
        model.ProcessingProductsNumber_OverrideForStore =
            await _settingService.SettingExistsAsync(productBackupSettings, x => x.ProcessingProductsNumber, storeId);
        model.ProductBackupTimer_OverrideForStore =
            await _settingService.SettingExistsAsync(productBackupSettings, x => x.ProductBackupTimer, storeId);
        model.ProductBackupStoragePath_OverrideForStore =
            await _settingService.SettingExistsAsync(productBackupSettings, x => x.ProductBackupStoragePath, storeId);

        return model;
    }

    public async Task<IList<ProductModel>> PrepareProductBackupModel()
    {
        if (_productBackupSettings.BackupConfigurationEnabled)
        {
            var modelList = new List<ProductModel>();
            var models = await _productService.GetNextProductsToExport();

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
                    CreatedOnUtc = model.CreatedOnUtc,
                    UpdatedOnUtc = model.UpdatedOnUtc,
                };
                //todo change to true
                model.Processed = false;
                await _service.UpdateProductAsync(model);
                modelList.Add(mappedModel);


                await System.IO.File.WriteAllTextAsync(
                    $"{_productBackupSettings.ProductBackupStoragePath}/" + model.Id + ".json",
                    JsonConvert.SerializeObject(mappedModel));
                await using var file =
                    System.IO.File.CreateText($"{_productBackupSettings.ProductBackupStoragePath}/" + model.Id +
                                              ".json");
                var serializer = new JsonSerializer();
                serializer.Serialize(file, mappedModel);
            }

            return modelList;
        }
        return null;
    }
    
    public async Task<List<PictureModel>> PrepareImageModel()
    {
        var pictureModelList = new List<PictureModel>();
        var pictureUrlsList = new List<string>();

        var productModelsList = await _productService.GetNextProductsToExport();

        foreach (var product in productModelsList)
        {
            var pictures = await _pictureService.GetPicturesByProductIdAsync(product.Id);

            pictureUrlsList.Clear();
            foreach (var picture in pictures)
            {
                var pictureUrl = await _pictureService.GetPictureUrlAsync(picture);
                pictureUrlsList.Add(pictureUrl.Url);
                var pictureModel = new PictureModel()
                {
                    AltAttribute = picture.AltAttribute,
                    IsNew = picture.IsNew,
                    MimeType = picture.MimeType,
                    SeoFilename = picture.SeoFilename,
                    TitleAttribute = picture.TitleAttribute,
                    VirtualPath = picture.VirtualPath,
                    UrlsString = pictureUrlsList
                };
                //todo export jpeg to local
                
                pictureModelList.Add(pictureModel);
            }
        }

        return pictureModelList;
    }
}


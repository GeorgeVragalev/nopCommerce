using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Plugin.Product.Backup.Models;
using Nop.Web.Models.Media;
using PictureModel = Nop.Plugin.Product.Backup.Models.PictureModel;

namespace Nop.Plugin.Product.Backup.Factory;

public interface IProductBackupFactory
{
    /// <summary>
    /// Prepare product backup model
    /// </summary>
    /// <param name="model">Product backup model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the webhook settings model
    /// </returns>
    Task<IList<ProductModel>> PrepareProductBackupModel();
    
    /// <summary>
    /// Prepare pciture model
    /// </summary>
    /// <param name="model">Picture model</param>
    /// <returns>
    /// A task that returns a list of pictureModels
    /// </returns>
    Task<List<PictureModel>> PrepareImageModel();
    
    /// <summary>
    /// Prepare product backup settings model
    /// </summary>
    /// <param name="model">Product backup model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the webhook settings model
    /// </returns>
    Task<ProductBackupSettingsModel> PrepareProductBackupSettingsModelAsync(ProductBackupSettingsModel model = null);
}
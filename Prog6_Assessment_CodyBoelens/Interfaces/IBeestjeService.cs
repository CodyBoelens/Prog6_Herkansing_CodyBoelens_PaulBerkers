using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;

namespace Prog6_Assessment_CodyBoelens.Interfaces
{
    public interface IBeestjeService
    {
        Task<List<BeestjeViewModel>> GetAllBeestjesAsync();
        Task<BeestjeViewModel> GetBeestjeByIdAsync(int id);
        Task<bool> CreateBeestjeAsync(BeestjeViewModel viewModel);
        Task<bool> UpdateBeestjeAsync(BeestjeViewModel viewModel);
        Task<bool> DeleteBeestjeAsync(int id);
        Task<List<Types>> GetAllTypesAsync();
        Task<List<String>> GetAllImageNamesAsync();
    }
}

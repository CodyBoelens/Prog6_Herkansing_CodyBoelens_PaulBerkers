using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;

namespace Prog6_Assessment_CodyBoelens.Interfaces
{
    public interface IBeestjeService
    {
        Task<List<BeestjeViewModels>> GetAllBeestjesAsync();
        Task<List<BeestjeViewModels>> GetAvailableBeestjesAsync(DateTime date);
        Task<BeestjeViewModels> GetBeestjeByIdAsync(int id);
        Task<bool> CreateBeestjeAsync(BeestjeViewModels viewModel);
        Task<bool> UpdateBeestjeAsync(BeestjeViewModels viewModel);
        Task<bool> DeleteBeestjeAsync(int id);
        Task<List<Types>> GetAllTypesAsync();
        Task<List<String>> GetAllImageNamesAsync();
        Task<List<BeestjeViewModels>> GetBeestjesByIdsAsync(List<int> ids);
    }
}

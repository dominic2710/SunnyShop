using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Functions
{
    public interface IClassifyNameFunction
    {
        Task<ClassifyName> GetNameByGroupIdAndCode(int groupId, int code);
        Task<IEnumerable<ClassifyName>> GetListNameByGroupId(int groupId);
        Task<ClassifyName> AddName(ClassifyName name);
        Task<int> UpdateName(ClassifyName name);
        Task<int> DeleteName(ClassifyName name);
        Task<int> DeleteAllName(int groupId);
    }
}

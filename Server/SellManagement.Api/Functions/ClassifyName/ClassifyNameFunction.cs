using Microsoft.EntityFrameworkCore;
using SellManagement.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Functions
{
    public class ClassifyNameFunction: IClassifyNameFunction
    {
        SellManagementContext _context;
        public ClassifyNameFunction(SellManagementContext context)
        {
            _context = context;
        }
        public async Task<ClassifyName> GetNameByGroupIdAndCode(int groupId, int code)
        {
            var entity = await _context.TblClassifiesName.Where(x => x.GroupId == groupId)
                                                .Where(x => x.Code == code)
                                                .ToListAsync();
            return entity.Select(ToNameModel).FirstOrDefault();
        }
        public async Task<IEnumerable<ClassifyName>> GetListNameByGroupId(int groupId)
        {
            var entity = await _context.TblClassifiesName.Where(x => x.GroupId == groupId)
                                                .OrderBy(x=> x.Code)
                                                .ToListAsync();
            return entity.Select(ToNameModel).ToList();
        }
        public async Task<ClassifyName> AddName(ClassifyName name)
        {
            var entity = new TblClassifyName
            {
                GroupId = name.GroupId,
                Code = name.Code,
                Name = name.Name
            };
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            name.Id = entity.Id;
            return name;
        }
        public async Task<int> UpdateName(ClassifyName name)
        {
            var entity = await _context.TblClassifiesName.Where(x => x.GroupId == name.GroupId)
                                                .Where(x => x.Code == name.Code)
                                                .FirstOrDefaultAsync();

            if (entity != null)
            {
                entity.Name = name.Name;
            }
            else
            {
                entity = new TblClassifyName
                {
                    GroupId = name.GroupId,
                    Code = name.Code,
                    Name = name.Name
                };
                await _context.AddAsync(entity);
            }
            await _context.SaveChangesAsync();

            return 1;
        }
        public async Task<int> DeleteName(ClassifyName name)
        {
            var entity = await _context.TblClassifiesName.Where(x => x.GroupId == name.GroupId)
                                                .Where(x => x.Code == name.Code)
                                                .FirstOrDefaultAsync();
            _context.Remove(entity);
            var count = await _context.SaveChangesAsync();
            return count;
        }

        public async Task<int> DeleteAllName(int groupId)
        {
            var entities = await _context.TblClassifiesName.Where(x => x.GroupId == groupId)
                                               .ToListAsync();
            _context.RemoveRange(entities);
            var count = await _context.SaveChangesAsync();
            return count;
        }

        private ClassifyName ToNameModel(TblClassifyName entity)
        {
            return entity == null ? new ClassifyName() : new ClassifyName
            {
                Id = entity.Id,
                GroupId = entity.GroupId,
                Code = entity.Code,
                Name =entity.Name
            };
        }

    }
}
